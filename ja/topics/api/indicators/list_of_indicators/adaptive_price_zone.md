# APZ

**Adaptive Price Zone (APZ)** は Lee Leibfarth によって開発されたテクニカルインジケーターで、市場のボラティリティに適応する動的なサポートゾーンとレジスタンスゾーンを作成します。

このインジケーターを使用するには、[AdaptivePriceZone](xref:StockSharp.Algo.Indicators.AdaptivePriceZone) クラスを使用する必要があります。

## 説明

APZ インジケーターは、平均価格の周囲に価格ゾーンを形成する 2 本のライン（上側と下側）で構成されます。このゾーンは現在の市場ボラティリティに応じて拡大および縮小します。市場のボラティリティが高まるとゾーンは拡大し、ボラティリティが低下するとゾーンは狭くなります。

APZ は特に次の用途に役立ちます。
- 潜在的なサポートレベルとレジスタンスレベルの特定
- 起こり得るトレンド反転ポイントの検出
- ボラティリティが増加または低下している期間の把握
- 価格ゾーンのブレイクアウトに基づく取引システムの作成

## パラメーター

このインジケーターには次のパラメーターがあります。
- **Period** - 計算期間（デフォルト値: 5）
- **BandPercentage** - バンド幅を定義するレンジの割合（デフォルト値: 2%）

## 計算

APZ の計算は Exponential Moving Average (EMA) と Average True Range (ATR) に基づきます。

1. まず、指定期間における価格の EMA を計算します。
   ```
   EMA = Exponential Moving Average of price over Period
   ```

2. 次に、ATR を使用してボラティリティを計算します。
   ```
   Volatility = Exponential Moving Average of ATR over Period
   ```

3. 上側および下側の APZ ラインは次のように計算されます。
   ```
   Upper Line = EMA + (Volatility * BandPercentage)
   Lower Line = EMA - (Volatility * BandPercentage)
   ```

価格が上側 APZ ラインを上回っている場合、これは上昇トレンドと見なせます。価格が下側 APZ ラインを下回っている場合、下降トレンドを示している可能性があります。価格が APZ ゾーン内で推移している場合、市場は保ち合いまたは横ばいの局面にある可能性があります。

![indicator_adaptive_price_zone](../../../../images/indicator_adaptive_price_zone.png)

## 関連項目

[BollingerBands](bollinger_bands.md)
[KeltnerChannels](keltner_channels.md)
[DonchianChannels](donchian_channels.md)
