
# 接続設定

**Runner** は、[Designer から設定をエクスポートする](export_from_designer.md)ことに加えて、コンソールインターフェイスからプログラムを設定する機能を提供します。これを行うには、**setup** コマンドを指定してプログラムを実行する必要があります。

```cmd
stocksharp.studio.runner setup
```

メニューが表示されます。

![接続設定 1 (1)](../../images/runner_setup_1.png)

接続項目を選択すると、プログラムはコネクター設定モードに入ります。

![接続設定 2 (1)](../../images/runner_setup_2.png)

ここでは、以前に保存した接続を編集するか、新しい接続を作成できます。

![接続設定 3 (1)](../../images/runner_setup_3.png)

必要な新規接続の種類を選択すると、プログラムはその設定の編集メニューに移動します。

![接続設定 4](../../images/runner_setup_4.png)

[Binance](../api/connectors/crypto_exchanges/binance.md) では、主要な設定を入力する必要があります。

![接続設定 5](../../images/runner_setup_5.png)

![接続設定 6](../../images/runner_setup_6.png)

![接続設定 7](../../images/runner_setup_7.png)

入力したデータが正しいことを確認するには、**確認** を選択します。

![接続設定 8](../../images/runner_setup_8.png)

接続チェックが開始されます。

![接続設定 9](../../images/runner_setup_9.png)

成功した場合、メッセージが表示されます。

![接続設定 10](../../images/runner_setup_10.png)

すべての設定を入力して確認したら、**保存** を押す必要があります。

![接続設定 11](../../images/runner_setup_11.png)

Data フォルダーに **connector.json** ファイルが作成され（まだ作成されていない場合）、保存された設定が含まれます。

[Telegram](../telegram_services.md) との統合を設定するには、メニュー項目を選択します。

![接続設定 1 (2)](../../images/runner_telegram_1.png)

そして、都合のよい方法で認証します。

![接続設定 2 (2)](../../images/runner_telegram_2.png)

トークンで認証する場合は、[https://stocksharp.ru/profile/](https://stocksharp.ru/profile/) のトークンを入力します。

![プロファイル](../../images/profile.png)

成功した場合、プログラムは利用可能な Telegram 操作オプションを表示します。

![接続設定 3 (2)](../../images/runner_telegram_3.png)
