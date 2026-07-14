# Webull コネクタの設定

[Webull OpenAPI](https://developer.webull.com/apis/docs/) でアプリケーションを作成し、認証情報を取得します。

- **キー** (`Key`) — Webull OpenAPI のアプリケーションキー。
- **シークレット** (`Secret`) — アプリケーションのシークレット。
- **アクセストークン** (`Token`) — 任意のアクセストークン。
- **口座** (`Account`) — 取引口座の識別子。省略すると、コネクタが口座一覧を自動的に取得します。
- **デモモード** (`IsDemo`) — Webull のテスト環境を使用します。

Webull のアプリケーションと口座で不要な場合、`Token` と `Account` は省略できます。
