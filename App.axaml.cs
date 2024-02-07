using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Launch_Connection.Views;

namespace Launch_Connection;

public partial class App : Application
{
    public static string Api_url = "http://127.0.0.1:1244/";
    public override void Initialize() => AvaloniaXamlLoader.Load(this);
    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }
    private void Show_Window(object? sender, System.EventArgs e) => MainWindow.Main.Show();
    private void Close_Window(object? sender, System.EventArgs e) => MainWindow.Main.Close();
}
