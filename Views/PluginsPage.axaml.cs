using System;
using Avalonia.Controls;
using RivFox.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;

namespace Launch_Connection.Views;

public partial class PluginsPage : UserControl
{
    public PluginsPage()
    {
        InitializeComponent();
        InitializeDisposition();
    }

    private void InitializeDisposition()
    {
        foreach (var item in Plugins_Source.Items)
            if (item is ComboBoxItem comboBoxItem && comboBoxItem.Tag?.ToString() == "0")
                Plugins_Source.SelectedItem = comboBoxItem;
        Data_Pulling();
    }

    private void Plugins_List_Refresh_Click(object? sender, RoutedEventArgs e) => Data_Pulling();

    private async void Data_Pulling()
    {
        Plugins_List_Refresh.IsEnabled = false;
        Loading.IsVisible = true;
        Plugins_List.IsVisible = false;
        Error.IsVisible = false;
        Error.Children.Clear();
        var res = await Network.GetRequest_Json($"{App.ApiUrl}official/lc/public/plugins", status_bool: true);
        Plugins_List.Items.Clear();
        if ((int?)res["status"] == 200)
        {
            List<Plugins_Binding> Plugins_Lists =
                JsonConvert.DeserializeObject<List<Plugins_Binding>>(res["bodys"]["List"].ToString());
            foreach (var item in Plugins_Lists)
            {
                item.File.Name = $"{item.File.Name}.{Services.System.ToString(true)}";
                Plugins_List.Items.Add(item);
            }

            Plugins_List.IsVisible = true;
        }
        else
        {
            Error.Spacing = 2.5;
            Error.Children.Add(new TextBlock { Text = "获取扩展发生错误刷新再试", HorizontalAlignment = HorizontalAlignment.Center });
            Error.Children.Add(new TextBlock
            {
                Text = res["excep"].ToString(), HorizontalAlignment = HorizontalAlignment.Center, Opacity = 0.5,
                MaxWidth = 420, MaxHeight = 240, TextTrimming = TextTrimming.WordEllipsis
            });
            var button = new Button { Content = "复制错误信息", HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0,4,0,0)};
            button.Click += (sender, args) => Overall.Copy(res["excep"].ToString());
            Error.Children.Add(button);
            Error.IsVisible = true;
        }

