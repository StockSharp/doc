# 認証

認証コンポーネントは、多くの取引所 API との安全な相互作用を確保するうえで重要な役割を果たします。API キーの保存、リクエスト用署名の生成、およびその他のセキュリティ面を担います。

## 主な機能

1. API キー（公開キー、秘密キー、パスフレーズ）の保存。
2. 特定の取引所の要件に従ったリクエスト用署名の生成。
3. HTTP リクエストへの必要な認証ヘッダーの追加。

## 実装例

以下は、認証を扱う `Authenticator` クラスの例です。

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

		// 秘密キーに基づいてハッシュアルゴリズムを作成
		_hasher = secret.IsEmpty() ? null : new HMACSHA256(secret.UnSecure().Base64());
	}

	protected override void DisposeManaged()
	{
		// ハッシュアルゴリズムのリソースを破棄
		_hasher?.Dispose();
		base.DisposeManaged();
	}

	// 認証器が署名を作成できるかどうかを示すフラグ
	public bool CanSign { get; }
	
	// API 公開キー
	public SecureString Key { get; }
	
	// API 秘密キー
	public SecureString Secret { get; }
	
	// パスフレーズ（取引所で必要な場合）
	public SecureString Passphrase { get; }

	// リクエスト署名を作成するメソッド
	public string MakeSign(string url, Method method, string parameters, out string timestamp)
	{
		// タイムスタンプを生成
		timestamp = DateTime.UtcNow.ToUnix().ToString("F0");

		// タイムスタンプ、メソッド、URL、パラメーターに基づいて署名を作成
		return _hasher
			.ComputeHash((timestamp + method.ToString().ToUpperInvariant() + url + parameters).UTF8())
			.Base64();
	}
}
```

## 推奨事項

- キーやパスフレーズなどの機密データの保存には `SecureString` を使用してください。
- 特に暗号プリミティブを使用する場合は、リソースを適切に破棄するために `IDisposable` インターフェイスを実装してください。
- 署名生成メソッドが、取引所 API ドキュメントの最新バージョンに対応していることを確認してください。

認証コンポーネントを適切に実装することで、取引所との安全な相互作用を確保し、コネクターの他の部分でリクエストを認可するプロセスを簡素化できます。
