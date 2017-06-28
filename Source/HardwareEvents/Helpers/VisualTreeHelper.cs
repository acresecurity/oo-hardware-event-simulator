using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HardwareEvents
{
    public static class VisualTreeHelper
    {
        private static void GetVisualChildren<T>(DependencyObject current, Collection<T> children) where T : DependencyObject
        {
            if (current == null)
                return;

            if (current.GetType() == typeof(T))
                children.Add((T)current);

            for (var i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(current); i++)
                GetVisualChildren(System.Windows.Media.VisualTreeHelper.GetChild(current, i), children);
        }

        public static Collection<T> GetVisualChildren<T>(DependencyObject current) where T : DependencyObject
        {
            if (current == null)
                return null;

            var children = new Collection<T>();

            GetVisualChildren(current, children);

            return children;
        }

        public static T GetVisualChild<T, P>(P templatedParent)
            where T : FrameworkElement
            where P : FrameworkElement
        {
            if (templatedParent == null)
                return null;

            var children = GetVisualChildren<T>(templatedParent);

            return children.FirstOrDefault(child => Equals(child.TemplatedParent, templatedParent));
        }
    }
}
