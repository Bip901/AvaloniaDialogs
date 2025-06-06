using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Styling;
using AvaloniaDialogs.Views;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaDialogs.Demo;

public partial class MainWindow : Window
{
    const string CUSTOM_STYLE_KEY = "CustomStyle";

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SwitchCustomTheme_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (((ToggleSwitch)sender!).IsChecked == true)
        {
            Styles.Add((IStyle)Resources[CUSTOM_STYLE_KEY]!);
        }
        else
        {
            Styles.Clear();
        }
    }

    private async void DialogButton_Click(object? sender, RoutedEventArgs args)
    {
        SingleActionDialog dialog = new()
        {
            Message = "Hello from C# code! Do you want to see a snackbar?",
            ButtonText = "Click me!"
        };
        if ((await dialog.ShowAsync()).HasValue)
        {
            Snackbar.Show("I'm a snackbar!", TimeSpan.FromSeconds(2), "I Know");
        }
    }

    private async void YesNoDialogButton_Click(object? sender, RoutedEventArgs e)
    {
        TwofoldDialog dialog = new()
        {
            Message = "Do you want to perform action X?",
            PositiveText = "Yes",
            NegativeText = "No"
        };
        if ((await dialog.ShowAsync()).GetValueOrDefault())
        {
            //User clicked "Yes"
        }
    }

    private async void SaveDiscardCancelDialogButton_Click(object? sender, RoutedEventArgs e)
    {
        ThreefoldDialog dialog = new()
        {
            Message = "You have unsaved changes. Save?",
            PositiveText = "Save",
            NegativeText = "Discard",
            NeutralText = "Cancel"
        };
        await dialog.ShowAsync();
    }

    private async void LoadingDialogButton_Click(object? sender, RoutedEventArgs e)
    {
        CancellationTokenSource cancelSrc = new();
        try
        {
            int result = await LoadingDialog.DoAsync(async () =>
            {
                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000, cancelSrc.Token);
                    count++;
                }
                return count;
            }, "Counting for 3 seconds...", cancelSrc);
            Snackbar.Show($"Total count: {result}", Snackbar.DURATION_SHORT);
        }
        catch (OperationCanceledException)
        { }
    }

    private async void CustomDialogButton_Click(object? sender, RoutedEventArgs e)
    {
        CustomDialog dialog = new();
        Optional<double> result = await dialog.ShowAsync();
        if (result.HasValue)
        {
            Snackbar.Show($"You picked: {result.Value:0.00}", TimeSpan.FromSeconds(2));
        }
    }

    private async void NestedDialogButton_Click(object? sender, RoutedEventArgs e)
    {
        CustomNestedDialog nestedDialog = new();
        await nestedDialog.ShowAsync();
    }
}