# Bollinger Bands

**Bollinger Bands** は、市場のボラティリティを測定するために使用されるオシレーター系インジケーターです。移動平均と比較して価格が高いか低いかを評価できます。中央のバンドは価格の単純移動平均に対応します。上側および下側のバンドは、移動平均に対して価格が高い、または低いと見なせる水準です。

このインジケーターを使用するには、[BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) クラスを使用する必要があります。
##### 計算

Bollinger Bands の計算には、対応する設定を持つ次のパラメーターが使用されます。
- 標準偏差の型 - 通常は double。
- Moving Average の期間 - トレーダーの裁量によります。

したがって、このインジケーターは中央、上側、下側の 3 本のラインで構成され、それぞれに式があります。

Middle Line (ML) = Moving Average (SMA (Close, N))
上側バンド = ML + (D x 標準偏差)
下側バンド = ML - (D x 標準偏差)、ここで

D - 設定で指定されたチャネル幅、標準偏差 (StdDev) - 標準偏差。次の式で計算されます: SQRT(Sum(Close, n))^2, n)/n)、ここで
Sum - n 期間の合計、n - 計算期間、SQRT - 平方根、Close - 終値。

![IndicatorBollingerBands](../../../../images/indicatorbollingerbands.png)

## 関連項目

[CHV](chv.md)
