# Index

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) \- Graphic control for editing [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity). 

[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) \- is a special type of index security based on a combination of several securities using mathematical formulas. This type has the [ExpressionIndexSecurity.Expression](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression), property, which stores the formula in text form and the list of underlying [ExpressionIndexSecurity.InnerSecurityIds](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds) securities. 

![GUI IndexSecurityWindow](../images/GUI_IndexSecurityWindow.png)

**Basic properties**

- [IndexEditor.Securities](xref:StockSharp.Xaml.IndexEditor.Securities) \- all available securities.
- [IndexEditor.Formula](xref:StockSharp.Xaml.IndexEditor.Formula) \- mathematical formula of the index.

To use [IndexEditor](xref:StockSharp.Xaml.IndexEditor), first you need to register a special service:

```cs
...
ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
...
```

Next, the securities necessary for index calculating should be passed to [IndexEditor](xref:StockSharp.Xaml.IndexEditor):

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
