# Authentifizierung

Die Authentifizierungskomponente spielt eine Schlüsselrolle bei der Gewährleistung einer sicheren Interaktion mit einer Reihe von Börsen-APIs. Sie ist verantwortlich für die Speicherung von API-Schlüsseln, die Generierung von Signaturen für Anfragen und andere Sicherheitsaspekte.

## Hauptfunktionen

1. Speicherung von API-Schlüsseln (öffentlicher Schlüssel, geheimer Schlüssel, Passphrase).
2. Generierung von Signaturen für Anfragen gemäß den Anforderungen einer bestimmten Börse.
3. Hinzufügen der erforderlichen Authentifizierungsheader zu HTTP-Anfragen.

## Implementierungsbeispiel

Nachfolgend finden Sie ein Beispiel für die Klasse `Authenticator` zur Arbeit mit Authentifizierung:

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

## Empfehlungen

- Verwenden Sie `SecureString` zum Speichern sensibler Daten wie Schlüssel und Passphrasen.
- Implementieren Sie die Schnittstelle `IDisposable`, um Ressourcen ordnungsgemäß freizugeben, insbesondere wenn kryptografische Primitiven verwendet werden.
- Stellen Sie sicher, dass die Methoden zur Signaturgenerierung der neuesten Version der API-Dokumentation der Börse entsprechen.

Bei ordnungsgemäßer Implementierung der Authentifizierungskomponente stellen Sie eine sichere Interaktion mit der Börse sicher und vereinfachen den Prozess der Autorisierung von Anfragen in anderen Teilen des Connectors.
