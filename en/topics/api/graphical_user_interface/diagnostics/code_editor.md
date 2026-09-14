# Code editor

![Screenshot: code editor with syntax highlighting](../../../../images/gui_codepanel.png)

[CodePanel](xref:StockSharp.Xaml.CodeEditor.CodePanel) - a source code editor with syntax highlighting, line numbers, completion and a compilation toolbar. It is used everywhere code is edited inside the application \- in strategies, indicators and scripts.

**Main properties**

- [CodePanel.Code](xref:StockSharp.Xaml.CodeEditor.CodePanel.Code) - the edited code and its compilation state.
- [CodePanel.ReadOnly](xref:StockSharp.Xaml.CodeEditor.CodePanel.ReadOnly) - prohibition to change the text.
- [CodePanel.ShowToolBar](xref:StockSharp.Xaml.CodeEditor.CodePanel.ShowToolBar) - toolbar visibility.
- [CodePanel.AutoCompile](xref:StockSharp.Xaml.CodeEditor.CodePanel.AutoCompile) - automatic compilation after an edit.

The editor does not show compilation errors itself \- it is convenient to show them next to it in the [ErrorsGrid](xref:StockSharp.Xaml.Code.ErrorsGrid) table and jump to the line on a double click.

Below are code snippets showing its usage:

```xaml
<Window x:Class="Sample.CodeWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:code="clr-namespace:StockSharp.Xaml.CodeEditor;assembly=StockSharp.Xaml.CodeEditor"
	Height="600" Width="900">
	<code:CodePanel x:Name="CodePanel" ShowToolBar="True" />
</Window>
```

```cs
// Load the source text into the editor
CodePanel.Code = new CodeInfo
{
	Text = File.ReadAllText("MyStrategy.cs"),
};

// After compilation show the errors in a separate table
CodePanel.CompiledCode += () => ErrorsGrid.Errors.AddRange(CodePanel.Code.Errors);

// View without editing
CodePanel.ReadOnly = true;
```

## See also

[Diagnostics](../diagnostics.md)
