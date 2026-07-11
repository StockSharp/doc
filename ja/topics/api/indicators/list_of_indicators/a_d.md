# A/D

**Acceleration/Deceleration (A/D)** は Bill Williams によって作成されたオシレーターです。トレンドのモメンタムの加速と減速を測定します。

このインジケーターを使用するには、[Acceleration](xref:StockSharp.Algo.Indicators.Acceleration) クラスを使用します。
##### 計算

A/D ヒストグラムは、推進力の 5/34 ヒストグラムの値と、このヒストグラムから取得した 5 期間単純移動平均との差です。値はクラシックなオシレーター用のものであり、設定では常に独自のパラメーターを指定できます。

中央値価格 = (HIGH + LOW) / 2
AO = SMA (中央値価格, 5) - SMA (中央値価格, 34)
A/D = AO - SMA (AO, 5)

ここで:

中央値価格 - 中央価格。
HIGH - バーの最高価格。
LOW - バーの最低価格。
SMA - 単純移動平均。
AO - [オーサムオシレーター](ao.md) インジケーター。

パラメーターは SMA 期間の値として設定されます。

![A/D のチャート](../../../../images/indicatoracceleration.png)

## 関連項目

[Alligator](alligator.md)
