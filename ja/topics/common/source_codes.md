# ソースコード

[S#](../api.md) のオープンソースコードは、複数のリポジトリに分かれています。[StockSharp コアリポジトリ](https://github.com/StockSharp/StockSharp) には、メッセージモデル、ビジネスエンティティ、共通コネクター抽象化、アルゴリズム、テストツールなど、プラットフォームの基盤が含まれます。プロバイダー固有のコネクター実装はコアリポジトリには格納されません。

プロバイダー固有のオープンコネクターは、すべて [StockSharp\/Connectors](https://github.com/StockSharp/Connectors) で管理されています。各コネクターは独立した .NET プロジェクトであり、リポジトリにはまとめてビルドするための `Connectors.slnx` が含まれています。

独立したブラウザーチャートエンジンと Web ターミナル向けチャートスタックは、[StockSharp\/JS-Charts](https://github.com/StockSharp/JS-Charts) で管理されています。[JavaScript チャート](../api/javascript_ui/charts.md)を参照してください。

[GitHub の使用手順](https://docs.github.com/ja/get-started/start-your-journey/hello-world)

ソースコード付きで利用できるコンポーネントの一覧:

- 独自接続を作成するための共通クラス。
- 市場データストレージの形式。
- 取引シミュレーター。
- 履歴シミュレーター (バックテスター)。
- テクニカル分析インジケーター (140 種以上)。
- 損益、スリッページ、遅延を計算するアルゴリズム。
- 任意の時間枠のローソク足、および時間ベースではないローソク足 (ティック、レンジなど) を構築するアルゴリズム。
- ロギング。
- インポートとエクスポート。

すべてのクローズドコンポーネントおよび完成済みプログラムのソースコードは、購入により利用できます。ソースコードの費用の詳細については、[ソースコード費用](https://stocksharp.com/ja/store/?groups=22) を参照してください。

## 推奨コンテンツ

[インストール手順](../api/setup.md)
