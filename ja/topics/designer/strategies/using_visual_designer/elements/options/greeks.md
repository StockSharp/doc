# Greeks

![Designer Greek 00](../../../../../../images/designer_greek_00.png)

このブロックは、現在時点の主要な「グリークス」である Delta、Gamma、Vega、Theta、Rho を計算するために使用します。

### 入力ソケット

入力ソケット

- **Model** - 計算モデル（例: Black-Scholes）。
- **Price of the Underlying Asset** - 原資産の価格。
- **Maximum Deviation** - 最大偏差。

### 出力ソケット

出力ソケット

- **Result** - 現在時点の主要な「グリークス」である Delta、Gamma、Vega、Theta、Rho の計算結果。

### パラメーター

パラメーター

- **Value** - 「グリーク」の種類である Delta、Gamma、Vega、Theta、Rho を取ることができ、このブロックからどの値を出力するかを決定します。

## 関連項目

[Hedging](black_scholes.md)
