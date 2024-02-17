using Avalonia.Controls;
using Avalonia.Interactivity;
using Newtonsoft.Json.Linq;
using RivFox.Network;

namespace Launch_Connection.Views;

public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
        InitializeDisposition();
    }

    private void InitializeDisposition()
    {
        Notice_Load();
    }

    private async void Notice_Load()
    {
        //Notice_Error.IsVisible = false;
        try
        {
            //var result = await Network.GetRequest($"{App.Api_url}official/lc/public/notice"); //公告获取
            //Notice.Children.Add(AvaloniaXamlLoader.Load((System.IServiceProvider?)this, result.ToString()));
        }
        catch
        {
            Notice_Error.IsVisible = true;
        }
    }

    private void Notice_Retry_Click(object sender, RoutedEventArgs e)
    {
        Notice_Load();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Notice_Load();
    }
}