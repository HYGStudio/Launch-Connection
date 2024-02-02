using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using System;
using System.Runtime.InteropServices;

#pragma warning disable CS8600
namespace Launch_Connection.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent(); InitializeDisposition();
    }
    private void InitializeDisposition()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Title += "Windows";
            _Title.Text += "Windows";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Title += "MacOS";
            _Title.Text += "MacOS";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Title += "Linux";
            _Title.Text += "Linux";
        }
    }
    private void NavigationView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var selectedItem = (NavigationViewItem)e.SelectedItem;
        if ((string)selectedItem.Tag == "HomePage") NavigationView_Frame.Navigate(typeof(HomePage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        else if ((string)selectedItem.Tag == "LogsPage") NavigationView_Frame.Navigate(typeof(LogsPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });

        var currentPage = NavigationView_Frame.Content as UserControl;
        Header.Text = currentPage.Tag.ToString();
    }
    private void Window_Close_Click(object sender, RoutedEventArgs e) => Close();
    private void Window_Minimise_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Window_Maximise_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
        else WindowState = WindowState.Maximized;
    }
}