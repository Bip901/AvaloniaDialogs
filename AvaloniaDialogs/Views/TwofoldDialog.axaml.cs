using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

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

    public static readonly StyledProperty<HorizontalAlignment> HorizontalButtonAlignmentProperty =
    AvaloniaProperty.Register<SingleActionDialog, HorizontalAlignment>(nameof(HorizontalButtonAlignment), HorizontalAlignment.Right);

    public HorizontalAlignment HorizontalButtonAlignment
    {
        get { return GetValue(HorizontalButtonAlignmentProperty); }
        set { SetValue(HorizontalButtonAlignmentProperty, value); }
    }

    public static readonly StyledProperty<Thickness> ButtonMarginProperty =
    AvaloniaProperty.Register<SingleActionDialog, Thickness>(nameof(ButtonMargin), new Thickness(8));

    public Thickness ButtonMargin
    {
        get { return GetValue(ButtonMarginProperty); }
        set { SetValue(ButtonMarginProperty, value); }
    }

    public TwofoldDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Key == Key.Left || e.Key == Key.Right)
        {
            if (NegativeButton.IsFocused)
            {
                PositiveButton.Focus(NavigationMethod.Directional);
            }
            else
            {
                NegativeButton.Focus(NavigationMethod.Directional);
            }
            e.Handled = true;
        }
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
