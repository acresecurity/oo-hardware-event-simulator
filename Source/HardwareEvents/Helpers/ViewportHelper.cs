using System.Reflection;
using System.Windows.Controls;

namespace HardwareEvents
{
    public static class ViewportHelper
    {
        public static bool IsInViewport(Control item, ScrollViewer scrollViewer)
        {
            if (item == null)
                return false;

            var scrollContentPresenter = (ScrollContentPresenter)scrollViewer.Template.FindName("PART_ScrollContentPresenter", scrollViewer);
            var isInViewportMethod = scrollViewer.GetType().GetMethod("IsInViewport", BindingFlags.NonPublic | BindingFlags.Instance);

            return (bool)isInViewportMethod.Invoke(scrollViewer, new object[] { scrollContentPresenter, item });
        }
    }
}
