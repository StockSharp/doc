# Índice

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) - Control gráfico para editar [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity). 

[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) - es un tipo especial de instrumento índice basado en una combinación de varios instrumentos mediante fórmulas matemáticas. Este tipo tiene la propiedad [ExpressionIndexSecurity.Expression](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression), que almacena la fórmula en forma de texto, y la lista de instrumentos subyacentes [ExpressionIndexSecurity.InnerSecurityIds](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds). 

![GUI IndexSecurityWindow](../../../../images/gui_indexsecuritywindow.png)

**Propiedades básicas**

- [IndexEditor.Securities](xref:StockSharp.Xaml.IndexEditor.Securities) - todos los instrumentos disponibles.
- [IndexEditor.Text](xref:StockSharp.Xaml.IndexEditor.Text) - fórmula matemática del índice.

Para usar [IndexEditor](xref:StockSharp.Xaml.IndexEditor), primero debe registrar un servicio especial:

```cs
...
ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
...
```

A continuación, los instrumentos necesarios para calcular el índice deben pasarse a [IndexEditor](xref:StockSharp.Xaml.IndexEditor):

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

