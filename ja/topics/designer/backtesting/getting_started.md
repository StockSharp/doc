# はじめに

例として、SMA ストラテジーの例を取り上げます。

履歴データでテストを実行するには、履歴データでテストするスキーマを持つストラテジーを選択する必要があります。ストラテジーは、ストラテジーフォルダー内の [Schemas](../user_interface/schemas.md) パネルで、対象のストラテジーをダブルクリックして選択します。

テストの前に、マーケットデータ（銘柄、ローソク足、ティック約定、または板）を読み込みます。これは [マーケットデータストレージ](../market_data_storage.md) で説明されています。

ストラテジーのタブに切り替えると、**リボン** の **エミュレーション** タブが自動的に開きます。このタブでテスト期間を設定します。マーケットデータ欄には必要なストレージ（[マーケットデータストレージ](../market_data_storage.md)）を指定し、銘柄欄には必要な銘柄を指定します。

SMA ストラテジーの例では、次のパラメーターを使用します。

1. AAPL@NASDAQ 銘柄
2. 標準ストレージ \\Documents\\StockSharp\\Designer\\Storage
3. ストレージ形式 - CSV
4. ストレージから取得するデータの種類 - Ticks
5. 板 - 生成
6. 板の深さ - 5
7. スプレッドサイズ - 2
8. 30 秒の時間枠のローソク足
9. 出来高 - 100

選択したパラメーターを設定する必要があります。

![Designer An example of backtesting 00](../../../images/designer_example_of_backtesting_00.png)

![Designer An example of backtesting 01](../../../images/designer_example_of_backtesting_01.png)

必要なすべてのパラメーターを設定したら、![Designer Interface Backtesting 01](../../../images/designer_interface_backtesting_01.png) ボタンをクリックしてストラテジーのテストを開始します。

テスト中またはテスト後に、テスト情報を含むチャートやテーブルを表示できます。

![Designer An example of backtesting 02](../../../images/designer_example_of_backtesting_02.png)

グラフから、ストラテジーで計画したとおり、移動平均の交差で取引が行われていることがわかります。また、注文が複数の約定で充足されていることも確認できます。これは生成された板を使用しているためで、テストの現実性が高まります。注文が複数の約定で充足されていることは、Trades テーブル、Statistics、および Positions チャートから確認できます。

![Designer An example of backtesting 03](../../../images/designer_example_of_backtesting_03.png)

**ポジションチャート** では、ストラテジーが運用数量を減らしていることがわかります。これは、生成された板の深さが 5 であり、その結果、200 ロットの注文を充足するには板全体の深さが不足していたためです。ストラテジーはポジションを反転するだけなので、板の深さが注文を充足するのに不足するたびに、注文サイズが減少しました。

![Designer An example of backtesting 04](../../../images/designer_example_of_backtesting_04.png)

**P\/L** チャートは、このようなパラメーターではストラテジーが不採算であることを示しています。

## 推奨コンテンツ

[ライブ実行](../live_execution/getting_started.md)
