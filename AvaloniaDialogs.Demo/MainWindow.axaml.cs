using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaDialogs.Views;
using System;

namespace AvaloniaDialogs.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SwitchCustomTheme_IsCheckedChanged(object? sender, RoutedEventArgs e)
        {
            if (((ToggleSwitch)sender!).IsChecked == true)
            {

            }
        }

        private async void DialogButton_Click(object? sender, RoutedEventArgs args)
        {
            SingleActionDialog dialog = new()
            {
                Message = "Hello from C# code!",
                ButtonText = "Click me!"
            };
            await dialog.ShowAsync();
        }

        private async void YesNoDialogButton_Click(object? sender, RoutedEventArgs e)
        {
            TwofoldDialog dialog = new()
            {
                Message = "Do you want to see a snackbar?",
                PositiveText = "Yes",
                NegativeText = "No"
            };
            if ((await dialog.ShowAsync()).GetValueOrDefault())
            {
                Snackbar.Show("You pressed yes!", TimeSpan.FromSeconds(2), "I Know");
            }
        }


        private async void SaveDiscardCancelDialogButton_Click(object? sender, RoutedEventArgs e)
        {
            ThreefoldDialog dialog = new()
            {
                Message = "You have unsaved changes. Save?",
                PositiveText = "Save",
                NegativeText = "Discard",
                NeturalText = "Cancel"
            };
            await dialog.ShowAsync();
        }
    }
}