﻿using MahApps.Metro.IconPacks;
using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;
using System.Windows.Media;

namespace OnPoint.WpfDotNet5
{
    public class MahIconDetails<TIcon, TForeground> : IconDetails where TForeground : class
    {
        public TIcon Icon { get => _Icon; set => this.RaiseAndSetIfChanged(ref _Icon, value); }
        private TIcon _Icon = default;

        public TIcon LargeIcon { get => _LargeIcon; set => this.RaiseAndSetIfChanged(ref _LargeIcon, value); }
        private TIcon _LargeIcon = default;

        public TForeground Foreground { get => _Foreground; set => this.RaiseAndSetIfChanged(ref _Foreground, value); }
        private TForeground _Foreground = null;

        public TForeground Background { get => _Background; set => this.RaiseAndSetIfChanged(ref _Background, value); }
        private TForeground _Background = null;

        public MahIconDetails() { }

        public MahIconDetails(TIcon icon, TForeground foreground, double width = 16, double height = 16, GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip)
        {
            Icon = icon;
            LargeIcon = icon;
            Foreground = foreground;
        }
    }


    public class MahIconDetailsSCB<TIcon> : MahIconDetails<TIcon, SolidColorBrush>
    {
        private static SolidColorBrush _Transparent = new SolidColorBrush(Colors.Transparent);

        public MahIconDetailsSCB()
        {
            Background = _Transparent;
        }

        public MahIconDetailsSCB(TIcon icon, SolidColorBrush foreground, double width = 16, double height = 16, GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, foreground, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip)
        {
            Background = _Transparent;
        }
    }


