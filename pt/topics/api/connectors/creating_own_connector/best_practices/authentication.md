# Autenticação

O componente de autenticação desempenha um papel fundamental para garantir a interação segura com uma série de APIs de exchanges. Ele é responsável por armazenar chaves de API, gerar assinaturas para solicitações e outros aspectos de segurança.

## Funções Principais

1. Armazenamento de chaves de API (chave pública, chave secreta, passphrase).
2. Geração de assinaturas para solicitações de acordo com os requisitos de uma exchange específica.
3. Adição dos cabeçalhos de autenticação necessários às solicitações HTTP.

## Exemplo de Implementação

Abaixo está um exemplo da classe `Authenticator` para trabalhar com autenticação:

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

		// Criar algoritmo de hash com base na chave secreta
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// Liberar recursos do algoritmo de hash
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// Indicador de se o autenticador pode criar assinaturas
	public bool CanSign { get; }

	// Chave pública da API
	public SecureString Key { get; }

	// Chave secreta da API
	public SecureString Secret { get; }

	// Passphrase (se exigida pela bolsa)
	public SecureString Passphrase { get; }

	// Método para criar uma assinatura de solicitação
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// Gerar timestamp
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// Criar assinatura com base em timestamp, método, URL e parâmetros
		return _hasher
			.ComputeHash((timestamp + method.ToString().ToUpperInvariant() + url + parameters).UTF8())
			.Base64();
	}
}
```

## Recomendações

- Use `SecureString` para armazenar dados sensíveis, como chaves e passphrases.
- Implemente a interface `IDisposable` para o descarte adequado de recursos, especialmente se primitivas criptográficas forem usadas.
- Certifique-se de que os métodos de geração de assinatura correspondam à versão mais recente da documentação da API da exchange.

Com a implementação adequada do componente de autenticação, você garantirá uma interação segura com a exchange e simplificará o processo de autorização de solicitações em outras partes do conector.
