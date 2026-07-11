# RemoteManager

Вкладка **RemoteManager** позволяет включать режим внешнего управления. Для включения этого режима необходимо зайти в меню настройки пользователей.

![Shell удаленный менеджер 00](../../../images/shell_remotemanager_00.png)

В появившемся окне задать **логин** и **пароль**.

![Shell удаленный менеджер 01](../../../images/shell_remotemanager_01.png)

После чего необходимо включить **серверный режим**. 

![Shell удаленный менеджер 02](../../../images/shell_remotemanager_02.png)

Теперь к Shell можно подключиться из другого Shell. 

Для этого необходимо запустить **другой** Shell. В нем перейти в настройки подключений.

![Shell удаленный менеджер 03](../../../images/shell_remotemanager_03.png)

В открывшемся окне настроить FIX‐подключение.

![Shell удаленный менеджер 04](../../../images/shell_remotemanager_04.png)

После чего нажать кнопку подключения.

![Shell удаленный менеджер 05](../../../images/shell_remotemanager_05.png)

При подключении все имевшиеся стратегии в Shell‐сервере будут доступны в Shell‐клиенте.

![Shell удаленный менеджер 06](../../../images/shell_remotemanager_06.png)

При нажатии на кнопку **Добавить** можно добавить еще одну стратегию в торговлю.

![Shell удаленный менеджер 07](../../../images/shell_remotemanager_07.png)

Так как Shell клиент поддерживает несколько серверов, при выборе добавления стратегии необходимо слева выбрать сервер, а справа будут все доступные стратегии на сервере.

![Shell удаленный менеджер 08](../../../images/shell_remotemanager_08.png)

После добавления стратегии она появится в списке стратегий.

![Shell удаленный менеджер 09](../../../images/shell_remotemanager_09.png)

При выборе стратегии справа будут вкладки с настройками стратегии, а также с ее статистикой.

После изменения настроек стратегии обязательно нажать кнопку **Применить изменения**, иначе изменения не применятся к стратегии.

![Shell удаленный менеджер 10](../../../images/shell_remotemanager_10.png)

Если в стратегии есть команда отличная от Start/Stop, то для ее применения ее необходимо задать в следующем поле.

![Shell удаленный менеджер 11](../../../images/shell_remotemanager_11.png)

И нажать кнопку отправки команды.

Для задания своей команды в стратегии необходимо переопределить метод [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))**(**[StockSharp.Messages.CommandMessage](xref:StockSharp.Messages.CommandMessage) cmdMsg **)**

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)
		
```

Базовый класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) только управляет запуском и остановкой стратегии.

## См. также

[Настройки подключения](../connections_settings.md)
