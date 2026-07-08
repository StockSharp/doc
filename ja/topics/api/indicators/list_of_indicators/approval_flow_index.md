# AFI

**Approval Flow Index (AFI)** は、出来高と価格変動の関係に基づいてトレンドの強さを測定するインジケーターです。

このインジケーターを使用するには、[ApprovalFlowIndex](xref:StockSharp.Algo.Indicators.ApprovalFlowIndex) クラスを使用する必要があります。

## 説明

Approval Flow Index (AFI) は、市場における注文フローの強度を評価し、現在のトレンドの強さを判断するのに役立ちます。このインジケーターは、取引出来高と価格変動の関係を分析して、潜在的な反転ポイントを特定したり、トレンド継続を確認したりします。

AFI インジケーターは次の用途に使用できます。
- 現在のトレンドの強さの判断
- 価格とインジケーター間のダイバージェンスの特定
- 潜在的な市場反転ポイントの探索

## パラメーター

このインジケーターには次のパラメーターがあります。
- **Length** - インジケーターの計算期間

## 計算

Approval Flow Index の計算は、特定期間における価格変化と出来高の分析に基づきます。

1. まず、期間の価格変化を計算します
2. 次に、この変化を取引出来高と関連付けます
3. 得られた値を選択した期間（Length パラメーター）にわたって合計します

AFI は、どれだけの取引出来高が価格変動を「承認」しているかを判断することを目的としています。

正の AFI 値は強い上昇トレンドを示し、負の値は下降トレンドを示唆します。ゼロに近い値は、明確なトレンドが存在しないことを示す場合があります。

![indicator_approval_flow_index](../../../../images/indicator_approval_flow_index.png)

## 関連項目

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
