using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Collections.Specialized;
#if SILVERLIGHT
using System.Linq;
#endif

namespace OpenOptions.dnaFusion.Flex.V1
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;

        private readonly Action<object> _method;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _controlEvent.Add(new WeakReference(value));
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
#if SILVERLIGHT
                var query = (from q in ControlEvent where ((EventHandler)q.Target) == value select q).FirstOrDefault();
                if (query != null)
                    ControlEvent.Remove(query);
#else
                _controlEvent.Remove(_controlEvent.Find(r => ((EventHandler)r.Target) == value));
#endif
            }
        }

        private readonly List<WeakReference> _controlEvent;

        public DelegateCommand(Action<object> method)
            : this(method, null)
        {
        }

        public DelegateCommand(Action<object> method, Predicate<object> canExecute)
        {
            _controlEvent = new List<WeakReference>();
            _method = method;
            _canExecute = canExecute;
        }

        public List<INotifyPropertyChanged> PropertiesToListenTo { get; set; }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (_controlEvent != null && _controlEvent.Count > 0)
            {
                _controlEvent.ForEach(ce =>
                {
                    ((EventHandler) ce.Target)?.Invoke(null, EventArgs.Empty);
                });
            }
        }

        public DelegateCommand ListenOn<TObservedType, TPropertyType>(TObservedType viewModel, Expression<Func<TObservedType, TPropertyType>> propertyExpression) where TObservedType : INotifyPropertyChanged
        {
            var propertyName = GetPropertyName(propertyExpression);
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                    RaiseCanExecuteChanged();
            };
            return this;
        }

        public void ListenForNotificationFrom<TObservedType>(TObservedType viewModel) where TObservedType : INotifyPropertyChanged
        {
            viewModel.PropertyChanged += (sender, e) =>
            {
                RaiseCanExecuteChanged();
            };
        }

        public DelegateCommand ListenOn<TObservedType>(TObservedType collection) where TObservedType : INotifyCollectionChanged
        {
            collection.CollectionChanged += (sender, e) =>
            {
                RaiseCanExecuteChanged();
            };
            return this;
        }

        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression) where T : INotifyPropertyChanged
        {
            var lambda = expression as LambdaExpression;
            var memberInfo = GetMemberExpression(lambda).Member;
            return memberInfo.Name;
        }

        private static MemberExpression GetMemberExpression(LambdaExpression lambda)
        {
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
                memberExpression = lambda.Body as MemberExpression;
            return memberExpression;
        }
    }
}
