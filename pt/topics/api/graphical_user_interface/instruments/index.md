# Índice

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) - controlo gráfico para editar [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity).

[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) - é um tipo especial de instrumento de índice baseado numa combinação de vários instrumentos através de fórmulas matemáticas. Este tipo tem a propriedade [ExpressionIndexSecurity.Expression](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression), que guarda a fórmula em formato de texto, e a lista de instrumentos subjacentes [ExpressionIndexSecurity.InnerSecurityIds](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds).

![Captura de ecrã de Índice](../../../../images/gui_indexsecuritywindow.png)

**Propriedades básicas**

- [IndexEditor.Securities](xref:StockSharp.Xaml.IndexEditor.Securities) - todos os instrumentos disponíveis.
- [IndexEditor.Text](xref:StockSharp.Xaml.IndexEditor.Text) - fórmula matemática do índice.

Para utilizar [IndexEditor](xref:StockSharp.Xaml.IndexEditor), primeiro é necessário registar um serviço especial:

```cs
...
ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
...
```

Em seguida, os instrumentos necessários para o cálculo do índice devem ser passados para [IndexEditor](xref:StockSharp.Xaml.IndexEditor):

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
