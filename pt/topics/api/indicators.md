# Indicadores

[S#](../api.md) fornece mais de 140 indicadores padrão de análise técnica. Isto permite usar indicadores prontos em vez de os criar de raiz. Também pode criar os seus próprios indicadores com base nos existentes, como mostrado na secção [Indicador personalizado](indicators/custom_indicator.md). Todas as classes base para trabalhar com indicadores, bem como os próprios indicadores, estão localizadas no namespace [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators).

## Integrar indicadores num algoritmo de negociação

1. Primeiro, é necessário criar um indicador. Um indicador é criado como um objeto .NET normal:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // É recomendado adicionar indicadores à coleção da estratégia
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. Em seguida, é necessário processar dados de mercado para os indicadores. A abordagem mais eficiente é usar o resultado devolvido pelo método [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)):

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // Processar a vela com indicadores e guardar imediatamente os resultados
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // Usar resultados para decisões de negociação
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // Sinal de compra
           BuyAtMarket();
       }
   }
   ```

   Um indicador aceita [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) como entrada. Alguns indicadores trabalham com um número simples, como [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage). Outros requerem uma vela completa, como [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice). Por isso, os valores de entrada têm de ser convertidos para [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) ou para [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue). O valor resultante do indicador segue as mesmas regras do valor de entrada.

3. Tanto os valores resultantes como os valores de entrada do indicador têm a propriedade [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal), que indica que o valor é final e que o indicador não mudará nesse ponto no tempo. Por exemplo, o indicador [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) é formado com base no preço de fecho da vela, mas no momento atual o preço de fecho final é desconhecido e está a mudar. Neste caso, o valor resultante de [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) será false. Se passar uma vela concluída ao indicador, tanto o valor de entrada como o valor resultante de [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) serão true.

4. **Abordagem recomendada**: usar diretamente os valores obtidos ao chamar o método [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)), em vez de chamar posteriormente [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator)):

   ```cs
   // Exemplo de estratégia com duas médias móveis
   private void ProcessCandle(ICandleMessage candle)
   {
       // Processar a vela com indicadores e guardar imediatamente os resultados
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // Desenhar no gráfico
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // Usar os valores obtidos para comparação
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // Verificar se ocorreu cruzamento
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // Ações de negociação com base no sinal
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

   Esta abordagem tem as seguintes vantagens:
   - Está alinhada com o modelo de processamento de dados em fluxo contínuo (receber → processar → usar o resultado)
   - É mais eficiente, pois evita aceder repetidamente ao contentor de valores acumulados
   - Elimina potenciais problemas de sincronização entre a chamada de Process e chamadas posteriores a GetCurrentValue

5. Abordagem não recomendada (menos eficiente):

   ```cs
   // Abordagem subótima
   foreach (var candle in candles)
   {
       // Processar a vela, mas ignorar o valor retornado
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // Depois tente obter valores via GetCurrentValue()
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```
   
   Com esta abordagem, há um acesso adicional ao contentor de valores históricos do indicador, o que introduz atrasos e quebra o modelo de fluxo contínuo do processamento de dados.

6. Todos os indicadores têm a propriedade [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed), que indica se o indicador está pronto para uso. Por exemplo, o indicador [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) tem um período e, até o indicador processar um número de velas igual ao período do indicador, será considerado não pronto para uso. E a propriedade [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) será false.

## Exemplo de uma estratégia completa de médias móveis

Abaixo está um exemplo de uma estratégia que usa corretamente indicadores, processando velas e utilizando os resultados do método Process:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;

	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;
	private IChartCandleElement _chartCandleElement;
	private IChartTradeElement _tradesElem;
	private IChart _chart;

	public SmaStrategy()
	{
		base.Name = "Estratégia SMA";

		// Inicializar parâmetros da estratégia
		_longSmaLength = Param(nameof(LongSmaLength), 80);
		_shortSmaLength = Param(nameof(ShortSmaLength), 30);
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Criar indicadores
		_shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
		_longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

		// Adicionar indicadores à coleção da estratégia
		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);

		// Inicializar gráfico
		_chart = GetChart();
		if (_chart != null)
		{
			InitChart();
		}
		
		// Assinar velas
		var subscription = new Subscription(_series.Value, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Connector.Subscribe(subscription);
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// Processar a vela com indicadores e guardar resultados
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Desenhar no gráfico
		DrawCandlesAndIndicators(candle, longValue, shortValue);
		
		// Verificar condições de negociação
		if (!IsFormedAndOnlineAndAllowTrading()) 
			return;

		// Comparar valores atuais e anteriores do indicador
		var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		// Verificar cruzamento
		if (isShortLessCurrent == isShortLessPrev) 
			return;

		var volume = Volume + Math.Abs(Position);

		// Ações de negociação com base no sinal
		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}

	private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
	{
		if (_chart == null) return;
		var data = _chart.CreateData();
		data.Group(candle.OpenTime)
			.Add(_chartCandleElement, candle)
			.Add(_longSmaIndicatorElement, longSma)
			.Add(_shortSmaIndicatorElement, shortSma);
		_chart.Draw(data);
	}

	// Outros métodos de inicialização do gráfico foram omitidos por brevidade
}
```

Este exemplo demonstra a abordagem correta para trabalhar com indicadores no modelo de fluxo contínuo do StockSharp.
