# Besten Preis für ein Instrument abrufen

Um eine Kauforder zum aktuell besten Preis für das Instrument zu registrieren, kann das folgende Schema verwendet werden:

![Designer Get the best rates for the tool 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

Für den Würfel [Variable](../elements/data_sources/variable.md) wird der Datentyp **Instrument** ausgewählt. Wenn das Instrument nicht angegeben ist, aber das Flag **Parameter** der Eigenschaftsgruppe **Allgemein** gesetzt ist, wird es aus der Strategie übernommen und an den Würfel [Orderbuch](../elements/market_depths/order_book.md) übergeben. Nachdem der Würfel [Orderbuch](../elements/market_depths/order_book.md) das aktuelle Instrument aus der Variablen erhalten hat, gibt er die Orderbuchänderungen des ausgewählten Instruments über den Ausgabeparameter weiter. Beim Empfang von Orderbuchänderungen wählt der Würfel [Konverter](../elements/converters/converter.md) daraus den aktuellen Wert des besten Kaufpreises aus.

## Empfohlene Inhalte

[Aktuelle Position abrufen](get_current_position.md)

