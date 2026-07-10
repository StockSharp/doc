# WCCI

**Woodies CCI (WCCI)** は、トレーダーの Ken Wood ("Woodies" として知られています) によって開発された、標準的な Commodity Channel Index (CCI) の修正版です。この CCI のバリエーションには追加の平滑化が含まれており、包括的な Woodies CCI 取引システムの一部として使用されます。

このインジケーターを使用するには、[WoodiesCCI](xref:StockSharp.Algo.Indicators.WoodiesCCI) クラスを使用する必要があります。

## 説明

Woodies CCI は、2本のラインを含むクラシックな CCI インジケーターの修正版です。
- 選択された期間 (通常は 14) を持つメイン CCI ライン
- メイン CCI ラインの単純移動平均である平滑化 CCI ライン

Woodies CCI システムは、これら2本のラインといくつかの主要水準を使用して売買シグナルを生成します。主な水準には次が含まれます。
- +100 と -100 (従来の買われ過ぎおよび売られ過ぎ水準)
- +200 と -200 (強い買われ過ぎおよび売られ過ぎ状態)
- ゼロライン (トレンド判断に重要)

Woodies CCI システムにおける主なシグナル:
- "Zero-line Reject" - CCI がゼロラインに接近した後に反発し、以前の方向へ継続する場合
- "Trend-line Break" - CCI が重要なトレンドラインをブレイクする場合
- "Reverse Divergence" - 価格と CCI の間の特定タイプのダイバージェンス

## パラメーター

- **Length** - メイン CCI ラインの計算期間 (通常は 14)
- **SMALength** - 2本目のラインを取得するためにメイン CCI ラインを平滑化する期間 (通常は 9)

## 計算

Woodies CCI の計算は、いくつかの手順で実行されます。

1. まず、標準 CCI を計算します。
   ```
   代表価格 (TP) = (High + Low + Close) / 3
   Average Value (SMA) = SMA(TP, Length)
   Mean Deviation (MD) = Sum(|TP - SMA|) / Length
   CCI = (TP - SMA) / (0.015 * MD)
   ```

2. 次に、平滑化 CCI ラインを計算します。
   ```
   Smooth CCI = SMA(CCI, SMALength)
   ```

Woodies CCI は、これら2本のラインの組み合わせを使用して売買シグナルを作成します。クラシックな Woodies システムでは、これらのラインのクロス、主要水準との相互作用、およびさまざまなパターンが取引判断の基礎となります。

![IndicatorWoodiesCCI](../../../../images/indicator_woodies_cci.png)

## 関連項目

[CCI](cci.md)
