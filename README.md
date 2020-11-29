# OnPoint
![On Point Logo](/Images/OnPointLogoSmall.png)

**Enhances the Reactive UI MVVM framework with easy to use classes for the most common use cases.**   
[GitHub source](https://github.com/SolidStateProgrammer/OnPoint)  
**In a nutshell:**  
ContentVM<T> - Tracks changes to a single item. Useful for binding to a [ContentControl](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol?view=net-5.0).  
DualContentVM<T,U> - Adds a second tracked item to ContentVM<T>.  
MultiContentVM<T> - Tracks changes to a collection items. Useful for binding to an [ItemsControl](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.itemscontrol?view=net-5.0).  
MultiContentTrackedVM<T> - Adds the ability to track the changed status of each item in a MultiContentVM<T>.  
CrudVM<T> - Adds create, read, update, and delete capabilities to MultiContentTrackedVM<T>.  
CrudServiceVM<T> - Routes all CRUD function to the given service. Just provide an instance of ICrudService and everything is handled for you!  
Icons can be added in view model code and are automatically rendered; see the example below.
  
**Every view model automatically tracks:**  
- Busy state  
- Cancelable commands  
- Visibility state  
- Enabled state  
- Command errors  
- Activation state  
- Error and HUD messages 

**See the test app for a complete example:**

![Test App](/Images/TestApp.jpg)

## Samples
1. Create a new WPF project in Visual Studio target .NET 5.  
2. Use NuGet to install Autofac, Reactive UI, Reactive UI WPF, OnPoint, and OnPoint WPF.  
3. Make the changes below.  
### Hello World
![Hello World](/Images/HelloWorld.jpg)  
MainWindow.xaml  

	<Window  
		x:Class="OnPointTest.MainWindow"  
    	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"  
    	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"  
    	Title="OnPoint Test" Height="450" Width="800"  
    	Content="{Binding Content}"
	/>

MainWindow.xaml.cs  

	using OnPoint.ViewModels;  
	using System.Windows;  
	namespace OnPointTest  
	{  
    	public partial class MainWindow : Window  
    	{  
        	public MainWindow()  
        	{  
            	InitializeComponent();  
            	DataContext = new ContentVM<string>("Hello World!");  
        	}  
    	}  
 	}
### List of Names
![List of Names](/Images/Names.jpg)  
MainWindow.xaml  

    <Window
        x:Class="OnPointTest.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="OnPoint Test" Height="300" Width="400">
        <DockPanel>
            <TextBlock DockPanel.Dock="Top" Text="{Binding Path=SelectedContent, StringFormat={}You selected: {0}}" />
            <ListBox ItemsSource="{Binding Path=Contents}" SelectedItem="{Binding Path=SelectedContent}" />
        </DockPanel>
    </Window>

MainWindow.xaml.cs  

    using OnPoint.ViewModels;
    using System.Windows;
    namespace OnPointTest
    {
        public partial class MainWindow : Window
        {
        	public MainWindow()
        	{
                InitializeComponent();
                DataContext = new MultiContentVM<string>("Ann","Bob","Charles");
        	}
        }
    } 

### Commands / Buttons
![List of Names](/Images/Buttons.jpg)  
MainWindow.xaml  

    <Window
            x:Class="OnPointTest.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            Title="OnPoint Test" Height="300" Width="400">
        <Window.Resources>
            <Style x:Key="BlankButton" TargetType="{x:Type ButtonBase}">
                <Setter Property="Command" Value="{Binding Command}" />
                <Setter Property="CommandParameter" Value="{Binding CommandParameter}" />
                <Setter Property="Content" Value="{Binding CommandText}"/>
                <Setter Property="ToolTip" Value="{Binding ToolTip}" />
                <Setter Property="Width" Value="{Binding Width}" />
                <Setter Property="Height" Value="{Binding Height}" />
                <Setter Property="Template">
                    <Setter.Value>
                        <ControlTemplate TargetType="ButtonBase">
                            <Border Background="{TemplateBinding Background}" CornerRadius="6" BorderThickness="1" BorderBrush="DimGray">
                                <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center" />
                            </Border>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
            </Style>
        </Window.Resources>

        <DockPanel>
            <TextBlock DockPanel.Dock="Top" Text="{Binding Title}" Margin="2" />
            <ItemsControl ItemsSource="{Binding Contents}" VerticalAlignment="Top">
                <ItemsControl.ItemsPanel>
                    <ItemsPanelTemplate>
                        <StackPanel IsItemsHost="True" Orientation="Horizontal" />
                    </ItemsPanelTemplate>
                </ItemsControl.ItemsPanel>
                <ItemsControl.ItemTemplate>
                    <DataTemplate>
                        <Button Style="{StaticResource BlankButton}" Margin="2" />
                    </DataTemplate>
                </ItemsControl.ItemTemplate>
            </ItemsControl>
        </DockPanel>
    </Window>

MainWindow.xaml.cs  

    using OnPoint.ViewModels;
    using ReactiveUI;
    using System.Windows;
    namespace OnPointTest
    {
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
                MultiContentVM<CommandVM> dataContext = new MultiContentVM<CommandVM>();
                var command = ReactiveCommand.Create((int x) => dataContext.Title = $"You clicked button #{x}");
                dataContext.AddContent(new CommandVM(command, 100, 25, "Refresh", null, "Refresh Tool Tip", 1));
                dataContext.AddContent(new CommandVM(command, 100, 25, "Add", null, "Add Tool Tip", 2));
                dataContext.AddContent(new CommandVM(command, 100, 25, "Delete", null, "Delete Tool Tip", 3));
                DataContext = dataContext;
            }
        }
    }
