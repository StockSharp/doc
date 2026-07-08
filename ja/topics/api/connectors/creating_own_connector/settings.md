# 設定の保存

独自のアダプターを作成する場合、設定を保存および読み込みできるようにしてください。このために、StockSharp は `SettingsStorage` オブジェクトを扱う `Save` メソッドと `Load` メソッドを使用します。

設定を保存するには、`Save` メソッドをオーバーライドする必要があります。

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

設定を読み込むには、`Load` メソッドをオーバーライドする必要があります。

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## 基本インターフェイス

アダプターは、いくつかの設定を標準化できる各種の基本インターフェイスを実装できます。これらのインターフェイスの実装は必須ではありませんが、基本インターフェイスのプロパティは接続編集ウィンドウの基本モードに表示されるため、アダプターの操作を簡単にできます。

基本インターフェイスの例:

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - 認証にキーとシークレットを必要とするアダプター用。
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - ログインとパスワードを使用するアダプター用。
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - 認証トークンを使用するアダプター用。
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - パスワードまたはパスフレーズを必要とするアダプター用。
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - デモモードをサポートするアダプター用。
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - サーバーアドレスの指定を必要とするアダプター用。
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - 送信者と受信者の識別子を使用するアダプター用。

これらのインターフェイスを実装する場合、対応するプロパティを追加する必要があります。

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
/// パスフレーズ。
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.PassphraseKey,
	Description = LocalizedStrings.PassphraseKey + LocalizedStrings.Dot,
	GroupName = LocalizedStrings.ConnectionKey,
	Order = 2)]
public SecureString Passphrase { get; set; }
```

## 表示設定用の属性

アダプター設定を操作するときのユーザー体験を向上させるには、`System.ComponentModel.DataAnnotations` 名前空間の属性を使用することをお勧めします。これらの属性を使用すると、各プロパティの表示名、説明、グループ、表示順序を設定できます。

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

設定ストレージを正しく実装し、基本インターフェイスを使用することで、StockSharp エコシステムに容易に統合できる、より使いやすく理解しやすいアダプターを作成できます。
