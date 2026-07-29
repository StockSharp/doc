# %R

**ウィリアムズ %R (%R, ウィリアムズ・パーセントレンジ)** は、0 から -100 の間で変動し、買われ過ぎおよび売られ過ぎの水準を表示するモメンタムインジケーターです。

このインジケーターを使用するには、[WilliamsR](xref:StockSharp.Algo.Indicators.WilliamsR) クラスを使用する必要があります。
##### 計算

ウィリアムズ・パーセントレンジインジケーターの計算式は、ストキャスティクスオシレーターの計算に使用されるものと似ています。

%R = - (MAX(HIGH(i - n)) - CLOSE(i)) / (MAX(HIGH(i - n)) - MIN(LOW(i - n))) * 100

ここで:

CLOSE(i) - 今日の終値。
MAX(HIGH(i - n)) - 過去 n 期間の最高値の最大値。
MIN(LOW(i - n)) - 過去 n 期間の最安値の最小値。

n の値はインジケーターのパラメーターとして設定されます。

![%R のチャート](../../../../images/indicatorwilliamsr.png)

## 関連項目

[ZigZag](zigzag.md)
