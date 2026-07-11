# ADL

**アキュムレーション/ディストリビューションライン (ADL)** は、Mark Chaikin によって開発された出来高インジケーターです。このインジケーターは、価格と出来高の相関を分析することで、市場における供給と需要の関係を評価します。

このインジケーターを使用するには、[AccumulationDistributionLine](xref:StockSharp.Algo.Indicators.AccumulationDistributionLine) クラスを使用する必要があります。

## 説明

アキュムレーション/ディストリビューションライン は、出来高と価格を使用して、証券がアキュムレーション（買い）局面にあるのか、ディストリビューション（売り）局面にあるのかを判定する累積インジケーターです。

ADL インジケーターは、トレンドの確認や潜在的な反転の警告に役立ちます。
- 価格が上昇している一方で ADL が下落している場合、上昇トレンドの弱さを示している可能性があります。
- 価格が下落している一方で ADL が上昇している場合、下降トレンドの潜在的な反転を示している可能性があります。

## 計算

アキュムレーション/ディストリビューションライン の計算は 2 つの手順で行われます。

**1. Volume Multiplier (CLV - Close Location Value) の計算:**
```
CLV = ((Close - Low) - (High - Close)) / (High - Low)
```

**2. ADL の計算:**
```
ADL = Previous ADL Value + CLV * Volume
```

ここで:
- Close - 期間の終値
- Low - 期間の最小価格
- High - 期間の最大価格
- Volume - 期間の取引出来高

(High - Low) が 0 に等しい場合、CLV は 0 に設定されます。

![IndicatorAccumulationDistributionLine](../../../../images/indicator_accumulation_distribution_line.png)

## 関連項目

[OBV](on_balance_volume.md)
