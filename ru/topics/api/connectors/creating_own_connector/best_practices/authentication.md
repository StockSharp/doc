# Аутентификация

Компонент аутентификации играет ключевую роль в обеспечении безопасного взаимодействия с рядом API бирж. Он отвечает за хранение ключей API, генерацию подписей для запросов и другие аспекты безопасности.

## Основные функции

1. Хранение ключей API (публичный ключ, секретный ключ, пассфраза).
2. Генерация подписей для запросов согласно требованиям конкретной биржи.
3. Добавление необходимых заголовков аутентификации к HTTP-запросам.

## Пример реализации

Ниже приведен пример класса `Authenticator` для работы с аутентификацией:

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

		// Создаем алгоритм хеширования на основе секретного ключа
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// Освобождаем ресурсы алгоритма хеширования
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// Флаг, указывающий, может ли аутентификатор создавать подписи
	public bool CanSign { get; }
	
	// Публичный ключ API
	public SecureString Key { get; }
	
	// Секретный ключ API
	public SecureString Secret { get; }
	
	// Пассфраза (если требуется биржей)
	public SecureString Passphrase { get; }

	// Метод для создания подписи запроса
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// Генерируем временную метку
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// Создаем подпись на основе временной метки, метода, URL и параметров
		return _hasher
			.ComputeHash((timestamp + method.ToString().ToUpperInvariant() + url + parameters).UTF8())
			.Base64();
	}
}
```

## Рекомендации

- Используйте `SecureString` для хранения чувствительных данных, таких как ключи и пассфразы.
- Реализуйте интерфейс `IDisposable` для корректного освобождения ресурсов, особенно если используются криптографические примитивы.
- Убедитесь, что методы генерации подписей соответствуют последней версии документации API биржи.

При правильной реализации компонента аутентификации вы обеспечите безопасное взаимодействие с биржей и упростите процесс авторизации запросов в других частях коннектора.