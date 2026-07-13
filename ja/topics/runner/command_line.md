
# コマンドライン

**Runner** はコンソールアプリケーションであり、コマンドラインでパラメーターを指定することで、さまざまなモードで起動できます。パラメーターなしでプログラムを起動すると、使用可能なパラメーターを示すヘルプメッセージが表示されます。

![コマンドライン 1](../../images/runner_command_line_1.png)

履歴データテスト用に **Runner** を起動する例:

```cmd
b -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec AAPL@NASDAQ -r json
```

使用可能なパラメーター:

- -s - ストラテジーファイルへのパス（拡張子は cs、json、または dll）。
- -t - （任意）dll ファイルが選択され、アセンブリに複数のストラテジークラスが含まれている場合、このパラメーターで必要な型を指定する必要があります。
- -h - 履歴データを含むディレクトリへのパス。[サーバーモード](../hydra_server.md)を使用する場合はネットワークアドレスにすることもできます。
- --hl - （任意）[サーバーモード](../hydra_server.md)で使用されるログイン。
- --hp - （任意）[サーバーモード](../hydra_server.md)で使用されるパスワード。
- --hf - YYYYMMDD 形式のテスト開始日。
- --ht - YYYYMMDD 形式のテスト終了日。
- -f - （任意）ストレージ形式（Binary または Csv）。
- --sec - （任意）[銘柄識別子](../api/instruments/instrument_identifier.md)。
- -r - （任意）テスト結果レポートの形式（json、xml、csv）。
- --tm - （任意）ストラテジーのタイムアウト。
- --memory - （任意）最大メモリサイズ（メガバイト単位）。
- --cpu - （任意）プロセッサーマスク。
- -l - （任意）ログレベル（Info、Debug、Error、Warning、Verbose）。

最適化用に **Runner** を起動する例:

```cmd
o -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec AAPL@NASDAQ -r json -p sma_optimization.json
```
履歴テストモードのすべてのパラメーターに加えて、次の追加パラメーターがあります。

- -p - パラメーターファイルへのパス。
- --ol - （任意）最大反復回数。
- --ob - （任意）同時にテストするストラテジー数。

パラメーターファイルの形式:

```json
[
	{
	"Name": "SMA_80",
	"Value": "200,201"
	},
	{
	"Name": "SMA_30",
	"From": "40",
	"To": "50",
	"Step": "1"
	},
	{
	"Name": "Security",
	"Value": "AAPL@NASDAQ,MSFT@NASDAQ"
	}
]
```

ライブ取引用に **Runner** を起動する例:

```cmd
l -s SmaStrategy.cs -c connector.json --tg telegram.json
```

- -c - 接続設定ファイル。
- --tg - Telegram 統合設定ファイル。
