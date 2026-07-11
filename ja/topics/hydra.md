# Hydra

![Hydra メイン](../images/hydra_main.png)

**Hydra** は、さまざまなソースからマーケットデータを自動的にダウンロードし、ローカルに保存します。銘柄、ローソク足、ティック取引、板情報、その他のデータ型をサポートしています。データは、高い圧縮率を提供する専用の **Hydra** バイナリ形式 (BIN) で保存することも、他のプログラムで分析するために CSV 形式で保存することもできます。

保存されたデータは、後で取引戦略で使用できます。戦略のテストについては、[バックテスト](api/testing.md) を参照してください。データには [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) を通じて直接アクセスできます ([マーケットデータストレージ](api/market_data_storage.md) を参照)。また、[Excel](https://en.wikipedia.org/wiki/Excel)、XML、TXT などの形式へエクスポートすることもできます ([インストールと操作](hydra/installing_hydra.md) を参照)。

**Hydra** は、たとえば板情報向けの [OpenECry](api/connectors/stock_market/openecry.md) や [Rithmic](api/connectors/stock_market/rithmic.md) など、履歴データソースとリアルタイムデータソースの両方で動作できます。プラグインモデルにより、カスタムデータソースを作成することもできます。詳細については、[ソースの作成](hydra/create_new_source.md) を参照してください。
