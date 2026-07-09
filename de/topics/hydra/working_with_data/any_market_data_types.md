# Beliebige Marktdatentypen

[Hydra](../../hydra.md) ermöglicht die Verwendung alternativer Datentypen, um verschiedene Arten von Marktdaten zu erhalten.

Dies ist erforderlich, wenn die Quelle das Herunterladen der benötigten Marktdaten nicht ermöglicht. So können zum Beispiel mehrere Marktdatentypen gleichzeitig verwendet werden, um das **Order book** zu erstellen.

WICHTIG\! **Order book** kann aus **Order Log** oder **Level 1** erstellt werden, sofern diese Datentypen die besten Preise enthalten.

Beachten Sie, dass **Level 1**-Werte aus jeder Quelle heruntergeladen werden können, die Echtzeit-Marktdaten bereitstellt. **Level 1** kann auch durch [Konvertierung](../tasks/converter.md) aus dem **Order book** erhalten werden.

Für die Erstellung gehen Sie wie folgt vor:

1. Wählen Sie den Zeitraum und das Instrument aus, für die Sie Marktdaten erhalten moechten.![hydra LEVEL 1 build depth data](../../../images/hydra_level1_build_depth_data.png)
2. Wählen Sie das Feld **Erstellen aus** aus und wählen Sie den erforderlichen Datentyp.![hydra type build data](../../../images/hydra_type_build_data.png)

   WICHTIG\! Wenn **Order Book, Order Log, Level 1** als Quelle für die Kerzenerstellung ausgewählt sind, erscheint eine Auswahl zusaetzlicher Parameter.![hydra ext proper build data](../../../images/hydra_ext_proper_build_data.png)
3. Nachdem Sie die Parameter festgelegt haben, klicken Sie auf die Schaltfläche ![hydra candles](../../../images/hydra_candles.png).![hydra LEVEL 1 build depth data result](../../../images/hydra_level1_build_depth_data_result.png)

Zum Erstellen von **Kerzen** ist auch die Option verfügbar, Kerzen mit größerem Time Frame aus Kerzen mit kleinerem Time Frame zu erstellen.

Wenn zum Beispiel Kerzen mit einem Time Frame von 1 Minute vorhanden sind, können Sie daraus Kerzen mit einem Time Frame von 5 Minuten erstellen, indem Sie in der Zeile **Erstellen aus** den entsprechenden Typ auswählen.

**Sehen Sie sich das [Video-Tutorial](../videos/building_order_books.md) an**
