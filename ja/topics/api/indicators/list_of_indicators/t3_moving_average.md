# T3MA

**T3移動平均 (T3MA)** は、Tim Tillson によって開発された高度な移動平均の一種です。T3 は、ボリュームファクターを持つ 3 重に平滑化された指数移動平均 (EMA) を表し、従来の移動平均と比べてより滑らかで、だましシグナルが発生しにくくなります。

このインジケーターを使用するには、[T3MovingAverage](xref:StockSharp.Algo.Indicators.T3MovingAverage) クラスを使用する必要があります。

## 説明

T3移動平均は、遅行やだましシグナルなど、従来の移動平均の欠点を解消するために開発されました。複数回の平滑化と調整可能なボリュームファクターにより、T3MA は価格トレンドをより正確に追従する滑らかな曲線を提供します。

T3MA の主な利点:
- 通常の移動平均と比べて遅行が少ない
- だましシグナルが少ない、より滑らかな曲線
- 調整可能なボリュームファクターにより、さまざまな市場環境に適応可能

T3MA は次の用途に使用できます。
- トレンド方向の判定
- 価格がインジケーターラインを交差したときのエントリーポイントとイグジットポイントの検出
- 異なる期間を持つ複数の T3MA のクロスオーバーに基づくトレーディングシステムの構築

## パラメーター

- **出来高係数** - 平滑化の度合いを決定するボリュームファクターです (通常は 0 から 1 の値で、推奨値は 0.7)。
- **期間** - 通常の移動平均における期間と同様の計算期間です。

## 計算

T3移動平均の計算は、いくつかの手順で実行されます。

1. 同じ期間で 6 つの連続する指数移動平均を計算します。
   ```
   EMA1 = EMA(Price, Length)
   EMA2 = EMA(EMA1, Length)
   EMA3 = EMA(EMA2, Length)
   EMA4 = EMA(EMA3, Length)
   EMA5 = EMA(EMA4, Length)
   EMA6 = EMA(EMA5, Length)
   ```

2. 得られた EMA とボリュームファクターに基づいて T3 を計算します。
   ```
   c1 = -VolumeFactor^3
   c2 = 3 * VolumeFactor^2 + 3 * VolumeFactor^3
   c3 = -6 * VolumeFactor^2 - 3 * VolumeFactor - 3 * VolumeFactor^3
   c4 = 1 + 3 * VolumeFactor + VolumeFactor^3 + 3 * VolumeFactor^2
   
   T3 = c1 * EMA6 + c2 * EMA5 + c3 * EMA4 + c4 * EMA3
   ```

VolumeFactor = 0 の場合、T3 は EMA3 (三重 EMA) と同等になります。VolumeFactor = 1 の場合、T3 は最大限に平滑化されます。

![T3MA のチャート](../../../../images/indicator_t3_moving_average.png)

## 関連項目

[EMA](ema.md)
[DEMA](dema.md)
[TEMA](tema.md)
