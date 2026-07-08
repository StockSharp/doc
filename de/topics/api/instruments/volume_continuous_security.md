# Volumenbasierte fortlaufende Futures (VolumeContinuousSecurity)

## Überblick

Die Klasse `VolumeContinuousSecurity` stellt einen kontinuierlichen Futures-Kontrakt dar, bei dem der Übergang (Rollover) zwischen Kontrakten auf Basis des Handelsvolumens oder des Open Interest erfolgt. Dies unterscheidet sich von `ExpirationContinuousSecurity`, bei dem der Wechsel nach vordefinierten Verfallsdaten erfolgt.

Beide Klassen erben von `ContinuousSecurity`, das wiederum von `BasketSecurity` erbt.

## Unterschied zu ExpirationContinuousSecurity

| Merkmal | ExpirationContinuousSecurity | VolumeContinuousSecurity |
|---|---|---|
| Rollover-Bedingung | Verfallsdatum (fix) | Volumen- oder Open-Interest-Schwelle |
| Konfiguration | Dictionary `SecurityId -> DateTime` | Liste `SecurityId` + `VolumeLevel` |
| Vorhersagbarkeit | Wechsel nach Zeitplan | Wechsel nach Marktbedingungen |
| Basket-Code | `CE` | `CV` |

`ExpirationContinuousSecurity` erfordert die manuelle Angabe von Übergangsdaten für jeden Kontrakt. `VolumeContinuousSecurity` wechselt automatisch zum nächsten Kontrakt, wenn dessen Handelsvolumen (oder Open Interest) die angegebene Schwelle überschreitet.

## Haupteigenschaften

```csharp
public class VolumeContinuousSecurity : ContinuousSecurity
{
    // Liste innerer Securities (Kontrakte), geordnet nach Rollover-Reihenfolge
    public SynchronizedList<SecurityId> InnerSecurities { get; }

    // Open Interest statt Volumen zur Rollover-Bestimmung verwenden
    public bool IsOpenInterest { get; set; }

    // Volumenschwelle, bei der der Wechsel zum nächsten Kontrakt erfolgt
    public Unit VolumeLevel { get; set; }
}
```

Die Eigenschaft `VolumeLevel` hat den Typ `Unit`, wodurch sowohl absolute als auch prozentuale Werte angegeben werden können.

## Verwendungsbeispiel

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Volumenbasierten kontinuierlichen Future erstellen
var continuous = new VolumeContinuousSecurity
{
    Id = "ES-CONT@CME",
    Board = ExchangeBoard.Cme,
};

// Kontrakte in Rollover-Reihenfolge hinzufügen
continuous.InnerSecurities.AddRange(new[]
{
    "ES-3.26@CME".ToSecurityId(),
    "ES-6.26@CME".ToSecurityId(),
    "ES-9.26@CME".ToSecurityId(),
});

// Volumenschwelle für den Wechsel festlegen
continuous.VolumeLevel = new Unit(10000);

// Oder Open Interest verwenden
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## Beispiel mit ExpirationContinuousSecurity zum Vergleich

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Verfallsbasierter kontinuierlicher Future
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "ES-CONT-EXP@CME",
    Board = ExchangeBoard.Cme,
};

// Exakte Übergangsdaten für jeden Kontrakt angeben
expContinuous.ExpirationJumps.Add(
    "ES-3.26@CME".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "ES-6.26@CME".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## Einsatzfälle

`VolumeContinuousSecurity` eignet sich für Situationen, in denen:

- Exakte Rollover-Daten nicht im Voraus bekannt sind
- Ein Wechsel auf Basis der Liquidität (Handelsvolumen oder Open Interest) erforderlich ist
- Ein anpassungsfähigerer Übergang benötigt wird, der auf Marktbedingungen reagiert

`ExpirationContinuousSecurity` ist vorzuziehen, wenn Verfallsdaten im Voraus bekannt sind und ein deterministischer Rollover erforderlich ist.

