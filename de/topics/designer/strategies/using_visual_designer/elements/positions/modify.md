# Position ändern

![Designer Positionsansicht ändern 00](../../../../../../images/designer_position_modify_00.png)

Die Komponente „Position ändern“ wird verwendet, um eine Handelsposition anhand angegebener Bedingungen zu ändern.

## Eingabeanschlüsse

- **Handelsinstrument**: Das Instrument, dessen Position geändert wird.
- **Auslöser**: Signal zum Aktivieren der Positionsänderung.
- **Portfolio**: Das Portfolio, in dem der Vorgang ausgeführt wird.
- **Volumen** (optional): Das Volumen für die Vorgänge „Erhöhen“ und „Verringern“. Wird für „Umkehren“ und „Position schließen“ nicht verwendet.
- **Letzter Preis** und **Letztes Volumen**: Für die Algorithmen „VWAP“ und „Iceberg“ sind Daten über den letzten Preis und das letzte Volumen der Transaktion erforderlich.
- **Abbrechen**: Signal zum Abbrechen der Positionseinrichtung, zum Beispiel wegen eines Timeouts.

## Ausgabeanschlüsse

- **Auftrag**: Informationen über die platzierte Order.
- **Transaktion**: Informationen über die für die Order ausgeführte Transaktion.
- **Saldo**: Dieser Anschluss überträgt Informationen über den Teil der Position, der am Ende des Positionsänderungsvorgangs nicht realisiert wurde. Der vom Anschluss zurückgegebene Wert gibt das Ergebnis des Vorgangs an:
  - `0` bedeutet, dass die Komponente den Positionsänderungsvorgang erfolgreich abgeschlossen hat und alle geplanten Aktionen ausgeführt wurden.
  - `-1` gibt an, dass die Komponente die Positionsänderung wegen einer Nichtübereinstimmung zwischen der aktuellen Position und der angegebenen Bedingung nicht gestartet hat (zum Beispiel, wenn die aktuelle Position ungleich null ist und die Bedingung „Position öffnen“ war).
  - Jeder Wert größer als `0` signalisiert, dass der Prozess der Positionsänderung vor Abschluss unterbrochen wurde. Dies kann durch Abbruch über die Schemalogik oder durch einen Fehler bei der Orderregistrierung geschehen.

## Parameter

- **Bedingung**: Bedingungen für die Positionsänderung:
  - `Keine`: Führt keine Aktionen aus.
  - `Position öffnen`: Öffnet eine Position in der angegebenen Richtung.
  - `Position schließen`: Schließt die aktuelle Position.
  - `Verringern`: Verringert die Größe der aktuellen Position.
  - `Erhöhen`: Erhöht die Größe der aktuellen Position.
  - `Umkehren`: Schließt die aktuelle Position und öffnet eine neue in entgegengesetzter Richtung.
- **Richtung**: Gibt die Richtung für „Position öffnen“ und „Keine“ an und dient bei anderen Bedingungen als optionaler Filter.
- **Algorithmus**: Optionen sind „Marktorder“, „VWAP“ und „Iceberg“.
- **Teil**: Der Anteil des Gesamtvolumens, der bei Verwendung von Algorithmen wie „VWAP“ oder „Iceberg“ in kleinere Segmente aufgeteilt wird.

Wenn die Komponente einen Auslöser empfängt, während sie bereits mit der Änderung des Volumens begonnen hat, ignoriert sie den neuen Auslöser. Wenn die Änderungsbedingungen mit dem aktuellen Zustand der Position inkompatibel sind (zum Beispiel beim Versuch, „Position öffnen“ auszuführen, obwohl bereits eine Position geöffnet ist), gibt die Komponente sofort `-1` über den Ausgabeanschluss **Saldo** zurück. Dies zeigt an, dass der Vorgang nicht erforderlich ist und nicht gestartet wurde.

## Hinweis

Für die Orderverwaltung auf niedriger Ebene kann die Komponente [Orderregistrierung](../orders/register.md) verwendet werden. Für die Positionsverwaltung auf höherer Ebene wird diese Komponente „Position ändern“ empfohlen.

## Siehe auch

- [Orderregistrierung](../orders/register.md)

