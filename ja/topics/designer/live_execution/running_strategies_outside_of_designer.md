# Designer の外部でストラテジーを実行する

[Designer](../../designer.md) で作成したストラテジーは、[API](../../api.md) を基に構築した独自のプログラム内でも、[Runner](../../runner.md) または [Shell](../../shell.md) アプリケーション内でも実行できます。

- [Runner](../../runner.md) は [Designer](../../designer.md) より高速でメモリ消費も少ないため、ストラテジーのパフォーマンスが向上します。この方法は、サーバー上でストラテジーを実行する場合に最適です。詳細は [Designer からのエクスポート](../../runner/export_from_designer.md) を参照してください。

- [Shell](../../shell.md) はソースコードとして提供されます。この方法は、ストラテジー専用に作成された独自のインターフェイスとともにストラテジーを配布する場合に最適です。詳細は [Designer で作成したストラテジーの実行](../../shell/run_strategies_from_designer.md) を参照してください。
