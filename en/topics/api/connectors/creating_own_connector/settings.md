# Storing Settings

When creating your own adapter, it is important to provide the ability to save and load settings. For this, StockSharp uses the `Save` and `Load` methods, which work with the `SettingsStorage` object.

To save settings, you need to override the `Save` method:

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

To load settings, you need to override the `Load` method:

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## Basic Interfaces

An adapter can implement various basic interfaces that allow standardizing some settings. Although the implementation of these interfaces is not mandatory, it can simplify working with the adapter, as the properties of the basic interfaces are displayed in the basic mode in the connection editing window.

Examples of basic interfaces:

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - for adapters that require a key and secret for authentication.
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - for adapters that use a login and password.
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - for adapters that use an authentication token.
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - for adapters that require a password or passphrase.
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - for adapters that support demo mode.
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - for adapters that require specifying a server address.
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - for adapters that use sender and recipient identifiers.

When implementing these interfaces, you need to add the corresponding properties:

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

## Attributes for Display Configuration

To improve the user experience when working with adapter settings, it is recommended to use attributes from the `System.ComponentModel.DataAnnotations` namespace. These attributes allow you to set the display name, description, group, and display order for each property.

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

Correct implementation of settings storage and the use of basic interfaces allows creating a more user-friendly and understandable adapter that is easily integrated into the StockSharp ecosystem.