# ヘッジング

![Designer Hedging 00](../../../../../../images/designer_hedging_00.png)

このキューブは、オプションのポジションをヘッジするために使用します。

### 入力ソケット

入力ソケット

- **Model** - 計算モデル（例: Black-Scholes）。
- **Instrument** - 銘柄、原資産。
- **Volume** - 数量の数値。
- **Position by underlying asset** - 原資産のポジション。
- **Flag** - ヘッジ処理を開始するシグナル（フラグ）。

### 出力ソケット

出力ソケット

- **Order** - 登録された注文。この注文は、注文別の Trades 要素を使用してその注文に対する約定を取得し、Chart panel キューブを使用してチャート上に表示するために使用できます。

### パラメーター

パラメーター

- **Hedging type** - ヘッジ種別。Delta、Gamma、Vega、Theta、または Rho の値を取ることができます。

## 推奨コンテンツ

[オプションのクォーティング](options_quoting.md)

