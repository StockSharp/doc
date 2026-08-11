# API ドキュメント

## 概要

StockSharp API は S# API とも呼ばれ、[Designer](designer.md)、[Terminal](terminal.md)、その他のカスタム取引ツールなどのトレーディングアプリケーションを構築するためのソフトウェア開発キット (SDK) です。マーケットデータ、注文ルーティング、戦略実行、テスト、取引用 UI コンポーネントの中核インフラストラクチャを提供します。

## 機能

- **戦略スクリプティング**: StockSharp API により、ユーザーは [Designer](designer/strategies/using_code.md) 内でトレーディング戦略を直接記述して実行できます。戦略は C#、F#、または Python を使用して開発、テスト、デプロイできます。

- **分析ツール**: API は詳細な [マーケットデータ分析](hydra/analytics.md) のために Hydra と統合されます。データ処理、保存、カスタム分析ワークフローをサポートします。

- **カスタムアプリケーション開発**: 開発者は StockSharp API を使用して、組み込みアプリケーションのスクリプティングだけに依存せず、スタンドアロンの [取引ソリューション](api/examples.md) を構築できます。

- **コネクタとデスクトップコントロール**: API には、リアルタイムマーケットデータと取引アクセスのための多数の [コネクタ](api/connectors.md) が含まれています。また、プロフェッショナルな取引プラットフォームを構築するためのカスタマイズ可能な [デスクトップコントロール](api/graphical_user_interface.md) も提供します。

## アーキテクチャ

StockSharp API は、モジュール性と [拡張性](api/connectors/creating_own_connector.md) を中心に構築されています。開発者はコアシステムを変更せずに、プラグインや追加モジュールで拡張できます。このアーキテクチャは、スケーラブルで保守しやすいトレーディングアプリケーションの構築に役立ちます。

## オープンソース

StockSharp API のコアはオープンソースです。ソースコードは GitHub で公開されているため、開発者はそれを調査、変更し、改善に貢献できます。

## GitHub リポジトリ

公式の StockSharp API ソースコードは、GitHub リポジトリで入手できます:

[StockSharp GitHub リポジトリ](https://github.com/stocksharp/stocksharp)
