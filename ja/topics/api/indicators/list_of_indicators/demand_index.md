# DI

**Demand Index (DI)** は、James Sibbett によって開発されたテクニカルインジケーターで、市場における需要の強さと買い圧力を評価するために、価格と出来高の関係を分析します。

このインジケーターを使用するには、[DemandIndex](xref:StockSharp.Algo.Indicators.DemandIndex) クラスを使用する必要があります。

## 説明

Demand Index (DI) は、価格と出来高の関係を評価し、売り圧力と比べて買い圧力（需要）がどれほど強いかを判断する総合的な出来高インジケーターです。このインジケーターは、価格変化と出来高変化の比率により、価格または出来高を個別に観察するだけの場合よりも、市場需要をより正確に評価できるという前提に基づいています。

DI は、次の市場状況を特定することを目的としています。
- 強い需要（買い圧力）
- 弱い需要（売り圧力）
- 価格と出来高の不均衡（潜在的な反転ポイント）
- 現在のトレンドの確認または否定

## パラメーター

このインジケーターには次のパラメーターがあります。
- **Length** - 計算期間（既定値: 13）

## 計算

Demand Index の計算はかなり複雑で、複数の段階を含みます。

1. 価格変化に基づく価格構成要素を計算します。
   ```
   Price Component = ((High + Low + Close) / 3) - ((Previous High + Previous Low + Previous Close) / 3)
   ```

2. 相対的な出来高変化を考慮して、出来高構成要素を計算します。

3. 価格構成要素と出来高構成要素の比率として需要を計算します。
   ```
   Raw Demand = Price Component / Volume Component
   ```

4. ノイズを減らすために、得られた値を平滑化します。
   ```
   Smoothed Demand = EMA(Raw Demand, Length)
   ```

5. 最終的なインデックスを得るために、結果を正規化します。
   ```
   Demand Index = 100 * Normalized(Smoothed Demand)
   ```

## 解釈

Demand Index はさまざまな方法で解釈できます。

1. **極端な水準**:
   - 高い正の値は、強い需要（買い圧力）を示します
   - 高い負の値は、弱い需要（売り圧力）を示します

2. **ゼロラインの交差**:
   - 下から上への交差は、強気シグナルと見なすことができます
   - 上から下への交差は、弱気シグナルと見なすことができます

3. **ダイバージェンス**:
   - 強気ダイバージェンス: 価格が新しい安値を形成する一方で、DI はより高い安値を形成します
   - 弱気ダイバージェンス: 価格が新しい高値を形成する一方で、DI はより低い高値を形成します

4. **DI のトレンド**:
   - 持続的な正の DI 値は、上昇トレンドを確認します
   - 持続的な負の DI 値は、下降トレンドを確認します

5. **極端な値**:
   - 非常に高い値または非常に低い値は、市場が買われ過ぎまたは売られ過ぎの状態にあることを示す場合があります

Demand Index は、誤シグナルを除外するために、他のインジケーターや分析手法と組み合わせて使用すると最も効果的です。

![indicator_demand_index](../../../../images/indicator_demand_index.png)

## 関連項目

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[BalanceOfPower](balance_of_power.md)