### Built in icons/images ([MahApps](https://github.com/MahApps/MahApps.Metro))
![Hello World](/Images/Icons.jpg)  
MainWindow.xaml  

    <Window
        x:Class="OnPointTest.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:OnPointWpf="clr-namespace:OnPoint.WpfDotNet5;assembly=OnPoint.WpfDotNet5" 
        Title="OnPoint Wpf Test" Height="300" Width="400">
        <DockPanel>
            <TextBlock DockPanel.Dock="Bottom" Text="https://github.com/MahApps/MahApps.Metro.IconPacks/wiki/IconPacks-Browser" Margin="4" />
            <ItemsControl ItemsSource="{Binding Contents}" Margin="4">
                <ItemsControl.ItemsPanel>
                    <ItemsPanelTemplate>
                        <WrapPanel IsItemsHost="True" />
                    </ItemsPanelTemplate>
                </ItemsControl.ItemsPanel>
                <ItemsControl.ItemTemplate>
                    <DataTemplate>
                        <OnPointWpf:Icon Margin="10" />
                    </DataTemplate>
                </ItemsControl.ItemTemplate>
            </ItemsControl>
        </DockPanel>
    </Window>

MainWindow.xaml.cs  

    using MahApps.Metro.IconPacks;
    using OnPoint.Universal;
    using OnPoint.ViewModels;
    using OnPoint.WpfDotNet5;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    namespace OnPointTest
    {
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
                MultiContentVM<IconVM> dataContext = new MultiContentVM<IconVM>();
                dataContext.AddContent(new IconVM(new BootstrapIconsIconDetails(PackIconBootstrapIconsKind.Alarm,
                    SCB(Colors.Red), 32, 32)));
                dataContext.AddContent(new IconVM(new BoxIconsIconDetails(PackIconBoxIconsKind.LogosBitcoin,
                    SCB(Colors.DarkGoldenrod), 20, 20, GridPosition.BottomLeft)));
                dataContext.AddContent(new IconVM(new PackIconEntypoDetails(PackIconEntypoKind.Cake,
                    SCB(Colors.Purple), 32, 32, flip: IconFlipOrientation.Vertical)));
                dataContext.AddContent(new IconVM(new EvaIconDetails(PackIconEvaIconsKind.Linkedin,
                    SCB(Colors.Black), 32, 32, flip: IconFlipOrientation.Horizontal)));
                dataContext.AddContent(new IconVM(new FeatherIconDetails(PackIconFeatherIconsKind.CloudLightning,
                    SCB(Colors.DarkBlue), 32, 32, flip: IconFlipOrientation.Both)));
                dataContext.AddContent(new IconVM(new FontAwesomeIconDetails(PackIconFontAwesomeKind.DownloadSolid,
                    SCB(Colors.DarkCyan), 32, 32, rotation: 45)));
                dataContext.AddContent(new IconVM(new IonIconDetails(PackIconIoniconsKind.EaseliOS,
                    SCB(Colors.DarkGray), 32, 32, rotation: 90)));
                dataContext.AddContent(new IconVM(new JamIconDetails(PackIconJamIconsKind.FastForward,
                    SCB(Colors.DarkRed), 32, 32, spin: true, spinDuration: 5)));
                dataContext.AddContent(new IconVM(new MaterialIconDetails(PackIconMaterialKind.Gamepad,
                    SCB(Colors.DarkGreen), 32, 32, spin: true, spinDuration: 5, spinAutoReverse: true)));
                BounceEase bounceEase = new BounceEase() { Bounces = 5, Bounciness = 4, EasingMode = EasingMode.EaseOut };
                dataContext.AddContent(new IconVM(new MaterialDesignIconDetails(PackIconMaterialDesignKind.HeadsetMic,
                    SCB(Colors.DarkKhaki), 32, 32, spin: true, spinDuration: 10, spinEasingFunction: bounceEase)));
                dataContext.AddContent(new IconVM(new MaterialLightIconDetails(PackIconMaterialLightKind.Inbox,
                    SCB(Colors.DarkMagenta), 32, 32)));
                dataContext.AddContent(new IconVM(new MicronsIconDetails(PackIconMicronsKind.BarChart,
                    SCB(Colors.DarkOliveGreen), 32, 32)));
                dataContext.AddContent(new IconVM(new ModernIconDetails(PackIconModernKind.Journal,
                    SCB(Colors.DarkOrange), 32, 32)));
                dataContext.AddContent(new IconVM(new OcticonsIconDetails(PackIconOcticonsKind.Key,
                    SCB(Colors.DarkRed), 32, 32)));
                dataContext.AddContent(new IconVM(new PicolIconDetails(PackIconPicolIconsKind.Label,
                    SCB(Colors.DarkSalmon), 32, 32)));
                dataContext.AddContent(new IconVM(new RPGAwesomeIconDetails(PackIconRPGAwesomeKind.Magnet,
                    SCB(Colors.DarkSeaGreen), 32, 32)));
                dataContext.AddContent(new IconVM(new SimpleIconDetails(PackIconSimpleIconsKind.Neo4j,
                    SCB(Colors.DarkSlateBlue), 32, 32)));
                dataContext.AddContent(new IconVM(new TypIconDetails(PackIconTypiconsKind.FolderOpen,
                    SCB(Colors.DarkSlateGray), 32, 32)));
                dataContext.AddContent(new IconVM(new UniIconDetails(PackIconUniconsKind.Package,
                    SCB(Colors.DarkTurquoise), 32, 32)));
                dataContext.AddContent(new IconVM(new WeatherIconDetails(PackIconWeatherIconsKind.Rain,
                    SCB(Colors.DarkViolet), 32, 32)));
                dataContext.AddContent(new IconVM(new ZondIconDetails(PackIconZondiconsKind.Airplane,
                    SCB(Colors.OliveDrab), 32, 32)));

                DataContext = dataContext;
            }

            private static SolidColorBrush SCB(Color color) => new SolidColorBrush(color);
        }
    }

App.xaml  

    <Application
        x:Class="OnPointTest.App"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        StartupUri="MainWindow.xaml">
        <Application.Resources>
            <ResourceDictionary>
                <ResourceDictionary.MergedDictionaries>
                    <ResourceDictionary Source="pack://application:,,,/OnPoint.WpfDotNet5;component/MahAppsIconTemplates.xaml" />
                </ResourceDictionary.MergedDictionaries>
            </ResourceDictionary>
        </Application.Resources>
    </Application>