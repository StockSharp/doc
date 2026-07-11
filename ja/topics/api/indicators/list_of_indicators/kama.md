# KAMA

**Kaufman適応移動平均 (AMA, KAMA, AMkA)** は、市場ノイズとボラティリティを考慮するために Perry Kaufman によって開発されました。KAMA インジケーターは、時間上の反転ポイントを特定し、全体的なトレンドを判断し、価格変動をフィルタリングするために使用できます。

インジケーターを使用するには、[KaufmanAdaptiveMovingAverage](xref:StockSharp.Algo.Indicators.KaufmanAdaptiveMovingAverage) クラスを使用する必要があります。

##### インジケーター設定。

- 高速 MA - 高速平滑化定数。
- 低速 MA - 低速平滑化定数。
- Period - Kaufman 移動平均の期間。

![KAMA のチャート](../../../../images/indicatorkaufmanadaptivemovingaverage.png)
