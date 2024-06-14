# AvaloniaDialogs

[![NuGet version](https://img.shields.io/nuget/v/AvaloniaDialogs.svg)](https://www.nuget.org/packages/AvaloniaDialogs/)

This library wraps [DialogHost.Avalonia](https://github.com/AvaloniaUtils/DialogHost.Avalonia/), providing a more convenient API and built-in common dialogs such as a Yes/No popup and a snackbar.

## [Quick Start Guide](./AvaloniaDialogs/README.md)

## Improvements over DialogHost.Avalonia

* Convenient async APIs - spawning a dialog is as easy as:

```csharp
SingleActionDialog dialog = new() {
    Message = "Hello from C# code!",
    ButtonText = "Click me!"
};
await dialog.ShowAsync();
```

* Built-in common dialogs: Loading Dialog, Snackbar, Twofold (e.g. Yes/No), Threefold (e.g. Cancel/Discard/Save)
* Easy extensibility: to create your own dialogs, just inherit from `BaseDialog` (or `BaseDialog<>` if your dialog returns a result). For example, [here's a custom dialog](./AvaloniaDialogs.Demo/CustomDialog.axaml.cs) that displays a slider and returns the user's selection.
* Added support for nested dialogs - that is, opening a new dialog while another is showing on the same DialogHost.
* Improved keyboard UX - this library ensures correct keyboard focus to prevent interacting with elements below the dialog; Dialogs can be closed by pressing Escape by default; and more.
