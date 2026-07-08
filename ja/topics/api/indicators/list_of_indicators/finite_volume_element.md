# FVE

**Finite Volume Element (FVE)** は、価格と出来高の関係を分析するために開発されたテクニカルインジケーターで、市場における買い手と売り手の圧力を評価するのに役立ちます。

このインジケーターを使用するには、[FiniteVolumeElement](xref:StockSharp.Algo.Indicators.FiniteVolumeElement) クラスを使用する必要があります。

## 説明

Finite Volume Element (FVE) は、価格変動の潜在的な強さを判断するために、価格変化と取引出来高の関係を分析します。これは、価格変化は対応する出来高によって確認されるときに最も重要である、という前提に基づいています。

FVE インジケーターは、出来高で重み付けされた価格変化をオシレーターに変換し、市場における買い手と売り手の相対的なバランスを判断するのに役立ちます。正の FVE 値は買い手優勢を示し、負の値は売り手優勢を示します。

FVE は特に次の用途に役立ちます:
- 現在のトレンドの強さと持続性の評価
- 潜在的な反転ポイントの識別
- 価格と出来高の間のダイバージェンスの判断
- 他のインジケーターからのシグナルの確認

## パラメーター

このインジケーターには次のパラメーターがあります:
- **Length** - 平滑化期間 (デフォルト値: 22)

## 計算

FVE インジケーターの計算には複数の手順が含まれます:

1. 現在および前の期間の典型価格を計算します:
   ```
   Typical Price = (High + Low + Close) / 3
   ```

2. 典型価格の変化を計算します:
   ```
   Price Change = Typical Price[current] - Typical Price[previous]
   ```

3. 出来高加重価格変化を計算します:
   ```
   Volume-Weighted Price Change = Price Change * Volume[current]
   ```

4. 市場規模を考慮して正規化します:
   ```
   Normalized Value = Volume-Weighted Price Change / (Average Volume over period * Price Volatility)
   ```

5. 累積合計と平滑化を行います:
   ```
   FVE = SMA(Cumulative Sum of Normalized Values, Length)
   ```

ここで:
- High, Low, Close - 最高値、最安値、終値
- Volume - 取引出来高
- SMA - 単純移動平均
- Length - 平滑化期間

## 解釈

FVE インジケーターは次のように解釈できます:

1. **ゼロラインのクロスオーバー**:
   - 負の値から正の値への移行は買い手圧力の増加を示し、強気シグナルと見なすことができます
   - 正の値から負の値への移行は売り手圧力の増加を示し、弱気シグナルと見なすことができます

2. **極端な値**:
   - 高い正の値 (+3 を超える) は、市場が買われ過ぎの状態にある可能性を示します
   - 高い負の値 (-3 を下回る) は、市場が売られ過ぎの状態にある可能性を示します

3. **ダイバージェンス**:
   - 強気ダイバージェンス (価格が新安値を形成する一方で、FVE がより高い安値を形成する) は、潜在的な上方反転を示す可能性があります
   - 弱気ダイバージェンス (価格が新高値を形成する一方で、FVE がより低い高値を形成する) は、潜在的な下方反転を示す可能性があります

4. **トレンド確認**:
   - 一貫して正の FVE 値は、上昇トレンドの強さを確認します
   - 一貫して負の FVE 値は、下降トレンドの強さを確認します

5. **FVE の変化率**:
   - FVE 値の急速な増加または減少は、強い価格変動のモメンタムを示す場合があります
   - FVE 値の変化の鈍化は、モメンタム減速の可能性を示す場合があります

6. **サポート水準とレジスタンス水準**:
   - FVE チャート上の過去の反転ポイントは、将来の反転の目安として機能することがあります

![indicator_finite_volume_element](../../../../images/indicator_finite_volume_element.png)

## 関連項目

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ForceIndex](force_index.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)

