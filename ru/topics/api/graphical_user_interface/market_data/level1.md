# Level1

![GUI Leve1Grid](../../../../images/gui_leve1grid.png)

[Level1Grid](xref:StockSharp.Xaml.Level1Grid) \- таблица для отображения полей Level1. Эта таблица использует данные в виде сообщений [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage). 

**Основные свойства**

- [Level1Grid.MaxCount](xref:StockSharp.Xaml.Level1Grid.MaxCount) \- максимальное число сообщений для показа.
- [Level1Grid.Messages](xref:StockSharp.Xaml.Level1Grid.Messages) \- список сообщений, добавленных в таблицу.
- [Level1Grid.SelectedMessage](xref:StockSharp.Xaml.Level1Grid.SelectedMessage) \- выбранное сообщение.
- [Level1Grid.SelectedMessages](xref:StockSharp.Xaml.Level1Grid.SelectedMessages) \- выбранные сообщения.

Ниже показаны фрагменты кода с его использованием. 

```xaml
<Window x:Class="Membrane02.Level1Window"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:sx="http://schemas.stocksharp.com/xaml"
        xmlns:local="clr-namespace:Membrane02"
        mc:Ignorable="d"
        Title="Level1Window" Height="300" Width="300" Closing="Window_Closing">
    <Grid>
        <sx:Level1Grid x:Name="Level1Grid" />
    </Grid>
</Window>
	  				
```
```cs
public Level1Window()
{
    InitializeComponent();
    _connector = MainWindow.This.Connector;
    _connector.NewMessage += OnNewMessage;
    if (!_connector.RegisteredSecurities.Contains(MainWindow.This.SelectedSecurity))
                _connector.SubscribeLevel1(MainWindow.This.SelectedSecurity);
}
private void OnNewMessage(Message message)
{
    if (message.Type == MessageTypes.Level1Change)
    {
        var level1Mes = (Level1ChangeMessage)message;
        if (level1Mes.SecurityId != MainWindow.This.SelectedSecurity.ToSecurityId())
            return;
        Level1Grid.Messages.Add(level1Mes);
    }
}
              		
	  				
```
