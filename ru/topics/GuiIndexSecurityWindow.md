# Индекс

[IndexEditor](../api/StockSharp.Xaml.IndexEditor.html) \- Графический контрол для редактирования [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html). 

[ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) \- специальный тип индексного инструмента, в основе которого лежит комбинирование нескольких инструментов при помощи математических формул. Этот тип имеет свойство [Expression](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression.html), в котором хранится формула в текстовом виде и список подлежащих инструментов [InnerSecurityIds](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds.html). 

![GUI IndexSecurityWindow](../images/GUI_IndexSecurityWindow.png)

**Основные свойства **

- [Securities](../api/StockSharp.Xaml.IndexEditor.Securities.html) \- все доступные инструменты.
- [Formula](../api/StockSharp.Xaml.IndexEditor.Formula.html) \- математическая формула индекса.

Для использования [IndexEditor](../api/StockSharp.Xaml.IndexEditor.html) предварительно необходимо зарегистрировать специальную службу:

```cs
...
ConfigManager.RegisterService\<ICompilerService\>(new RoslynCompilerService());
...
```

Далее в [IndexEditor](../api/StockSharp.Xaml.IndexEditor.html) следует передать инструменты необходимые для расчета индекса:

```cs
...
IndexEditor.Securities.AddRange(SecurityProvider.LookupAll());
SecurityProvider.Added +\= OnAdded;
...
private void OnAdded(IEnumerable\<Security\> securities)
     {
         IndexEditor.Securities.AddRange(securities);
     }
```
