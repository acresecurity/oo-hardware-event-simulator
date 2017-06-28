using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace HardwareEvents
{
    // Taken from http://forums.silverlight.net/t/65277.aspx/1

    public static class TreeViewExtensions
    {
        public static TreeViewItem ContainerFromItem(this TreeView treeView, object item)
        {
            var containerThatMightContainItem = (TreeViewItem)treeView.ItemContainerGenerator.ContainerFromItem(item);
            if (containerThatMightContainItem != null)
                return containerThatMightContainItem;
            return ContainerFromItem(treeView.ItemContainerGenerator, treeView.Items, item);
        }

        private static TreeViewItem ContainerFromItem(ItemContainerGenerator parentItemContainerGenerator, IEnumerable itemCollection, object item)
        {
            foreach (var curChildItem in itemCollection)
            {
                if (curChildItem == null)
                    continue;

                var parentContainer = (TreeViewItem)parentItemContainerGenerator.ContainerFromItem(curChildItem);
                if (parentContainer != null)
                {
                    var containerThatMightContainItem = (TreeViewItem)parentContainer.ItemContainerGenerator.ContainerFromItem(item);
                    if (containerThatMightContainItem != null)
                        return containerThatMightContainItem;
                    var recursionResult = ContainerFromItem(parentContainer.ItemContainerGenerator, parentContainer.Items, item);
                    if (recursionResult != null)
                        return recursionResult;
                }
            }
            return null;
        }

        public static object ItemFromContainer(this TreeView treeView, TreeViewItem container)
        {
            var itemThatMightBelongToContainer = (TreeViewItem)treeView.ItemContainerGenerator.ItemFromContainer(container);
            if (itemThatMightBelongToContainer != null)
                return itemThatMightBelongToContainer;
            return ItemFromContainer(treeView.ItemContainerGenerator, treeView.Items, container);
        }

        private static object ItemFromContainer(ItemContainerGenerator parentItemContainerGenerator, IEnumerable itemCollection, DependencyObject container)
        {
            foreach (var curChildItem in itemCollection)
            {
                var parentContainer = (TreeViewItem)parentItemContainerGenerator.ContainerFromItem(curChildItem);
                var itemThatMightBelongToContainer = (TreeViewItem)parentContainer.ItemContainerGenerator.ItemFromContainer(container);
                if (itemThatMightBelongToContainer != null)
                    return itemThatMightBelongToContainer;
                var recursionResult = ItemFromContainer(parentContainer.ItemContainerGenerator, parentContainer.Items, container) as TreeViewItem;
                if (recursionResult != null)
                    return recursionResult;
            }
            return null;
        }
    }
}