    public class BootstrapIconsIconDetails : MahIconDetailsSCB<PackIconBootstrapIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public BootstrapIconsIconDetails() { }
        public BootstrapIconsIconDetails(PackIconBootstrapIconsKind icon) : base(icon, _Black) { }
        public BootstrapIconsIconDetails(PackIconBootstrapIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public BootstrapIconsIconDetails(PackIconBootstrapIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class BoxIconsIconDetails : MahIconDetailsSCB<PackIconBoxIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public BoxIconsIconDetails() { }
        public BoxIconsIconDetails(PackIconBoxIconsKind icon) : base(icon, _Black) { }
        public BoxIconsIconDetails(PackIconBoxIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public BoxIconsIconDetails(PackIconBoxIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class PackIconEntypoDetails : MahIconDetailsSCB<PackIconEntypoKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public PackIconEntypoDetails() { }
        public PackIconEntypoDetails(PackIconEntypoKind icon) : base(icon, _Black) { }
        public PackIconEntypoDetails(PackIconEntypoKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public PackIconEntypoDetails(PackIconEntypoKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class EvaIconDetails : MahIconDetailsSCB<PackIconEvaIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public EvaIconDetails() { }
        public EvaIconDetails(PackIconEvaIconsKind icon) : base(icon, _Black) { }
        public EvaIconDetails(PackIconEvaIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public EvaIconDetails(PackIconEvaIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class FeatherIconDetails : MahIconDetailsSCB<PackIconFeatherIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public FeatherIconDetails() { }
        public FeatherIconDetails(PackIconFeatherIconsKind icon) : base(icon, _Black) { }
        public FeatherIconDetails(PackIconFeatherIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public FeatherIconDetails(PackIconFeatherIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class FontAwesomeIconDetails : MahIconDetailsSCB<PackIconFontAwesomeKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public FontAwesomeIconDetails() { }
        public FontAwesomeIconDetails(PackIconFontAwesomeKind icon) : base(icon, _Black) { }
        public FontAwesomeIconDetails(PackIconFontAwesomeKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public FontAwesomeIconDetails(PackIconFontAwesomeKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class IonIconDetails : MahIconDetailsSCB<PackIconIoniconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public IonIconDetails() { }
        public IonIconDetails(PackIconIoniconsKind icon) : base(icon, _Black) { }
        public IonIconDetails(PackIconIoniconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public IonIconDetails(PackIconIoniconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class JamIconDetails : MahIconDetailsSCB<PackIconJamIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public JamIconDetails() { }
        public JamIconDetails(PackIconJamIconsKind icon) : base(icon, _Black) { }
        public JamIconDetails(PackIconJamIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public JamIconDetails(PackIconJamIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class MaterialIconDetails : MahIconDetailsSCB<PackIconMaterialKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public MaterialIconDetails() { }
        public MaterialIconDetails(PackIconMaterialKind icon) : base(icon, _Black) { }
        public MaterialIconDetails(PackIconMaterialKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public MaterialIconDetails(PackIconMaterialKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class MaterialDesignIconDetails : MahIconDetailsSCB<PackIconMaterialDesignKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public MaterialDesignIconDetails() { }
        public MaterialDesignIconDetails(PackIconMaterialDesignKind icon) : base(icon, _Black) { }
        public MaterialDesignIconDetails(PackIconMaterialDesignKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public MaterialDesignIconDetails(PackIconMaterialDesignKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class MaterialLightIconDetails : MahIconDetailsSCB<PackIconMaterialLightKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public MaterialLightIconDetails() { }
        public MaterialLightIconDetails(PackIconMaterialLightKind icon) : base(icon, _Black) { }
        public MaterialLightIconDetails(PackIconMaterialLightKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public MaterialLightIconDetails(PackIconMaterialLightKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class MicronsIconDetails : MahIconDetailsSCB<PackIconMicronsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public MicronsIconDetails() { }
        public MicronsIconDetails(PackIconMicronsKind icon) : base(icon, _Black) { }
        public MicronsIconDetails(PackIconMicronsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public MicronsIconDetails(PackIconMicronsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class ModernIconDetails : MahIconDetailsSCB<PackIconModernKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public ModernIconDetails() { }
        public ModernIconDetails(PackIconModernKind icon) : base(icon, _Black) { }
        public ModernIconDetails(PackIconModernKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public ModernIconDetails(PackIconModernKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class OcticonsIconDetails : MahIconDetailsSCB<PackIconOcticonsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public OcticonsIconDetails() { }
        public OcticonsIconDetails(PackIconOcticonsKind icon) : base(icon, _Black) { }
        public OcticonsIconDetails(PackIconOcticonsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public OcticonsIconDetails(PackIconOcticonsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class PicolIconDetails : MahIconDetailsSCB<PackIconPicolIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public PicolIconDetails() { }
        public PicolIconDetails(PackIconPicolIconsKind icon) : base(icon, _Black) { }
        public PicolIconDetails(PackIconPicolIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public PicolIconDetails(PackIconPicolIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class RPGAwesomeIconDetails : MahIconDetailsSCB<PackIconRPGAwesomeKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public RPGAwesomeIconDetails() { }
        public RPGAwesomeIconDetails(PackIconRPGAwesomeKind icon) : base(icon, _Black) { }
        public RPGAwesomeIconDetails(PackIconRPGAwesomeKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public RPGAwesomeIconDetails(PackIconRPGAwesomeKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class SimpleIconDetails : MahIconDetailsSCB<PackIconSimpleIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public SimpleIconDetails() { }
        public SimpleIconDetails(PackIconSimpleIconsKind icon) : base(icon, _Black) { }
        public SimpleIconDetails(PackIconSimpleIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public SimpleIconDetails(PackIconSimpleIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class TypIconDetails : MahIconDetailsSCB<PackIconTypiconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public TypIconDetails() { }
        public TypIconDetails(PackIconTypiconsKind icon) : base(icon, _Black) { }
        public TypIconDetails(PackIconTypiconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public TypIconDetails(PackIconTypiconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class UniIconDetails : MahIconDetailsSCB<PackIconUniconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public UniIconDetails() { }
        public UniIconDetails(PackIconUniconsKind icon) : base(icon, _Black) { }
        public UniIconDetails(PackIconUniconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public UniIconDetails(PackIconUniconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class WeatherIconDetails : MahIconDetailsSCB<PackIconWeatherIconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public WeatherIconDetails() { }
        public WeatherIconDetails(PackIconWeatherIconsKind icon) : base(icon, _Black) { }
        public WeatherIconDetails(PackIconWeatherIconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public WeatherIconDetails(PackIconWeatherIconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }


    public class ZondIconDetails : MahIconDetailsSCB<PackIconZondiconsKind>
    {
        private static SolidColorBrush _Black = new SolidColorBrush(Colors.Black);
        public ZondIconDetails() { }
        public ZondIconDetails(PackIconZondiconsKind icon) : base(icon, _Black) { }
        public ZondIconDetails(PackIconZondiconsKind icon, SolidColorBrush solidColorBrush) : base(icon, solidColorBrush) { }
        public ZondIconDetails(PackIconZondiconsKind icon, SolidColorBrush solidColorBrush, double width = 16, double height = 16,
            GridPosition gridPosition = GridPosition.Fill, IconFlipOrientation flip = IconFlipOrientation.None,
            double rotation = 0, bool spin = false, double spinDuration = 0, object spinEasingFunction = null, bool spinAutoReverse = false, string toolTip = default)
            : base(icon, solidColorBrush, width, height, gridPosition, flip, rotation, spin, spinDuration, spinEasingFunction, spinAutoReverse, toolTip) { }
    }
}