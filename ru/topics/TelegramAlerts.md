# Алерты

Сервис отправки из программ (например, [Designer](Designer.md) или собственная программа) сообщений в закрытые и публичные каналы или группы в месседжере Telegram.

Для настройки:

1. Пройдите процесс [авторизации у бота](TelegramAuth.md).

2. Создайте канал или группу (закрытую, или открытую).

   ![TelegramChannelCreating.png](../images/TelegramChannelCreating.png)
   ![TelegramChannelType.png](../images/TelegramChannelType.png)

3. Добавьте бота [StockSharpBot](https://t.me/StockSharpBot)

   ![TelegramAddBot.png](../images/TelegramAddBot.png)

4. Сделайте его администратором

   ![TelegramMakeAdmin.png](../images/TelegramMakeAdmin.png)

5. Необходимые разрешения для корректной работы

   ![TelegramBotPermissions.png](../images/TelegramBotPermissions.png)

6. Напишите в канал или группу специальное слово **activate**

   ![TelegramChannelActivate.png](../images/TelegramChannelActivate.png)

7. В случае успеха вы получите ответ

   ![TelegramChannelActivated.png](../images/TelegramChannelActivated.png)

Созданный канал доступен для ваших стратегий и торговых роботов:

- В случае использования [Designer](Designer.md) нажмите на список каналов в панели вверху:

  ![DesignerRibbonChannels.png](../images/DesignerRibbonChannels.png)

  В появившемся окне вы увидите списки всех каналов и групп, где вы активировали бота:

  ![TelegramListChannels.png](../images/TelegramListChannels.png)

  Нажав на кнопку с иконкой Телеграм будет отправлено тестовое сообщение. В случае его получения, будет означать, что все сделано верно.

  ![DesignerTestMessage.png](../images/DesignerTestMessage.png)

  *В бесплатном тарифе добавляется строчка, упоминающая сайт StockSharp. В случае платных тарифов эта строчка убирается.*

  Если у вас несколько каналов вывода в Телеграм, и вы хотите разные стратегии выводить в разные каналы, вы можете указать конкретные каналы для каждой стратегии в свойствах:

  ![DesignerRemoteSettings.png](../images/DesignerRemoteSettings.png)

- В других программах настройки сделаны аналогично [Designer](Designer.md). К примеру в программе [Hydra](Hydra.md) вы можете настроить логирование ошибок закачки маркет данных, если [Hydra](Hydra.md) расположена на сервере, и вам необходимо оперативно получать информацию о нерабочем подключении.
- В случае [Shell](Shell.md) или [S\#](StockSharpAbout.md) вы можете посмотреть код, интегрирующий ваши стратегии с сервисом Телеграм.