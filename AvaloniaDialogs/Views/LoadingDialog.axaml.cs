using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaDialogs.Views;

/// <summary>
/// A loading dialog that performs an optionally cancellable task.
/// </summary>
/// <remarks>Calling ShowAsync on this dialog is guaranteed to return a completed task. However, it could be faulted or cancelled.</remarks>
public partial class LoadingDialog : BaseDialog<Task>
{
    public static readonly StyledProperty<HorizontalAlignment> HorizontalButtonAlignmentProperty =
    AvaloniaProperty.Register<LoadingDialog, HorizontalAlignment>(nameof(HorizontalButtonAlignment), HorizontalAlignment.Right);

    public HorizontalAlignment HorizontalButtonAlignment
    {
        get { return GetValue(HorizontalButtonAlignmentProperty); }
        set { SetValue(HorizontalButtonAlignmentProperty, value); }
    }

    /// <summary>
    /// The message shown in the dialog.
    /// </summary>
    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly StyledProperty<string> MessageProperty =
    AvaloniaProperty.Register<LoadingDialog, string>(nameof(Message), string.Empty);

    /// <summary>
    /// The text shown in the cancellation button. Only applicable if <see cref="IsCancellable"/> is true.
    /// </summary>
    public string ButtonCancelText
    {
        get => GetValue(ButtonCancelTextProperty);
        set => SetValue(ButtonCancelTextProperty, value);
    }

    public static readonly StyledProperty<string> ButtonCancelTextProperty =
    AvaloniaProperty.Register<LoadingDialog, string>(nameof(ButtonCancelText), "Cancel");

    /// <summary>
    /// The <see cref="IsCancellable"/> property.
    /// </summary>
    public static readonly DirectProperty<LoadingDialog, bool> IsCancellableProperty =
    AvaloniaProperty.RegisterDirect<LoadingDialog, bool>(nameof(IsCancellable), o => o.IsCancellable);

    /// <summary>
    /// Whether this dialog can be canceled at all. This property is read-only and defined by whether a <see cref="CancellationTokenSource"/> was passed to the constructor of this instance.
    /// </summary>
    /// <remarks>See also: <seealso cref="IsCancellationAllowed"/></remarks>
    [MemberNotNullWhen(true, nameof(cancellationTokenSource))]
    public bool IsCancellable => cancellationTokenSource != null;

    /// <summary>
    /// The <see cref="IsCancellationAllowed"/> property.
    /// </summary>
    public static readonly DirectProperty<LoadingDialog, bool> IsCancellationAllowedProperty =
    AvaloniaProperty.RegisterDirect<LoadingDialog, bool>(nameof(IsCancellationAllowed), o => o.IsCancellationAllowed);

    /// <summary>
    /// Whether the dialog can be cancelled by the user right now. This value alone is not enough to cancel - <see cref="IsCancellable"/> must also be true.
    /// </summary>
    public bool IsCancellationAllowed
    {
        get => _isCancellationAllowed;
        set
        {
            SetAndRaise(IsCancellationAllowedProperty, ref _isCancellationAllowed, value);
        }
    }
    private bool _isCancellationAllowed = true;

    /// <summary>
    /// Whether the task has completed, not necessarily successfully.
    /// </summary>
    /// <remarks>You may wonder why not use <see cref="Task.IsCompleted"/> instead. The reason is possible race conditions. This variable is only modified from the UI thread.</remarks>
    private bool taskCompleted;
    private readonly Task task;
    private readonly CancellationTokenSource? cancellationTokenSource;

    /// <summary>
    /// Creates a new <see cref="LoadingDialog"/> which runs the given task.
    /// </summary>
    /// <param name="task">The task to run.</param>
    /// <param name="cancellationTokenSource">Optionally, a cancellation token to allow the user to cancel the task. Affects <see cref="IsCancellable"/>.</param>
    public LoadingDialog(Task task, CancellationTokenSource? cancellationTokenSource = null)
    {
        InitializeComponent();
        this.task = task;
        this.cancellationTokenSource = cancellationTokenSource;
        DataContext = this;
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (Design.IsDesignMode) return;
        try
        {
            await Task.Run(() => task);
        }
        catch //Exceptions should be handled by the caller of ShowAsync
        { }
        taskCompleted = true;
        Close(task);
    }

    /// <summary>
    /// This constructor should only be used by the designer. Use the other constructor instead.
    /// </summary>
    public LoadingDialog() : this(Task.CompletedTask)
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("This constructor should only be used by the designer. Use the other constructor instead.");
        }
    }

    private void OnCancellationRequested()
    {
        if (IsCancellable && IsCancellationAllowed)
        {
            cancellationTokenSource.Cancel();
            IsCancellationAllowed = false;
        }
    }

    public void ButtonCancel_Clicked(object? sender, RoutedEventArgs args)
    {
        OnCancellationRequested();
    }

    public override bool OnClosing()
    {
        //Ensure the dialog only ever exists with a value, even when clicking away from it or calling Close() with no arguments.
        if (!taskCompleted)
        {
            OnCancellationRequested();
            return false;
        }
        return true;
    }
}
