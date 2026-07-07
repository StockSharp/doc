# Hydra

![hydra main](../images/hydra_main.png)

O **Hydra** descarrega automaticamente dados de mercado de diferentes fontes e armazena-os localmente. Suporta instrumentos, candles, ticks de negócios, livros de ordens e outros tipos de dados. Os dados podem ser guardados no formato binário especial do **Hydra** (BIN), que proporciona uma elevada compressão, ou em formato CSV para análise noutros programas.

Os dados guardados podem ser usados posteriormente por estratégias de negociação. Para testes de estratégias, consulte [Backtesting](api/testing.md). Os dados podem ser acedidos diretamente através de [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) (consulte [Armazenamento de dados de mercado](api/market_data_storage.md)) ou exportados para formatos como [Excel](https://en.wikipedia.org/wiki/Excel), XML ou TXT (consulte [Instalação e funcionamento](hydra/installing_hydra.md)).

O **Hydra** pode trabalhar tanto com fontes de dados históricas como em tempo real, por exemplo [OpenECry](api/connectors/stock_market/openecry.md) ou [Rithmic](api/connectors/stock_market/rithmic.md) para livros de ordens. O modelo de plugins também permite criar fontes de dados personalizadas. Para detalhes, consulte [Criar fonte](hydra/create_new_source.md).
