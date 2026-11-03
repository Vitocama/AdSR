using System;
using System.Windows;
using System.Windows.Controls;

namespace AdSR.Common
{
    public class ViewSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            var vmType = item.GetType();
            var viewTypeName = vmType.FullName.Replace("ViewModel", "View");
            var viewType = vmType.Assembly.GetType(viewTypeName);

            if (viewType == null)
                throw new Exception($"View non trovata: attesa '{viewTypeName}' per {vmType.FullName}");

            return new DataTemplate(vmType)
            {
                VisualTree = new FrameworkElementFactory(viewType)
            };
        }
    }
}
