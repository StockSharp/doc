# Momentum

**Momentum** インジケーターは、一定期間における金融商品の価格変化の大きさを測定します。曲線が最大値または最小値に達したときに、買われ過ぎおよび売られ過ぎの局面を示します。インジケーターに平滑化された移動平均を追加すると、トレンド変化の解釈が向上します。

インジケーターを使用するには、[Momentum](xref:StockSharp.Algo.Indicators.Momentum) クラスを使用します。
##### 計算

Momentum は、現在の価格と n 期間前の価格との比率として定義されます:

MOMENTUM = CLOSE(i) / CLOSE(i - n) * 100

ここで:
CLOSE(i) - 現在のバーの終値;
CLOSE(i - n) - n 本前のバーの終値。


![IndicatorMomentum](../../../../images/indicatormomentum.png)

## 関連項目

[マネーフロー指数](money_flow_index.md)
