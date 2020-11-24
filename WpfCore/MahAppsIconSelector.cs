using MahApps.Metro.IconPacks;
using OnPoint.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OnPoint.WpfCore
{
    public class MahAppsIconSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate retVal = null;
            if (item is IconDetailsPath)
            {
                retVal = ((FrameworkElement)container).FindResource("IconPath") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconBootstrapIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconBootstrapIcons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconBoxIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconBoxIcons") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconEntypoKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconEntypo") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconEvaIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconEvaIcons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconFeatherIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconFeatherIcons") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconFontAwesomeKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconFontAwesome") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconIoniconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconIonicons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconJamIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconJamIcons") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconMaterialKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconMaterial") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconMaterialDesignKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconMaterialDesign") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconMaterialLightKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconMaterialLight") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconMicronsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconMicrons") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconModernKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconModern") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconOcticonsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconOcticons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconPicolIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconPicolIcons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconRPGAwesomeKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconRPGAwesome") as DataTemplate;
            }
            else if (item is MahIconDetails<PackIconSimpleIconsKind, SolidColorBrush>)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconSimpleIcons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconTypiconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconTypicons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconUniconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconUnicons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconWeatherIconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconWeatherIcons") as DataTemplate;
            }
            else if (item is MahIconDetails <PackIconZondiconsKind, SolidColorBrush >)
            {
                retVal = ((FrameworkElement)container).FindResource("PackIconZondicons") as DataTemplate;
            }
            return retVal;
        }
    }
}