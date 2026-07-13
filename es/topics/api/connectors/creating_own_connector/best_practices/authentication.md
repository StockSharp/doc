# Autenticación

El componente de autenticación desempeña un papel clave para garantizar la interacción segura con una serie de API de bolsas. Es responsable de almacenar las claves de API, generar firmas para las solicitudes y otros aspectos de seguridad.

## Funciones Principales

1. Almacenamiento de claves de API (clave pública, clave secreta, frase de acceso).
2. Generación de firmas para solicitudes de acuerdo con los requisitos de una bolsa específica.
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

		// Crear algoritmo de hash basado en la clave secreta
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// Liberar recursos del algoritmo de hash
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// Indicador de si el autenticador puede crear firmas
	public bool CanSign { get; }

	// Clave pública de API
	public SecureString Key { get; }

	// Clave secreta de API
	public SecureString Secret { get; }

	// Frase de acceso (si la bolsa la requiere)
	public SecureString Passphrase { get; }

	// Método para crear una firma de solicitud
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// Generar timestamp
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// Crear firma basada en timestamp, método, URL y parámetros
		return _hasher
			.ComputeHash((timestamp + method.ToString().ToUpperInvariant() + url + parameters).UTF8())
			.Base64();
	}
}
```

## Recomendaciones

- Utilice `SecureString` para almacenar datos sensibles, como claves y frases de acceso.
- Implemente la interfaz `IDisposable` para una correcta liberación de recursos, especialmente si se utilizan primitivas criptográficas.
- Asegúrese de que los métodos de generación de firmas correspondan a la última versión de la documentación de la API de la bolsa.

Con una implementación adecuada del componente de autenticación, garantizará una interacción segura con la bolsa y simplificará el proceso de autorización de solicitudes en otras partes del conector.
</content>
