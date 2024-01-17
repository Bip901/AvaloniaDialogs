using Avalonia;
using Avalonia.Input;
using DialogHostAvalonia;
using System;

namespace AvaloniaDialogs.Views;

public class ReactiveDialogHost : DialogHost
{
    protected override Type StyleKeyOverride => typeof(DialogHost);

    public static readonly StyledProperty<Key> CloseKeyProperty =
        AvaloniaProperty.Register<ReactiveDialogHost, Key>(nameof(CloseKey), Key.Escape);

    public Key CloseKey
    {
        get => GetValue(CloseKeyProperty);
        set => SetValue(CloseKeyProperty, value);
    }

    public ReactiveDialogHost()
    {
        DialogClosing += ReactiveDialogHost_DialogClosing;
    }

    private void ReactiveDialogHost_DialogClosing(object? sender, DialogClosingEventArgs e)
    {
        if (DialogContent is BaseDialog dialog)
        {
            if (!dialog.OnClosing())
            {
                e.Cancel();
            }
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (IsOpen && e.Key == CloseKey)
        {
            Close(Identifier);
            e.Handled = true;
        }
    }
}
