# Order Registration

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

Die Komponente „Order Registration“ wird verwendet, um Handelsorders für ein ausgewähltes Instrument zu platzieren.

## Eingabe-Sockets

- **Instrument** - das ausgewählte Instrument für die Order.
- **Price** - gibt den Preis für eine Limit-Order an.
- **Trigger** - Aktivierungssignal für die Order; akzeptiert jeden Wert außer `False`.
- **Volume** - die Anzahl der Instrumente für die Order.
- **Portfolio** - das Portfolio, in dem die Order platziert wird.

## Ausgabe-Sockets

- **Order** - Informationen über die platzierte Order.
- **Error** - Informationen über Fehler bei der Orderregistrierung.
- **Transaction** - Informationen über die für die Order ausgeführte Transaktion.
- **Cancellation** - Signal, dass die Order storniert wurde.
- **Executed** - Signal, dass die Order vollständig ausgeführt wurde.
- **Completed** - Signal, das die Ereignisse Fehler, Stornierung oder vollständige Ausführung der Order zusammenfasst.

## Parameter

- **Direction** - bestimmt, ob es sich um eine Kauf- oder Verkaufsorder handelt.
- **Market Order** - gibt an, ob die Order eine Market-Order ist.
- **Zero Price** - wenn der Preis auf null gesetzt ist, wird die Order als Market-Order registriert.
- **Lifetime** - die Dauer, für die eine Limit-Order aktiv bleibt.

## Einrichtung bedingter Orders

**Conditional Order** - eine Order mit zusätzlichen Bedingungen, die abhängig von der aktuellen Marktsituation den Zeitpunkt der Platzierung im Handelssystem bestimmen.

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** - die Verbindung, über die die Order platziert wird.
- **Stop Order Type** - der Typ der Stop-Order.
- **Result** - das Ergebnis der ausgeführten Stop-Order.
- **Instrument Identifier** - die Kennung des Instruments für Stop-Orders mit Bedingungen, die sich auf ein anderes Instrument beziehen.
- **Stop Price Condition** - die Stop-Preis-Bedingung. Wird für Orders wie „Stop price for another instrument“ verwendet.
- **Stop Price** - der Stop-Preis, der die Auslösebedingung für die Stop-Order festlegt.
- **Stop-Limit Price** - ähnlich wie Stop Price, wird aber nur für Orders des Typs „Take-profit and stop-limit“ verwendet.
- **Stop-Limit at Market Price** - gibt an, ob die „Stop-Limit“-Order zum Marktpreis ausgeführt wird.
- **Condition Check Interval** - das Zeitintervall, in dem die Bedingungen der Order nur innerhalb des angegebenen Zeitraums geprüft werden (bei null erfolgen keine Prüfungen). Wird für die Typen „Take-profit and stop-limit“ und „Take-profit and stop-limit by order“ verwendet.
- **Conditional Order Execution Identifier** - die Kennung der bedingten Order auf Ausführungsbasis.
- **Direction of Conditional Order by Execution** - die Richtung der bedingten Order auf Ausführungsbasis.
- **Activation on Partial Execution** - die Teilausführung der Order wird berücksichtigt. Eine „on-execution“-Order wird bei Teilausführung der Bedingungsorder aktiviert.
- **Executed Volume** - verwendet das ausgeführte Volumen der Order als Menge für die Platzierung der Stop-Order. Die Anzahl der Wertpapiere in einer „on-execution“-Order wird als ausgeführtes Volumen der Bedingungsorder übernommen.
- **Price of Linked Order** - der Preis der verknüpften Limit-Order.
- **Withdrawal on Partial Execution** - gibt an, ob die Stop-Order bei Teilausführung der verknüpften Limit-Order zurückgezogen wird.
- **Offset from Maximum** - der Versatz vom Maximum (Minimum) des Preises der letzten Transaktion.
- **Protective Spread** - die Größe des Schutz-Spreads.
- **Take-Profit at Market Price** - gibt an, ob die „Take-Profit“-Order zum Marktpreis ausgeführt wird.

## Hinweis

Die Arbeit mit Orders ist eine Low-Level-Methode zur Positionsverwaltung. Für eine Verwaltung auf höherer Ebene wird empfohlen, die Komponente „Modify Position“ zu verwenden, die unter [Modify Position](../positions/modify.md) beschrieben ist.

## Siehe auch

[Modify Position](../positions/modify.md)

