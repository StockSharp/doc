# 连接设置

除[通过 Designer 导出设置](export_from_designer.md)外，**Runner** 还支持通过控制台界面配置程序。为此，请使用 **setup** 命令启动程序：

```cmd
stocksharp.studio.runner setup
```

程序将显示菜单：

![连接设置 1 (1)](../../images/runner_setup_1.png)

选择连接后，程序将进入连接器设置模式：

![连接设置 2 (1)](../../images/runner_setup_2.png)

可以编辑已保存的连接，也可以创建新连接：

![连接设置 3 (1)](../../images/runner_setup_3.png)

选择所需的新连接类型后，程序将打开其设置编辑菜单：

![连接设置 4](../../images/runner_setup_4.png)

对于 [Binance](../api/connectors/crypto_exchanges/binance.md)，需要输入主要设置：

![连接设置 5](../../images/runner_setup_5.png)

![连接设置 6](../../images/runner_setup_6.png)

![连接设置 7](../../images/runner_setup_7.png)

要验证输入数据是否正确，请选择 **检查**：

![连接设置 8](../../images/runner_setup_8.png)

程序将开始检查连接：

![连接设置 9](../../images/runner_setup_9.png)

检查成功后会显示相应消息：

![连接设置 10](../../images/runner_setup_10.png)

输入并验证所有设置后，必须单击 **保存**：

![连接设置 11](../../images/runner_setup_11.png)

程序将在 Data 文件夹中创建 **connector.json** 文件（如果该文件尚不存在），并将设置保存到其中。

要设置与 [Telegram](../telegram_services.md) 的集成，请选择对应菜单项：

![连接设置 1 (2)](../../images/runner_telegram_1.png)

然后选择方便的方式完成身份验证：

![连接设置 2 (2)](../../images/runner_telegram_2.png)

使用令牌进行身份验证时，请输入从 [https://stocksharp.com/zh/profile/](https://stocksharp.com/zh/profile/) 获取的令牌：

![个人资料](../../images/profile.png)

验证成功后，程序将显示可用的 Telegram 操作选项：

![连接设置 3 (2)](../../images/runner_telegram_3.png)
