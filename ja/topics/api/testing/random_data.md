# ランダムデータ

ランダムデータでのテストは、特別な種類のテストです。これは最適なパラメーターを探索するためのものではありません。代わりに、取引アルゴリズムをさまざまな取引所シナリオにさらすことで、コード内のエラーを検出できます。

一般に、アルゴリズム開発では一定のシナリオセットだけが使用されます。そのため、特殊な状況が発生した場合、アルゴリズムが正しく反応しなかったり、例外をスローしたりする可能性があります。たとえば、次のような状況が発生することがあります。

- ストラテジーが [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) ローソク足を扱い、各反復で要求された期間のローソク足が常に存在すると想定している場合。取引が 1 件もない状態で期間が始まると、ローソク足は作成されません。その結果、適切な処理がない場合、[NullReferenceException](xref:System.NullReferenceException) がスローされ、ストラテジーは停止します。
- ストラテジーが流動性の低い銘柄を扱い、[IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) を使用している場合。ストラテジーは、板情報が常に埋まっていると想定しています。ある時点で、板情報が半分だけ埋まっていることがあります (たとえば、買気配はあるが売気配がない)。ストラテジーがこの状況を想定していない場合、注文を誤って登録するか、例外をスローしてストラテジーが停止します。
- ストラテジーが価格レベルを計算する場合。コードは、事前に設定されたレベルが突破されるのをストラテジーが待つように書かれています。レベルが計算され、正しく設定されていない場合、それらは決して突破されないか、常にそのうち 1 つだけが突破されます。その結果、ストラテジーは取引をまったく行わないか、それらの取引で損失を出します。

これらのシナリオや、事前に予測できないその他多くの取引所動作シナリオに対して、[S#](../../api.md) はランダムデータでのテストを提供します。これは、一様な一意性により、短い範囲で最大数の条件を生成できます。

## 移動平均ストラテジーのランダムデータテスト

1. SampleRandomEmulation の例 (*..Samples\/Testing\/SampleRandomEmulation*) は、SampleHistoryTesting の例とほぼ同じです (その説明は[履歴データでのテスト](historical_data.md)セクションにあります)。これは、統一された [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) クラスを使用しているためです。ただし、[履歴データでのテスト](historical_data.md)とは異なり、ランダムデータでのテストでは市場データは読み込まれず、「オンザフライ」で生成されます。そのため、この例には、板情報用とティック約定用の 2 つのランダムデータジェネレーターが追加されています。SampleHistoryTesting では、履歴が保存されていないため、板情報用の 1 つのジェネレーターだけが使用されます。

   ```cs
   _connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
   {
       IsSubscribe = true,
       Generator = new RandomWalkTradeGenerator(new SecurityId { SecurityCode = security.Code })
       {
           Interval = TimeSpan.FromSeconds(1),
           MaxVolume = maxVolume,
           MaxPriceStepCount = 3,	
           GenerateOriginSide = true,
           MinVolume = minVolume,
           RandomArrayLength = 99,
       }
   });
   _connector.SubscribeMarketDepth(new TrendMarketDepthGenerator(_connector.GetSecurityId(security)) { GenerateDepthOnEachTrade = false });
   ```
2. この例の実行結果は次のとおりです。![sampleemulationtest](../../../images/sample_emulation_test.png)
