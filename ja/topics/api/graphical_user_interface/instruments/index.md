# インデックス

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) は、[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) を編集するためのグラフィックコントロールです。

[ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) は、複数の銘柄を数式で組み合わせたものに基づく特殊な種類のインデックス銘柄です。この種類には [ExpressionIndexSecurity.Expression](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.Expression) プロパティがあり、数式をテキスト形式で格納します。また、基礎となる [ExpressionIndexSecurity.InnerSecurityIds](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity.InnerSecurityIds) 銘柄のリストも格納します。

![インデックス のスクリーンショット](../../../../images/gui_indexsecuritywindow.png)

**基本プロパティ**

- [IndexEditor.Securities](xref:StockSharp.Xaml.IndexEditor.Securities) - 利用可能なすべての銘柄。
- [IndexEditor.Text](xref:StockSharp.Xaml.IndexEditor.Text) - インデックスの数式。

[IndexEditor](xref:StockSharp.Xaml.IndexEditor) を使用するには、まず特殊なサービスを登録する必要があります。

```cs
...
ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
...
```

次に、インデックス計算に必要な銘柄を [IndexEditor](xref:StockSharp.Xaml.IndexEditor) に渡す必要があります。

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
