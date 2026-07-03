# Parámetros de optimización

La optimización se realiza sobre parámetros de estrategia que tienen los siguientes tipos:

- Numéricos (enteros y fraccionarios)
- Tiempo ([TimeSpan](xref:System.TimeSpan))
- Valor booleano (True-False)
- Valor [Unit](../../api/strategies/unit_type.md)

De forma predeterminada, todos los parámetros con estos tipos aparecen en la [tabla de parámetros del optimizador](brute_force.md). Para excluir un parámetro de la optimización:

- Para un [diagrama](../strategies/using_visual_designer.md), seleccione el cubo requerido, abra sus propiedades, cambie a **Advanced settings** y desactive la casilla **Parameter**:

![Designer Optimization 01](../../../images/designer_optimization_01.png)

- En el caso de [código](../strategies/using_code.md), debe escribir código al definir un parámetro y cambiar la propiedad [CanOptimize](xref:StockSharp.Algo.Strategies.IStrategyParam.CanOptimize):

```cs
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);
			
// desactivar el parámetro para optimización
_long.CanOptimize = false;
```

Después de cambiar los parámetros de optimización disponibles, vuelva a abrir el [panel de optimización](brute_force.md).
