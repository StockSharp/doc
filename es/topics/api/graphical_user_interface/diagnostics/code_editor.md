# Editor de código

![Captura de pantalla: editor de código con resaltado de sintaxis](../../../../images/gui_codepanel.png)

[CodePanel](xref:StockSharp.Xaml.CodeEditor.CodePanel) - un editor de código fuente con resaltado de sintaxis, numeración de líneas, autocompletado y una barra de herramientas de compilación. Se usa allí donde el código se edita dentro de la aplicación: en estrategias, indicadores y scripts.

**Propiedades principales**

- [CodePanel.Code](xref:StockSharp.Xaml.CodeEditor.CodePanel.Code) - el código editado y su estado de compilación.
- [CodePanel.ReadOnly](xref:StockSharp.Xaml.CodeEditor.CodePanel.ReadOnly) - prohibición de modificar el texto.
- [CodePanel.ShowToolBar](xref:StockSharp.Xaml.CodeEditor.CodePanel.ShowToolBar) - visibilidad de la barra de herramientas.
- [CodePanel.AutoCompile](xref:StockSharp.Xaml.CodeEditor.CodePanel.AutoCompile) - compilación automática tras una edición.

El editor no muestra por sí mismo los errores de compilación: conviene mostrarlos al lado en la tabla [ErrorsGrid](xref:StockSharp.Xaml.Code.ErrorsGrid) y saltar a la línea con doble clic.

A continuación se muestran fragmentos de código con su uso:

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
// Cargamos el texto fuente en el editor
CodePanel.Code = new CodeInfo
{
	Text = File.ReadAllText("MyStrategy.cs"),
};

// Tras la compilación mostramos los errores en una tabla aparte
CodePanel.CompiledCode += () => ErrorsGrid.Errors.AddRange(CodePanel.Code.Errors);

// Visualización sin edición
CodePanel.ReadOnly = true;
```

## Ver también

[Diagnóstico](../diagnostics.md)
