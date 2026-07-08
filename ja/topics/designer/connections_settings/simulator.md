# シミュレーター

[Designer](../../designer.md) では、作成したストラテジーを **Simulation** モードで実行できます。**Simulation** をカスタマイズするには、次の操作を実行する必要があります。

1. **Connect** ![Designer The quick access toolbar 00](../../../images/designer_quick_access_toolbar_00.png) ボタンの横にある矢印をクリックすると、**Emulator settings** ボタンが表示されます。

![Designer The connection settings 00](../../../images/designer_connection_settings_00.png)

2. **Emulator settings** ボタンをクリックすると、**Emulator settings** ウィンドウが開きます。

![Designer Properties emulation 00](../../../images/designer_properties_emulation_00.png)

1. **Simulator**

- **Use emulator** - エミュレーターを使用します。
- **Instruments** - 銘柄。

2. **Settings**

- **Combine on touch** - エミュレーション中、約定価格が注文価格に接触したとき（つまり、注文価格と等しいとき）に約定を結合します。
- **Market depth (lifetime)** - エミュレーター内で板が保持される最大時間です。この時間内に更新がない場合、板は消去されます。このプロパティは、データに欠落がある場合に古い板を削除するために使用できます。
- **Errors percentage** - 新規注文を登録するときのエラーの割合値です。値は 0（エラーなし）から 100 までです。
- **Latency** - 登録済み注文の遅延の最小値です。
- **Reregistering** - 単一の約定という形式で注文の再登録がサポートされるかどうか。
- **Buffering period** - 応答を単一パッケージ内のバッチとして送信します。ネットワーク遅延と取引所コアのバッファー処理がエミュレートされます。
- **Order ID** - エミュレーターが注文の識別子を生成するときの開始番号です。
- **Trade ID** - エミュレーターが約定の識別子を生成するときの開始番号です。
- **Transaction** - エミュレーターが注文トランザクションの識別子を生成するときの開始番号です。
- **Spread size** - 価格刻み単位でのスプレッドサイズです。ティック約定から板を生成する際にスプレッドを決定するために使用されます。
- **Depth of book** - ティックから生成される板の最大深さです。
- **Number of volume steps** - 注文がティック約定より大きい場合の出来高ステップ数です。ティック約定をテストするときに使用されます。
- **Portfolios interval** - ポートフォリオ再計算間隔です。間隔がゼロの場合、再計算は実行されません。
- **Change time** - 注文と約定の時刻を取引所時刻に変更します。
- **Time zone** - 取引所のタイムゾーンに関する情報です。
- **Price shift** - 最終約定からの価格シフトで、次のセッションの最大価格と最小価格の境界を決定します。
- **Add extra volume** - 大量注文を登録するときに板注文へ追加の出来高を追加します。

## 推奨コンテンツ

[チャート](../user_interface/components/chart.md)
