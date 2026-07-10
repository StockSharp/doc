# Hydra

![hydra Hauptansicht](../images/hydra_main.png)

**Hydra** lädt Marktdaten automatisch aus verschiedenen Quellen herunter und speichert sie lokal. Unterstützt werden Instrumente, Kerzen, Tick-Trades, Orderbücher und weitere Datentypen. Daten können im speziellen binären **Hydra**-Format (BIN), das eine hohe Komprimierung bietet, oder im CSV-Format zur Analyse in anderen Programmen gespeichert werden.

Gespeicherte Daten können später von Handelsstrategien verwendet werden. Informationen zum Strategietesting finden Sie unter [Backtesting](api/testing.md). Auf die Daten kann direkt über [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) zugegriffen werden (siehe [Marktdatenspeicher](api/market_data_storage.md)), oder sie können in Formate wie [Excel](https://en.wikipedia.org/wiki/Excel), XML oder TXT exportiert werden (siehe [Installation und Betrieb](hydra/installing_hydra.md)).

**Hydra** kann sowohl mit historischen als auch mit Echtzeit-Datenquellen arbeiten, zum Beispiel mit [OpenECry](api/connectors/stock_market/openecry.md) oder [Rithmic](api/connectors/stock_market/rithmic.md) für Orderbücher. Das Plugin-Modell ermöglicht außerdem das Erstellen eigener Datenquellen. Details finden Sie unter [Quelle erstellen](hydra/create_new_source.md).
