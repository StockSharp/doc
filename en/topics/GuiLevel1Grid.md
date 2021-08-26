# Level1

![GUI Leve1Grid](~/images/GUI_Leve1Grid.png)

[Level1Grid](../api/StockSharp.Xaml.Level1Grid.html) is the table for displaying the Level1 fields. This table uses the data as [Level1ChangeMessage](../api/StockSharp.Messages.Level1ChangeMessage.html) messages. 

**Main properties**

- [MaxCount](../api/StockSharp.Xaml.Level1Grid.MaxCount.html) \- the maximum number of messages to display.
- [Messages](../api/StockSharp.Xaml.Level1Grid.Messages.html) \- the list of messages added to the table.
- [SelectedMessage](../api/StockSharp.Xaml.Level1Grid.SelectedMessage.html) \- the selected message.
- [SelectedMessages](../api/StockSharp.Xaml.Level1Grid.SelectedMessages.html) \- selected messages.

Below is the code snippet with its use. 

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
