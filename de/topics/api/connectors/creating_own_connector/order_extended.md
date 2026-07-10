# Erweiterte Auftragsbedingungen

Bei der Arbeit mit einigen Börsen oder Handelssystemen können die Standardfelder für die Registrierung eines Auftrags nicht ausreichen. Zum Beispiel, wenn Folgendes erforderlich ist:

1. Bei der Registrierung von [Stop-Aufträgen](../../orders_management/create_new_stop_order.md).
2. Wenn Sie zusätzliche Eigenschaften angeben müssen, um benutzerdefinierte Auftragsregeln festzulegen.

StockSharp bietet ein flexibles System für die Arbeit mit solchen erweiterten Auftragsbedingungen.

## Basisklasse OrderCondition

[OrderCondition](xref:StockSharp.Messages.OrderCondition) ist eine abstrakte Basisklasse für alle Auftragsbedingungen. Sie stellt die grundlegende Funktionalität bereit:

- Wörterbuch `Parameters` zum Speichern zusätzlicher Auftragsparameter
- Methode `Clone()` zum Erstellen einer Kopie der Bedingung
- Überschriebene Methode `ToString()` für eine bequeme Ausgabe von Informationen über die Bedingung

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

## Spezialisierte Schnittstellen

StockSharp definiert mehrere Schnittstellen für bestimmte Arten von Auftragsbedingungen:

- [ITakeProfitOrderCondition](xref:StockSharp.Messages.ITakeProfitOrderCondition) - für Aufträge mit Take-Profit-Bedingung
- [IStopLossOrderCondition](xref:StockSharp.Messages.IStopLossOrderCondition) - für Aufträge mit Stop-Loss-Bedingung
- [IWithdrawOrderCondition](xref:StockSharp.Messages.IWithdrawOrderCondition) - für Auszahlungsaufträge
- [IRepoOrderCondition](xref:StockSharp.Messages.IRepoOrderCondition) - für REPO-Aufträge
- [INtmOrderCondition](xref:StockSharp.Messages.INtmOrderCondition) - für Aufträge im Modus ausgehandelter Geschäfte (NDM)

## BaseWithdrawOrderCondition

[BaseWithdrawOrderCondition](xref:StockSharp.Messages.BaseWithdrawOrderCondition) ist eine Basisklasse für Auftragsbedingungen, die die Auszahlung von Geldern unterstützen. Sie implementiert die Schnittstelle `IWithdrawOrderCondition` und enthält Felder für die Auszahlungstransaktion.

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

Die Klasse `CoinbaseOrderCondition` erbt von `BaseWithdrawOrderCondition`, da Coinbase die programmatische Auszahlung von Assets unterstützt. Darüber hinaus implementiert sie die Schnittstelle `IStopLossOrderCondition`, wodurch sie für Stop-Loss-Aufträge verwendet werden kann.

```cs
[Serializable]
[DataContract]
[Display(ResourceType = typeof(LocalizedStrings), Name = LocalizedStrings.CoinbaseKey)]
public class CoinbaseOrderCondition : BaseWithdrawOrderCondition, IStopLossOrderCondition
{
	/// <summary>
	/// Initialisiert eine neue Instanz von <see cref="CoinbaseOrderCondition"/>.
	/// </summary>
	public CoinbaseOrderCondition()
	{
	}

	/// <summary>
	/// Aktivierungspreis, bei dessen Erreichen eine Order platziert wird.
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

## Verwendung im Adapter

Bei der Entwicklung eines eigenen Adapters können Sie eine eigene Klasse von Auftragsbedingungen erstellen, indem Sie sie von `OrderCondition` oder einem ihrer Nachkommen ableiten und die erforderlichen Schnittstellen implementieren. Dies ermöglicht es Ihnen, Unterstützung für Parameter hinzuzufügen, die für Ihre Börse spezifisch sind.

Um den vom Adapter unterstützten Typ der Auftragsbedingung anzugeben, wird das Attribut [OrderConditionAttribute](xref:StockSharp.Messages.OrderConditionAttribute) verwendet.

```cs
[OrderCondition(typeof(CoinbaseOrderCondition))]
public partial class CoinbaseMessageAdapter
```

Ein solcher Ansatz bietet Flexibilität bei der Arbeit mit verschiedenen Börsen und deren einzigartigen Anforderungen an Auftragsparameter, während gleichzeitig die Einheitlichkeit innerhalb der StockSharp-Architektur gewahrt bleibt.
