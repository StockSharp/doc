# Authentifizierung

Die Authentifizierungskomponente spielt eine Schlüsselrolle bei der Gewährleistung einer sicheren Interaktion mit einer Reihe von Börsen-APIs. Sie ist verantwortlich für die Speicherung von API-Schlüsseln, die Generierung von Signaturen für Anfragen und andere Sicherheitsaspekte.

## Hauptfunktionen

1. Speicherung von API-Schlüsseln (öffentlicher Schlüssel, geheimer Schlüssel, Kennphrase).
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

		// Hashalgorithmus auf Basis des geheimen Schlüssels erstellen
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// Ressourcen des Hashalgorithmus freigeben
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// Flag, ob der Authentifikator Signaturen erstellen kann
	public bool CanSign { get; }

	// Öffentlicher API-Schlüssel
	public SecureString Key { get; }

	// Geheimer API-Schlüssel
	public SecureString Secret { get; }

	// Kennphrase (falls von der Börse erforderlich)
	public SecureString Passphrase { get; }

	// Methode zum Erstellen einer Anfragesignatur
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// Zeitstempel erzeugen
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// Signatur auf Basis von Zeitstempel, Methode, URL und Parametern erstellen
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
