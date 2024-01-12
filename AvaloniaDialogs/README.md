# AvaloniaDialogs

This library wraps [DialogHost.Avalonia](https://github.com/AvaloniaUtils/DialogHost.Avalonia/), providing a more convenient API and built-in common dialogs such as a Yes/No popup and a snackbar.

## Quick Start

1. Add the DialogHost styles to your `App.axaml`:

```xml
<Application xmlns:dialogHostAvalonia="using:DialogHostAvalonia">
  <!-- ... -->

  <Application.Styles>
    <dialogHostAvalonia:DialogHostStyles/>
  </Application.Styles>

  <!-- ... -->
</Application>
```

2. Add a dialog host to your window:

```xml
<Window xmlns:dialogs="using:AvaloniaDialogs.Views">
  <!-- ... -->

  <dialogs:ReactiveDialogHost CloseOnClickAway="True">
    <!-- Your window content here, e.g. StackPanel... -->
  </dialogs:ReactiveDialogHost >
</Window>
```

3. Spawn dialogs whenever you want!

```csharp
SingleActionDialog dialog = new() {
    Message = "Hello from C# code!",
    ButtonText = "Click me!"
};
await dialog.ShowAsync();
```

Browse the `AvaloniaDialogs.Demo` project for an example that includes styling.
