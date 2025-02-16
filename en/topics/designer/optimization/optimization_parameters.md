# Optimization Parameters

Optimization is performed on strategy parameters that have the following types:

- Numerical (integer and fractional)
- Time ([TimeSpan](xref:System.TimeSpan))
- Boolean value (True-False)
- [Unit](../../api/strategies/unit_type.md) value

By default, all parameters with these types will be represented in the [optimizer parameters table](brute_force.md). If it is necessary to exclude some parameter from the optimization, then:

- In the case of a [diagram](../strategies/using_visual_designer.md), it is necessary to select the required cube, open its property, switch to **Advanced settings** and turn off the **Parameter** checkbox:

![Designer Optimization 01](../../../images/designer_optimization_01.png)

- In the case of [code](../strategies/using_code.md), you need to write code when defining a parameter and change the [CanOptimize](xref:StockSharp.Algo.Strategies.IStrategyParam.CanOptimize) property:

```cs
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);
			
// turn off param for optimization
_long.CanOptimize = false;
```

After changing the available optimization parameters, it is necessary to reopen the [optimization panel](brute_force.md).