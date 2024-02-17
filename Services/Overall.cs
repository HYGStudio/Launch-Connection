using Avalonia.Controls;

namespace Launch_Connection;

public class Overall
{
    public static void Copy(string text)
    {
        Window window = new();
        window.Clipboard.SetTextAsync(text);
    }
}