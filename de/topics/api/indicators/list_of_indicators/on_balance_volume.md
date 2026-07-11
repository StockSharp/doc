# OBV

**On-Balance-Volumen (OBV)** ist ein von Joseph Granville entwickelter technischer Indikator, der das Handelsvolumen nutzt, um Preisänderungen vorherzusagen, indem er das Volumen basierend auf der Preisrichtung akkumuliert.

Um den Indikator verwenden zu können, müssen Sie die Klasse [OnBalanceVolume](xref:StockSharp.Algo.Indicators.OnBalanceVolume) verwenden.

## Beschreibung

On-Balance-Volumen (OBV) ist ein kumulativer Indikator, der das Volumen erhöht, wenn der Schlusskurs steigt, und das Volumen verringert, wenn der Schlusskurs fällt. Der Indikator basiert auf dem Konzept, dass Volumenänderungen Preisänderungen vorausgehen. Nach dieser Theorie ist zu erwarten, dass der Preis irgendwann steigt, wenn das Volumen deutlich zunimmt, ohne dass es zu einer entsprechenden Preisänderung kommt, und umgekehrt.

OBV zielt darauf ab, Momente zu erkennen, in denen „Smart Money“ (große institutionelle Anleger) Positionen akkumulieren oder verteilen, was zukünftige Preisbewegungen vorhersagen kann. Der Indikator ist besonders nützlich, um Divergenzen zwischen Preis und Volumen zu identifizieren, die auf mögliche Marktumkehrungen hinweisen können.

Der OBV-Indikator wurde erstmals 1963 von Joseph Granville in seinem Buch „Granvilles neuer Schlüssel zu Börsengewinnen“ vorgestellt und hat sich seitdem zu einem der am häufigsten verwendeten Volumenindikatoren entwickelt.

## Berechnung

Die On-Balance-Volumen-Berechnung ist sehr einfach:

1. Legen Sie den anfänglichen OBV-Wert fest (normalerweise 0 oder eine beliebige Zahl):
   ```
   OBV[initial] = 0
   ```

2. Für jede weitere Periode:
   ```
   Wenn Close[current] > Close[previous], dann:
       OBV[current] = OBV[previous] + Volume[current]
   Wenn Close[current] < Close[previous], dann:
       OBV[current] = OBV[previous] - Volume[current]
   Wenn Close[current] = Close[previous], dann:
       OBV[current] = OBV[previous]
   ```

Dabei gilt:
- Close - Schlusskurs
- Volume - Handelsvolumen

## Interpretation

On-Balance-Volumen kann wie folgt interpretiert werden:

1. **Trendanalyse**:
   - Steigender OBV weist auf ein in den Markt eintretendes Volumen (Akkumulation) hin, was einen Preisanstieg vorhersagen kann
   - Ein fallender OBV weist darauf hin, dass das Volumen den Markt verlässt (Verteilung), was einen Preisrückgang vorhersagen kann
   - Der flache OBV weist auf keine gerichtete Volumenbewegung hin, was einem Seitwärtstrend entsprechen könnte

2. **Bestätigung des Preistrends**:
   - Wenn sich OBV in die gleiche Richtung wie der Preis bewegt, bestätigt dies den aktuellen Preistrend
   - Wenn sich OBV und der Preis in entgegengesetzte Richtungen bewegen, kann dies auf eine mögliche Trendumkehr hinweisen

3. **Abweichungen**:
   - Bullische Divergenz: Preis bildet ein neues Tief, während OBV ein höheres Tief bildet (Kaufsignal)
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während OBV ein niedrigeres Hoch bildet (Verkaufssignal)

4. **OBV-Ausbrüche**:
   - Der Ausbruch des Widerstands- oder Unterstützungsniveaus von OBV geht oft einem ähnlichen Ausbruch auf dem Preisdiagramm voraus
   - Händler können OBV-Trendlinienausbrüche nutzen, um zukünftige Preisbewegungen vorherzusagen

5. **Grundlinie**:
   - Einige Händler verwenden gleitende Durchschnitte des OBV als „Basislinien“.
   - Das Überschreiten des gleitenden Durchschnitts durch OBV kann Handelssignale erzeugen

6. **Muster der technischen Analyse**:
   - Auf dem OBV-Chart können sich klassische technische Analysemuster bilden, wie z. B. „Kopf und Schultern“, „Doppelboden“ usw.
   - Diese Muster können zusätzliche Handelssignale liefern

7. **Volumenspitzen**:
   - Plötzliche starke Veränderungen bei OBV können auf erhebliche Veränderungen der Marktstimmung hinweisen
   - Solche „Volumenspitzen“ gehen häufig erheblichen Preisbewegungen voraus

Es ist wichtig zu beachten, dass OBV ein kumulativer Indikator ist, sodass sein absoluter Wert keine große Bedeutung hat. Entscheidend ist die Richtung der OBV-Bewegung und ihr Verhältnis zur Preisbewegung.

![indicator_on_balance_volume](../../../../images/indicator_on_balance_volume.png)

## Siehe auch

[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
[NegativeVolumeIndex](negative_volume_index.md)
