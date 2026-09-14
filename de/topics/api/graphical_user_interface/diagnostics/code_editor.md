# Code-Editor

![Bildschirmfoto: Code-Editor mit Syntaxhervorhebung](../../../../images/gui_codepanel.png)

[CodePanel](xref:StockSharp.Xaml.CodeEditor.CodePanel) - ein Quelltexteditor mit Syntaxhervorhebung, Zeilennummern, Vervollständigung und einer Symbolleiste für die Kompilierung. Er wird überall dort eingesetzt, wo Code direkt in der Anwendung bearbeitet wird \- in Strategien, Indikatoren und Skripten.

**Haupteigenschaften**

- [CodePanel.Code](xref:StockSharp.Xaml.CodeEditor.CodePanel.Code) - der bearbeitete Code und sein Kompilierungszustand.
- [CodePanel.ReadOnly](xref:StockSharp.Xaml.CodeEditor.CodePanel.ReadOnly) - Sperre gegen Textänderungen.
- [CodePanel.ShowToolBar](xref:StockSharp.Xaml.CodeEditor.CodePanel.ShowToolBar) - Anzeige der Symbolleiste.
- [CodePanel.AutoCompile](xref:StockSharp.Xaml.CodeEditor.CodePanel.AutoCompile) - automatische Kompilierung nach einer Änderung.

Kompilierungsfehler zeigt der Editor nicht selbst an \- sie lassen sich daneben in der Tabelle [ErrorsGrid](xref:StockSharp.Xaml.Code.ErrorsGrid) ausgeben, von der aus ein Doppelklick zur Zeile springt.

Nachfolgend Codeausschnitte zur Verwendung:

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
// Quelltext in den Editor laden
CodePanel.Code = new CodeInfo
{
	Text = File.ReadAllText("MyStrategy.cs"),
};

// Nach der Kompilierung die Fehler in einer eigenen Tabelle ausgeben
CodePanel.CompiledCode += () => ErrorsGrid.Errors.AddRange(CodePanel.Code.Errors);

// Ansicht ohne Bearbeitung
CodePanel.ReadOnly = true;
```

## Siehe auch

[Diagnostik](../diagnostics.md)
