# FDI

**Fractal Dimension Index (FDI)** は、価格系列の粗さを定量化します。

このインジケーターを使用するには、[FractalDimension](xref:StockSharp.Algo.Indicators.FractalDimension) クラスを使用する必要があります。

## 説明

FDI は 1 から 2 の範囲を取り、市場の挙動を反映します:
- 1 に近い値は、持続的なトレンド (より滑らかな経路) を示します。
- 1.5 付近の値はランダムウォークに対応します。
- 2 に近い値は、レンジ相場またはノイズの多い市場を示します。

このインジケーターはフラクタル幾何に基づいており、価格経路がどれほど複雑であるかを測定します。

## パラメーター

このインジケーターには次のパラメーターがあります:
- **Length** - 計算期間 (デフォルト値: 30)

## 計算

FDI は、価格経路の総長と全体の高値-安値範囲を比較することで計算されます:

1. 期間内の連続する価格間の絶対差を合計し、価格経路の長さを求めます。
2. 期間内の最大高値と最小安値の差を求めます。
3. 次を使用して FDI を計算します:
   ```
   FDI = 1 + (log(PathLength) - log(Range)) / log(2 * (Length - 1))
   ```
4. 結果を 1 から 2 の間に制限します。

## 解釈

- **1 に近い FDI** - 強いトレンド性のある挙動。
- **1.5 付近の FDI** - ランダムウォーク。トレンドの強さは中立です。
- **2 に近い FDI** - 荒い、または横ばいの市場。

![indicator_fractal_dimension](../../../../images/indicator_fractal_dimension.png)

## 関連項目

[Hurst Exponent](hurst_exponent.md)

[Fractal Adaptive Moving Average](fractal_adaptive_moving_average.md)

