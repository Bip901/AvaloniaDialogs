using Avalonia;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using System;

namespace AvaloniaDialogs;

public static class UIUtil
{
    /// <summary>
    /// Focuses the input element as soon as it's attached to the visual tree (which could be immediately).
    /// </summary>
    public static void FocusEventually(this InputElement control)
    {
        if (control.IsAttachedToVisualTree())
        {
            FocusImmediately(control);
        }
        else
        {
            control.AttachedToVisualTree += Control_AttachedToVisualTree;
        }

        static void Control_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is InputElement inputElement)
            {
                inputElement.AttachedToVisualTree -= Control_AttachedToVisualTree;
                FocusImmediately(inputElement);
            }
        }

        static void FocusImmediately(InputElement control)
        {
            control.Focus();
        }
    }

    /// <summary>
    /// Recursively traverses the logical tree using depth-first search and returns the first matching element, or null if not found.
    /// </summary>
    public static ILogical? SelectLogicalDescendant(ILogical root, Predicate<ILogical> selector)
    {
        if (selector(root))
            return root;
        foreach (ILogical child in root.LogicalChildren)
        {
            ILogical? result;
            if ((result = SelectLogicalDescendant(child, selector)) != null)
                return result;
        }
        return null;
    }
}
