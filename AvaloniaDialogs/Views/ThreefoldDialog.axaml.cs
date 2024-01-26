using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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


    public static readonly StyledProperty<string> NeutralTextProperty =
    AvaloniaProperty.Register<SingleActionDialog, string>(nameof(NeutralText));

    public string NeutralText
    {
        get { return GetValue(NeutralTextProperty); }
        set { SetValue(NeutralTextProperty, value); }
    }

    public static readonly StyledProperty<Thickness> ButtonMarginProperty =
    AvaloniaProperty.Register<SingleActionDialog, Thickness>(nameof(ButtonMargin), new Thickness(8));

    public Thickness ButtonMargin
    {
        get { return GetValue(ButtonMarginProperty); }
        set { SetValue(ButtonMarginProperty, value); }
    }

    public static readonly StyledProperty<GridLength> NeutralButtonSpacerWidthProperty =
    AvaloniaProperty.Register<SingleActionDialog, GridLength>(nameof(NeutralButtonSpacerWidth), GridLength.Star);

    public GridLength NeutralButtonSpacerWidth
    {
        get { return GetValue(NeutralButtonSpacerWidthProperty); }
        set { SetValue(NeutralButtonSpacerWidthProperty, value); }
    }

    public static readonly StyledProperty<GridLength> NeutralButtonWidthProperty =
    AvaloniaProperty.Register<SingleActionDialog, GridLength>(nameof(NeutralButtonWidth), GridLength.Auto);

    public GridLength NeutralButtonWidth
    {
        get { return GetValue(NeutralButtonWidthProperty); }
        set { SetValue(NeutralButtonWidthProperty, value); }
    }

    public static readonly StyledProperty<GridLength> PositiveButtonWidthProperty =
    AvaloniaProperty.Register<SingleActionDialog, GridLength>(nameof(PositiveButtonWidth), GridLength.Auto);

    public GridLength PositiveButtonWidth
    {
        get { return GetValue(PositiveButtonWidthProperty); }
        set { SetValue(PositiveButtonWidthProperty, value); }
    }

    public static readonly StyledProperty<GridLength> NegativeButtonWidthProperty =
    AvaloniaProperty.Register<SingleActionDialog, GridLength>(nameof(NegativeButtonWidth), GridLength.Auto);

    public GridLength NegativeButtonWidth
    {
        get { return GetValue(NegativeButtonWidthProperty); }
        set { SetValue(NegativeButtonWidthProperty, value); }
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

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Key == Key.Left)
        {
            if (PositiveButton.IsFocused)
            {
                NegativeButton.Focus(NavigationMethod.Directional);
            }
            else if (NeutralButton.IsFocused)
            {
                PositiveButton.Focus(NavigationMethod.Directional);
            }
            else
            {
                NeutralButton.Focus(NavigationMethod.Directional);
            }
        }
        else if (e.Key == Key.Right)
        {
            if (NeutralButton.IsFocused)
            {
                NegativeButton.Focus(NavigationMethod.Directional);
            }
            else if (NegativeButton.IsFocused)
            {
                PositiveButton.Focus(NavigationMethod.Directional);
            }
            else
            {
                NeutralButton.Focus(NavigationMethod.Directional);
            }
        }
    }
}