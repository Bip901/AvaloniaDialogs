using Avalonia;
using Avalonia.Interactivity;
using System;

namespace AvaloniaDialogs.Views;

/// <summary>
/// A dialog which displays information and a single button.
/// </summary>
public partial class SingleActionDialog : BaseDialog<EventArgs>
{
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<SingleActionDialog, string>(nameof(Message));

    public string Message
    {
        get { return GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    public static readonly StyledProperty<string> ButtonTextProperty =
    AvaloniaProperty.Register<SingleActionDialog, string>(nameof(ButtonText));

    public string ButtonText
    {
        get { return GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }

    public SingleActionDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void ButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(EventArgs.Empty);
    }
}