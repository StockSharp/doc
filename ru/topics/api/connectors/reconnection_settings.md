# Настройки переподключения

Все коннекторы предоставляют возможность настраивать переподключение на случай разрыва соединения. В графическом элементе [Окно настройки подключений](../graphical_user_interface/connection_settings_window.md) это выглядит следующим образом: 

![API GUI ReconnectionSettings](../../../images/api_gui_reconnectionsettings.png)

**Свойства переподключения**

- **Интервал** \- Интервал, с которым будут происходить попытки подключения. 
- **Первоначально** \- Количество попыток установить первоначальное соединение, если оно не было установлено (тайм\-аут, сетевой сбой и тд). 
- **Переподключение** \- Количество попыток переподключиться, если подключение было разорвано в процессе работы. 
- **Время ожидания** \- Время ожидания успешного подключения\/отключения. 
- **Режим работы** \- Режим работы, во время которого необходимо производить подключения. 

## Программная настройка переподключения

Механизм переподключения настраивается через свойство [ReConnectionSettings](xref:StockSharp.Messages.IMessageAdapter.ReConnectionSettings) и позволяет отслеживать следующие сценарии ошибок: 

- Невозможно установить соединение (отсутствует связь, неправильный логин\-пароль и т.д.). Через свойство [ReConnectionSettings.AttemptCount](xref:StockSharp.Messages.ReConnectionSettings.AttemptCount) задается количество попыток для установки соединения. По умолчанию оно равно 0, что означает, что режим отключен. \-1 означает бесконечное количество попыток. 
- Соединение было разорвано в процессе работы. Через свойство [ReConnectionSettings.ReAttemptCount](xref:StockSharp.Messages.ReConnectionSettings.ReAttemptCount) задается количество попыток для переустановки соединения. По умолчанию оно равно 100. \-1 означает бесконечное количество попыток. 0 \- режим отключен. 
- В процессе установки или отключения соединения соответствующие события [IConnector.Connected](xref:StockSharp.BusinessEntities.IConnector.Connected) или [IConnector.Disconnected](xref:StockSharp.BusinessEntities.IConnector.Disconnected) могут не приходить долгое время. Для таких ситуаций можно использовать свойство [ReConnectionSettings.TimeOutInterval](xref:StockSharp.Messages.ReConnectionSettings.TimeOutInterval), чтобы задать максимально допустимое время отсутствия успешного события. Если по истечению данного времени желаемое событие не возникает, то вызывается событие [IConnector.ConnectionError](xref:StockSharp.BusinessEntities.IConnector.ConnectionError) с ошибкой окончания ожидания. 

1. При создании шлюза необходимо проинициализировать настройки механизма переподключений через свойство [ReConnectionSettings](xref:StockSharp.Messages.IMessageAdapter.ReConnectionSettings): 

   ```cs
   // инициализируем механизм переподключения (будет автоматически соединяться
   // каждые 10 секунд, если шлюз потеряется связь с сервером)
   Connector.Adapter.ReConnectionSettings.Interval = TimeSpan.FromSeconds(10);
   // переподключение будет работать только во время работы биржи РТС
   // (чтобы отключить переподключение когда торгов нет штатно, например, ночью)
   Connector.Adapter.ReConnectionSettings.WorkingTime = Exchange.Rts.WorkingTime;
   ```
2. Чтобы проверить, как работает механизм контроля соединения, можно выключить подключение к Интернету: 

   ![transactions](../../../images/transactions.png)
3. Ниже приведен лог программы, в котором видно, что приложение изначально находится в подключенном состоянии, а после выключения интернета приложение пытается переподключиться. После восстановления интернета восстанавливается подключение приложения: 

   ![API ReconnectionLog](../../../images/api_reconnectionlog.png)
4. Так как в [Connector](xref:StockSharp.Algo.Connector) может использоваться несколько подключений, то по умолчанию события связанные с переподключением такие как [Restored](xref:StockSharp.Algo.Connector.ConnectionRestored) не вызываются, а адаптеры подключений самостоятельно пытаются переподключиться. Для того чтобы событие начало вызываться необходимо у адаптера выставить свойство [BasketMessageAdapter.SuppressReconnectingErrors](xref:StockSharp.Algo.BasketMessageAdapter.SuppressReconnectingErrors) в **false**. 

   ```cs
   Connector.Adapter.SuppressReconnectingErrors = false;
   Connector.ConnectionError += error => this.Sync(() => MessageBox.Show(this, "Соединение потеряно"));
   Connector.ConnectionRestored += adapter => this.Sync(() => MessageBox.Show(this, "Соединение восстановлено"));
   ```

   ![sampleconnectionerror](../../../images/sample_connection_error.png)![sampleconnectionrestore](../../../images/sample_connection_restored.png)