        Loading.IsVisible = false;
        Plugins_List_Refresh.IsEnabled = true;
    }

    private async void Plugins_List_Info_Click(object? sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        if (button.DataContext is Plugins_Binding plugins_Binding)
        {
            var stackPanel_Link = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = 400, MaxHeight = 280,
                Margin = new Thickness(0, 5, 0, 0)
            };
            var stackPanel = new StackPanel();

            if (plugins_Binding.Other.Website != null)
                stackPanel_Link.Children.Add(new HyperlinkButton
                    { Content = "官网", NavigateUri = new Uri((string)plugins_Binding.Other.Website) });
            if (plugins_Binding.Other.Policy != null)
                stackPanel_Link.Children.Add(new HyperlinkButton
                    { Content = "服务条款", NavigateUri = new Uri((string)plugins_Binding.Other.Policy) });
            if (plugins_Binding.Other.Privacy != null)
                stackPanel_Link.Children.Add(new HyperlinkButton
                    { Content = "隐私政策", NavigateUri = new Uri((string)plugins_Binding.Other.Privacy) });

            stackPanel.Children.Add(new TextBlock { Text = $"版本: {plugins_Binding.Version}" });
            stackPanel.Children.Add(new TextBlock { Text = $"介绍: {plugins_Binding.Intro}" });
            stackPanel.Children.Add(new TextBlock { Text = $"基于: {plugins_Binding.Based}" });
            stackPanel.Children.Add(new TextBlock { Text = $"平台: {plugins_Binding.Terrace}" });
            stackPanel.Children.Add(new TextBlock { Text = $"文件名: {plugins_Binding.File.Name}" });
            stackPanel.Children.Add(new TextBlock { Text = $"MD5: {plugins_Binding.File.Md5}" });
            stackPanel.Children.Add(stackPanel_Link);

            new ContentDialog
            {
                Title = $"{plugins_Binding.Title} #{plugins_Binding.Id}",
                Content = new ScrollViewer
                {
                    Content = stackPanel
                },
                CloseButtonText = "确定",
                DefaultButton = ContentDialogButton.Close
            }.ShowAsync();
        }

        //var a = await Plugins_ContentDialog.ShowAsync();
    }

    private void Plugins_List_Download_Click(object? sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        if (button.DataContext is Plugins_Binding plugins_Binding)
        {
            var webClient = new WebClient();
            var textBlock = new TextBlock();
            var _textBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2, 0, 0, 0)
            };
            var progressBar = new ProgressBar();
            var stackPanel = new StackPanel();
            var _stackPanel = new StackPanel();
            stackPanel.Spacing = 1;
            stackPanel.Children.Add(_stackPanel);
            stackPanel.Children.Add(progressBar);
            _stackPanel.Orientation = Orientation.Horizontal;
            _stackPanel.Children.Add(textBlock);
            _stackPanel.Children.Add(_textBlock);
            var contentDialog = new ContentDialog
            {
                Title = $"正在安装插件 {plugins_Binding.Title} #{plugins_Binding.Id}",
                Content = new ScrollViewer { Content = stackPanel },
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Close,
            };
            contentDialog.CloseButtonClick += (dialog, args) =>
            {
                webClient.CancelAsync();
                File.Delete(@$"./Plugins/{plugins_Binding.File.Name}");
            };
            contentDialog.ShowAsync();
            webClient.DownloadFileAsync(new Uri((string)plugins_Binding.Url.Windows),
                @$"./Plugins/{plugins_Binding.File.Name}");
            webClient.DownloadProgressChanged += (sender, e) =>
            {
                progressBar.Value = e.ProgressPercentage;
                textBlock.Text = "已下载:";
                _textBlock.Text = $"{e.ProgressPercentage}%";
            };
            webClient.DownloadFileCompleted += (sender, e) =>
            {
                webClient.CancelAsync();
                if (e.Error != null)
                {
                    contentDialog.Hide();
                    if (File.Exists(@$"./Plugins/{plugins_Binding.File.Name}"))
                        File.Delete(@$"./Plugins/{plugins_Binding.File.Name}");
                    var _contentDialog = new ContentDialog
                    {
                        Title = $"{plugins_Binding.Title} #{plugins_Binding.Id} 安装失败",
                        Content = new TextBlock { Text = e.Error.Message, TextWrapping = TextWrapping.Wrap },
                        CloseButtonText = "确定",
                        PrimaryButtonText = "复制",
                        DefaultButton = ContentDialogButton.Close
                    };
                    _contentDialog.PrimaryButtonClick += (dialog, args) => Overall.Copy(e.Error.Message);
                    _contentDialog.ShowAsync();
                }
                else
                {
                    var hash = MD5.Create().ComputeHash(File.OpenRead(@$"./Plugins/{plugins_Binding.File.Name}"));
                    var md5 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower().ToString();
                    var _md5 = plugins_Binding.File.Md5.ToString();
                    contentDialog.Hide();
                    if (md5 == _md5)
                    {
                        new ContentDialog
                        {
                            Title = $"{plugins_Binding.Title} #{plugins_Binding.Id} 安装成功",
                            Content = new TextBlock { Text = "扩展安装完毕" },
                            CloseButtonText = "确定",
                            DefaultButton = ContentDialogButton.Close,
                        }.ShowAsync();
                    }
                    else
                    {
                        //File.Delete(@$"./Plugins/{plugins_Binding.File.Name}");
                        var _contentDialog = new ContentDialog
                        {
                            Title = $"{plugins_Binding.Title} #{plugins_Binding.Id} 安装失败",
                            Content = new TextBlock { Text = $"MD5效验失败, 点击复制可获取MD5值\n本地md5: {md5}\n云端md5: {_md5}" },
                            CloseButtonText = "确定",
                            PrimaryButtonText = "复制",
                            DefaultButton = ContentDialogButton.Close
                        };
                        _contentDialog.PrimaryButtonClick += (dialog, args) =>
                            Overall.Copy($"本地md5: {md5}\n云端md5: {_md5}");
                        _contentDialog.ShowAsync();
                    }
                }
            };
        }
    }

    private class Plugins_Binding()
    {
        public object Id { set; get; }
        public object Title { set; get; }
        public object Intro { set; get; }
        public object Based { set; get; }
        public object Terrace { set; get; }
        public object Version { set; get; }
        public _Other Other { set; get; }
        public _Url Url { set; get; }
        public _File File { set; get; }

        public class _Other()
        {
            public object Website { set; get; }
            public object Policy { set; get; }
            public object Privacy { set; get; }
        }

        public class _Url()
        {
            public object Windows { set; get; }
        }

        public class _File()
        {
            public object Name { set; get; }
            public object Md5 { set; get; }
        }
    }
}