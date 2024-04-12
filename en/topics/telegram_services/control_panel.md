# Control Panel

A service for managing trading strategies and robots via a Telegram bot.

For setup, go through the [bot authorization process](authorization.md) beforehand.

After that, the bot is ready for use. Next, for the bot to start seeing your strategies, you need to:

- In the case of using the [Designer](../designer.md) program, enable the Remote mode on the Cloud panel:

  ![DesignerRibbon.png](../../images/designerribbon.png)

  All strategies that are run in Live mode will automatically be transferred to the telegram bot, and you will be able to control them from your phone.

  In the bot [StockSharpBot](https://t.me/StockSharpBot) by selecting the /apps command you can see a list of all programs:

  ![TelegramControlApps.png](../../images/telegramcontrolapps.png)

  Once the desired program is selected, you can see the strategies and their controls:

  ![TelegramControlApp.png](../../images/telegramcontrolapp.png)

  ![TelegramControlStrategies.png](../../images/telegramcontrolstrategies.png)

  ![TelegramControlStrategy.png](../../images/telegramcontrolstrategy.png)

- In the case of using [Shell](../shell.md), you need to go to the Remote Manager panel and make settings similar to [Designer](../designer.md).
- In the case of using [Hydra](../hydra.md), all actions are done similarly to [Designer](../designer.md). Integration with [Hydra](../hydra.md) allows you to manage the downloading of market data, monitor quantitative statistics.

  ![TelegramHydra.png](../../images/telegramhydra.png)
  ![TelegramHydraStat.png](../../images/telegramhydrastat.png)

- In the case of using [S#](../api.md), you can make integration using the code from [Shell](../shell.md). Thanks to [S#](../api.md) being cross-platform, your robots can be run on any operating system.