# Hydra

![hydra main](../images/hydra_main.png)

**Hydra** descarga automáticamente datos de mercado desde distintas fuentes y los almacena localmente. Admite instrumentos, velas, operaciones tick, libros de órdenes y otros tipos de datos. Los datos pueden almacenarse en el formato binario especial de **Hydra** (BIN), que proporciona alta compresión, o en formato CSV para analizarlos en otros programas.

Los datos almacenados pueden usarse posteriormente por estrategias de trading. Para probar estrategias, consulte [Backtesting](api/testing.md). Se puede acceder a los datos directamente mediante [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) (consulte [Almacenamiento de datos de mercado](api/market_data_storage.md)) o exportarlos a formatos como [Excel](https://en.wikipedia.org/wiki/Excel), XML o TXT (consulte [Instalación y funcionamiento](hydra/installing_hydra.md)).

**Hydra** puede trabajar tanto con fuentes de datos históricos como en tiempo real, por ejemplo [OpenECry](api/connectors/stock_market/openecry.md) o [Rithmic](api/connectors/stock_market/rithmic.md) para libros de órdenes. El modelo de plugins también permite crear fuentes de datos personalizadas. Para más detalles, consulte [Creación de una fuente](hydra/create_new_source.md).
