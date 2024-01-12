using Avalonia.Interactivity;
using AvaloniaDialogs.Views;

namespace AvaloniaDialogs.Demo;

/// <summary>
/// This custom dialog returns the value of a slider.
/// </summary>
public partial class CustomDialog : BaseDialog<double>
{
    private const double MAX_AWESOMENESS = 90;

    public CustomDialog()
    {
        InitializeComponent();
    }

    private async void ButtonConfirm_Click(object? sender, RoutedEventArgs e)
    {
        if (SliderAwesomeness.Value >= MAX_AWESOMENESS)
        {
            await new SingleActionDialog()
            {
                Message = $"Awesomeness must be lower than {MAX_AWESOMENESS}",
                ButtonText = "OK"
            }.ShowAsync();
            return;
        }
        Close(SliderAwesomeness.Value);
    }
}
