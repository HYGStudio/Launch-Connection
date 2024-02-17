using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using Launch_Connection.Views;
using RivFox.Network;

namespace Launch_Connection;

public partial class MainWindow : Window
{
    public static MainWindow Main;

    public MainWindow()
    {
        InitializeComponent();
        InitializeDisposition();
        Main = this;
    }

    private async void InitializeDisposition()
    {
        //System judgment
        Title += Services.System.ToString();
        _Title.Text += Services.System.ToString();

        //Update checks
        var res = await Network.GetRequest_Json($"{App.ApiUrl}official/lc/public/version", status_bool: true);
        if ((int?)res["status"] == 200 && (string?)res["bodys"]["Version"] != "2.0.0")
        {
            Update_ContentDialog.Title += $"发现新版本 {(string?)res["bodys"]["Version"]}!";
            Update_Version.Text = $"版本更新: 2.0.0 => {(string?)res["bodys"]["Version"]}";
            Update_Logs.Text = $"更新日志: \n{(string?)res["bodys"]["Logs"]}";
            await Update_ContentDialog.ShowAsync();
        }
        //检查提示
    }

    private void NavigationView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var selectedItem = (NavigationViewItem)e.SelectedItem;
        var tag = (string)selectedItem.Tag;

        var pageTypes = new Dictionary<string, Type>
        {
            //MenuItems
            { "HomePage", typeof(HomePage) },
            { "TunnelsPage", typeof(TunnelsPage) },
            { "LogsPage", typeof(LogsPage) },
            { "PluginsPage", typeof(PluginsPage) },
            //FooterMenuItems
            { "UserPage", typeof(UserPage) },
            { "SettingsPage", typeof(SettingsPage) }
        };

        if (pageTypes.TryGetValue(tag, out var pageType))
            NavigationView_Frame.Navigate(
                pageType, null,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                }
            );

        var currentPage = NavigationView_Frame.Content as UserControl;
        Header.Text = currentPage.Tag.ToString();
    }

    private void Window_Close_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void Window_Minimise_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Window_Maximise_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
        else WindowState = WindowState.Maximized;
    }
}