# 存储设置

创建自己的适配器时，必须支持设置的保存和加载。为此，StockSharp 使用 `Save` 和 `Load` 方法，并通过 `SettingsStorage` 对象存取设置值。

要保存设置，需要重写 `Save` 方法：

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

要加载设置，需要重写 `Load` 方法：

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

适配器可以实现不同的基础接口，用于标准化常见设置。实现这些接口不是强制要求，但可以简化适配器使用，因为基础接口中的属性会显示在连接编辑窗口的基本模式中。

基本接口示例：

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - 用于需要 API key 和 secret 进行身份验证的适配器。
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - 用于使用登录名和密码的适配器。
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - 用于使用身份验证令牌的适配器。
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - 用于需要密码短语的适配器。
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - 用于支持演示模式的适配器。
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - 用于需要指定服务器地址的适配器。
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - 用于使用发送方和接收方标识符的适配器。

实现这些接口时，需要添加相应的属性：

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
/// 密码短语。
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

为了改善适配器设置的用户体验，建议使用 `System.ComponentModel.DataAnnotations` 命名空间中的特性。这些特性可为每个属性设置显示名称、说明、分组和显示顺序。

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

正确实现设置存储并使用基础接口，可以让适配器更易用、更容易理解，也更容易集成到 StockSharp 生态系统中。
