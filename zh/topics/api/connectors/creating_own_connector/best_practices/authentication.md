# 认证

认证组件在确保与多个交易所 API 安全交互方面起着关键作用。它负责存储 API 密钥、为请求生成签名以及其他安全相关的方面。

## 主要功能

1. 存储 API 密钥（公钥、私钥、密码短语）。
2. 根据特定交易所的要求为请求生成签名。
3. 向 HTTP 请求添加必要的身份验证头。

## 实现示例

下面是一个用于处理身份验证的 `Authenticator` 类示例：

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

## 推荐

- 使用 `SecureString` 存储敏感数据，例如密钥和密码短语。
- 实现 `IDisposable` 接口以正确处置资源，特别是当使用加密原语时。
- 确保签名生成方法对应交易所 API 文档的最新版本。

通过正确实施身份验证组件，您将确保与交易所的安全交互，并简化连接器其他部分请求授权的过程。