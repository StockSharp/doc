# DeMarker

**DeMarker (DeM)** インジケーターは、現在のバーの極値を前のバーの極値と比較することで、買い圧力と売り圧力を評価します。
買われ過ぎおよび売られ過ぎのゾーンを強調し、潜在的な転換点を見つけるのに役立ちます。

このインジケーターを使用するには、[DeMarker](xref:StockSharp.Algo.Indicators.DeMarker) クラスを使用します。

## 計算

1. 各バーについて中間値を計算します。
   `DeMax = max(High − PreviousHigh, 0)`
   `DeMin = max(PreviousLow − Low, 0)`
2. `DeMax` と `DeMin` を、長さ **期間** の移動平均で平滑化します。
3. 最終値を計算します。
   `DeMarker = SMA(DeMax, Length) / (SMA(DeMax, Length) + SMA(DeMin, Length))`.

出力は 0 から 1 の間に正規化されます。

## パラメーター

- **期間** — インジケーターの応答性を制御する平滑化期間。

## 解釈

- **0.7 超** — 買われ過ぎの状態で、下方向への調整の可能性があります。
- **0.3 未満** — 売られ過ぎの状態で、上方向への反転の可能性があります。
- 価格とインジケーターの間の **ダイバージェンス** は、トレンド変化を警告します。

DeMarker は、逆張りエントリーだけでなく、モメンタムオシレーターからのシグナル確認にも使用できます。

![DeMarker のチャート](../../../../images/indicator_demarker.png)

## 関連項目

[RSI](rsi.md)
[ストキャスティクスオシレーター](stochastic_oscillator.md)
[モメンタム](momentum.md)

