using System.Windows;

namespace OnPoint.WpfCore
{
    public static class WpfExtensions
    {
        public static Visibility Invert(this Visibility visibility, bool useHidden = false) => visibility == Visibility.Visible ? (useHidden ? Visibility.Hidden : Visibility.Collapsed) : Visibility.Visible;
    }
}