# StockSharp について

[StockSharp (S#)](https://stocksharp.com/ja/store/) は、株式、先物、オプション、暗号資産、外国為替を含むグローバル市場向けの**無料**トレーディングアプリケーションを提供します。従来型のトレーディングロボットから HFT (高頻度取引) システムまで、手動で取引することも、自動売買戦略を実行することもできます。

**90 以上のブローカー、取引所、データソースをサポート:** [コネクター](topics/api/connectors.md)。

S# は、利用可能なコネクタでサポートされている任意のブローカー、取引所、データソースで動作します。

> [!NOTE]
> すべてのプログラムは [インストーラー](topics/installer.md) ユーティリティを通じてインストールされます。

### Designer

![StockSharp について のスクリーンショット 1](images/stocksharptitle_0.png)

[Designer](topics/designer.md) は、アルゴリズム取引戦略を作成するための汎用アプリケーションです:

- マウス操作で編集できるビジュアル戦略デザイナー。
- 統合された [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) エディター。
- カスタムインジケーターの簡単な作成。
- 組み込みデバッガー。
- 複数の取引所、取引会場、ブローカーへの接続。
- グローバル市場との互換性。
- 戦略スキーマをチームと共有する機能。

[詳細...](topics/designer.md)

### Hydra

![StockSharp について のスクリーンショット 2](images/stocksharptitle_1.png)

[Hydra](topics/hydra.md) は、過去およびリアルタイムのマーケットデータを自動的にダウンロードします:

- [コネクター](topics/api/connectors.md) を通じて多数のデータソースをサポート。
- 高い圧縮率 (約定 1 件あたり 2 バイト、板情報 1 件あたり 7 バイト)。
- あらゆるデータ型 (ローソク足、ティック、板情報、注文ログ、オプション、ニュースなど) を処理。
- 保存済みデータへの API アクセス。
- CSV、Excel、XML、またはデータベースへのエクスポート機能。
- CSV インポート機能。
- スケジュールされたタスクと、実行中の Hydra インスタンス間でのインターネット経由の自動同期。

[詳細...](topics/hydra.md)

### Terminal

![Terminal メイン 00](images/terminal_main_00.png)

[Terminal](topics/terminal.md) は、取引およびチャート表示用のアプリケーション (トレーディングターミナル) です:

- クリックでチャートから直接取引できます。
- 任意の時間枠をサポートします。
- 出来高、ティック、レンジ、Renko など、さまざまなローソク足タイプを備えています。
- クラスターおよびボックスチャートを含みます。

### Shell

Shell は、ニーズに合わせてすばやくカスタマイズできる既製のグラフィカルフレームワークを提供し、C# の完全なオープンソースコードが付属しています:

- 完全なソースコードを含みます。
- StockSharp プラットフォームのすべての接続をサポート: FIX/FAST、暗号資産取引所 (現在 30 以上) など。
- Designer スキーマのサポート。
- 柔軟なユーザーインターフェイス。
- 戦略テストツール (統計、エクイティ、レポート)。
- 戦略設定の保存と読み込み。
- 複数戦略の同時実行。
- 戦略パフォーマンスの詳細な把握 (注文、トランザクション、ポジション、収益、ログなど)。
- スケジュールされた戦略起動。

### API

[API](topics/api.md) は、トレーディングロボットおよびアルゴリズム取引システムを専門的に開発するための C# ライブラリです。

### 製品

- [Designer](topics/designer.md) - 汎用アルゴリズム戦略デザイナー。
- [Hydra](topics/hydra.md) - マーケットデータのダウンロードプログラム。
- [API](topics/api.md) - [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) でトレーディングロボットを開発するためのライブラリ。
- [Terminal](topics/terminal.md) - トレーディングターミナル。
- [Shell](topics/shell.md) - ソースコード付きの、戦略向け既製グラフィカルフレームワーク。
- [MATLAB](topics/matlab.md) - MATLAB と取引システムの統合。MATLAB スクリプトから取引できます。

[ダウンロード](https://stocksharp.com/ja/products/download/)

## 推奨コンテンツ

[参考資料](topics/common/reference_materials.md)
