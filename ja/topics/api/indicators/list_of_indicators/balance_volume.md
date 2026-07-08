# BV

**Balance Volume (BV)** は、価格変化に基づいて出来高の蓄積と分配を追跡するテクニカルインジケーターです。

このインジケーターを使用するには、[BalanceVolume](xref:StockSharp.Algo.Indicators.BalanceVolume) クラスを使用する必要があります。

## 説明

Balance Volume (BV) インジケーターは、価格変化と出来高の関係を分析するために設計されています。出来高の変化が価格の動きとどのように対応しているかをトレーダーが判断するのに役立ち、これにより現在のトレンドの強さまたは弱さを示すことができます。

BV の主な考え方は、出来高が価格方向を確認すべきだというものです。出来高の増加を伴って価格が上昇する場合、これは強い上昇トレンドを示します。逆に、出来高の増加を伴って価格が下落する場合、これは強い下降トレンドを示唆します。

BV インジケーターは特に次の用途に有用です。
- 現在のトレンドの強さを確認する
- 潜在的なトレンド反転を特定する
- 価格と出来高の間のダイバージェンスを検出する
- 蓄積と分配の水準を判断する

## 計算

Balance Volume インジケーターの計算は、終値を前回の終値と比較し、出来高で重み付けすることに基づいています。

```
If Close > Previous Close:
	BV = Previous BV + Volume
If Close < Previous Close:
	BV = Previous BV - Volume
If Close = Previous Close:
	BV = Previous BV
```

ここで:
- Close - 現在の終値
- Previous Close - 前回の終値
- Volume - 現在の出来高
- Previous BV - 前回の Balance Volume インジケーター値

## 解釈

- **価格上昇を伴う BV の上昇** - 上昇トレンドの確認であり、買い手の強い関心を示します
- **価格下落を伴う BV の低下** - 下降トレンドの確認であり、売り手の強い関心を示します
- **価格が横ばいまたは下落している中での BV の上昇** - 潜在的な蓄積であり、上方反転に先行する可能性があります
- **価格が横ばいまたは上昇している中での BV の低下** - 潜在的な分配であり、下方反転に先行する可能性があります
- **BV と価格の間のダイバージェンス** - 反転の可能性に対する警告です。
  - BV が低下する一方で価格が上昇している場合、すばやい下方反転が差し迫っている可能性があります
  - BV が上昇する一方で価格が下落している場合、すばやい上方反転が差し迫っている可能性があります

![indicator_balance_volume](../../../../images/indicator_balance_volume.png)

## 関連項目

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
