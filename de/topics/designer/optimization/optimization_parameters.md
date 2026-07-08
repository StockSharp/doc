# Optimierungsparameter

Die Optimierung wird für Strategieparameter mit den folgenden Typen durchgeführt:

- Numerisch (ganzzahlig und gebrochen)
- Zeit ([TimeSpan](xref:System.TimeSpan))
- Boolescher Wert (True-False)
- [Unit](../../api/strategies/unit_type.md)-Wert

Standardmäßig erscheinen alle Parameter mit diesen Typen in der [Tabelle der Optimiererparameter](brute_force.md). Um einen Parameter von der Optimierung auszuschließen:

- Für ein [Diagramm](../strategies/using_visual_designer.md) wählen Sie den erforderlichen Würfel aus, öffnen seine Eigenschaften, wechseln zu **Advanced settings** und deaktivieren das Kontrollkästchen **Parameter**:

![Designer Optimization 01](../../../images/designer_optimization_01.png)

- Bei [Code](../strategies/using_code.md) müssen Sie beim Definieren eines Parameters Code schreiben und die Eigenschaft [CanOptimize](xref:StockSharp.Algo.Strategies.IStrategyParam.CanOptimize) ändern:

```cs
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);

// Parameter für die Optimierung deaktivieren
_long.CanOptimize = false;
```

Öffnen Sie nach dem Ändern der verfügbaren Optimierungsparameter das [Optimierungs-Panel](brute_force.md) erneut.

