# Tradier 的图形配置

对于所有 StockSharp 产品，图形连接设置在 [连接设置窗口](../../../graphical_user_interface/connection_settings_window.md) 界面形式中进行：

![API GUI 设置 Tradier](../../../../../images/api_gui_settings_tradier.png)

- **令牌** - 授权令牌。
- **演示** - 演示模式。

OAuth 授权：

1. 您可以直接将令牌插入“令牌”字段中。
2. 如果您将令牌字段留空并且未选择“演示”模式，将使用 OAuth 授权。

OAuth 授权过程：

1. 当你点击“检查”按钮时，会弹出一个窗口：

   ![OAuth 开始](../../../../../images/oauth_start.png)

2. 点击“开始”后，用户将被重定向到 Tradier 网站进行登录：

   ![Tradier 登录](../../../../../images/api_gui_settings_tradier_2.png)

3. 在 Tradier 网站上，您需要允许 StockSharp 应用程序访问交易操作：

   ![Tradier 权限](../../../../../images/api_gui_settings_tradier_3.png)

4. 之后，您将被重定向回 StockSharp 网站，程序将自动登录。

## 另请参阅

[连接器](../../../connectors.md)

[OAuth](../../oauth.md)

[图形配置](../../graphical_configuration.md)

[创建您自己的连接器](../../creating_own_connector.md)

[保存和加载设置](../../save_and_load_settings.md)
