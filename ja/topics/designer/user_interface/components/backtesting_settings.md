# バックテスト設定

**Properties** パネルは、既定ではストラテジータブの右側に最小化されています。このパネルは、エミュレーションまたは Live 取引のプロパティのテーブルです。特定のプロパティを選択すると、そのプロパティの詳細な説明がテーブルの下部に表示されます。すべてのプロパティはグループに分類されています。

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Settings**

- **Market data** – データストレージです。
- **Storage format** – ストレージ形式です。
- **Data type** – データ型です。
- **Time frame** – 指定した時間枠のローソク足を使用します。
- **Maximum quote volume in generated depth** – 生成される板情報における最大気配数量です。
- **Interval** – 時間間隔です。
- **Unrealized P\/L** – 未実現利益を再計算する間隔です。
- **Trades** – 使用する約定です。
- **Marked depth** – 使用する板情報です。
- **Order log** – 注文ログを使用します。
- **Number of strategies** – 同時にテストされるストラテジー数です。
- **Logging level** – ログレベルです。
- **Combine on touch** – エミュレーション中、約定価格が注文価格に触れたとき（つまり注文価格と等しいとき）に注文を結合します。
- **Marked depth (lifetime)** – 板情報がエミュレーター内に存在する最大時間です。この時間内に更新がなかった場合、板情報は削除されます。このプロパティは、データに欠落がある場合に古い板情報を削除するために使用できます。
- **Errors percentage** – 新規注文の登録時に発生するエラーの割合値です。値は 0（エラーなし）から 100 まで指定できます。
- **Latency** – 登録された注文レイテンシーの最小値です。
- **Reregistering** – 注文の再登録が単一の取引としてサポートされるかどうかです。
- **Buffering period** – 一定間隔で応答を単一パケットとして送信します。ネットワークレイテンシーと取引所コアのバッファリング動作をエミュレートします。
- **Order ID** – エミュレーターが注文の識別子を生成し始める番号です。
- **Trade ID** – エミュレーターが約定の識別子を生成し始める番号です。
- **Transaction** – エミュレーターが注文トランザクションの識別子を生成し始める番号です。
- **Spread size** – 価格ステップ単位のスプレッドサイズです。ティック約定から板情報を生成する際に、スプレッドを指定するために使用されます。
- **Depth of book** – ティックから生成される板情報の最大深度です。
- **Number of volume steps** – 注文がティック約定より大きくなる数量ステップ数です。ティック約定でのテストに使用されます。
- **Portfolios interval** – ポートフォリオ再計算の間隔です。間隔がゼロの場合、再計算は実行されません。
- **Change time** – 注文と約定の時刻を取引所時刻に変更します。
- **Time zone** – 取引所が所在するタイムゾーンに関する情報です。
- **Price shift** – 最終約定からの価格シフトで、次のセッションの最大価格と最小価格の制限を指定します。
- **Add extra volume** – 大きな数量の注文が登録されるときに、板情報へ追加数量を加えます。
- **[Commissions](../commissions.md)** – 手数料（ブローカー手数料、取引所手数料など）です。

**Logging**

- **Logging level** – この要素のログレベルです。

**Setting**

- **[Risk management](../risk_management.md)** – リスク管理設定です。

**Diagram parameters**

- **Security** - 銘柄です。
- **Portfolio** - ポートフォリオです。

**Diagram parameters** を入力しない場合、エミュレーション時には **Emulation** タブの **Instrument** フィールドの銘柄が使用され、ポートフォリオとしては既定でテスト用ポートフォリオが使用されます。

## 推奨コンテンツ

[チャート](chart.md)
