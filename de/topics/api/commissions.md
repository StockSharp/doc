# Provisionssystem

[S#](../api.md) implementiert ein flexibles System zur Provisionsberechnung über den [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager). Der Manager nimmt Order- und Trade-Nachrichten entgegen und berechnet Provisionen anhand konfigurierter Regeln.

## Die Schnittstelle ICommissionManager

Die Schnittstelle [ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) definiert den Basisvertrag:

- **Regeln** — eine Sammlung von [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule)-Regeln zur Provisionsberechnung.
- **Provision** — der gesamte aufgelaufene Provisionsbetrag (decimal).
- **Reset()** — setzt den Zustand des Managers und aller Regeln zurück.
- **Process(Message)** — verarbeitet eine Nachricht; gibt die Provision für die angegebene Nachricht oder `null` zurück.

## Die Schnittstelle ICommissionRule

Jede Regel implementiert [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule):

- **Titel** — der Titel der Regel.
- **Wert** — der Provisionswert ([Unit](xref:Ecng.ComponentModel.Unit)), kann absolut oder prozentual sein.
- **Process(ExecutionMessage)** — berechnet die Provision für eine bestimmte Nachricht.

Die Basisklasse [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) enthält eine Hilfsmethode `GetValue(price, volume)`:

- Für **absolute** Werte gibt sie `Value` unverändert zurück.
- Für **prozentuale** Werte berechnet sie `(price * volume * Value) / 100`.

## Regeltypen

### Order-Regeln

| Klasse | Beschreibung |
|-------|-------------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | Provision pro Order (basierend auf Orderpreis und -volumen). |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | Provision basierend auf dem Ordervolumen. Für absolute Werte: `Value * volume`. |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | Provision für jeweils N Orders. Die Eigenschaft `Count` legt den Schwellenwert fest. |

### Trade-Regeln

| Klasse | Beschreibung |
|-------|-------------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | Provision pro Trade (basierend auf Trade-Preis und -Volumen). |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | Provision basierend auf dem Trade-Volumen. |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | Provision: `price * volume * Value`. |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | Provision für jeweils N Trades. Die Eigenschaft `Count` legt den Schwellenwert fest. |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | Provision für jeden Umsatzschwellenwert. Die Eigenschaft `TurnOver` legt den Schwellenwert fest. |

### Filter-Regeln

| Klasse | Beschreibung |
|-------|-------------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | Provision nur für ein bestimmtes Instrument. Eigenschaft `Security`. |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | Provision nur für ein bestimmtes Börsenboard. Eigenschaft `Board`. |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | Provision nur für einen bestimmten Instrumententyp. Eigenschaft `SecurityType`. |

## Integration über den Adapter

Die Klasse [CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) umschließt einen inneren Adapter und berechnet automatisch Provisionen für ein- und ausgehende [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)-Nachrichten. Wenn bei einer Nachricht das Feld `Commission` nicht gesetzt ist, füllt der Adapter es aus dem Manager.

## Integration mit der Strategy

Die Strategie ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) stellt die Eigenschaft `Commission` bereit, über die Sie die aufgelaufene Provision verfolgen können.

## Anwendungsbeispiel

```cs
var manager = new CommissionManager();

// Feste Kommission von 1,5 pro Trade
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 0,1 % des Umsatzes für Futures
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// Kommission von 50 je 100 Orders
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// Kommission von 10 je 1.000.000 Umsatz
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// Nachricht verarbeiten
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Kommission für Nachricht: {commission.Value}");
}

// Gesamte aufgelaufene Kommission
Console.WriteLine($"Gesamte Kommission: {manager.Commission}");
```

## Zustand zurücksetzen

Die Methode `Reset()` setzt die Gesamtprovision auf null zurück und ruft `Reset()` bei jeder Regel auf, wodurch interne Zähler (Orderanzahl, aktueller Umsatz usw.) gelöscht werden:

```cs
manager.Reset();
```
