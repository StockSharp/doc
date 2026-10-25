# コンポーネント

取引中は、市場、自分の注文、ポジションを把握する必要があります。そのために、[ワークスペース](workspace.md) には、領域の右端にある縦のバーのボタンでグラフィカルコンポーネントを追加します。ボタンを押すたびに、その種類のパネルがもう 1 つ開きます。

![Terminal コンポーネント 00](../../../images/terminal_components_00.png)

- [銘柄](components/instruments.md) - 選択したすべての銘柄に関する情報を表示する銘柄テーブルです。
- [Level 1](components/level_1.md) - 選択した銘柄の Level 1 の変更履歴を表示するテーブルです。
- [売買](components/buy_sell.md) - 銘柄の最良価格が表示され、注文の発注と取消を行えるチケットのグループです。
- [板情報](components/order_book.md) - 買いおよび売りの指値注文のテーブルです。
- [チャート](components/chart.md) - 選択した銘柄のローソク足とインジケーターです。チャートから取引することもできます。
- [P&L エクイティ](../../designer/user_interface/components/pnl_equity.md) - 損益（未実現と実現）と手数料のチャートです。
- [自分の約定](../../designer/user_interface/components/trades.md) - 自己約定のテーブルです。
- [注文](components/orders.md) - 自分の注文のテーブルです。テーブルの上には、注文を取り消すボタンと、表示する注文の選択があります。
- [ポジション](components/positions.md) - 現在のポジションのテーブルです。テーブルの上には、ポジションをクローズするボタンと反転するボタンがあります。
- [ティック約定](../../designer/user_interface/components/tick_trades.md) - 選択した銘柄について、取引所のすべての約定を表示するテーブルです。
- [ポジションチャート](../../designer/user_interface/components/positions.md) - ポジションのチャートです。
- **プロパティ** - 領域で選択したオブジェクト（銘柄、注文、約定、ポジション）のプロパティです。
- [ポジション（オプション）](components/positions_options.md) - オプション別ポジションのグラフィック表示です。
- [オプションデスク](components/option_desk.md) - 原資産に対するオプションの主要パラメーターのテーブルです。
- [スマイルボラティリティ](components/smile_volatility.md) - 同一の原資産で異なる権利行使価格を持つオプションの期待ボラティリティです。
- [注文ログ](components/order_log.md) - 選択した銘柄について、取引所のすべての注文を表示するテーブルです。
- [Level 2](components/level_2.md) - 1 つの銘柄の板情報を 2 つのテーブルで表示し、その下にチケットがあります。

ウィンドウ全体のパネル（[ログ](logs.md)、[ポートフォリオ](portfolios.md)、[ボードエディター](board_editor.md)、**マーケットデータ**、[ニュース](components/news.md)、[リスク](risk_management.md)）は、[メインメニュー](ribbon.md) の **ビュー** メニューから開きます。

すべてのコンポーネントのテーブルは同じように動作します。コピー、検索、フィルター、並べ替え、列の設定の方法については、[テーブルの操作](tables.md) セクションで説明しています。
