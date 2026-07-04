# Index

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) ist ein grafisches Steuerelement zum Bearbeiten von [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity).

[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) ist ein spezieller Typ eines Indexinstruments, das auf einer Kombination mehrerer Instrumente mithilfe mathematischer Formeln basiert. Dieser Typ besitzt die Eigenschaft [ExpressionIndexSecurity.Expression](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression), in der die Formel in Textform gespeichert wird, sowie die Liste der zugrunde liegenden Instrumente [ExpressionIndexSecurity.InnerSecurityIds](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds).

![GUI IndexSecurityWindow](../../../../images/gui_indexsecuritywindow.png)

**Grundeigenschaften**

- [IndexEditor.Securities](xref:StockSharp.Xaml.IndexEditor.Securities) - alle verfügbaren Instrumente.
- [IndexEditor.Text](xref:StockSharp.Xaml.IndexEditor.Text) - mathematische Formel des Index.

Um [IndexEditor](xref:StockSharp.Xaml.IndexEditor) zu verwenden, müssen Sie zunächst einen speziellen Dienst registrieren:

```cs
...
ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
...
```

Anschließend müssen die für die Indexberechnung erforderlichen Instrumente an [IndexEditor](xref:StockSharp.Xaml.IndexEditor) übergeben werden:

```cs
...
IndexEditor.Securities.AddRange(SecurityProvider.LookupAll());
SecurityProvider.Added += OnAdded;
...
private void OnAdded(IEnumerable<Security> securities)
		{
			IndexEditor.Securities.AddRange(securities);
		}
```
