# WAD

**ウィリアムズ・アキュムレーション/ディストリビューション (WAD)** は、Larry Williams によって開発された出来高インジケーターです。従来の Accumulation/Distribution ラインとは異なり、WAD インジケーターは買い手または売り手の圧力を判断するために、現在期間の終値と前期間の終値の関係に注目します。

このインジケーターを使用するには、[WilliamsAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsAccumulationDistribution) クラスを使用する必要があります。

## 説明

ウィリアムズ・アキュムレーション/ディストリビューション インジケーターは、潜在的なトレンド反転を示す可能性がある価格と出来高の不一致を特定するように設計されています。WAD は、現在の価格変動における弱さを明らかにするのに特に有用です。

WAD の主な特徴:
- 正の値はアキュムレーション (買い圧力) を示します
- 負の値はディストリビューション (売り圧力) を示します
- WAD と価格の間のダイバージェンスは、価格反転に先行する可能性があります

インジケーターの主な用途:
- 現在のトレンドを確認する
- 潜在的な価格反転を特定する
- 買い手または売り手の圧力を判断する

## 計算

ウィリアムズ・アキュムレーション/ディストリビューション インジケーターは、次のロジックを使用して計算されます。

1. 現在期間の真の値幅保護 (TRP) を決定します。
   ```
   TRP = Max(High - Low, |High - Close_prev|, |Low - Close_prev|)
   ```

2. 現在期間の Accumulation/Distribution (AD) 値を計算します。
   - Close > Close_prev の場合 (市場が上昇):
      ```
      AD = Close - Min(Low, Close_prev)
      ```
   - Close < Close_prev の場合 (市場が下落):
      ```
      AD = Close - Max(High, Close_prev)
      ```
   - Close = Close_prev の場合:
      ```
      AD = 0
      ```

3. AD 値を累積して WAD 値を計算します。
   ```
   WAD = 前の WAD 値 + AD
   ```

このインジケーターは正の値と負の値を累積し、価格変動と比較するために使用できる累積ラインを形成します。

![WAD のチャート](../../../../images/indicator_williams_accumulation_distribution.png)

## 関連項目

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
