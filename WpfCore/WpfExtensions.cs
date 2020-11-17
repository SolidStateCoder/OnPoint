using OnPoint.Universal;
using System.Windows;

namespace OnPoint.WpfCore
{
    /// <summary>
    /// Contains helper methods to act on Wpf classes.
    /// </summary>
    public static class WpfExtensions
    {
        /// <summary>
        /// Flip the <see cref="Visibility"/>.
        /// </summary>
        /// <param name="visibility">The current status</param>
        /// <param name="useHidden">Whether to use <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Collapsed"/></param>
        /// <returns>The new <see cref="Visibility"/> status</returns>
        public static Visibility Invert(this Visibility visibility, bool useHidden = false) => visibility == Visibility.Visible ? (useHidden ? Visibility.Hidden : Visibility.Collapsed) : Visibility.Visible;


        /// <summary>
        /// Convert <see cref="WindowState"/> to <see cref="AppDisplayState"/>.
        /// </summary>
        /// <param name="windowState"><see cref="WindowState"/></param>
        /// <returns><see cref="AppDisplayState"/></returns>
        public static AppDisplayState ToAppDisplayState(this WindowState windowState) =>
            windowState switch
            {
                WindowState.Normal => AppDisplayState.Normal,
                WindowState.Maximized => AppDisplayState.Maximized,
                WindowState.Minimized => AppDisplayState.Minimized,
                _ => AppDisplayState.Normal
            };


        /// <summary>
        /// Convert <see cref="AppDisplayState"/> to <see cref="WindowState"/>.
        /// </summary>
        /// <param name="appDisplayState"><see cref="AppDisplayState"/></param>
        /// <returns><see cref="WindowState"/></returns>
        public static WindowState ToWindowState(this AppDisplayState appDisplayState) =>
            appDisplayState switch
            {
                AppDisplayState.Normal => WindowState.Normal,
                AppDisplayState.Maximized => WindowState.Maximized,
                AppDisplayState.Minimized => WindowState.Minimized,
                _ => WindowState.Normal
            };
    }
}