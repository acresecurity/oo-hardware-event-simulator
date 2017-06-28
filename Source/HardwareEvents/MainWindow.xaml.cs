using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CookComputing.XmlRpc;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            HardwareTreeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(ItemExpanded));

            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(provider.ServiceUrl);
            service.BeginFindControllers(provider.ApiKey, 1,
                lAsyncResult =>
                {
                    try
                    {
                        var result = new List<ControllerViewModel>();
                        service.EndFindControllers(lAsyncResult).ForEach(p => result.Add(new ControllerViewModel(p, provider)));
                        ViewModel = result;
                        Dispatcher.Invoke(DispatcherPriority.Normal, (Action)StartStatusTimer);
                    }
                    catch (XmlRpcFaultException)
                    {

                    }
                }, null);
        }

        private void ItemExpanded(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            var populate = item?.DataContext as IPopulation;
            if (populate != null && populate.NeedsToByPopulated())
                populate.Populate();
        }

        private List<ControllerViewModel> _viewModel;

        public List<ControllerViewModel> ViewModel
        {
            get => _viewModel;
            private set
            {
                _viewModel = value;
                RaisePropertyChanged();
            }
        }

        private int _pulseDelay = 10;

        public int PulseDelay
        {
            get => _pulseDelay;
            set
            {
                _pulseDelay = value;
                RaisePropertyChanged();
            }
        }

        private int _repeat = 1;

        public int Repeat
        {
            get => _repeat;
            set
            {
                _repeat = value;
                RaisePropertyChanged();
            }
        }

        private bool _randomize = true;

        public bool Randomize
        {
            get => _randomize;
            set
            {
                _randomize = value;
                RaisePropertyChanged();
            }
        }

        private void Items<T>(IEnumerable items, ICollection<T> visibleItems) where T : class
        {
            foreach (var i in items)
            {
                var container = HardwareTreeView.ContainerFromItem(i);
                if (container.DataContext != null && !(container.DataContext is DummyLoadingObject))
                {
                    if (container.DataContext is T)
                        visibleItems.Add((T)container.DataContext);
                    if (container.HasItems && container.IsExpanded)
                        Items(container.Items, visibleItems);
                }
            }
        }

        private List<T> Items<T>() where T : class
        {
            var result = new List<T>();
            Items(HardwareTreeView.Items, result);
            return result;
        }

        private void VisibleItems<T>(IEnumerable items, ICollection<T> visibleItems, ScrollViewer scrollViewer) where T : class
        {
            foreach(var i in items)
            {
                var container = HardwareTreeView.ContainerFromItem(i);
                if (ViewportHelper.IsInViewport(container, scrollViewer) && container.DataContext != null && !(container.DataContext is DummyLoadingObject))
                {
                    if (container.DataContext is T)
                        visibleItems.Add((T) container.DataContext);
                    if (container.HasItems && container.IsExpanded)
                        VisibleItems(container.Items, visibleItems, scrollViewer);
                }
            }
        }

        /// <summary>
        /// We only want to poll for hardware status for items that are actually "viewable" 
        /// no sense getting status and updating that status if the item isn't even going to be
        /// visible to reflect the status
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> GetVisibleItems<T>() where T : class
        {
            var result = new List<T>();
            VisibleItems(HardwareTreeView.Items, result, VisualTreeHelper.GetVisualChild<ScrollViewer, ItemsControl>(HardwareTreeView));
            return result;
        }

        #region Status Handling Routines

        private DateTime _lastUpdate;
        private DispatcherTimer _statusDispatcherTimer;
        private bool _pollActive;

        private void StopPolling()
        {
            _pollActive = false;
            if (_statusDispatcherTimer != null)
            {
                _statusDispatcherTimer.Stop();
                _statusDispatcherTimer.Tick -= StatusDispatcherTimerTick;
            }
        }

        private void UpdateStatus(IEnumerable<DNAStatus> status, List<IDNAStatus> visibleItems)
        {
            foreach (var item in status)
            {
                var dnaStatus = visibleItems.First(d => d.PackedAddress == item.Address);
                if (dnaStatus != null)
                {
                    dnaStatus.Status = item;
                    if (item.LastUpdate > _lastUpdate)
                        _lastUpdate = item.LastUpdate;
                }
            }
        }

        private void StartStatusTimer()
        {
            _pollActive = true;
            _statusDispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _statusDispatcherTimer.Tick += StatusDispatcherTimerTick;
            _statusDispatcherTimer.Start();
        }

        private void StatusDispatcherTimerTick(object sender, EventArgs e)
        {
            var visibleItems = GetVisibleItems<IDNAStatus>();
            if (visibleItems.Count == 0)
            {
                if (_pollActive)
                    _statusDispatcherTimer.Start();
                return;
            }

            var hardware = visibleItems.Select(
                p => Convert.ToInt64(p.PackedAddress))
                .ToArray();

            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_HardwareStatus_Async>(provider.ServiceUrl);
            service.BeginRetrieveStatus(provider.ApiKey, hardware,
                lAsyncResult =>
                {
                    try
                    {
                        var status = service.EndRetrieveStatus(lAsyncResult);
                        Dispatcher.Invoke(DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                UpdateStatus(status, visibleItems);
                                if (_pollActive)
                                    _statusDispatcherTimer.Start();
                            }));
                    }
                    catch (Exception)
                    {
                    }
                }, null);
        }

        #endregion

        #region Event Generation

        private DispatcherTimer _eventsDispatcherTimer;

        private bool _eventsActive;

        public bool EventsActive
        {
            get => _eventsActive;
            set
            {
                _eventsActive = value;
                RaisePropertyChanged();
            }
        }

        public void StartGeneratingEvents()
        {
            EventsActive = true;
            _eventsDispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(PulseDelay) };
            _eventsDispatcherTimer.Tick += EventsDispatcherTimerTick;
            _eventsDispatcherTimer.Start();
        }

        private void StopGeneratingEvents()
        {
            EventsActive = false;
            if (_eventsDispatcherTimer != null)
            {
                _eventsDispatcherTimer.Stop();
                _eventsDispatcherTimer.Tick -= EventsDispatcherTimerTick;
            }
        }

        private void EventsDispatcherTimerTick(object sender, EventArgs e)
        {
            var events = Items<IEventGenerator>()
                .Where(p => p.Events != null && !p.IsChildOfDoor)
                .SelectMany(p => p.Events)
                .Where(p => p.IsSelected && p.CreateEvent != null)
                .Select(p => p.CreateSendEvent())
                .ToList();

            if (!events.Any())
            {
                if (EventsActive)
                    _eventsDispatcherTimer.Start();
                return;
            }

            if (Randomize)
                events.Shuffle();

            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            for (var i = 0; i < Repeat; i++)
            {
                if (!EventsActive)
                    return;

                foreach (var item in events)
                {
                    if (!EventsActive)
                        return;

                    service.BeginSendEvent(provider.ApiKey, item, lAsyncResult =>
                    {
                        try
                        {
                            service.EndSendEvent(lAsyncResult);
                        }
                        catch (Exception)
                        {
                        }
                    }, null);
                }
            }

            if (EventsActive)
                _eventsDispatcherTimer.Start();
        }

        private DelegateCommand _startEventGenerationCommand;

        public DelegateCommand StartEventGenerationCommand
        {
            get
            {
                if (_startEventGenerationCommand == null)
                {
                    _startEventGenerationCommand = new DelegateCommand(
                        p => StartGeneratingEvents(),
                        p => !EventsActive).ListenOn(this, p => p.EventsActive);
                }
                return _startEventGenerationCommand;
            }
        }

        private DelegateCommand _stopEventGenerationCommand;

        public DelegateCommand StopEventGenerationCommand
        {
            get
            {
                if (_stopEventGenerationCommand == null)
                {
                    _stopEventGenerationCommand = new DelegateCommand(
                        p => StopGeneratingEvents(),
                        p => EventsActive).ListenOn(this, p => p.EventsActive);
                }
                return _stopEventGenerationCommand;
            }
        }

        #endregion

        #region INotifyPropertyChanged, INotifyPropertyChanging

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
