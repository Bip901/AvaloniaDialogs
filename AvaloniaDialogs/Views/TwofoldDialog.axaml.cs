using Avalonia;
using Avalonia.Interactivity;

namespace AvaloniaDialogs.Views;

/// <summary>
/// A dialog with a positive and a negative button.
/// </summary>
public partial class TwofoldDialog : BaseDialog<bool>
{
    public static readonly StyledProperty<string> MessageProperty =
    AvaloniaProperty.Register<SingleActionDialog, string>(nameof(Message));

    public string Message
    {
        get { return GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    public static readonly StyledProperty<string> PositiveTextProperty =
    AvaloniaProperty.Register<SingleActionDialog, string>(nameof(PositiveText));

    public string PositiveText
    {
        get { return GetValue(PositiveTextProperty); }
        set { SetValue(PositiveTextProperty, value); }
    }

    public static readonly StyledProperty<string> NegativeTextProperty =
    AvaloniaProperty.Register<SingleActionDialog, string>(nameof(NegativeText));

    public string NegativeText
    {
        get { return GetValue(NegativeTextProperty); }
        set { SetValue(NegativeTextProperty, value); }
    }

    public TwofoldDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void PositiveButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }

    public void NegativeButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
