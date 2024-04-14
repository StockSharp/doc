# Индекс

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) \- Графический контрол для редактирования [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity). 

[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) \- специальный тип индексного инструмента, в основе которого лежит комбинирование нескольких инструментов при помощи математических формул. Этот тип имеет свойство [ExpressionIndexSecurity.Expression](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression), в котором хранится формула в текстовом виде и список подлежащих инструментов [ExpressionIndexSecurity.InnerSecurityIds](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds). 

![GUI IndexSecurityWindow](../../../../images/gui_indexsecuritywindow.png)

**Основные свойства**

- [IndexEditor.Securities](xref:StockSharp.Xaml.IndexEditor.Securities) \- все доступные инструменты.
- [IndexEditor.Text](xref:StockSharp.Xaml.IndexEditor.Text) \- математическая формула индекса.

Для использования [IndexEditor](xref:StockSharp.Xaml.IndexEditor) предварительно необходимо зарегистрировать специальную службу:

```cs
...
ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
...
```

Далее в [IndexEditor](xref:StockSharp.Xaml.IndexEditor) следует передать инструменты необходимые для расчета индекса:

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
