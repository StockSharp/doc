# RMI

**相対モメンタム指数 (RMI)** は、Roger Altman によって提案された従来の RSI 指標の修正版です。一定期間における価格の上昇と下落の比率を計算する古典的な RSI とは異なり、RMI は選択されたモメンタム期間における相対的な価格変化を考慮します。

この指標を使用するには、[RelativeMomentumIndex](xref:StockSharp.Algo.Indicators.RelativeMomentumIndex) クラスを使用する必要があります。

## 説明

相対モメンタム指数 (RMI) は、モメンタム期間パラメーターを追加することで古典的な RSI を改善します。これにより、トレーダーは主計算期間を変更せずに指標の感度を調整できます。

RSI と同様に、RMI は 0 から 100 の間で振動します。
- 70 を上回る値は通常、買われ過ぎの市場を示します
- 30 を下回る値は、売られ過ぎの市場を示します
- 50 の中心線は、主要な市場の動きの方向を判断するための基準点として機能します

RMI は、潜在的なトレンド反転点の特定と、現在のトレンドの強さの確認に特に有用です。

## パラメーター

- **MomentumPeriod** - 価格比較の時間ラグを定義するモメンタム期間。
- **Length** - 指標を計算するための主期間（RSI の期間に類似）。

## 計算

RMI の計算は複数のステップで実行されます。

1. 現在の価格と n 期間前の価格との差としてモメンタムを計算します。
   ```
   Momentum = Price(current) - Price(current - MomentumPeriod)
   ```

2. モメンタムを正（U）と負（D）に分けます。
   ```
   Momentum > 0 の場合、U = Momentum, D = 0
   Momentum < 0 の場合、U = 0, D = |Momentum|
   ```

3. 指定された期間における正および負のモメンタムの平均値を計算します。
   ```
   AverageU = SMA(U, Length)
   AverageD = SMA(D, Length)
   ```

4. 相対力を計算します。
   ```
   RS = AverageU / AverageD
   ```

5. 相対モメンタム指数 に変換します。
   ```
   RMI = 100 - (100 / (1 + RS))
   ```

![RMI のチャート](../../../../images/indicator_relative_momentum_index.png)

## 関連項目

[RSI](rsi.md)
[モメンタム](momentum.md)
