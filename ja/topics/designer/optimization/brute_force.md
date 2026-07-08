# 総当たり

ストラテジー最適化モードに切り替えるには、**Emulation** タブの **Optimization** ボタンをクリックします。最適化の例は、[キューブから](../strategies/using_visual_designer/first_strategy.md)作成した SMA ストラテジーを使用して説明します。

![Designer Optimization 00](../../../images/designer_optimization_00.png)

ワークスペースに Optimization + 「ストラテジー名」という名前のタブが開きます。**Optimization** タブは、**Properties** と **Optimization Result** の 2 つの領域に分かれています。

![Designer Optimization 02](../../../images/designer_optimization_02.png)

- **Properties** 領域は、複数の表を含むタブで構成されています。1 つ目は、[反復処理される](optimization_parameters.md)ストラテジーパラメーターです。2 つ目は、[遺伝的最適化](genetic.md)の設定です。3 つ目は、オプティマイザーのシステム設定です。たとえば、最適化に使用するスレッド数やコア数をそこで変更できます。
- **Optimization Result** 領域は表であり、各行は一意のパラメーターでストラテジーをテストした結果です。また、**Optimization Result** 領域には、最適化の進行状況、経過時間、および最適化完了までの推定時間を示す進行状況バーがあります。さらに、結果を [3D チャート](3d_chart.md) 形式で表示するためのタブもあります。

反復処理用のパラメーターを設定すると、1000 回を超える反復が発生します。オプティマイザーを開始すると、結果の上部にある進行状況に、予定されている反復回数、すでに完了した回数、および完了までにおおよそ必要な時間に関するデータが表示されます。

![Designer Optimization 03](../../../images/designer_optimization_03.png)

## 関連項目

[バックテストの例](../backtesting/getting_started.md)

