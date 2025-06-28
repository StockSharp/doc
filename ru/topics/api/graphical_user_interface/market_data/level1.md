# Level1

![GUI Leve1Grid](../../../../images/gui_leve1grid.png)

[Level1Grid](xref:StockSharp.Xaml.Level1Grid) - таблица для отображения полей Level1. Эта таблица использует данные в виде сообщений [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage). 

**Основные свойства**

- [Level1Grid.MaxCount](xref:StockSharp.Xaml.Level1Grid.MaxCount) - максимальное число сообщений для показа.
- [Level1Grid.Messages](xref:StockSharp.Xaml.Level1Grid.Messages) - список сообщений, добавленных в таблицу.
- [Level1Grid.SelectedMessage](xref:StockSharp.Xaml.Level1Grid.SelectedMessage) - выбранное сообщение.
- [Level1Grid.SelectedMessages](xref:StockSharp.Xaml.Level1Grid.SelectedMessages) - выбранные сообщения.

Ниже показаны фрагменты кода с его использованием:

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
	
	// Подписываемся на событие получения данных Level1
	_connector.Level1Received += OnLevel1Received;
	
	// Создаем подписку на данные Level1, если еще не подписаны
	var security = MainWindow.This.SelectedSecurity;
	if (!_connector.Subscriptions.Any(s => 
			s.DataType == DataType.Level1 && 
			s.SecurityId == security.ToSecurityId()))
	{
		var subscription = new Subscription(DataType.Level1, security);
		_connector.Subscribe(subscription);
	}
}

private void OnLevel1Received(Subscription subscription, Level1ChangeMessage level1Message)
{
	// Проверяем, относится ли сообщение к выбранному инструменту
	if (level1Message.SecurityId != MainWindow.This.SelectedSecurity.ToSecurityId())
		return;
		
	// Добавляем сообщение в Level1Grid
	this.GuiAsync(() => Level1Grid.Messages.Add(level1Message));
}

private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
	// Отписываемся от событий при закрытии окна
	if (_connector != null)
		_connector.Level1Received -= OnLevel1Received;
}
```

### Рекомендуемый способ обработки данных Level1

```cs
// Создание подписки на Level1 с использованием нескольких инструментов
public void SubscribeToLevel1(IEnumerable<Security> securities)
{
	foreach (var security in securities)
	{
		var subscription = new Subscription(DataType.Level1, security);
		_connector.Subscribe(subscription);
	}
	
	// Подписываемся на событие получения данных Level1
	_connector.Level1Received += OnLevel1Received;
}

// Обработчик события получения данных Level1
private void OnLevel1Received(Subscription subscription, Level1ChangeMessage level1Message)
{
	// Проверяем, нужно ли обрабатывать именно это сообщение
	if (IsLevel1Needed(subscription))
	{
		// Обновляем GUI в потоке пользовательского интерфейса
		this.GuiAsync(() => 
		{
			// Добавляем сообщение в Level1Grid
			Level1Grid.Messages.Add(level1Message);
			
			// Обрабатываем изменения в полях Level1
			foreach (var change in level1Message.Changes)
			{
				switch (change.Key)
				{
					case Level1Fields.LastTradePrice:
						// Обрабатываем изменение цены последней сделки
						var lastPrice = (decimal)change.Value;
						Console.WriteLine($"Последняя цена {security.Code}: {lastPrice}");
						break;
						
					case Level1Fields.BestBidPrice:
						// Обрабатываем изменение лучшей цены покупки
						var bestBid = (decimal)change.Value;
						Console.WriteLine($"Лучший бид {security.Code}: {bestBid}");
						break;
						
					case Level1Fields.BestAskPrice:
						// Обрабатываем изменение лучшей цены продажи
						var bestAsk = (decimal)change.Value;
						Console.WriteLine($"Лучший аск {security.Code}: {bestAsk}");
						break;
				}
			}
		});
	}
}
```