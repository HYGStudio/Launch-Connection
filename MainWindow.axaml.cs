using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace Launch_Connection.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void NavigationView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var selectedItem = (NavigationViewItem)e.SelectedItem;
        if ((string)selectedItem.Tag == "HomePage") NavigationView_Frame.Navigate(typeof(HomePage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
    }
}
