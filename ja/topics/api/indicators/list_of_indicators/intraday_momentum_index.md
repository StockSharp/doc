# IMI

**Intraday Momentum Index (IMI)** は、Tushar Chande によって開発されたテクニカル指標で、日中価格形成の原理と RSI の概念を組み合わせて、日中モメンタムを測定します。

この指標を使用するには、[IntradayMomentumIndex](xref:StockSharp.Algo.Indicators.IntradayMomentumIndex) クラスを使用する必要があります。

## 説明

Intraday Momentum Index (IMI) は、古典的な Relative Strength Index (RSI) の修正版として作成され、日中の市場動態の分析に特化して適応されています。従来の RSI のように連続する終値を使用する代わりに、IMI は各期間の終値と始値を比較します。

IMI は、指定された期間において、終値が始値をどのくらいの頻度と強さで上回るか (正のモメンタム)、または始値を下回るか (負のモメンタム) を評価します。これにより、日中変動の優勢な方向と強さを特定できます。

この指標は特に次の用途に役立ちます。
- 日中の市場方向を判断する
- 潜在的な反転ポイントを特定する
- 買われ過ぎおよび売られ過ぎ水準を判断する
- 価格とモメンタムの間のダイバージェンスを検出する

## パラメーター

この指標には次のパラメーターがあります。
- **Length** - 計算期間 (デフォルト値: 14)

## 計算

Intraday Momentum Index の計算には、次の手順が含まれます。

1. 日中の価格変動を決定します。
   ```
   Gain = Close - Open, if Close > Open
   Loss = Open - Close, if Close < Open
   ```

2. Length 期間における正および負の変動の合計を計算します。
   ```
   Sum Gains = Length 期間のすべての Gains の合計
   Sum Losses = Length 期間のすべての Losses の合計
   ```

3. RSI に似た式を使用して IMI を計算します。
   ```
   IMI = 100 * (Sum Gains / (Sum Gains + Sum Losses))
   ```

注: (Sum Gains + Sum Losses) がゼロに等しい場合、ゼロ除算を避けるために IMI は 50 に設定されます。

## 解釈

Intraday Momentum Index は RSI と同様に解釈されます。

1. **値の範囲**:
   - IMI は 0 から 100 の間で振動します
   - 50 を超える値は、正の日中モメンタムが優勢であることを示します
   - 50 未満の値は、負の日中モメンタムが優勢であることを示します

2. **買われ過ぎおよび売られ過ぎ水準**:
   - 70 を超える値は、通常、市場の買われ過ぎ状態を示すと見なされます
   - 30 未満の値は、通常、市場の売られ過ぎ状態を示すと見なされます

3. **センターラインのクロスオーバー**:
   - 50 ラインを下から上へ交差する場合、強気シグナルと見なすことができます
   - 50 ラインを上から下へ交差する場合、弱気シグナルと見なすことができます

4. **ダイバージェンス**:
   - 強気ダイバージェンス: 価格が新安値を形成する一方で、IMI はより高い安値を形成します
   - 弱気ダイバージェンス: 価格が新高値を形成する一方で、IMI はより低い高値を形成します

5. **フェイルドスイング**:
   - 上昇トレンド中に IMI が買われ過ぎ水準に到達できない場合、トレンドの弱さを示す可能性があります
   - 下降トレンド中に IMI が売られ過ぎ水準に到達できない場合、トレンドの弱さを示す可能性があります

6. **トレンド確認**:
   - 50 を超える IMI 値が持続する場合、上昇トレンドを確認します
   - 50 未満の IMI 値が持続する場合、下降トレンドを確認します

![indicator_intraday_momentum_index](../../../../images/indicator_intraday_momentum_index.png)

## 関連項目

[RSI](rsi.md)
[IntradayIntensityIndex](intraday_intensity_index.md)
[Momentum](momentum.md)
[RelativeMomentumIndex](relative_momentum_index.md)
