# ATR

**Average True Range (ATR)** は、現在のボラティリティの水準を示すインジケーターです。

このインジケーターを使用するには、[AverageTrueRange](xref:StockSharp.Algo.Indicators.AverageTrueRange) クラスを使用する必要があります。
##### インジケーターの計算
  
インジケーターの計算は True Range (TR) の算出から始まります。TR は、次の 3 つの値の最大値として計算されます。  
- 現在の高値と安値の差。  
- 現在の高値と前回の終値の差（絶対値）。  
- 現在の安値と前回の終値の差（絶対値）。  

TRt = max(High(t)-Low(t); High(t) - Close(t-1); Close(t-1)-Low(t))  

価格変動の方向ではなく 2 点間の距離に関心があるため、正の値を保証する目的で絶対値が使用されます。  
  
この指標に基づいて ATR が計算されます。ATR には期間 N という 1 つのパラメーターだけがあります。既定では 14 期間のインジケーターが使用されますが、自分の戦略に合わせて調整できます。式は次のようになります（これは指数移動平均の形式の 1 つです）。  
  
ATR(t) = ((ATR(t-1) x (N-1)) + TR(t)) / N  

![IndicatorAverageTrueRange](../../../../images/indicatoraveragetruerange.png)

## 関連項目

[AO](ao.md)

