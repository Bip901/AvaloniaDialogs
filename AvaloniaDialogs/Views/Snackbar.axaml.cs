using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaDialogs.Views;

public partial class Snackbar : UserControl
{
    public const int DURATION_SHORT = 2000;
    public const int DURATION_LONG = 3500;
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

    private void ButtonAction_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        clickAction?.Invoke();
        CancelTimeout();
        Hide();
    }

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
    }

    private void Hide()
    {
        PseudoClasses.Remove(SNACKBAR_SHOWING_PSEUDO_CLASS);
        PseudoClasses.Add(SNACKBAR_HIDING_PSEUDO_CLASS);
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