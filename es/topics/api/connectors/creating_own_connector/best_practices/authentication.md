# Autenticación

El componente de autenticación desempeña un papel clave para garantizar la interacción segura con una serie de API de exchanges. Es responsable de almacenar las claves de API, generar firmas para las solicitudes y otros aspectos de seguridad.

## Funciones Principales

1. Almacenamiento de claves de API (clave pública, clave secreta, passphrase).
2. Generación de firmas para solicitudes de acuerdo con los requisitos de un exchange específico.
3. Adición de los encabezados de autenticación necesarios a las solicitudes HTTP.

## Ejemplo de Implementación

A continuación se muestra un ejemplo de la clase `Authenticator` para trabajar con la autenticación:

```cs
class Authenticator : Disposable
{
	private readonly HashAlgorithm _hasher;

	public Authenticator(bool canSign, SecureString key, SecureString secret, SecureString passphrase)
	{
		CanSign = canSign;
		Key = key;
		Secret = secret;
		Passphrase = passphrase;

		// Create a hashing algorithm based on the secret key
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// Dispose of the hashing algorithm resources
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// Flag indicating whether the authenticator can create signatures
	public bool CanSign { get; }

	// API public key
	public SecureString Key { get; }

	// API secret key
	public SecureString Secret { get; }

	// Passphrase (if required by the exchange)
	public SecureString Passphrase { get; }

	// Method for creating a request signature
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// Generate a timestamp
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// Create a signature based on the timestamp, method, URL, and parameters
		return _hasher
			.ComputeHash((timestamp + method.ToString().ToUpperInvariant() + url + parameters).UTF8())
			.Base64();
	}
}
```

## Recomendaciones

- Utilice `SecureString` para almacenar datos sensibles, como claves y passphrases.
- Implemente la interfaz `IDisposable` para una correcta liberación de recursos, especialmente si se utilizan primitivas criptográficas.
- Asegúrese de que los métodos de generación de firmas correspondan a la última versión de la documentación de la API del exchange.

Con una implementación adecuada del componente de autenticación, garantizará una interacción segura con el exchange y simplificará el proceso de autorización de solicitudes en otras partes del conector.
</content>
