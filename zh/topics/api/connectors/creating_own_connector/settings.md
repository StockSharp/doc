# 存储设置

在创建您自己的适配器时，提供保存和加载设置的能力非常重要。为此，StockSharp 使用 `Save` 和 `Load` 方法，这些方法与 `SettingsStorage` 对象一起使用。

要保存设置，您需要重写 `Save` 方法：

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

要加载设置，您需要重写 `Load` 方法：

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## 基本接口

适配器可以实现各种基本接口，以便标准化某些设置。虽然这些接口的实现不是强制性的，但它可以简化适配器的使用，因为基本接口的属性会在连接编辑窗口的基本模式中显示。

基本接口示例：

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - 用于需要密钥和秘密进行身份验证的适配器。
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - 用于使用登录名和密码的适配器。
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - 用于使用身份验证令牌的适配器。
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - 用于需要密码或口令的适配器。
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - 用于支持演示模式的适配器。
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - 用于需要指定服务器地址的适配器。
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - 用于使用发送方和接收方标识符的适配器。

在实现这些接口时，你需要添加相应的属性：

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

## 显示配置的属性

为了在使用适配器设置时改善用户体验，建议使用 `System.ComponentModel.DataAnnotations` 命名空间中的属性。这些属性允许您为每个属性设置显示名称、描述、分组和显示顺序。

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

正确实现设置存储和使用基本接口可以创建一个更用户友好且易于理解的适配器，并且可以轻松集成到 StockSharp 生态系统中。