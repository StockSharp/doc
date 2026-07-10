# Orderregistrierung

![Designer Positionseröffnung 00](../../../../../../images/designer_position_opening_00.png)

Die Komponente „Orderregistrierung“ wird verwendet, um Handelsorders für ein ausgewähltes Instrument zu platzieren.

## Eingabe-Sockets

- **Handelsinstrument** - das ausgewählte Instrument für die Order.
- **Preis** - gibt den Preis für eine Limit-Order an.
- **Auslöser** - Aktivierungssignal für die Order; akzeptiert jeden Wert außer `False`.
- **Volumen** - die Anzahl der Instrumente für die Order.
- **Portfolio** - das Portfolio, in dem die Order platziert wird.

## Ausgabe-Sockets

- **Auftrag** - Informationen über die platzierte Order.
- **Fehler** - Informationen über Fehler bei der Orderregistrierung.
- **Transaktion** - Informationen über die für die Order ausgeführte Transaktion.
- **Stornierung** - Signal, dass die Order storniert wurde.
- **Ausgeführt** - Signal, dass die Order vollständig ausgeführt wurde.
- **Abgeschlossen** - Signal, das die Ereignisse Fehler, Stornierung oder vollständige Ausführung der Order zusammenfasst.

## Parameter

- **Richtung** - bestimmt, ob es sich um eine Kauf- oder Verkaufsorder handelt.
- **Market-Order** - gibt an, ob die Order eine Market-Order ist.
- **Nullpreis** - wenn der Preis auf null gesetzt ist, wird die Order als Market-Order registriert.
- **Lebensdauer** - die Dauer, für die eine Limit-Order aktiv bleibt.

## Einrichtung bedingter Orders

**Bedingte Order** - eine Order mit zusätzlichen Bedingungen, die abhängig von der aktuellen Marktsituation den Zeitpunkt der Platzierung im Handelssystem bestimmen.

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Verbindung** - die Verbindung, über die die Order platziert wird.
- **Stop-Order-Typ** - der Typ der Stop-Order.
- **Ergebnis** - das Ergebnis der ausgeführten Stop-Order.
- **Instrumentkennung** - die Kennung des Instruments für Stop-Orders mit Bedingungen, die sich auf ein anderes Instrument beziehen.
- **Stop-Preis-Bedingung** - die Stop-Preis-Bedingung. Wird für Orders wie „Stop-Preis für ein anderes Instrument“ verwendet.
- **Stop-Preis** - der Stop-Preis, der die Auslösebedingung für die Stop-Order festlegt.
- **Stop-Limit-Preis** - ähnlich wie der Stop-Preis, wird aber nur für Orders des Typs „Take-Profit und Stop-Limit“ verwendet.
- **Stop-Limit zum Marktpreis** - gibt an, ob die „Stop-Limit“-Order zum Marktpreis ausgeführt wird.
- **Intervall der Bedingungsprüfung** - das Zeitintervall, in dem die Bedingungen der Order nur innerhalb des angegebenen Zeitraums geprüft werden (bei null erfolgen keine Prüfungen). Wird für die Typen „Take-Profit und Stop-Limit“ und „Take-Profit und Stop-Limit nach Order“ verwendet.
- **Ausführungskennung der bedingten Order** - die Kennung der bedingten Order auf Ausführungsbasis.
- **Richtung der bedingten Order nach Ausführung** - die Richtung der bedingten Order auf Ausführungsbasis.
- **Aktivierung bei Teilausführung** - die Teilausführung der Order wird berücksichtigt. Eine Order „nach Ausführung“ wird bei Teilausführung der Bedingungsorder aktiviert.
- **Ausgeführtes Volumen** - verwendet das ausgeführte Volumen der Order als Menge für die Platzierung der Stop-Order. Die Anzahl der Wertpapiere in einer Order „nach Ausführung“ wird als ausgeführtes Volumen der Bedingungsorder übernommen.
- **Preis der verknüpften Order** - der Preis der verknüpften Limit-Order.
- **Rücknahme bei Teilausführung** - gibt an, ob die Stop-Order bei Teilausführung der verknüpften Limit-Order zurückgezogen wird.
- **Abstand vom Maximum** - der Versatz vom Maximum (Minimum) des Preises der letzten Transaktion.
- **Schutz-Spread** - die Größe des Schutz-Spreads.
- **Take-Profit zum Marktpreis** - gibt an, ob die „Take-Profit“-Order zum Marktpreis ausgeführt wird.

## Hinweis

Die Arbeit mit Orders ist eine Low-Level-Methode zur Positionsverwaltung. Für eine Verwaltung auf höherer Ebene wird empfohlen, die Komponente „Position ändern“ zu verwenden, die unter [Position ändern](../positions/modify.md) beschrieben ist.

## Siehe auch

[Position ändern](../positions/modify.md)

