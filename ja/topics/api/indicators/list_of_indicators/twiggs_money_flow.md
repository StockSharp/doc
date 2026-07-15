# TMF

**ツイッグス・マネーフロー (TMF)** は、チャイキン・マネーフロー インジケーターの改良版として Colin Twiggs によって開発された出来高インジケーターです。TMF は市場センチメントの変化に対してより敏感で、誤シグナルが少なくなります。

このインジケーターを使用するには、[TwiggsMoneyFlow](xref:StockSharp.Algo.Indicators.TwiggsMoneyFlow) クラスを使用する必要があります。

## 説明

ツイッグス・マネーフロー は、価格と出来高の関係を分析して、市場へのマネーフローの流入または流出の方向を判定します。従来の出来高インジケーターとは異なり、TMF は値を -1 から +1 の間に正規化することでノイズを排除します。

TMF の主な特徴:
- 正の値は、銘柄への資金流入（強気センチメント）を示します
- 負の値は、銘柄からの資金流出（弱気センチメント）を示します
- 0 の値は、需要と供給の均衡を示します

このインジケーターは、次の用途に役立ちます:
- 現在のトレンドの確認、またはトレンドの弱さの特定
- 価格とマネーフローの間のダイバージェンスの検出
- 潜在的な市場反転ポイントの特定

## パラメーター

- **期間** - 指数移動平均の計算期間。通常は 21 の値を使用します。

## 計算

ツイッグス・マネーフロー の計算は、いくつかの手順で実行されます:

1. 真の値幅を計算します:
   ```
   TR = Max(High - Low, |High - 前回終値|, |Low - 前回終値|)
   ```

2. ツイッグス・マネーフロー出来高 (TMFV) を決定します:
   ```
   TMFV = Volume * ((Close - Low - (High - Close)) / TR)
   ```
   (High - Low = 0) の場合、TMFV = 0

3. TMFV と出来高の指数移動平均を計算します:
   ```
   EMA_TMFV = EMA(TMFV, Length)
   EMA_Volume = EMA(Volume, Length)
   ```

4. 最終的な TMF 値:
   ```
   TMF = EMA_TMFV / EMA_Volume
   ```

TMF 値は -1（強い弱気シグナル）から +1（強い強気シグナル）の範囲になります。

![TMF のチャート](../../../../images/indicator_twiggs_money_flow.png)

## 関連項目

[ADL](accumulation_distribution_line.md)
[マネーフローインデックス](money_flow_index.md)
[OBV](on_balance_volume.md)
