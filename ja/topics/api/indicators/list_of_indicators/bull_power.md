# ブルパワー

**ブルパワー** は、エルダー・レイシステムにおける強気側の対応要素です。バーの高値を平均価格と比較することで、買い手が価格を指数移動平均 (EMA) よりどれほど強く押し上げているかを測定します。

このインジケーターを使用するには、[BullPower](xref:StockSharp.Algo.Indicators.BullPower) クラスを使用します。

## 説明

このインジケーターは次の式を使用します。

`ブルパワー = High - EMA`。

- 正の値は強気の圧力を確認し、上昇トレンドを支えます。
- 値がゼロ方向またはゼロを下回る方向へ低下する場合、強気側の弱まりを示します。
- 極端なピークは、特に EMA が下向きの場合、調整に先行することがあります。

## パラメーター

ブルパワー は [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage) からパラメーターを継承します。

- **Length** - EMA 期間。
- **Alpha**（任意） - 該当する場合の平滑化係数。

## 使用方法

- 上昇する ブルパワー と上昇する EMA は、あわせてトレンドの強さを確認します。
- 価格が新高値を付けている一方で ブルパワー の読み取り値がより高くならない場合、弱気ダイバージェンスが形成されます。
- ブルパワー と ベアパワー を価格 EMA と組み合わせて、完全な [エルダー・レイ](elder_ray.md) 構造を評価します。

![indicator_bull_power](../../../../images/indicator_bull_power.png)

## 関連項目

[ベアパワー](bear_power.md)
[エルダー・レイ](elder_ray.md)
[指数移動平均](ema.md)
