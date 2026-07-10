# Level1

![Visualização GUI nível1Grid](../../../../images/gui_leve1grid.png)

[Level1Grid](xref:StockSharp.Xaml.Level1Grid) - uma tabela para apresentar campos Level1. Esta tabela utiliza dados sob a forma de mensagens [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage).

**Propriedades principais**

- [Level1Grid.MaxCount](xref:StockSharp.Xaml.Level1Grid.MaxCount) - número máximo de mensagens a apresentar.
- [Level1Grid.Messages](xref:StockSharp.Xaml.Level1Grid.Messages) - lista de mensagens adicionadas à tabela.
- [Level1Grid.SelectedMessage](xref:StockSharp.Xaml.Level1Grid.SelectedMessage) - mensagem selecionada.
- [Level1Grid.SelectedMessages](xref:StockSharp.Xaml.Level1Grid.SelectedMessages) - mensagens selecionadas.

Abaixo encontram-se fragmentos de código que demonstram a sua utilização:

```xaml
<Window x:Class="Membrane02.Level1Window"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		xmlns:sx="http://schemas.stocksharp.com/xaml"
		xmlns:local="clr-namespace:Membrane02"
		mc:Ignorable="d"
		Title="Janela Level1" Height="300" Width="300" Closing="Window_Closing">
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
	
	// Assinar evento de recebimento de dados Level1
	_connector.Level1Received += OnLevel1Received;
	
	// Criar assinatura de dados Level1 se ainda não estiver assinada
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
	// Verificar se a mensagem pertence ao instrumento selecionado
	if (level1Message.SecurityId != MainWindow.This.SelectedSecurity.ToSecurityId())
		return;
		
	// Adicionar mensagem ao Level1Grid
	this.GuiAsync(() => Level1Grid.Messages.Add(level1Message));
}

private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
	// Cancelar assinatura de eventos ao fechar a janela
	if (_connector != null)
		_connector.Level1Received -= OnLevel1Received;
}
```

### Forma recomendada de processar dados Level1

```cs
// Criar assinatura de Level1 usando vários instrumentos
public void SubscribeToLevel1(IEnumerable<Security> securities)
{
	foreach (var security in securities)
	{
		var subscription = new Subscription(DataType.Level1, security);
		_connector.Subscribe(subscription);
	}
	
	// Assinar evento de recebimento de dados Level1
	_connector.Level1Received += OnLevel1Received;
}

// Manipulador do evento de recebimento de dados Level1
private void OnLevel1Received(Subscription subscription, Level1ChangeMessage level1Message)
{
	// Verificar se precisamos processar esta mensagem específica
	if (IsLevel1Needed(subscription))
	{
		// Atualizar GUI na thread da interface
		this.GuiAsync(() => 
		{
			// Adicionar mensagem ao Level1Grid
			Level1Grid.Messages.Add(level1Message);
			
			// Processar alterações nos campos Level1
			foreach (var change in level1Message.Changes)
			{
				switch (change.Key)
				{
					case Level1Fields.LastTradePrice:
						// Processar alteração do preço da última negociação
						var lastPrice = (decimal)change.Value;
						Console.WriteLine($"Último preço {security.Code}: {lastPrice}");
						break;
						
					case Level1Fields.BestBidPrice:
						// Processar alteração do melhor preço bid
						var bestBid = (decimal)change.Value;
						Console.WriteLine($"Melhor bid {security.Code}: {bestBid}");
						break;
						
					case Level1Fields.BestAskPrice:
						// Processar alteração do melhor preço ask
						var bestAsk = (decimal)change.Value;
						Console.WriteLine($"Melhor ask {security.Code}: {bestAsk}");
						break;
				}
			}
		});
	}
}
```
