using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using DialogHostAvalonia;
using System;
using System.Threading.Tasks;

namespace AvaloniaDialogs.Views;


/// <summary>
/// A user-control which is meant to pop on the screen for some user action.
/// </summary>
public abstract class BaseDialog : UserControl
{
    /// <summary>
    /// Called before closing this dialog. Return false to cancel.
    /// </summary>
    public virtual bool OnClosing()
    {
        return true;
    }
}

/// <summary>
/// A user-control which is meant to pop on the screen for some user action.
/// </summary>
public abstract class BaseDialog<TResult> : BaseDialog
{
    /// <summary>
    /// Shows this dialog and returns the result, or null if it was closed without a result (e.g. by an "Escape" press).
    /// </summary>
    public virtual async Task<Optional<TResult>> ShowAsync()
    {
        object? result = await DialogHost.Show(this, (string?)null);
        if (result == null)
            return Optional<TResult>.Empty;
        return new Optional<TResult>((TResult)result);
    }

    public virtual void Close()
    {
        DialogHost.Close(null, null);
    }

    protected virtual void Close(TResult result)
    {
        DialogHost.Close(null, result);
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        DoInitialFocus();
    }

    /// <summary>
    /// Focus something in order to remove the focus from behind the dialog, otherwise the user may still interact with background elements using the keyboard
    /// </summary>
    /// <remarks>The default implementation focuses the first focusable child using DFS. If no such element was found, nothing is focused - this is undesirable behavior!</remarks>
    protected virtual void DoInitialFocus()
    {
        InputElement? firstFocusableDescandant = (InputElement?)UIUtil.SelectLogicalDescendant(this, x => x is InputElement inputElement && inputElement.Focusable);
        firstFocusableDescandant?.FocusEventually();
    }
}
