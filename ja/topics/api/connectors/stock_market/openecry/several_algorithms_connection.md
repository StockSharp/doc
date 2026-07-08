# 複数アルゴリズムの接続

特定のユーザーまたはアプリケーションによっては、OEC サーバーが複数アプリケーションの同時接続をサポートしていない場合があります。この場合、他の接続が中断されることがあります。これらの制限を回避するため、この [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) 実装は、OEC サーバーへの単一接続を通じた複数アプリケーションの同時動作をサポートしています - [OECRemoting](https://gainfutures.com/gainfuturesapi)。

次の [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) モードがサポートされています。

- [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) - [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) は切断されます。アプリケーションは OEC サーバーへの独自の接続を作成します。アプリケーションは他のアプリケーションの [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) として機能できません。
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) - アプリケーションは OEC サーバーへの独自の接続を作成します。
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) - 初期化時に [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) モードで実行されているローカルアプリケーションを検索します。そのようなアプリケーションが見つかった場合、それらの OEC サーバーへの接続を使用します。見つからない場合、アプリケーションは [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) モードに入ります。

[OECRemoting](https://gainfutures.com/gainfuturesapi) モードを明示的に設定するには、[OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) オブジェクトの作成直後に目的のモードを指定する必要があります。たとえば、[Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) モードを設定するには、次のようにします。

```cs
Trader.RemotingRequired = OECRemoting.Secondary;
		
```

既定では、[OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader) アダプターは [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None) モードで動作します。
