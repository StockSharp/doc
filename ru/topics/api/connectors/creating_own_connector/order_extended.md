# Расширенные условия заявок

При работе с некоторыми биржами или торговыми системами стандартных полей для регистрации заявки может быть недостаточно. Например, когда требуется:

1. При регистрации [стоп-заявок](../../orders_management/create_new_stop_order.md).
2. Когда требуется указать дополнительные свойства для задания кастомных правил заявки.

StockSharp предоставляет гибкую систему для работы с такими расширенными условиями заявок.

## Базовый класс OrderCondition

[OrderCondition](xref:StockSharp.Messages.OrderCondition) - это абстрактный базовый класс для всех условий заявок. Он предоставляет основную функциональность:

- Словарь `Parameters` для хранения дополнительных параметров заявки
- Метод `Clone()` для создания копии условия
- Переопределенный метод `ToString()` для удобного вывода информации об условии

```cs
public class MyOrderCondition : OrderCondition
{
    public decimal? SpecialPrice
    {
        get => (decimal?)Parameters[nameof(SpecialPrice)];
        set => Parameters[nameof(SpecialPrice)] = value;
    }
}
```

## Специализированные интерфейсы

StockSharp определяет несколько интерфейсов для специфических типов условий заявок:

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - для заявок с условием Take-Profit
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - для заявок с условием Stop-Loss
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - для заявок на вывод средств
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - для РЕПО заявок
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - для заявок режима переговорных сделок (РПС)

## BaseWithdrawOrderCondition

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) - это базовый класс для условий заявок, поддерживающих вывод средств. Он реализует интерфейс `IWithdrawOrderCondition` и содержит поля для транзакции по выводу средств.

```cs
public class MyWithdrawCondition : BaseWithdrawOrderCondition
{
    public string DestinationAddress
    {
        get => (string)Parameters[nameof(DestinationAddress)];
        set => Parameters[nameof(DestinationAddress)] = value;
    }
}
```

## CoinbaseOrderCondition

Класс `CoinbaseOrderCondition` наследуется от `BaseWithdrawOrderCondition`, так как Coinbase поддерживает программный вывод активов. Кроме того, он реализует интерфейс `IStopLossOrderCondition`, что позволяет использовать его для стоп-лосс заявок.

```cs
[Serializable]
[DataContract]
[Display(ResourceType = typeof(LocalizedStrings), Name = LocalizedStrings.CoinbaseKey)]
public class CoinbaseOrderCondition : BaseWithdrawOrderCondition, IStopLossOrderCondition
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CoinbaseOrderCondition"/>.
	/// </summary>
	public CoinbaseOrderCondition()
	{
	}

	/// <summary>
	/// Activation price, when reached an order will be placed.
	/// </summary>
	[DataMember]
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.StopPriceKey,
		Description = LocalizedStrings.StopPriceDescKey,
		GroupName = LocalizedStrings.StopLossKey,
		Order = 0)]
	public decimal? StopPrice
	{
		get => (decimal?)Parameters.TryGetValue(nameof(StopPrice));
		set => Parameters[nameof(StopPrice)] = value;
	}

	decimal? IStopLossOrderCondition.ClosePositionPrice { get; set; }

	decimal? IStopLossOrderCondition.ActivationPrice
	{
		get => StopPrice;
		set => StopPrice = value;
	}

	bool IStopLossOrderCondition.IsTrailing
	{
		get => false;
		set {  }
	}
}
```

## Использование в адаптере

При разработке собственного адаптера, вы можете создать свой класс условий заявок, наследуя его от `OrderCondition` или одного из его потомков, и реализуя необходимые интерфейсы. Это позволит добавить поддержку специфических для вашей биржи параметров заявок.

Для указания типа условия заявки, поддерживаемого адаптером, используется атрибут [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute).

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

Такой подход обеспечивает гибкость при работе с различными биржами и их уникальными требованиями к параметрам заявок, сохраняя при этом единообразие в рамках архитектуры StockSharp.