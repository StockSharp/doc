# StockSharp の戦略

## はじめに

StockSharp は、取引戦略の作成、テスト、実行のための強力なインフラストラクチャを提供します。アルゴリズム取引戦略を開発するための基盤は、基本クラス [Strategy](xref:StockSharp.Algo.Strategies.Strategy) です。このクラスは、市場データの操作、取引操作の実行、結果の分析に使用する標準機能と抽象化のセットを提供します。

## ナビゲーション

### 戦略の基本

- [戦略での市場データ購読](strategies/subscriptions.md) - 戦略で市場データ購読を使用するための詳細ガイドです。サブスクリプションの作成と構成、ライフサイクル管理、状態監視について説明します。

- [戦略での指標](strategies/indicators.md) - 戦略でテクニカル分析指標を扱うための情報です。戦略への指標追加、形成状態の制御、取引ロジックでの使用について扱います。

- [戦略での取引操作](strategies/trading_operations.md) - 戦略で取引操作を実行するためのガイドです。注文の作成と送信、ポジションのクローズ、状態監視のためのメソッドについて説明します。

- [ポジション保護](strategies/take_profit_and_stop_loss.md) - テイクプロフィットとストップロスを使用してオープンポジションを保護する仕組みの説明です。ポジション保護に対するローカル方式とサーバー方式を検討します。

- [戦略パラメーター](strategies/parameters.md) - [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を通じて戦略パラメーターを扱うためのガイドです。構成可能なパラメーターの作成、GUI での表示設定、最適化での使用方法について説明します。

- [戦略でのロギング](strategies/logging.md) - 戦略でロギング機構を使用し、アルゴリズムのパフォーマンスを追跡およびデバッグするためのガイドです。

### 高度な機能

- [戦略プラットフォーム互換性](strategies/compatibility.md) - 各種 StockSharp プラットフォームと互換性のある戦略を作成するための推奨事項です: [Designer](../designer.md)、[Shell](../shell.md)、[Runner](../runner.md)、およびクラウドテスト。

- [戦略での高レベル API](strategies/high_level_api.md) - サブスクリプション、指標、チャート、ポジション保護の操作を簡素化する高レベルメソッドの説明です。取引ロジックに集中して、より明快なコードを書く方法を説明します。

- [戦略でのチャート操作](strategies/chart.md) - 戦略データをチャート上に可視化するためのガイドです。チャートへのアクセス、エリアの作成、要素の追加、データの描画方法を説明します。

- [設定の保存と読み込み](strategies/settings_saving_and_loading.md) - [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) および [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) メソッドを通じて戦略設定を保存および読み込む仕組みの説明です。

- [状態の読み込み](strategies/orders_and_trades_loading.md) - たとえば取引セッション中に戦略を再起動する場合などに、以前に実行された注文と取引を戦略へ読み込むためのガイドです。

- [価格丸め](strategies/shrink_price.md) - [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) メソッドを使用して、戦略内で価格を正しく丸めるためのガイドです。

- [Unit 型](strategies/unit_type.md) - パーセンテージ、ポイント、pips などの数量に対する算術演算を簡素化する [Unit](xref:StockSharp.Messages.Unit) データ型の説明です。

- [イベントモデル](strategies/event_model.md) - [IMarketRule](xref:StockSharp.Algo.IMarketRule) に基づく戦略イベントモデルの説明です。市場イベントに反応するルールの作成、条件の組み合わせ、ルールライフサイクル管理について扱います。

## 戦略開発の開始

独自の戦略開発を始めるには、次を推奨します。

1. StockSharp における戦略の一般原則を理解するために、戦略を扱う基本を把握します。

2. 市場データの受信と処理の仕組みを理解するために、[戦略での市場データ購読](strategies/subscriptions.md) セクションを学習します。

3. テクニカル分析指標の扱いを理解するために、[戦略での指標](strategies/indicators.md) セクションを確認します。

4. 取引操作の仕組みを理解するために、[戦略での取引操作](strategies/trading_operations.md) セクションを参照します。

5. 戦略構成の仕組みを学ぶために、[戦略パラメーター](strategies/parameters.md) セクションを確認します。

6. 組み込みの高レベル機能を使用して戦略コードを簡素化するために、[戦略での高レベル API](strategies/high_level_api.md) セクションに慣れます。

## 戦略テスト

StockSharp は、戦略をテストするためのさまざまな方法を提供します。

- **履歴データテスト** - 履歴データ上で戦略の有効性を評価できます。
- **パラメーター最適化** - 最適な戦略パラメーター値を見つけるのに役立ちます。
- **仮想口座テスト** - 実資金をリスクにさらすことなく、リアルタイムモードで戦略パフォーマンスを確認できます。

テスト方法と戦略パフォーマンス評価の詳細な説明は、[テスト](../api/testing.md) セクションにあります。
