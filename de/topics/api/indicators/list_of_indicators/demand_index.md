# DI

**Nachfrageindex (DI)** ist ein von James Sibbett entwickelter technischer Indikator, der die Beziehung zwischen Preis und Volumen analysiert, um die Nachfragestärke und den Käuferdruck auf dem Markt zu beurteilen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [DemandIndex](xref:StockSharp.Algo.Indicators.DemandIndex) verwenden.

## Beschreibung

Der Nachfrageindex (DI) ist ein umfassender Volumenindikator, der das Verhältnis zwischen Preis und Volumen bewertet, um zu bestimmen, wie stark der Käuferdruck (Nachfrage) im Vergleich zum Verkäuferdruck ist. Der Indikator basiert auf der Annahme, dass das Verhältnis von Preisänderung zu Volumenänderung eine genauere Einschätzung der Marktnachfrage ermöglicht als die einfache Beobachtung von Preis oder Volumen einzeln.

DI zielt darauf ab, die folgenden Marktsituationen zu identifizieren:
- Starke Nachfrage (Käuferdruck)
- Schwache Nachfrage (Verkäuferdruck)
- Ungleichgewicht zwischen Preis und Volumen (potenzielle Umkehrpunkte)
- Bestätigung oder Widerlegung des aktuellen Trends

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 13)

## Berechnung

Die Nachfrageindex-Berechnung ist recht komplex und umfasst mehrere Schritte:

1. Berechnung der Preiskomponente aufgrund der Preisänderung:
   ```
   Preiskomponente = ((High + Low + Close) / 3) - ((vorheriges Hoch + vorheriges Tief + vorheriger Schlusskurs) / 3)
   ```

2. Berechnung der Volumenkomponente unter Berücksichtigung der relativen Volumenänderung.

3. Berechnung der Nachfrage als Verhältnis von Preis- und Volumenkomponenten:
   ```
   Roh-Nachfrage = Preiskomponente / Volumenkomponente
   ```

4. Glätten der erhaltenen Werte, um das Rauschen zu reduzieren:
   ```
   geglättete Nachfrage = EMA(Roh-Nachfrage, Length)
   ```

5. Normalisieren des Ergebnisses, um den endgültigen Index zu erhalten:
   ```
   Nachfrageindex = 100 * Normalized(geglättete Nachfrage)
   ```

## Interpretation

Der Nachfrageindex kann auf verschiedene Arten interpretiert werden:

1. **Extreme Level**:
   - Hohe positive Werte deuten auf eine starke Nachfrage hin (Käuferdruck)
   - Hohe negative Werte weisen auf eine schwache Nachfrage hin (Verkäuferdruck)

2. **Nulllinienübergänge**:
   - Ein Übergang von unten nach oben kann als bullisches Signal angesehen werden
   - Ein Übergang von oben nach unten kann als bärisches Signal angesehen werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, aber DI bildet ein höheres Tief
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, aber DI bildet ein niedrigeres Hoch

4. **DI-Trends**:
   - Anhaltend positive DI-Werte bestätigen einen Aufwärtstrend
   - Anhaltend negative DI-Werte bestätigen einen Abwärtstrend

5. **Extreme Werte**:
   - Sehr hohe oder sehr niedrige Werte können auf überkaufte oder überverkaufte Marktbedingungen hinweisen

Der Einsatz des Nachfrageindex ist am effektivsten, wenn er mit anderen Indikatoren und Analysemethoden kombiniert wird, um falsche Signale herauszufiltern.

![indicator_demand_index](../../../../images/indicator_demand_index.png)

## Siehe auch

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[BalanceOfPower](balance_of_power.md)
