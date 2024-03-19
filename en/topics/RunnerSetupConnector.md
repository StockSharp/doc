
# Connection Setup

**Runner**, in addition to [exporting settings through Designer](RunnerDesignerExport.md), offers the ability to configure the program through its console interface. To do this, you need to run the program with the **setup** command:

```cmd
stocksharp.studio.runner setup
```

A menu will appear:

![runner_setup_1](../images/runner_setup_1.png)

By selecting the Connections item, the program will enter the connector setup mode:

![runner_setup_2](../images/runner_setup_2.png)

Here you can either edit a previously saved connection or create a new one:

![runner_setup_3](../images/runner_setup_3.png)

After selecting the required type of new connection, the program will move to the editing menu of its settings:

![runner_setup_4](../images/runner_setup_4.png)

For [Binance](Binance.md), you need to enter its main settings:

![runner_setup_5](../images/runner_setup_5.png)

![runner_setup_6](../images/runner_setup_6.png)

![runner_setup_7](../images/runner_setup_7.png)

To verify the correctness of the entered data, select **Check**:

![runner_setup_8](../images/runner_setup_8.png)

The connection check will start:

![runner_setup_9](../images/runner_setup_9.png)

In case of success, a message will be displayed:

![runner_setup_10](../images/runner_setup_10.png)

After entering all settings and verifying them, you must press **Save**:

![runner_setup_11](../images/runner_setup_11.png)

In the Data folder, a **connector.json** file will be created (if it was not created earlier), which will contain the saved settings.

To set up integration with [Telegram](Telegram.md), select the menu item:

![runner_telegram_1](../images/runner_telegram_1.png)

And authenticate by a convenient method:

![runner_telegram_2](../images/runner_telegram_2.png)

For authentication by token, enter the token from [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

![Profile](../images/Profile.png)

In case of success, the program will display the available Telegram operation options:

![runner_telegram_3](../images/runner_telegram_3.png)
