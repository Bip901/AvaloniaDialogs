using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaDialogs.Views;

public partial class Snackbar : UserControl
{
    public static readonly TimeSpan DURATION_SHORT = TimeSpan.FromSeconds(2.0);
    public static readonly TimeSpan DURATION_LONG = TimeSpan.FromSeconds(4.0);
    private const string SNACKBAR_HIDING_PSEUDO_CLASS = ":hiding";
    private const string SNACKBAR_SHOWING_PSEUDO_CLASS = ":showing";

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Snackbar, string>(nameof(Text), string.Empty);

    public string ActionText
    {
        get => GetValue(ActionTextProperty);
        set => SetValue(ActionTextProperty, value);
    }

    public static readonly StyledProperty<string> ActionTextProperty =
        AvaloniaProperty.Register<Snackbar, string>(nameof(ActionText), string.Empty);

    private CancellationTokenSource? cancelSource;
    private Action? clickAction;

    public Snackbar()
    {
        InitializeComponent();
        DataContext = this;
        ButtonAction.Click += ButtonAction_Click;
    }

    private void ButtonAction_Click(object? sender, RoutedEventArgs e)
    {
        TriggerAction();
    }

    /// <summary>
    /// Simulates the action button being clicked. If this snackbar is not visible, doesn't do anything.
    /// </summary>
    public void TriggerAction()
    {
        if (ButtonAction.IsEffectivelyEnabled)
        {
            clickAction?.Invoke();
            CancelTimeout();
            Hide();
        }
    }

    /// <summary>
    /// Shows this snackbar with the given content.
    /// </summary>
    /// <remarks>If this snackbar is already visible, its content is immediately replaced and the timeout restarts.</remarks>
    /// <param name="text">The text to display.</param>
    /// <param name="duration">The duration after which to hide the snackbar. Recommended values: <seealso cref="DURATION_SHORT"/>, <seealso cref="DURATION_LONG"/></param>
    /// <param name="actionText">The text to show on the action button, or null to not include an action button.</param>
    /// <param name="action">The action to perform when the action button is clicked.</param>
    public void Show(string text, TimeSpan duration, string? actionText = null, Action? action = null)
    {
        Text = text;
        ActionText = actionText ?? string.Empty;
        clickAction = action;
        Show();
        CancelTimeout();
        cancelSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelSource.Token;
        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(duration, cancelToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            Dispatcher.UIThread.Post(Hide);
        });
    }

    private void Show()
    {
        PseudoClasses.Remove(SNACKBAR_HIDING_PSEUDO_CLASS);
        PseudoClasses.Add(SNACKBAR_SHOWING_PSEUDO_CLASS);
        ButtonAction.Focusable = true;
        ButtonAction.IsEnabled = true;
    }

    private void Hide()
    {
        PseudoClasses.Remove(SNACKBAR_SHOWING_PSEUDO_CLASS);
        PseudoClasses.Add(SNACKBAR_HIDING_PSEUDO_CLASS);
        ButtonAction.Focusable = false;
        ButtonAction.IsEnabled = false;
    }

    private void CancelTimeout()
    {
        if (cancelSource != null)
        {
            cancelSource.Cancel();
            cancelSource.Dispose();
            cancelSource = null;
        }
    }
}