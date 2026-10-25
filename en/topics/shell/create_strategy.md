# Create strategy

The strategies included in [Shell](../shell.md) are located in `Shell.Avalonia/Shell.Core/Strategies/`. Your own strategy is added there as well and goes through three steps: the strategy class, an entry in the catalog and, if needed, its own panel.

## Strategy class

A strategy is a regular descendant of `Strategy` from StockSharp. There is nothing Shell-specific in it:

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "My strategy";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// The group is taken from the resources so that the parameter goes to the same group as the standard ones.
			.SetDisplay("Length", "Moving average length", LocalizedStrings.Parameters)
			.SetOptimize(10, 50, 5);
	}

	public int Length
	{
		get => _length.Value;
		set => _length.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// data subscriptions and trading logic
	}
}
```

The entry point is `OnStarted2(DateTime)`, not `OnStarted`: by the time it is called, the strategy has already received the instrument, the portfolio and the connection, so you can subscribe to data right here.

A parameter declared with `Param(...)` and a specified `SetOptimize(...)` automatically appears on the [Optimization](user_interface/optimization.md) tab: the list of iterated parameters and the iteration bounds are read from the strategy itself rather than set separately.

## Strategy catalog

For the strategy to appear in the selection window, it has to be added in two places.

The first is the factory in `BuiltInStrategies`, which creates an instance by the type identifier:

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

The second is the template description in `LegacyStrategyTemplates`, from which the strategy name in the list and the set of parameters with their types and default values are taken:

```cs
new StrategyTemplateDefinition(MyTypeId, "My strategy", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "Length", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` returns the instrument, the portfolio and the volume — every strategy has them; the rest are its own parameters. The description fields go in this order: key, label, type, default value, whether it is required, minimum and maximum.

`StrategyTemplateKind` distinguishes only the kinds for which the shell has its own panel (`Sma`, `SmaIndex`, `StairsTrend`, `StairsCountertrend`, `Designer`); a strategy without its own panel is `Generic`.

The values the user sets on the **Strategy** panel are applied to the instance in `StrategyMaterializer`: parsing of new parameters is added there as well.

The strategy settings are saved and restored by the shell: you do not need to write separate saving code.

## Own panel

If the standard parameter panel is enough, nothing needs to be done — Shell shows it by itself. An own panel is needed when the strategy has something to show beyond its parameters: how to set it up and how to monitor it is described in the [Create strategy panel](create_strategy_panel.md) section.

## See also

[Create strategy panel](create_strategy_panel.md)

[Run strategies from Designer](run_strategies_from_designer.md)
