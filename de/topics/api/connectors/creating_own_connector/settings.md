# Speichern von Einstellungen

Achten Sie beim Erstellen eines eigenen Adapters darauf, die Möglichkeit zum Speichern und Laden von Einstellungen bereitzustellen. Dafür verwendet StockSharp die Methoden `Save` und `Load`, die mit dem Objekt `SettingsStorage` arbeiten.

Um Einstellungen zu speichern, müssen Sie die Methode `Save` überschreiben:

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

Um Einstellungen zu laden, müssen Sie die Methode `Load` überschreiben:

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## Basisschnittstellen

Ein Adapter kann verschiedene Basisschnittstellen implementieren, die es ermöglichen, einige Einstellungen zu standardisieren. Obwohl die Implementierung dieser Schnittstellen nicht zwingend erforderlich ist, kann sie die Arbeit mit dem Adapter vereinfachen, da die Eigenschaften der Basisschnittstellen im Basismodus im Fenster zur Bearbeitung der Verbindung angezeigt werden.

Beispiele für Basisschnittstellen:

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - für Adapter, die einen Schlüssel und ein Secret für die Authentifizierung benötigen.
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - für Adapter, die einen Login und ein Passwort verwenden.
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - für Adapter, die ein Authentifizierungstoken verwenden.
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - für Adapter, die ein Passwort oder eine Passphrase benötigen.
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - für Adapter, die den Demomodus unterstützen.
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - für Adapter, die die Angabe einer Serveradresse erfordern.
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - für Adapter, die Absender- und Empfängerkennungen verwenden.

Bei der Implementierung dieser Schnittstellen müssen Sie die entsprechenden Eigenschaften hinzufügen:

```cs
/// <inheritdoc />
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.KeyKey,
	Description = LocalizedStrings.KeyKey + LocalizedStrings.Dot,
	GroupName = LocalizedStrings.ConnectionKey,
	Order = 0)]
public SecureString Key { get; set; }

/// <inheritdoc />
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.SecretKey,
	Description = LocalizedStrings.SecretDescKey,
	GroupName = LocalizedStrings.ConnectionKey,
	Order = 1)]
public SecureString Secret { get; set; }

/// <summary>
/// Passphrase.
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.PassphraseKey,
	Description = LocalizedStrings.PassphraseKey + LocalizedStrings.Dot,
	GroupName = LocalizedStrings.ConnectionKey,
	Order = 2)]
public SecureString Passphrase { get; set; }
```

## Attribute zur Konfiguration der Anzeige

Um die Benutzererfahrung bei der Arbeit mit Adaptereinstellungen zu verbessern, wird empfohlen, Attribute aus dem Namensraum `System.ComponentModel.DataAnnotations` zu verwenden. Diese Attribute ermöglichen es, für jede Eigenschaft den Anzeigenamen, die Beschreibung, die Gruppe und die Anzeigereihenfolge festzulegen.

```cs
[MediaIcon("Coinbase_logo.svg")]
[Doc("topics/api/connectors/crypto_exchanges/coinbase.html")]
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.CoinbaseKey,
	Description = LocalizedStrings.CryptoConnectorKey,
	GroupName = LocalizedStrings.CryptocurrencyKey)]
[MessageAdapterCategory(MessageAdapterCategories.Crypto | MessageAdapterCategories.RealTime | MessageAdapterCategories.OrderLog |
	MessageAdapterCategories.Free | MessageAdapterCategories.Level1 | MessageAdapterCategories.Transactions)]
public partial class CoinbaseMessageAdapter : MessageAdapter, IKeySecretAdapter, IPassphraseAdapter
```

Die korrekte Implementierung der Einstellungsspeicherung und die Verwendung von Basisschnittstellen ermöglichen es, einen benutzerfreundlicheren und verständlicheren Adapter zu erstellen, der sich leicht in das StockSharp-Ökosystem integrieren lässt.
