# 优化参数

可以对以下类型的策略参数执行优化：

- 数值（整数和小数）
- 时间（[TimeSpan](xref:System.TimeSpan)）
- 布尔值（True-False）
- [Unit](../../api/strategies/unit_type.md) 值

默认情况下，所有这些类型的参数都会显示在[优化器参数表](brute_force.md)中。如果需要从优化中排除某个参数，请按以下方式操作：

- 对于[策略图](../strategies/using_visual_designer.md)，选择所需模块并打开其属性，切换到 **Advanced settings**，然后清除 **Parameter** 复选框：

![Designer Optimization 01](../../../images/designer_optimization_01.png)

- 对于[代码](../strategies/using_code.md)，在定义参数时编写相应代码，并修改 [CanOptimize](xref:StockSharp.Algo.Strategies.IStrategyParam.CanOptimize) 属性：

```cs
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);
			
// turn off param for optimization
_long.CanOptimize = false;
```

更改可用的优化参数后，需要重新打开[优化面板](brute_force.md)。
