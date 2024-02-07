using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#pragma warning disable CS8600
#pragma warning disable CS8602
namespace Launch_Connection.Views;

public partial class MainWindow : Window
{
    public static MainWindow Main;
    public MainWindow()
    {
        InitializeComponent(); InitializeDisposition(); Main = this;
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
        var tag = (string)selectedItem.Tag;

        var pageTypes = new Dictionary<string, Type>
        {
            //MenuItems
            {"HomePage", typeof(HomePage)},
            {"TunnelsPage", typeof(TunnelsPage)},
            {"LogsPage", typeof(LogsPage)},
            {"PluginsPage", typeof(PluginsPage)},
            //FooterMenuItems
            {"UserPage", typeof(UserPage)},
            {"SettingsPage", typeof(SettingsPage)}
        };

        if (pageTypes.TryGetValue(tag, out var pageType)) NavigationView_Frame.Navigate(
            pageType, null,
            new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromLeft
            }
        );

        var currentPage = NavigationView_Frame.Content as UserControl;
        Header.Text = currentPage.Tag.ToString();
    }
    private void Window_Close_Click(object sender, RoutedEventArgs e) => Hide();
    private void Window_Minimise_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Window_Maximise_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
        else WindowState = WindowState.Maximized;
    }
}