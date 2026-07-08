# 身份认证

身份认证组件在确保与多个交易所 API 安全交互方面起着关键作用。它负责存储 API 密钥、为请求生成签名以及其他安全相关的方面。

## 主要功能

1. 存储 API 密钥（公钥、私钥、密码短语）。
2. 根据特定交易所的要求为请求生成签名。
3. 向 HTTP 请求添加必要的身份验证头。

## 实现示例

下面是一个用于处理身份认证的 `Authenticator` 类示例：

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

		// 基于密钥创建哈希算法
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// 释放哈希算法资源
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// 指示认证器是否可以创建签名的标志
	public bool CanSign { get; }
	
	// API 公钥
	public SecureString Key { get; }
	
	// API 密钥
	public SecureString Secret { get; }
	
	// Passphrase (if required by the exchange)
	public SecureString Passphrase { get; }

	// 创建请求签名的方法
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// 生成时间戳
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// 基于时间戳、方法、URL 和参数创建签名
		return _hasher
			.ComputeHash((timestamp + method.ToString().ToUpperInvariant() + url + parameters).UTF8())
			.Base64();
	}
}
```

## 建议

- 使用 `SecureString` 存储敏感数据，例如密钥和密码短语。
- 实现 `IDisposable` 接口以正确释放资源，尤其是在使用加密原语时。
- 确保签名生成方法与交易所 API 文档的最新版本保持一致。

正确实现身份认证组件后，你就能确保与交易所之间的安全交互，并简化连接器其他部分请求授权的流程。
