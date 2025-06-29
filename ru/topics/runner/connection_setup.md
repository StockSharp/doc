# Настройка подключения

**Runner** помимо [экспорта настроек через Дизайнер](export_from_designer.md) предоставляет возможность настроить программу через ее консольный интерфейс. Для этого необходимо запустить программу с командой **setup**:

```cmd
stocksharp.studio.runner setup
```

Появится меню:

![runner_setup_1](../../images/runner_setup_1.png)

Выбрав пункт Подключения, программа перейдет в режим настройки коннектора:

![runner_setup_2](../../images/runner_setup_2.png)

Здесь можно как отредактировать уже ранее сохраненное подключение, так и создать новое:

![runner_setup_3](../../images/runner_setup_3.png)

Выбрав нужный тип нового подключения, программа переместит в меню редактирования его настроек:

![runner_setup_4](../../images/runner_setup_4.png)

Для [Binance](../api/connectors/crypto_exchanges/binance.md) необходимо задать его основные настройки:

![runner_setup_5](../../images/runner_setup_5.png)

![runner_setup_6](../../images/runner_setup_6.png)

![runner_setup_7](../../images/runner_setup_7.png)

Для правильности введенных данных выберите **Проверить**:

![runner_setup_8](../../images/runner_setup_8.png)

Начнется проверка подключения:

![runner_setup_9](../../images/runner_setup_9.png)

В случае успеха будет показано сообщение:

![runner_setup_10](../../images/runner_setup_10.png)

После ввода всех настроек и их проверки необходимо нажать **Сохранить**:

![runner_setup_11](../../images/runner_setup_11.png)

В папке Data будет создан файл **connector.json** (если он не был создан ранее), который будет содержать сохраненные настройки.

Для настройки интеграции с [Telegram](../telegram_services.md) выберите пункт меню:

![runner_telegram_1](../../images/runner_telegram_1.png)

И авторизуйтесь по удобному способу:

![runner_telegram_2](../../images/runner_telegram_2.png)

Для авторизации по токену введите токен из [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

![Profile](../../images/profile.png)

В случае успеха программа выведет на экран доступные варианты работы с Телеграм:

![runner_telegram_3](../../images/runner_telegram_3.png)