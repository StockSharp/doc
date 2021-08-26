# Index

[IndexEditor](../api/StockSharp.Xaml.IndexEditor.html) \- Graphic control for editing [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html). 

[ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) \- is a special type of index security based on a combination of several securities using mathematical formulas. This type has the [Expression](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression.html), property, which stores the formula in text form and the list of underlying [InnerSecurityIds](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds.html) securities. 

![GUI IndexSecurityWindow](~/images/GUI_IndexSecurityWindow.png)

**Basic properties**

- [Securities](../api/StockSharp.Xaml.IndexEditor.Securities.html) \- all available securities.
- [Formula](../api/StockSharp.Xaml.IndexEditor.Formula.html) \- mathematical formula of the index.

To use [IndexEditor](../api/StockSharp.Xaml.IndexEditor.html), first you need to register a special service:

```cs
...
ConfigManager.RegisterService\<ICompilerService\>(new RoslynCompilerService());
...
```

Next, the securities necessary for index calculating should be passed to [IndexEditor](../api/StockSharp.Xaml.IndexEditor.html):

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
