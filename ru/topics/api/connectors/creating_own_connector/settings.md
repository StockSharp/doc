# Хранение настроек

При создании собственного адаптера важно обеспечить возможность сохранения и загрузки настроек. Для этого в StockSharp используются методы `Save` и `Load`, которые работают с объектом `SettingsStorage`.

Для сохранения настроек необходимо переопределить метод `Save`:

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

Для загрузки настроек необходимо переопределить метод `Load`:

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## Базовые интерфейсы

Адаптер может реализовывать различные базовые интерфейсы, которые позволяют стандартизировать некоторые настройки. Хотя реализация этих интерфейсов не обязательна, она может упростить работу с адаптером, так как свойства базовых интерфейсов отображаются в базовом режиме в окне редактирования подключений.

Примеры базовых интерфейсов:

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - для адаптеров, требующих ключ и секрет для аутентификации.
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - для адаптеров, использующих логин и пароль.
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - для адаптеров, использующих токен аутентификации.
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - для адаптеров, требующих пароль или фразу-пароль.
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - для адаптеров, поддерживающих демо-режим.
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - для адаптеров, требующих указания адреса сервера.
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - для адаптеров, использующих идентификаторы отправителя и получателя.

При реализации этих интерфейсов необходимо добавить соответствующие свойства:

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

## Атрибуты для настройки отображения

Для улучшения пользовательского опыта при работе с настройками адаптера, рекомендуется использовать атрибуты из пространства имен `System.ComponentModel.DataAnnotations`. Эти атрибуты позволяют задать отображаемое имя, описание, группу и порядок отображения для каждого свойства.

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
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter, IKeySecretAdapter, IPassphraseAdapter
```

Корректная реализация хранения настроек и использование базовых интерфейсов позволяет создать более удобный и понятный для пользователя адаптер, который легко интегрируется в экосистему StockSharp.