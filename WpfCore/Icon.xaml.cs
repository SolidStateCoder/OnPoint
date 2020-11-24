using OnPoint.Universal;
using OnPoint.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace OnPoint.WpfCore
{
    public partial class Icon : UserControl
    {
        public Icon()
        {
            Loaded += Icon_Loaded;
            InitializeComponent();
        }

        private void Icon_Loaded(object sender, RoutedEventArgs args)
        {
            if (icon1.DataContext is IconDetails)
            {
                SetIconPosition(icon1, (IconDetails)icon1.DataContext);
            }
        }

        private void SetIconPosition(ContentControl contentControl, IconDetails iconDetails)
        {
            switch (iconDetails.GridPosition)
            {
                case GridPosition.TopLeft:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Left;
                    contentControl.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case GridPosition.TopCenter:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Center;
                    contentControl.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case GridPosition.TopRight:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Right;
                    contentControl.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case GridPosition.MidLeft:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Left;
                    contentControl.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case GridPosition.MidCenter:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Center;
                    contentControl.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case GridPosition.MidRight:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Right;
                    contentControl.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case GridPosition.BottomLeft:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Left;
                    contentControl.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case GridPosition.BottomCenter:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Center;
                    contentControl.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case GridPosition.BottomRight:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Right;
                    contentControl.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case GridPosition.TopRow:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    contentControl.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case GridPosition.MiddleRow:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    contentControl.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case GridPosition.BottomRow:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    contentControl.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case GridPosition.LeftColumn:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Left;
                    contentControl.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case GridPosition.MiddleColumn:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Center;
                    contentControl.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case GridPosition.RightColumn:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Right;
                    contentControl.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case GridPosition.Fill:
                default:
                    contentControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    contentControl.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
            }
        }
    }
}