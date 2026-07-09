# Armazenamento de definições

Ao criar seu próprio adaptador, certifique-se de fornecer a capacidade de salvar e carregar configurações. Para isso, o StockSharp usa os métodos `Save` e `Load`, que trabalham com o objeto `SettingsStorage`.

Para salvar as configurações, você precisa sobrescrever o método `Save`:

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

Para carregar as configurações, você precisa sobrescrever o método `Load`:

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## Interfaces Básicas

Um adaptador pode implementar várias interfaces básicas que permitem padronizar algumas configurações. Embora a implementação dessas interfaces não seja obrigatória, ela pode simplificar o trabalho com o adaptador, já que as propriedades das interfaces básicas são exibidas no modo básico na janela de edição de conexão.

Exemplos de interfaces básicas:

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - para adaptadores que exigem uma chave e um segredo para autenticação.
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - para adaptadores que usam login e senha.
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - para adaptadores que usam um token de autenticação.
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - para adaptadores que exigem uma senha ou frase secreta.
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - para adaptadores que suportam modo demo.
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - para adaptadores que exigem a especificação de um endereço de servidor.
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - para adaptadores que usam identificadores de remetente e destinatário.

Ao implementar essas interfaces, você precisa adicionar as propriedades correspondentes:

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
/// Frase-passe.
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.PassphraseKey,
	Description = LocalizedStrings.PassphraseKey + LocalizedStrings.Dot,
	GroupName = LocalizedStrings.ConnectionKey,
	Order = 2)]
public SecureString Passphrase { get; set; }
```

## Atributos para Configuração de Exibição

Para melhorar a experiência do usuário ao trabalhar com as configurações do adaptador, recomenda-se usar atributos do namespace `System.ComponentModel.DataAnnotations`. Esses atributos permitem definir o nome de exibição, a descrição, o grupo e a ordem de exibição para cada propriedade.

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

A implementação correta do armazenamento de configurações e o uso de interfaces básicas permitem criar um adaptador mais amigável e compreensível, que se integra facilmente ao ecossistema StockSharp.
