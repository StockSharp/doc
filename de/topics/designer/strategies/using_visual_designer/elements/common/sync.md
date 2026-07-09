## Synchronisierung

![Designer Sync 00](../../../../../../images/designer_sync_00.png)

Der Block Synchronisierung dient dazu, Daten aus verschiedenen Quellen (zum Beispiel Kerzen verschiedener Instrumente, unterschiedliche Zeitrahmen, Kombinationen aus Kerzen und Transaktionen) zu sammeln und zu synchronisieren und sie anschließend auszugeben, wenn eine bestimmte Menge angesammelt wurde. Dieser Block ist nützlich zum Erstellen eigener Indizes oder für Arbitrage.

## Eingabe-Sockets

- **Eingang**: Wenn eine neue Datenquelle verbunden wird, werden automatisch ein entsprechender ausgehender Socket und ein neuer eingehender Socket erstellt. Die Anzahl eingehender Werte ist unbegrenzt.

## Parameter

- **Intervall**: Legt die Zeit fest, nach der Daten aktualisiert oder gelöscht werden müssen. Wenn ein eingehender Wert mit einer Zeit eintrifft, die den vorherigen Wert plus Intervall überschreitet, werden alte Daten gelöscht und ein neuer Zyklus der Datensammlung beginnt.
- **Elemente löschen**: Wenn diese Option aktiviert ist, werden Daten nach ihrer Sammlung für alle verbundenen eingehenden Sockets gelöscht; andernfalls werden Daten gesammelt, bis Daten aus dem nächsten Zeitintervall erscheinen.

## Verwendungsbeispiele

1. Erstellen eines eigenen Index für mehrere Aktien, bei dem unterschiedliche Zeitreihen aus verschiedenen Datenquellen berücksichtigt werden müssen.
2. Arbitrage zwischen verschiedenen Märkten mithilfe synchronisierter Zeitdaten, um zeitliche Preisunterschiede zu erkennen.

![Designer Sync 01](../../../../../../images/designer_sync_01.png)
