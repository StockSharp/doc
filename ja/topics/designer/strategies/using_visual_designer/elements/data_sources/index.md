# Index

![Designer Index 00](../../../../../../images/designer_index_00.png)

このキューブは、独自のインデックスを作成するために使用されます。

### 出力ソケット

出力ソケット

- **Security** - 計算されたインデックスで、**Security** として表されます。

### パラメーター

パラメーター

- **Index** - 複数の銘柄を組み合わせた数式（例: (AAPL@NASDAQ+10)\*(abs(20\/GOOG@NYSE))。
- **Ignore errors** - 設定されたフラグは、インデックス計算時にエラーが無視されることを示します。
- **Calculate extended information** - 設定されたフラグは、インデックス計算時に、基本情報（Total volume、Opening price、Closing price、Highest price、Lowest price）に加えて、拡張情報（Total trade turnover、Opening volume、Closing volume、Maximum volume、Minimum volume）が計算されることを示します。

使用可能な数式は、[Formula](../common/formula.md) キューブと同様です。

## 推奨コンテンツ

[Variable](variable.md)
