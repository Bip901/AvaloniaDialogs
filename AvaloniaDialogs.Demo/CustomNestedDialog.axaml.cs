using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using AvaloniaDialogs.Views;

namespace AvaloniaDialogs.Demo;

/// <summary>
/// This custom dialog demonstrates the ability to open new dialogs while another dialog is already showing.
/// </summary>
public partial class CustomNestedDialog : BaseDialog
{
    private static readonly IBrush[] backgroundsLight = new[] { Brushes.LavenderBlush, Brushes.LightCyan, Brushes.Honeydew, Brushes.LightGoldenrodYellow, Brushes.PowderBlue };
    private static readonly IBrush[] backgroundsDark = new[] { Brushes.DarkSlateBlue, Brushes.DarkCyan, Brushes.DarkOliveGreen, Brushes.Sienna, Brushes.MidnightBlue };
    private readonly int dialogNumber;
    
    public CustomNestedDialog(int dialogNumber = 1)
    {
        InitializeComponent();
        this.dialogNumber = dialogNumber;
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Dark)
        {
            Background = backgroundsDark[dialogNumber % backgroundsDark.Length];
        }
        else
        {
            Background = backgroundsLight[dialogNumber % backgroundsLight.Length];
        }
        TextTitle.Text = $"This is dialog #{dialogNumber}. Do you want to open another dialog on top?";
    }

    private async void ButtonOpenAnother_Click(object? sender, RoutedEventArgs e)
    {
        CustomNestedDialog newDialog = new(dialogNumber + 1);
        await newDialog.ShowAsync();
    }
    
    private void ButtonClose_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
