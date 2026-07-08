# WVAD

**Williams Variable Accumulation Distribution (WVAD)** は、Larry Williams によって開発された累積出来高インジケーターです。始値、終値、高値、安値、取引出来高の関係を分析することで、買い圧力と売り圧力を評価します。

このインジケーターを使用するには、[WilliamsVariableAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsVariableAccumulationDistribution) クラスを使用します。

## 説明

WVAD インジケーターは、各バー内で買い手または売り手が価格変動をどの程度支配しているかを測定し、この値を出来高で重み付けします。終値が始値より高い場合は買い手優勢を示し、その逆も同様です。High-Low レンジは正規化係数として使用されます。

インジケーターの主な用途:
- 現在のトレンドを確認する
- インジケーターと価格の間のダイバージェンスを特定する
- 買い圧力または売り圧力を判断する
- 出来高を考慮して価格変動の強さを評価する

## 計算

WVAD インジケーターは、次の式を使用して計算されます。

```
WVAD = WVAD(previous) + ((Close - Open) / (High - Low)) * Volume
```

ここで:
- Close - 現在期間の終値
- Open - 現在期間の始値
- High - 現在期間の最高値
- Low - 現在期間の最安値
- Volume - 現在期間の取引出来高
- WVAD(previous) - 前のインジケーター値

High = Low の場合 (レンジがゼロの場合)、その期間の値は加算されません。

このインジケーターは累積型です。値は新しい期間ごとに累積されます。

## 関連項目

[WAD](williams_accumulation_distribution.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)

