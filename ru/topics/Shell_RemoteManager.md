# RemoteManager

Вкладка [RemoteManager](Shell_RemoteManager.md) позволяет включать режим внешнего управления. Для включения этого режима необходимо зайти в меню настройки пользователей.

![Shell RemoteManager 00](../images/Shell_RemoteManager_00.png)

В появившемся окне задать **логин** и **пароль**

![Shell RemoteManager 01](../images/Shell_RemoteManager_01.png)

После чего необходимо включить **серверный режим**. 

![Shell RemoteManager 02](../images/Shell_RemoteManager_02.png)

Теперь к Shell можно подключиться из другого Shell. 

Для этого необходимо запустить **другой** Shell. В нем перейти в настройки подключений.

![Shell RemoteManager 03](../images/Shell_RemoteManager_03.png)

В открывшемся окне настроить Fix подключение

![Shell RemoteManager 04](../images/Shell_RemoteManager_04.png)

После чего нажать кнопку подключения

![Shell RemoteManager 05](../images/Shell_RemoteManager_05.png)

При подключении все имевшиеся стратегии в Shell сервере будут доступны в Shell клиенте

![Shell RemoteManager 06](../images/Shell_RemoteManager_06.png)

По нажатию на кнопку **Добавить** можно добавить, еще стратегию в торговлю.

![Shell RemoteManager 07](../images/Shell_RemoteManager_07.png)

Так как Shell клиент поддерживает несколько серверов. То при выборе добавления стратегии необходимо слева выбрать сервер, а справа будут все доступные стратегии на сервере.

![Shell RemoteManager 08](../images/Shell_RemoteManager_08.png)

После добавления стратегии она появится в списке стратегий.

![Shell RemoteManager 09](../images/Shell_RemoteManager_09.png)

При выборе стратегии справа будут вкладки с настройками стратегии, а также с ее статистикой.

После изменения настроек стратегии обязательно нажать кнопку **Применить изменения**, иначе изменения не применятся к стратегии.

![Shell RemoteManager 10](../images/Shell_RemoteManager_10.png)

Если в стратегии есть команда отличная от Start\/Stop то для ее применения ее необходимо задать в следующем поле

![Shell RemoteManager 11](../images/Shell_RemoteManager_11.png)

И нажать кнопку отправки команды.

Для задания своей команды в стратегии необходимо переопределить метод [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)
		
```

Базовый класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) только управляет запуском и остановкой стратегии.

## См. также

[Настройки подключения](Shell_Connection_settings.md)
