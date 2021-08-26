# Level1

![GUI Leve1Grid](~/images/GUI_Leve1Grid.png)

[Level1Grid](../api/StockSharp.Xaml.Level1Grid.html) \- таблица для отображения полей Level1. Эта таблица использует данные в виде сообщений [Level1ChangeMessage](../api/StockSharp.Messages.Level1ChangeMessage.html). 

**Основные свойства**

- [MaxCount](../api/StockSharp.Xaml.Level1Grid.MaxCount.html) \- максимальное число сообщений для показа.
- [Messages](../api/StockSharp.Xaml.Level1Grid.Messages.html) \- список сообщений, добавленных в таблицу.
- [SelectedMessage](../api/StockSharp.Xaml.Level1Grid.SelectedMessage.html) \- выбранное сообщение.
- [SelectedMessages](../api/StockSharp.Xaml.Level1Grid.SelectedMessages.html) \- выбранные сообщения.

Ниже показаны фрагменты кода с его использованием. 

```xaml
\<Window x:Class\="Membrane02.Level1Window"
        xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
        xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
        xmlns:d\="http:\/\/schemas.microsoft.com\/expression\/blend\/2008"
        xmlns:mc\="http:\/\/schemas.openxmlformats.org\/markup\-compatibility\/2006"
        xmlns:sx\="http:\/\/schemas.stocksharp.com\/xaml"
        xmlns:local\="clr\-namespace:Membrane02"
        mc:Ignorable\="d"
        Title\="Level1Window" Height\="300" Width\="300" Closing\="Window\_Closing"\>
    \<Grid\>
        \<sx:Level1Grid x:Name\="Level1Grid" \/\>
    \<\/Grid\>
\<\/Window\>
	  				
```
```cs
public Level1Window()
{
    InitializeComponent();
    \_connector \= MainWindow.This.Connector;
    \_connector.NewMessage +\= OnNewMessage;
    if (\!\_connector.RegisteredSecurities.Contains(MainWindow.This.SelectedSecurity))
                \_connector.SubscribeLevel1(MainWindow.This.SelectedSecurity);
}
private void OnNewMessage(Message message)
{
    if (message.Type \=\= MessageTypes.Level1Change)
    {
        var level1Mes \= (Level1ChangeMessage)message;
        if (level1Mes.SecurityId \!\= MainWindow.This.SelectedSecurity.ToSecurityId())
            return;
        Level1Grid.Messages.Add(level1Mes);
    }
}
              		
	  				
```
