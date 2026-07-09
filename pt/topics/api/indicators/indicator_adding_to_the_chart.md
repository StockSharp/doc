# Adicionar um indicador ao gráfico

﻿# Adicionar um indicador ao gráfico

O exemplo seguinte demonstra como adicionar um indicador para desenhar num gráfico:

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _sma;
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _longMaElem;

// Inicialização do gráfico e do indicador
private void InitializeChart()
{
	// _chart - StockSharp.Xaml.Charting.Chart
	// Criar uma área do gráfico
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	
	// Criar um elemento do gráfico que representa velas
	_candlesElem = new ChartCandleElement();
	_area.Elements.Add(_candlesElem);
	
	// Criar um elemento do gráfico que representa o indicador
	_longMaElem = new ChartIndicatorElement
	{
		Title = "Long"
	};
	_area.Elements.Add(_longMaElem);
	
	// Criar um indicador
	_sma = new SimpleMovingAverage() { Length = 80 };
	
	// Subscrever o evento de receção de velas
	_connector.CandleReceived += OnCandleReceived;
}

// Método para subscrever velas
private void SubscribeToCandles()
{
	// Criar uma subscrição de velas com o período especificado
	_candleSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),
		_security)
	{
		MarketData = 
		{
			// Pedir dados históricos de 30 dias
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			// Receber apenas velas concluídas
			IsFinishedOnly = true
		}
	};
	
	// Iniciar a subscrição
	_connector.Subscribe(_candleSubscription);
}

// Manipulador do evento de receção de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Verificar se a vela pertence à nossa subscrição
	if (subscription != _candleSubscription)
		return;
	
	// Verificar o estado da vela
	if (candle.State != CandleStates.Finished)
		return;
	
	// Processar a vela com o indicador
	var longValue = _sma.Process(candle);
	
	// Criar dados para desenho
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_longMaElem, longValue);
	
	// Desenhar no gráfico na thread da UI
	this.GuiAsync(() => _chart.Draw(data));
}

// Método para anular a subscrição ao fechar a janela
private void UnsubscribeFromCandles()
{
	if (_candleSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_candleSubscription);
		_candleSubscription = null;
	}
}
```

![indicators chart](../../../images/indicators_chart.png)

## Exemplo de trabalho com vários indicadores

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _shortSma;
private SimpleMovingAverage _longSma;
private ChartArea _mainArea;
private ChartArea _indicatorArea;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _shortSmaElem;
private ChartIndicatorElement _longSmaElem;
private RelativeStrengthIndex _rsi;
private ChartIndicatorElement _rsiElem;

// Inicialização do gráfico e dos indicadores
private void InitializeChartWithMultipleIndicators()
{
	// Criar a área principal para velas e médias móveis
	_mainArea = new ChartArea();
	_chart.Areas.Add(_mainArea);
	
	// Criar uma área para RSI
	_indicatorArea = new ChartArea();
	_chart.Areas.Add(_indicatorArea);
	
	// Criar elementos do gráfico
	_candlesElem = new ChartCandleElement();
	_shortSmaElem = new ChartIndicatorElement { Title = "SMA (short)" };
	_longSmaElem = new ChartIndicatorElement { Title = "SMA (long)" };
	_rsiElem = new ChartIndicatorElement { Title = "RSI" };
	
	// Definir as cores dos elementos
	_shortSmaElem.Color = Colors.Red;
	_longSmaElem.Color = Colors.Blue;
	_rsiElem.Color = Colors.Green;
	
	// Adicionar elementos às respetivas áreas
	_mainArea.Elements.Add(_candlesElem);
	_mainArea.Elements.Add(_shortSmaElem);
	_mainArea.Elements.Add(_longSmaElem);
	_indicatorArea.Elements.Add(_rsiElem);
	
	// Criar indicadores
	_shortSma = new SimpleMovingAverage { Length = 9 };
	_longSma = new SimpleMovingAverage { Length = 20 };
	_rsi = new RelativeStrengthIndex { Length = 14 };
	
	// Subscrever o evento de receção de velas
	_connector.CandleReceived += OnCandleReceivedMultipleIndicators;
	
	// Criar uma subscrição de velas
	_candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			IsFinishedOnly = true
		}
	};
	
	// Iniciar a subscrição
	_connector.Subscribe(_candleSubscription);
}

// Manipulador do evento de receção de velas para vários indicadores
private void OnCandleReceivedMultipleIndicators(Subscription subscription, ICandleMessage candle)
{
	// Verificar se a vela pertence à nossa subscrição
	if (subscription != _candleSubscription)
		return;
	
	if (candle.State != CandleStates.Finished)
		return;
	
	// Processar a vela com indicadores
	var shortSmaValue = _shortSma.Process(candle);
	var longSmaValue = _longSma.Process(candle);
	var rsiValue = _rsi.Process(candle);
	
	// Criar dados para desenho
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_shortSmaElem, shortSmaValue)
			.Add(_longSmaElem, longSmaValue)
			.Add(_rsiElem, rsiValue);
	
	// Desenhar no gráfico na thread da UI
	this.GuiAsync(() => _chart.Draw(data));
}
```

## Ver também

[Componentes para construir gráficos](../graphical_user_interface/charts.md)
