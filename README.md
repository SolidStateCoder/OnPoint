## OnPoint
![On Point Logo](/Images/OnPointLogoSmall.png)

Enhances the Reactive UI MVVM framework with easy to use classes for the most common use cases.  
In a nutshell:  
ContentVM<T> - Tracks changes to a single item. Useful for binding to a [ContentControl](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol?view=net-5.0).  
DualContentVM<T,U> - Adds a second tracked item to ContentVM<T>.  
MultiContentVM<T> - Tracks changes to a collection items. Useful for binding to an [ItemsControl](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.itemscontrol?view=net-5.0).  
MultiContentTrackedVM<T> - Adds the ability to track the changed status of each item in a MultiContentVM<T>.  
CrudVM<T> - Adds create, read, update, and delete capabilities to MultiContentTrackedVM<T>.  
CrudServiceVM<T> - Routes all CRUD function to the given service. Just provide an instance of ICrudService and everything is handled for you!.  

![Test App](/Images/TestApp.jpg)

### Samples
1. Create a new WPF project in Visual Studio target .NET 5.  
2. Use NuGet to install Autofac, Reactive UI, and OnPoint.  
3. Make the changes below.  
#### Hello World
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
#### List of Names
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