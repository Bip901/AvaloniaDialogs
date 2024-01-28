using Avalonia;
using Avalonia.Interactivity;

namespace AvaloniaDialogs.Views;

/// <summary>
/// A dialog with a neutral, negative and positive button.
/// </summary>
public partial class ThreefoldDialog : BaseDialog<ThreefoldDialog.ButtonType>
{
    public enum ButtonType
    {
        Negative,
        Positive,
        Neutral
    }

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


    public static readonly StyledProperty<string> NeturalTextProperty =
    AvaloniaProperty.Register<SingleActionDialog, string>(nameof(NeturalText));

    public string NeturalText
    {
        get { return GetValue(NeturalTextProperty); }
        set { SetValue(NeturalTextProperty, value); }
    }

    public ThreefoldDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void PositiveButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(ButtonType.Positive);
    }

    public void NegativeButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(ButtonType.Negative);
    }

    public void NeutralButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(ButtonType.Neutral);
    }
}