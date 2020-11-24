using MahApps.Metro.IconPacks;
using NodaTime;
using OnPoint.Universal;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace OnPoint.WpfCore
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanNegatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool ? !((bool)value) : false;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is bool ? !((bool)value) : false;
    }


    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value is bool) && ((bool)value) ? parameter as Brush : null;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }


    [ValueConversion(typeof(string), typeof(string))]
    public class CapitalLetterSplitConverter : IValueConverter
    {
        public string Convert(string value) => Convert(value, typeof(string), null, CultureInfo.CurrentCulture) as string;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string retVal = string.Empty;
            if (value != null)
            {
                string stringVal = value.ToString();
                bool isAllUpper = true;
                foreach (Char c in stringVal)
                {
                    if (Char.IsLower(c))
                    {
                        isAllUpper = false;
                        break;
                    }
                }
                if (isAllUpper)
                {
                    retVal = stringVal;
                }
                else
                {
                    retVal = value?.ToString()?.SplitAtCapitals();
                }
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string retVal = string.Empty;
            if (value is string)
            {
                retVal = ((string)value).Replace(" ", String.Empty);
            }
            return retVal;
        }
    }


    [ValueConversion(typeof(IconFlipOrientation), typeof(PackIconFlipOrientation))]
    public class MahIconFlipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PackIconFlipOrientation retVal = PackIconFlipOrientation.Normal;
            if (value is IconFlipOrientation flipOrientation)
            {
                retVal = flipOrientation switch
                {
                    IconFlipOrientation.Horizontal => PackIconFlipOrientation.Horizontal,
                    IconFlipOrientation.Vertical => PackIconFlipOrientation.Vertical,
                    IconFlipOrientation.Both => PackIconFlipOrientation.Both,
                    _ => PackIconFlipOrientation.Normal
                };
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IconFlipOrientation retVal = IconFlipOrientation.Normal;
            if (value is PackIconFlipOrientation flipOrientation)
            {
                retVal = flipOrientation switch
                {
                    PackIconFlipOrientation.Horizontal => IconFlipOrientation.Horizontal,
                    PackIconFlipOrientation.Vertical => IconFlipOrientation.Vertical,
                    PackIconFlipOrientation.Both => IconFlipOrientation.Both,
                    PackIconFlipOrientation.Normal => IconFlipOrientation.Normal,
                    _ => IconFlipOrientation.Normal
                };
            }
            return retVal;
        }
    }


    public class MahIconToImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object value = values[0];

            Brush brush = Brushes.Black;
            if (values.Length > 1 && values[1] is Brush)
            {
                brush = (Brush)values[1];
            }
            GeometryDrawing geoDrawing = new GeometryDrawing()
            {
                Brush = brush,
                Pen = new Pen(brush, .25)
            };

            if (value is PackIconBootstrapIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconBootstrapIcons() { Kind = (PackIconBootstrapIconsKind)value }.Data);
            }
            else if (value is PackIconBoxIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconBoxIcons() { Kind = (PackIconBoxIconsKind)value }.Data);
            }
            else if (value is PackIconEntypoKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconEntypo() { Kind = (PackIconEntypoKind)value }.Data);
            }
            else if (value is PackIconEvaIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconEvaIcons() { Kind = (PackIconEvaIconsKind)value }.Data);
            }
            else if (value is PackIconFeatherIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconFeatherIcons() { Kind = (PackIconFeatherIconsKind)value }.Data);
            }
            else if (value is PackIconFontAwesomeKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconFontAwesome() { Kind = (PackIconFontAwesomeKind)value }.Data);
            }
            else if (value is PackIconIoniconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconIonicons() { Kind = (PackIconIoniconsKind)value }.Data);
            }
            else if (value is PackIconJamIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconJamIcons() { Kind = (PackIconJamIconsKind)value }.Data);
            }
            else if (value is PackIconMaterialKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconMaterial() { Kind = (PackIconMaterialKind)value }.Data);
            }
            else if (value is PackIconMaterialDesignKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconMaterialDesign() { Kind = (PackIconMaterialDesignKind)value }.Data);
            }
            else if (value is PackIconMaterialLightKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconMaterialLight() { Kind = (PackIconMaterialLightKind)value }.Data);
            }
            else if (value is PackIconMicronsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconMicrons() { Kind = (PackIconMicronsKind)value }.Data);
            }
            else if (value is PackIconModernKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconModern() { Kind = (PackIconModernKind)value }.Data);
            }
            else if (value is PackIconOcticonsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconOcticons() { Kind = (PackIconOcticonsKind)value }.Data);
            }
            else if (value is PackIconPicolIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconPicolIcons() { Kind = (PackIconPicolIconsKind)value }.Data);
            }
            else if (value is PackIconRPGAwesomeKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconRPGAwesome() { Kind = (PackIconRPGAwesomeKind)value }.Data);
            }
            else if (value is PackIconSimpleIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconSimpleIcons() { Kind = (PackIconSimpleIconsKind)value }.Data);
            }
            else if (value is PackIconTypiconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconTypicons() { Kind = (PackIconTypiconsKind)value }.Data);
            }
            else if (value is PackIconUniconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconUnicons() { Kind = (PackIconUniconsKind)value }.Data);
            }
            else if (value is PackIconWeatherIconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconWeatherIcons() { Kind = (PackIconWeatherIconsKind)value }.Data);
            }
            else if (value is PackIconZondiconsKind)
            {
                geoDrawing.Geometry = Geometry.Parse(new PackIconZondicons() { Kind = (PackIconZondiconsKind)value }.Data);
            }

            return new DrawingImage { Drawing = new DrawingGroup { Children = { geoDrawing } } };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }


    [ValueConversion(typeof(object), typeof(DrawingImage))]
    public class MahIconToJustImageConverter : IValueConverter
    {
        private MahIconToImageConverter _Converter = new MahIconToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => _Converter.Convert(new object[] { value }, targetType, parameter, culture);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }


    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility retVal = Visibility.Visible;
            if (value == null)
            {
                retVal = Visibility.Collapsed;
            }
            //
            if (parameter != null && parameter.ToString().Equals("1"))
            {
                retVal = (retVal == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed);
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }


    [ValueConversion(typeof(long), typeof(DateTime))]
    public class TicksToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? retVal = null;
            long input = 0;
            if (Int64.TryParse(value?.ToString(), out input))
            {
                retVal = Instant.FromUnixTimeTicks(input * 10000).ToDateTimeUtc().ToLocalTime();
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long? retVal = null;
            DateTime? input = value as DateTime?;
            if (input.HasValue)
            {
                retVal = Instant.FromDateTimeUtc(input.Value.ToUniversalTime()).ToUnixTimeTicks() / 10000;
            }
            return retVal;
        }
    }


    [ValueConversion(typeof(TimeSpan), typeof(DateTime))]
    public class TimeSpanToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? retVal = null;
            TimeSpan? timeSpan = value as TimeSpan?;
            if (timeSpan == null && value is TimeSpan)
            {
                timeSpan = (TimeSpan)value;
            }
            //
            if (timeSpan != null)
            {
                retVal = new DateTime() + timeSpan;
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan? retVal = null;
            DateTime? input = value as DateTime?;
            if (input.HasValue)
            {
                retVal = new TimeSpan(0, input.Value.Hour, input.Value.Minute, input.Value.Second, input.Value.Millisecond);
            }
            return retVal;
        }
    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        public Visibility FalseValue { get; set; } = Visibility.Hidden;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility retVal = FalseValue;
            if (value != null && value is bool && ((bool)value))
            {
                retVal = Visibility.Visible;
            }
            //
            if (parameter != null && parameter.ToString().Equals("1"))
            {
                if (retVal == Visibility.Visible)
                {
                    retVal = FalseValue;
                }
                else
                {
                    retVal = Visibility.Visible;
                }
            }

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool retVal = false;
            if (value != null && value is Visibility && ((Visibility)value) == Visibility.Visible)
            {
                retVal = true;
            }
            //
            if (parameter != null && parameter.ToString().Equals("1"))
            {
                retVal = !retVal;
            }
            return retVal;
        }
    }
}