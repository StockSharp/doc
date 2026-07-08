# DPO

**Detrended Price Oscillator (DPO)** は、ピークからピーク、またはボトムからボトムまでの価格サイクルの期間を推定するために、価格トレンドを除去するオシレーターです。ストキャスティクス収束や移動平均収束拡散法（MACD）などの他のオシレーターとは異なり、DPO はモメンタム指標ではありません。価格のピークとボトムを強調し、それらはエントリーおよびイグジットポイントの推定に使用されます。

この指標を使用するには、[DetrendedPriceOscillator](xref:StockSharp.Algo.Indicators.DetrendedPriceOscillator) クラスを使用する必要があります。

Detrended Price Oscillator は、現在の価格値から単純移動平均（SMA）を差し引いて計算されます。移動平均の長さはユーザーが決定します。

![IndicatorDetrendedPriceOscillator](../../../../images/indicatordetrendedpriceoscillator.png)

## 関連項目

[DMI](dmi.md)

