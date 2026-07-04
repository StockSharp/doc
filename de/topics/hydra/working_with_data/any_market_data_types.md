# Beliebige Marktdatentypen

[Hydra](../../hydra.md) ermoeglicht die Verwendung alternativer Datentypen, um verschiedene Arten von Marktdaten zu erhalten.

Dies ist erforderlich, wenn die Quelle das Herunterladen der benoetigten Marktdaten nicht ermoeglicht. So koennen zum Beispiel mehrere Marktdatentypen gleichzeitig verwendet werden, um das **Order book** zu erstellen.

WICHTIG\! **Order book** kann aus **Order Log** oder **Level 1** erstellt werden, sofern diese Datentypen die besten Preise enthalten.

Beachten Sie, dass **Level 1**-Werte aus jeder Quelle heruntergeladen werden koennen, die Echtzeit-Marktdaten bereitstellt. **Level 1** kann auch durch [Konvertierung](../tasks/converter.md) aus dem **Order book** erhalten werden.

Fuer die Erstellung gehen Sie wie folgt vor:

1. Waehlen Sie den Zeitraum und das Instrument aus, fuer die Sie Marktdaten erhalten moechten.![hydra LEVEL 1 build depth data](../../../images/hydra_level1_build_depth_data.png)
2. Waehlen Sie das Feld **Build from** aus und waehlen Sie den erforderlichen Datentyp.![hydra type build data](../../../images/hydra_type_build_data.png)

   WICHTIG\! Wenn **Order Book, Order Log, Level 1** als Quelle fuer die Kerzenerstellung ausgewaehlt sind, erscheint eine Auswahl zusaetzlicher Parameter.![hydra ext proper build data](../../../images/hydra_ext_proper_build_data.png)
3. Nachdem Sie die Parameter festgelegt haben, klicken Sie auf die Schaltflaeche ![hydra candles](../../../images/hydra_candles.png).![hydra LEVEL 1 build depth data result](../../../images/hydra_level1_build_depth_data_result.png)

Zum Erstellen von **Candles** ist auch die Option verfuegbar, Kerzen mit groesserem Time Frame aus Kerzen mit kleinerem Time Frame zu erstellen.

Wenn zum Beispiel Kerzen mit einem Time Frame von 1 Minute vorhanden sind, koennen Sie daraus Kerzen mit einem Time Frame von 5 Minuten erstellen, indem Sie in der Zeile **Build from** den entsprechenden Typ auswaehlen.

**Sehen Sie sich das [Video-Tutorial](../videos/building_order_books.md) an**
