# Редактор кода

![Снимок экрана: редактор кода с подсветкой синтаксиса](../../../../images/gui_codepanel.png)

[CodePanel](xref:StockSharp.Xaml.CodeEditor.CodePanel) - редактор исходного кода с подсветкой синтаксиса, нумерацией строк, автодополнением и панелью инструментов компиляции. Используется везде, где код правится прямо в приложении \- в стратегиях, индикаторах и скриптах.

**Основные свойства**

- [CodePanel.Code](xref:StockSharp.Xaml.CodeEditor.CodePanel.Code) - редактируемый код и его состояние компиляции.
- [CodePanel.ReadOnly](xref:StockSharp.Xaml.CodeEditor.CodePanel.ReadOnly) - запрет на изменение текста.
- [CodePanel.ShowToolBar](xref:StockSharp.Xaml.CodeEditor.CodePanel.ShowToolBar) - показ панели инструментов.
- [CodePanel.AutoCompile](xref:StockSharp.Xaml.CodeEditor.CodePanel.AutoCompile) - автоматическая компиляция после правки.

Ошибки компиляции редактор не показывает сам \- их удобно выводить рядом в таблице [ErrorsGrid](xref:StockSharp.Xaml.Code.ErrorsGrid) и по двойному щелчку переходить к строке.

Ниже показаны фрагменты кода с его использованием:

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
// Загружаем исходный текст в редактор
CodePanel.Code = new CodeInfo
{
	Text = File.ReadAllText("MyStrategy.cs"),
};

// После компиляции выводим ошибки в отдельную таблицу
CodePanel.CompiledCode += () => ErrorsGrid.Errors.AddRange(CodePanel.Code.Errors);

// Просмотр без правки
CodePanel.ReadOnly = true;
```

## См. также

[Диагностика](../diagnostics.md)
