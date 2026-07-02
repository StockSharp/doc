# Almacenamiento de configuraciones

Al crear su propio adaptador, asegúrese de proporcionar la capacidad de guardar y cargar configuraciones. Para ello, StockSharp utiliza los métodos `Save` y `Load`, que trabajan con el objeto `SettingsStorage`.

Para guardar las configuraciones, debe sobrescribir el método `Save`:

```cs
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(Key), Key);
	storage.SetValue(nameof(Secret), Secret);
	storage.SetValue(nameof(Passphrase), Passphrase);
}
```

Para cargar las configuraciones, debe sobrescribir el método `Load`:

```cs
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	Key = storage.GetValue<SecureString>(nameof(Key));
	Secret = storage.GetValue<SecureString>(nameof(Secret));
	Passphrase = storage.GetValue<SecureString>(nameof(Passphrase));
}
```

## Interfaces básicas

Un adaptador puede implementar varias interfaces básicas que permiten estandarizar algunas configuraciones. Aunque la implementación de estas interfaces no es obligatoria, puede simplificar el trabajo con el adaptador, ya que las propiedades de las interfaces básicas se muestran en el modo básico en la ventana de edición de conexión.

Ejemplos de interfaces básicas:

1. [IKeySecretAdapter](xref:StockSharp.Messages.IKeySecretAdapter) - para adaptadores que requieren una clave y un secreto para la autenticación.
2. [ILoginPasswordAdapter](xref:StockSharp.Messages.ILoginPasswordAdapter) - para adaptadores que utilizan un inicio de sesión y una contraseña.
3. [ITokenAdapter](xref:StockSharp.Messages.ITokenAdapter) - para adaptadores que utilizan un token de autenticación.
4. [IPassphraseAdapter](xref:StockSharp.Messages.IPassphraseAdapter) - para adaptadores que requieren una contraseña o frase de contraseña.
5. [IDemoAdapter](xref:StockSharp.Messages.IDemoAdapter) - para adaptadores que admiten el modo demo.
6. [IAddressAdapter<TAddress>](xref:StockSharp.Messages.IAddressAdapter`1) - para adaptadores que requieren especificar una dirección de servidor.
7. [ISenderTargetAdapter](xref:StockSharp.Messages.ISenderTargetAdapter) - para adaptadores que utilizan identificadores de remitente y destinatario.

Al implementar estas interfaces, debe agregar las propiedades correspondientes:

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

## Atributos para la configuración de la visualización

Para mejorar la experiencia del usuario al trabajar con las configuraciones del adaptador, se recomienda usar atributos del espacio de nombres `System.ComponentModel.DataAnnotations`. Estos atributos permiten establecer el nombre para mostrar, la descripción, el grupo y el orden de visualización de cada propiedad.

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

La correcta implementación del almacenamiento de configuraciones y el uso de interfaces básicas permite crear un adaptador más amigable y comprensible para el usuario, que se integra fácilmente en el ecosistema de StockSharp.
