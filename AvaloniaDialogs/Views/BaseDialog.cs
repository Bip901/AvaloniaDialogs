using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using DialogHostAvalonia;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AvaloniaDialogs.Views;

/// <summary>
/// A user-control which is meant to pop on the screen for some user action, and optionally returns a result.
/// </summary>
/// <remarks>For dialogs that return a result, inherit from <seealso cref="BaseDialog{TResult}"/> instead for type safety.</remarks>
public abstract class BaseDialog : UserControl
{
    private IInputElement? previousFocus;
    private TaskCompletionSource<object?>? showNestedTask;
    private string? lastShowDialogHostIdentifier;

    /// <summary>
    /// Returns whether this dialog is being shown on top of another dialog.
    /// </summary>
    [MemberNotNullWhen(true, nameof(showNestedTask))]
    internal bool IsShowingNested => showNestedTask != null;

    /// <summary>
    /// Called before closing this dialog. Return false to cancel.
    /// </summary>
    public virtual bool OnClosing()
    {
        return true;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        IFocusManager? focusManager = TopLevel.GetTopLevel(this)?.FocusManager;
        previousFocus = focusManager?.GetFocusedElement();
        DoInitialFocus();
    }

    /// <summary>
    /// Focus something in order to remove the focus from behind the dialog, otherwise the user may still interact with background elements using the keyboard
    /// </summary>
    /// <remarks>The default implementation focuses the first focusable child using DFS. If no such element was found, nothing is focused - this is undesirable behavior!</remarks>
    protected virtual void DoInitialFocus()
    {
        InputElement? firstFocusableDescandant = (InputElement?)UIUtil.SelectLogicalDescendant(this, x => x is InputElement inputElement && inputElement.Focusable);
        firstFocusableDescandant?.Focus();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        previousFocus?.Focus();
    }

    /// <summary>
    /// Shows this dialog and waits until it's closed.
    /// If you want the dialog's result in a type-safe way, use <see cref="BaseDialog{TResult}.ShowAsync"/> instead.
    /// </summary>
    public async Task<object?> ShowAsync(string? dialogIdentifier = null)
    {
        DialogSession? currentSession = DialogHost.GetDialogSession(dialogIdentifier);
        if (currentSession == null)
        {
            lastShowDialogHostIdentifier = dialogIdentifier;
            return await DialogHost.Show(this, dialogIdentifier);
        }
        else
        {
            return await ShowNestedAsync(currentSession);
        }
    }

    private async Task<object?> ShowNestedAsync(DialogSession session)
    {
        object? previousContent = session.Content;
        showNestedTask = new((session, previousContent));
        session.UpdateContent(this);
        return await showNestedTask.Task.ConfigureAwait(false);
    }

    /// <summary>
    /// Closes this dialog without returning a result.
    /// </summary>
    public void Close()
    {
        Close(null);
    }

    protected internal void Close(object? result)
    {
        if (IsShowingNested)
        {
            (DialogSession session, object? previousContent) = (ValueTuple<DialogSession, object?>)showNestedTask.Task.AsyncState!;
            if (previousContent != null)
            {
                session.UpdateContent(previousContent);
            }
            showNestedTask.TrySetResult(result);
            showNestedTask = null;
        }
        else
        {
            DialogHost.Close(lastShowDialogHostIdentifier, result);
        }
    }
}

/// <summary>
/// A user-control which is meant to pop on the screen for some user action, and optionally returns a result.
/// </summary>
public abstract class BaseDialog<TResult> : BaseDialog
{
    /// <summary>
    /// Shows this dialog and returns the result, or null if it was closed without a result (e.g. by an "Escape" press).
    /// </summary>
    public new async Task<Optional<TResult>> ShowAsync(string? dialogIdentifier = null)
    {
        object? result = await base.ShowAsync(dialogIdentifier);
        if (result == null)
            return Optional<TResult>.Empty;
        return new Optional<TResult>((TResult)result);
    }

    protected void Close(TResult result)
    {
        base.Close(result);
    }
}
