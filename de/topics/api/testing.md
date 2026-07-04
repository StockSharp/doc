# Backtesting\/Emulation

Strategien, die mit [Strategy](xref:StockSharp.Algo.Strategies.Strategy) geschrieben wurden, können in drei Modi getestet werden:

1. Testing mit [historischen Daten](testing/historical_data.md). Mit dieser Datenart können sowohl Marktanalysen zum Finden von Mustern als auch die Optimierung von Strategieparametern durchgeführt werden.
2. Testing mit [Zufallsdaten](testing/random_data.md). Ein praktisches Werkzeug für das erste Testen von Strategien, um Fehler in Algorithmen zu finden, oder für automatisierte Tests, die nach Zeitplan laufen.
3. Testing im [Simulator](testing/simulator.md) auf Basis von Daten, die über eine echte Verbindung zum Handelssystem empfangen werden, zum Beispiel von [OpenECry](connectors/stock_market/openecry.md), jedoch ohne tatsächliche Orderregistrierung. Die Ausführung wird anhand eingehender Orderbücher emuliert.

Bei allen drei Modi liegt der Schwerpunkt darauf, dass der mit [Strategy](xref:StockSharp.Algo.Strategies.Strategy) geschriebene Strategiecode beim Wechsel vom realen Handel zum Testing und zurück nicht geändert werden muss. Dies wird durch die Implementierung der Hauptschnittstelle [IConnector](xref:StockSharp.BusinessEntities.IConnector) erreicht, die als Gateway zum Handelssystem dient. Wie diese Schnittstelle verwendet wird, wurde bereits im Abschnitt [API](../api.md) gezeigt. Im Testing-Modus agiert nicht das reale Handelssystem, sondern die Emulation als Handelssystem, abhängig vom ausgewählten Modus. Daher weiß der Strategiecode nicht, ob er mit einer echten Börse handelt oder mit einer Emulation.

