# Exemplo de estratégia C#

A criação de uma estratégia a partir de código-fonte será demonstrada usando o exemplo da estratégia SMA, semelhante ao exemplo da estratégia SMA montada a partir de cubos na secção [Criar algoritmo a partir de cubos](../../using_visual_designer/first_strategy.md).

Esta secção não descreve construções da linguagem C# (nem a [Strategy](../../../../api/strategies.md), que é usada como base para criar estratégias), mas menciona funcionalidades específicas para trabalhar com código no **Designer**.

> [!TIP]
> As estratégias criadas no **Designer** são compatíveis com estratégias criadas na [API](../../../../api.md), devido à utilização da classe base comum [Strategy](../../../../api/strategies.md). Isto torna a execução dessas estratégias fora do **Designer** significativamente mais simples do que executar [esquemas](../../../live_execution/running_strategies_outside_of_designer.md).

1. Os parâmetros da estratégia são criados usando uma abordagem especial:

```cs
_candleTypeParam = this.Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);


private readonly StrategyParam<DataType> _candleTypeParam;

public DataType CandleType
{
	get => _candleTypeParam.Value;
	set => _candleTypeParam.Value = value;
}

private readonly StrategyParam<int> _long;

public int Long
{
	get => _long.Value;
	set => _long.Value = value;
}

private readonly StrategyParam<int> _short;

public int Short
{
	get => _short.Value;
	set => _short.Value = value;
}
```

Ao usar a classe [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1), a abordagem de guardar e restaurar definições é aplicada automaticamente.

2. Ao criar indicadores e subscrever dados de mercado, é necessário ligá-los para que os dados recebidos da subscrição possam atualizar os valores dos indicadores:

```cs
// ---------- criar indicadores -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- ligar o conjunto de velas e indicadores ----

var subscription = SubscribeCandles(CandleType);

subscription
	// ligar indicadores às velas
	.Bind(longSma, shortSma, OnProcess)
	// iniciar o processamento
	.Start();
```

3. Ao trabalhar com gráficos, é importante considerar que, ao executar a estratégia [fora do Designer](../../../live_execution/running_strategies_outside_of_designer.md), o objeto do gráfico pode não existir.

```cs
var area = CreateChartArea();

// area pode ser null caso não exista GUI (estratégia alojada no Runner ou numa aplicação de consola própria)
if (area != null)
{
	DrawCandles(area, subscription);

	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	DrawOwnTrades(area);
}
```

4. Inicie a proteção da posição através de [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), se exigido pela lógica da estratégia:

```cs
// iniciar proteção por take profit e/ou stop loss
StartProtection(TakeValue, StopValue);
```

5. A lógica da estratégia propriamente dita, implementada no método OnProcess. O método é chamado pela subscrição criada no passo 1:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// caso tenhamos subscrito apenas velas não finalizadas
	if (candle.State != CandleStates.Finished)
		return;

	// calcular novos valores para a curta e a longa
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong)
	{
		// ocorreu um cruzamento

		// se a curta for menor do que a longa, venda; caso contrário, compra
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// calcular o tamanho para abrir posição ou inverter
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		var priceStep = GetSecurity().PriceStep ?? 1;

		// calcular o preço da ordem como preço de fecho + deslocamento
		var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		// guardar os valores atuais da curta e da longa
		_isShortLessThenLong = isShortLessThenLong;
	}
}
```
