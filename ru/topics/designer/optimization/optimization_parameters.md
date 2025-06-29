# Параметры оптимизации

Оптимизация производится по параметрам стратегии, имеющим следующий тип:

- числовые (целочисленные и дробные)
- время ([TimeSpan](xref:System.TimeSpan))
- булевое значение (Истина-Ложь)
- [Unit](../../api/strategies/unit_type.md) значение

По-умолчанию все параметры с этими типами будут представлены в таблице [параметров оптимизатора](brute_force.md). В случае, если необходимо отключить какой-то параметр из оптимизации, то:

- в случае [схемы](../strategies/using_visual_designer.md) необходимо выбрать нужный кубик, открыть его свойство, переключиться на **Расширенные настройки** и выключить флажок **Параметр**:

![Designer Optimization 01](../../../images/designer_optimization_01.png)

- в случае [кода](../strategies/using_code.md) необходимо прописать код при определении параметра, и изменить свойство [CanOptimize](xref:StockSharp.Algo.Strategies.IStrategyParam.CanOptimize):

```cs
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);
			
// turn off param for optimization
_long.CanOptimize = false;
```

После изменения доступных параметров оптимизации необходимо переоткрыть [панель с оптимизацией](brute_force.md).
