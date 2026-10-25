# Создание собственной стратегии

Стратегии, входящие в [Shell](../shell.md), лежат в `Shell.Avalonia/Shell.Core/Strategies/`. Собственная добавляется туда же и проходит три шага: класс стратегии, запись в каталоге и \- если нужно \- собственная панель.

## Класс стратегии

Стратегия \- это обычный наследник `Strategy` из StockSharp. Ничего специфичного для Shell в ней нет:

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "Моя стратегия";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// Группа берётся из ресурсов, чтобы параметр попал в ту же группу, что и стандартные.
			.SetDisplay("Длина", "Длина скользящей средней", LocalizedStrings.Parameters)
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

		// подписки на данные и торговая логика
	}
}
```

Точка входа \- `OnStarted2(DateTime)`, а не `OnStarted`: к моменту её вызова стратегия уже получила инструмент, портфель и подключение, поэтому подписываться на данные можно прямо здесь.

Параметр, объявленный через `Param(...)` с заданным `SetOptimize(...)`, автоматически появляется на вкладке [Оптимизация](user_interface/optimization.md): список перебираемых параметров и границы перебора читаются с самой стратегии, а не задаются отдельно.

## Каталог стратегий

Чтобы стратегия появилась в окне выбора, её нужно добавить в два места.

Первое \- фабрика в `BuiltInStrategies`, которая создаёт экземпляр по идентификатору типа:

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

Второе \- описание шаблона в `LegacyStrategyTemplates`, откуда берутся имя стратегии в списке и набор параметров с их типами и значениями по умолчанию:

```cs
new StrategyTemplateDefinition(MyTypeId, "Моя стратегия", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "Длина", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` возвращает инструмент, портфель и объём \- они есть у каждой стратегии; остальное \- её собственные параметры. Поля описания идут в порядке: ключ, надпись, тип, значение по умолчанию, обязательность, минимум и максимум.

`StrategyTemplateKind` различает только те виды, для которых у оболочки есть своя панель (`Sma`, `SmaIndex`, `StairsTrend`, `StairsCountertrend`, `Designer`); стратегия без собственной панели \- это `Generic`.

Значения, которые пользователь задаёт на панели **Стратегия**, применяются к экземпляру в `StrategyMaterializer`: там же добавляется разбор новых параметров.

Настройки стратегии сохраняются и восстанавливаются оболочкой: отдельный код сохранения писать не нужно.

## Собственная панель

Если стандартной панели параметров достаточно, делать ничего не нужно \- Shell покажет её сам. Собственная панель нужна, когда стратегии есть что показать сверх параметров: как её настраивать и как за ней наблюдать описано в разделе [Создание собственных панелей для стратегий](create_strategy_panel.md).

## См. также

[Создание собственных панелей для стратегий](create_strategy_panel.md)

[Запуск стратегии, созданной в Designer](run_strategies_from_designer.md)
