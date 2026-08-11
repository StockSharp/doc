# デスクトップコントロール

## S# のデスクトップコンポーネント

[S#](../api.md) には、多数の独自グラフィカルコンポーネントが含まれています。コンポーネントは [StockSharp.Xaml](xref:StockSharp.Xaml)、[StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting)、[StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram) 名前空間に配置されています。

[S#](../api.md) には、次のためのさまざまなコントロールがあります。

- データ（銘柄、ポートフォリオ、アドレス）の検索と選択。
- 注文の作成。
- 取引所およびその他の情報（約定、注文、トランザクション、オーダーブック、ログなど）の表示。
- チャートの描画。

XAML コードで [S#](../api.md) のグラフィカルコントロールにアクセスするには、対応する名前空間のエイリアスを定義し、そのエイリアスを XAML コードで使用する必要があります。方法は次の例に示されています。

```xaml
<Window x:Class="SampleSmartSMA.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		xmlns:charting="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.XamlStr570}" Height="700" Width="900">
	
	<Grid>
	</Grid>
</Window>
	
```
