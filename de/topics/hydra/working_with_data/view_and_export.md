# Anzeigen und Exportieren

Die empfangenen [Hydra](../../hydra.md)-Daten können in speziellen Panels angezeigt werden.

Klicken Sie dazu auf der Registerkarte Common auf eine der folgenden Schaltflächen: [Ticks](view_and_export/ticks.md), [Orderbücher](view_and_export/order_books.md), [Kerzengenerierung](candles_generation.md), [Order-Log](view_and_export/order_log.md), [Level 1](view_and_export/level_1_.md), [Nachrichten](view_and_export/news.md), [Transaktionen](view_and_export/transactions.md), [Optionsdesk](view_and_export/option_desk.md), [Indikatoren](view_and_export/indicators.md), [Positionen](view_and_export/positions.md).

Alternativ klicken Sie mit der rechten Maustaste auf den erforderlichen Datentyp, wie in der Abbildung gezeigt, oder doppelklicken Sie auf den erforderlichen Datentyp.

![hydra view export](../../../images/hydra_view_export.png)

Jedes Panel enthält eine allgemeine Oberfläche mit folgenden Einstellungen:

![hydra export 00](../../../images/hydra_export_00.png)

- Die obere Zeile zeigt den Marktdatenspeicher und dessen Format an (BIN oder CSV).
- In der unteren Zeile wird der Zeitraum festgelegt, für den die Daten angefordert werden. Wenn Sie auf die Schaltfläche **Instrument auswählen** klicken, erscheint das Fenster zur Instrumentauswahl, in dem Sie ein oder mehrere Instrumente auswählen können. Werden mehrere Instrumente ausgewählt, sortiert das Programm beim anschließenden Export nach Excel oder CSV die Daten der verschiedenen Instrumente automatisch in unterschiedliche Dateien.
- Wenn beim Erstellen einer Datentabelle die Menge der geladenen Daten das festgelegte Limit überschreitet, erscheint auf dem Bildschirm ein Fenster:![hydra tick limit](../../../images/hydra_tick_limit.png)

  In diesem Fall müssen Sie das Limit für geladene Daten erhöhen.
- Wenn die Daten aus Quellen empfangen wurden, deren Zeitzone nicht mit der aktuellen Zeitzone übereinstimmt, können Sie die Zeitzone anpassen. Nach dem Aufbau werden die Daten in der vom Benutzer ausgewählten Zone angezeigt. ![hydra TZ](../../../images/hydra_tz.png)
- Da einige Quellen bestimmte Daten nicht zum Download bereitstellen, enthält das Programm das Feld [Build from](any_market_data_types.md). Über dieses Feld kann der Benutzer Marktdaten aus einem anderen Marktdatentyp aufbauen. Dieselbe Funktion kann verwendet werden, um Marktdaten ohne zusätzlichen Download auf Basis bereits vorhandener Daten zu erstellen.
- Nachdem die oben genannten Parameter ausgewählt wurden, klicken Sie auf die Schaltfläche ![hydra find](../../../images/hydra_find.png).![hydra candles tf](../../../images/hydra_candles_tf.png)

Über das Kontextmenü können verschiedene Parameter der Tabelle mit Marktdatenwerten konfiguriert werden: Zeilengruppierung, verfügbare Spalten, Anzeigeformat usw.

![hydra export context](../../../images/hydra_export_context.png)
