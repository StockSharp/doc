# EMV

**Bewegungsleichtigkeit (EMV)** ist ein von Richard Arms entwickelter technischer Indikator, der Preisänderungen mit dem Volumen korreliert, um zu beurteilen, wie leicht sich der Preis nach oben oder unten bewegt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [EaseOfMovement](xref:StockSharp.Algo.Indicators.EaseOfMovement) verwenden.

## Beschreibung

Der Bewegungsleichtigkeit (EMV)-Indikator wurde entwickelt, um die Beziehung zwischen Preisbewegung und Volumen zu messen. Das Hauptkonzept des Indikators besteht darin, dass sich der Preis in einem Aufwärtstrend bei geringem Volumen leicht nach oben bewegen sollte, während er in einem Abwärtstrend auch bei geringem Volumen leicht nach unten gehen sollte.

EMV kombiniert Informationen über Preisspanne, Preisänderung und Volumen, um ein Maß für die „Leichtigkeit“ der Preisbewegung zu erstellen. Positive EMV-Werte zeigen an, dass der Preis relativ leicht steigt, während negative Werte darauf hinweisen, dass der Preis relativ leicht fällt.

Der Indikator ist besonders nützlich für:
- Bestätigung der Stärke oder Schwäche des aktuellen Trends
- Identifizieren potenzieller Umkehrpunkte
- Abweichungen vom Preis erkennen
- Bewertung der „Qualität“ der Preisbewegung unter Berücksichtigung des Volumens

## Parameter

Der Indikator hat die folgenden Parameter:
- **Länge** – Glättungszeitraum (Standardwert: 14)

## Berechnung

Die Berechnung des Bewegungsleichtigkeit-Indikators umfasst die folgenden Schritte:

1. Mittelpunktbewegung berechnen:
   ```
   Mittelpunkt = (High + Low) / 2
   Mittelpunktsbewegung = Mittelpunkt[current] - Mittelpunkt[previous]
   ```

2. Berechnen Sie das Box-Verhältnis (Volumen-Abstand-Koeffizient):
   ```
   Volumen-Distanz-Verhältnis = Volume / (High - Low)
   ```

3. Berechnen Sie EMV für eine Periode:
   ```
   Einperioden-EMV = Mittelpunktsbewegung / Volumen-Distanz-Verhältnis
   ```

4. Glätten Sie, um das endgültige EMV zu erhalten:
   ```
   EMV = SMA(Einperioden-EMV, Length)
   ```

Dabei gilt:
- High – höchster Preis der Kerze
- Low – niedrigster Preis der Kerze
- Volume - Handelsvolumen
- SMA – einfacher gleitender Durchschnitt

## Interpretation

Der EMV-Indikator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Der Übergang von unten nach oben (von negativen zu positiven Werten) kann als zinsbullisches Signal angesehen werden, das darauf hindeutet, dass der Preis leicht zu steigen beginnt
   - Ein Übergang von oben nach unten (von positiven zu negativen Werten) kann als bärisches Signal angesehen werden, was darauf hindeutet, dass der Preis beginnt, sich leicht nach unten zu bewegen

2. **Extreme Werte**:
   - Hohe positive Werte zeigen an, dass der Preis sehr leicht steigt (bei geringem Volumen)
   - Hohe negative Werte zeigen an, dass sich der Preis sehr leicht nach unten bewegt (bei geringem Volumen).

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während EMV ein höheres Tief bildet (kann auf eine mögliche Aufwärtsumkehr hinweisen)
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während EMV ein niedrigeres Hoch bildet (kann auf eine mögliche Abwärtsumkehr hinweisen)

4. **EMV-Trends**:
   - Anhaltend positive Werte bestätigen einen Aufwärtstrend
   - Anhaltend negative Werte bestätigen einen Abwärtstrend
   - Schwankungen um Null können auf einen Seitwärtstrend oder eine Konsolidierung hinweisen

5. **Volumenanalyse**:
   - Wenn der Preis mit einem positiven EMV steigt, bestätigt dies die Stärke der Aufwärtsbewegung
   - Wenn der Preis mit einem negativen EMV fällt, bestätigt dies die Stärke der Abwärtsbewegung
   - Wenn der Preis bei einem negativen EMV steigt oder bei einem positiven EMV fällt, kann dies auf die Instabilität der aktuellen Bewegung hinweisen

![EMV Diagramm](../../../../images/indicator_ease_of_movement.png)

## Siehe auch

[ForceIndex](force_index.md)
[BalanceOfPower](balance_of_power.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
