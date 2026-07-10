# GAPO

**Gopalakrishnan Range Index (GAPO)** は、対数スケールを使用して市場のボラティリティを測定するために Tushar Gopalakrishnan によって開発されたテクニカルインジケーターです。

このインジケーターを使用するには、[GopalakrishnanRangeIndex](xref:StockSharp.Algo.Indicators.GopalakrishnanRangeIndex) クラスを使用する必要があります。

## 説明

Gopalakrishnan Range Index (GAPO) は、対数スケールを使用して特定期間にわたる全体の価格レンジを測定するボラティリティインジケーターです。これは Tushar Gopalakrishnan によって開発され、"Technical Analysis of Stocks & Commodities" 誌で発表されました。

GAPO は、指定された期間における最高価格と最低価格の間の対数比を測定することで、極端な市場変動を評価します。このアプローチにより、特に急激な価格変動の期間中に、インジケーターはボラティリティの増加をより正確に反映できます。

GAPO インジケーターは特に次の用途に役立ちます:
- 高ボラティリティ期間と低ボラティリティ期間の識別
- 極端な動きの後の潜在的な反転ポイントの検出
- 他のボラティリティベースのインジケーターのパラメーター調整
- 現在の市場状況に合わせた取引戦略の適応

## パラメーター

このインジケーターには次のパラメーターがあります:
- **Length** - 計算期間 (デフォルト値: 10)

## 計算

Gopalakrishnan Range Index の計算は非常に単純です:

```
GAPO = log(N) * log(Highest High - Lowest Low)
```

ここで:
- log - 自然対数
- N - 期間数 (Length)
- Highest High - Length 期間内の最高高値
- Lowest Low - Length 期間内の最低安値

## 解釈

Gopalakrishnan Range Index は次のように解釈できます:

1. **絶対値**:
   - 高い GAPO 値は高ボラティリティの期間を示します
   - 低い GAPO 値は低ボラティリティの期間を示します
   - 極端に高い値は、市場の過度な伸びと潜在的な反転の可能性を示す場合があります

2. **GAPO のトレンド**:
   - GAPO 値の上昇はボラティリティの増加を示します
   - GAPO 値の低下はボラティリティの低下を示します
   - GAPO の急上昇は、新しいトレンドの動きの始まりを示す場合があります

3. **相対水準**:
   - 現在の GAPO 値を過去の水準と比較することで、相対的なボラティリティを評価できます
   - 過去レンジの 95 パーセンタイルを上回る値は、極端なボラティリティを示す場合があります
   - 過去レンジの 5 パーセンタイルを下回る値は、異常に低いボラティリティを示す場合があります

4. **取引戦略**:
   - 高ボラティリティ期間 (高い GAPO 値) では、ストップロスと利益目標の幅を広げることが適切な場合があります
   - 低ボラティリティ期間 (低い GAPO 値) では、レンジ取引戦略がより適している場合があります
   - 極端な GAPO 値は、反転ポイントを見つけるための逆張りインジケーターとして使用できます

5. **他のインジケーターとの組み合わせ**:
   - GAPO は他のインジケーターからのシグナルをフィルタリングするために使用できます
   - 高ボラティリティ期間中は、トレンドインジケーターのシグナルの信頼性が高くなる場合があります
   - 低ボラティリティ期間中は、オシレーターのシグナルがより効果的な場合があります

![indicator_gopalakrishnan_range_index](../../../../images/indicator_gopalakrishnan_range_index.png)

## 関連項目

[ATR](atr.md)
[ChoppinessIndex](choppiness_index.md)
[真の値幅](true_range.md)
