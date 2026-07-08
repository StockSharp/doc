
# 接続設定

**Runner** は、[Designer から設定をエクスポートする](export_from_designer.md)ことに加えて、コンソールインターフェイスからプログラムを設定する機能を提供します。これを行うには、**setup** コマンドを指定してプログラムを実行する必要があります。

```cmd
stocksharp.studio.runner setup
```

メニューが表示されます。

![runner_setup_1](../../images/runner_setup_1.png)

Connections 項目を選択すると、プログラムはコネクター設定モードに入ります。

![runner_setup_2](../../images/runner_setup_2.png)

ここでは、以前に保存した接続を編集するか、新しい接続を作成できます。

![runner_setup_3](../../images/runner_setup_3.png)

必要な新規接続の種類を選択すると、プログラムはその設定の編集メニューに移動します。

![runner_setup_4](../../images/runner_setup_4.png)

[Binance](../api/connectors/crypto_exchanges/binance.md) では、主要な設定を入力する必要があります。

![runner_setup_5](../../images/runner_setup_5.png)

![runner_setup_6](../../images/runner_setup_6.png)

![runner_setup_7](../../images/runner_setup_7.png)

入力したデータが正しいことを確認するには、**Check** を選択します。

![runner_setup_8](../../images/runner_setup_8.png)

接続チェックが開始されます。

![runner_setup_9](../../images/runner_setup_9.png)

成功した場合、メッセージが表示されます。

![runner_setup_10](../../images/runner_setup_10.png)

すべての設定を入力して確認したら、**Save** を押す必要があります。

![runner_setup_11](../../images/runner_setup_11.png)

Data フォルダーに **connector.json** ファイルが作成され（まだ作成されていない場合）、保存された設定が含まれます。

[Telegram](../telegram_services.md) との統合を設定するには、メニュー項目を選択します。

![runner_telegram_1](../../images/runner_telegram_1.png)

そして、都合のよい方法で認証します。

![runner_telegram_2](../../images/runner_telegram_2.png)

トークンで認証する場合は、[https://stocksharp.ru/profile/](https://stocksharp.ru/profile/) のトークンを入力します。

![Profile](../../images/profile.png)

成功した場合、プログラムは利用可能な Telegram 操作オプションを表示します。

![runner_telegram_3](../../images/runner_telegram_3.png)
