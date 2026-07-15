# ALMA

**Arnaud Legoux移動平均 (ALMA)** は Arnaud Legoux によって開発されたインジケーターで、市場ノイズを除去し、シグナルの遅延を減らすよう最適化されています。

このインジケーターを使用するには、[ArnaudLegouxMovingAverage](xref:StockSharp.Algo.Indicators.ArnaudLegouxMovingAverage) クラスを使用する必要があります。

## 説明

ALMA は、2 つのデータ平滑化アプローチの利点を組み合わせています。
1. 市場ノイズの除去（ほとんどの移動平均と同様）
2. 遅延の最小化（多くの平滑化インジケーターに典型的）

ALMA インジケーターは重み関数として正規（Gaussian）分布を使用し、offset パラメーターと sigma パラメーターによって細かく調整できます。これにより、テクニカル分析において非常に柔軟で効果的なツールになります。

ALMA は次の用途に使用されます。
- 現在のトレンドの判断
- 反転ポイントの特定
- クロスオーバーに基づく取引システムの作成

## パラメーター

このインジケーターには次のパラメーターがあります。
- **期間** - 計算期間（分析するローソク足の数）
- **Sigma** - sigma。Gaussian 曲線の形状を制御するパラメーター（推奨値: 6）
- **オフセット** - offset。平滑化と応答速度を制御するパラメーター（推奨値: 0.85）

## 計算

ALMA の計算はいくつかの段階で行われます。

1. 正規（Gaussian）分布に基づき、ウィンドウ内の各データポイントの重みを決定します。
   ```
   m = floor(Offset * (Length - 1))
   s = Length / Sigma
   
   0 から Length-1 までの各 i について:
   w(i) = exp(-((i - m)^2) / (2 * s^2))
   ```

2. 重みを正規化します。
   ```
   Sum_of_weights = すべての w(i) の合計
   
   0 から Length-1 までの各 i について:
   w_norm(i) = w(i) / Sum_of_weights
   ```

3. ALMA を加重和として計算します。
   ```
   ALMA = sum(Price(t-i) * w_norm(i))（0 から Length-1 までのすべての i）
   ```

ここで:
- Length - ALMA の期間
- Offset - offset パラメーター（0 から 1）
- Sigma - sigma パラメーター（通常は 2 から 8）

![ALMA のチャート](../../../../images/indicator_arnaud_legoux_moving_average.png)

## 関連項目

[SMA](sma.md)
[EMA](ema.md)
[T3MA](t3_moving_average.md)
[ZLEMA](zero_lag_exponential_moving_average.md)
