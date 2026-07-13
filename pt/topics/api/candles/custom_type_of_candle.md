# Tipo personalizado de vela

[S#](../../api.md) permite estender as capacidades de construção de velas, oferecendo a possibilidade de trabalhar com tipos de vela personalizados. Isso é útil nos casos em que você precisa trabalhar com velas que atualmente não são suportadas pelo [S#](../../api.md). Abaixo está o processo de criação do seu próprio tipo de vela usando o exemplo de velas Delta (velas formadas com base na diferença entre volumes de compra e venda).

## Implementando velas Delta

1. Primeiro, você precisa criar seu próprio tipo de mensagem de vela. O tipo deve herdar da classe [CandleMessage](xref:StockSharp.Messages.CandleMessage):

   ```cs
   /// <summary>
   /// Vela formada com base no delta dos volumes de compra e venda.
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
       // Obtemos o identificador do tipo de mensagem pelo helper
       // para usar o mesmo valor em RegisterCandleType

       /// <summary>
       /// Inicializar uma nova instância de <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }

       /// <summary>
       /// Valor limite de delta para formação da vela.
       /// </summary>
       public decimal DeltaThreshold { get; set; }

       /// <summary>
       /// Valor atual de delta.
       /// </summary>
       public decimal CurrentDelta { get; set; }

       /// <summary>
       /// Criar uma cópia de <see cref="DeltaCandleMessage"/>.
       /// </summary>
       /// <returns>Cópia.</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }

       /// <summary>
       /// Parâmetro da vela.
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }

       /// <summary>
       /// Tipo do argumento da vela.
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. Em seguida, você precisa criar seu próprio tipo de dado na classe [DataType](xref:StockSharp.Messages.DataType):

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// Definir um MessageType único para velas delta.
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;

       /// <summary>
       /// Tipo de dados <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public static readonly DataType CandleDelta =
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();

       /// <summary>
       /// Criar um tipo de dados para velas delta.
       /// </summary>
       /// <param name="threshold">Valor limite de delta.</param>
       /// <returns>Tipo de dados.</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }

       /// <summary>
       /// Registrar o tipo de vela delta no sistema.
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // Registrar novo tipo de vela no StockSharp
           Extensions.RegisterCandleType<decimal>(
               typeof(DeltaCandleMessage),      // Tipo de mensagem de vela
               DeltaCandleType,                // Tipo de mensagem
               "delta",                        // Nome do arquivo para armazenamento
               str => str.To<decimal>(),       // Conversor de string para parâmetro
               arg => arg.ToString(),          // Conversor de parâmetro para string
               a => a > 0,                     // Validador de parâmetros
               false                           // Se essas velas podem ser obtidas da fonte (não apenas construídas)
           );
       }
   }
   ```

3. Em seguida, você precisa criar um construtor de velas para o novo tipo. Para isso, crie uma implementação de [CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1):

   ```cs
   /// <summary>
   /// Construtor de velas para o tipo <see cref="DeltaCandleMessage"/>.
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// Inicializa uma nova instância de <see cref="DeltaCandleBuilder"/>.
       /// </summary>
       /// <param name="exchangeInfoProvider">Provedor de informações da bolsa.</param>
       public DeltaCandleBuilder(IExchangeInfoProvider exchangeInfoProvider)
           : base(exchangeInfoProvider)
       {
       }

       /// <inheritdoc />
       protected override DeltaCandleMessage CreateCandle(ICandleBuilderSubscription subscription, ICandleBuilderValueTransform transform)
       {
           var time = transform.Time;

           return FirstInitCandle(subscription, new DeltaCandleMessage
           {
               DeltaThreshold = subscription.Message.GetArg<decimal>(),
               OpenTime = time,
               CloseTime = time,
               HighTime = time,
               LowTime = time,
               CurrentDelta = 0
           }, transform);
       }

       /// <inheritdoc />
       protected override bool IsCandleFinishedBeforeChange(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           // A vela fecha quando o valor absoluto de delta excede o limite
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }

       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);

           // Atualizar delta com base no lado da negociação
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. Em seguida, você precisa registrar o construtor de velas em [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider):

   ```cs
   private Connector _connector;
   ...
   // Registrar tipo de vela delta no sistema
   DeltaCandleHelper.RegisterDeltaCandleType();

   // Registrar construtor de velas delta
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. Crie uma assinatura para as velas do tipo `DeltaCandleMessage` e solicite dados a partir dela:

   ```cs
   // Valor limite de delta
   decimal deltaThreshold = 1000m;

   // Criar assinatura para velas delta
   var subscription = new Subscription(
       // Usar nosso método de extensão para criar um tipo de dados
       deltaThreshold.Delta(),
       security)
   {
       MarketData =
       {
           // Especificar que as velas serão construídas a partir de ticks
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };

   // Assinar o evento de vela recebida
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;

       var deltaCandle = (DeltaCandleMessage)candle;

       // Processar vela delta
       Console.WriteLine($"Vela delta {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };

   // Assinar a transição para o modo online
   _connector.SubscriptionOnline += sub =>
   {
       if (sub == subscription)
           Console.WriteLine("A assinatura de velas delta passou para o modo online");
   };

   // Iniciar a assinatura
   _connector.Subscribe(subscription);
   ```

## Usando velas Delta em Estratégias de Negociação

Exemplo de uma estratégia simples usando velas Delta:

```cs
public class DeltaCandleStrategy : Strategy
{
	private readonly StrategyParam<decimal> _deltaThreshold;
	private readonly StrategyParam<decimal> _volume;
	private readonly StrategyParam<decimal> _signalDelta;

	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _deltaIndicatorElement;

	public decimal DeltaThreshold
	{
		get => _deltaThreshold.Value;
		set => _deltaThreshold.Value = value;
	}

	public decimal Volume
	{
		get => _volume.Value;
		set => _volume.Value = value;
	}

	public decimal SignalDelta
	{
		get => _signalDelta.Value;
		set => _signalDelta.Value = value;
	}

	public DeltaCandleStrategy()
	{
		// Parâmetros da estratégia
		_deltaThreshold = Param(nameof(DeltaThreshold), 1000m)
			.SetDisplay("Valor limite de delta", "Valor do delta de volume para formação de velas", "Configurações principais")
			.SetGreaterThanZero()
			.SetCanOptimize(true)
			.SetOptimize(500m, 2000m, 100m);

		_volume = Param(nameof(Volume), 1m)
			.SetDisplay("Volume da ordem", "Volume para operações de negociação", "Configurações principais")
			.SetGreaterThanZero();

		_signalDelta = Param(nameof(SignalDelta), 500m)
			.SetDisplay("Delta mínimo para sinal", "Valor mínimo de delta para geração de sinal", "Configurações principais")
			.SetGreaterThanZero()
			.SetCanOptimize(true);

		Name = "DeltaCandleStrategy";
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Inicialização do gráfico, se disponível
		_chart = GetChart();
		if (_chart != null)
		{
			var area = _chart.AddArea();
			_chartCandleElement = area.AddCandles();
			_deltaIndicatorElement = area.AddIndicator();
			_deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
			_deltaIndicatorElement.Color = System.Drawing.Color.Purple;
		}

		// Criar uma assinatura para velas delta
		var subscription = new Subscription(DeltaThreshold.Delta(), Security)
		{
			MarketData =
			{
				BuildMode = MarketDataBuildModes.Build,
				BuildFrom = DataType.Ticks
			}
		};

		// Criar uma regra para processar velas delta
		this
			.WhenCandleReceived(subscription)
			.Do(ProcessDeltaCandle)
			.Apply(this);

		// Iniciar a assinatura
		Subscribe(subscription);
	}

	private void ProcessDeltaCandle(ICandleMessage candle)
	{
		// Desenhar no gráfico, se disponível
		if (_chart != null)
		{
			var deltaCandle = (DeltaCandleMessage)candle;

			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);

			_chart.Draw(data);
		}

		// Processar apenas velas concluídas
		if (candle.State != CandleStates.Finished)
			return;

		var deltaCandle = (DeltaCandleMessage)candle;

		// Verificar se o delta é suficiente para um sinal
		if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
		{
			this.AddInfoLog($"Delta {deltaCandle.CurrentDelta} é menor que o valor de limiar {SignalDelta}. Nenhum sinal é gerado.");
			return;
		}

		// A direção da operação depende do sinal do delta
		var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;

		this.AddInfoLog($"Vela delta concluída. Delta: {deltaCandle.CurrentDelta}. Direção: {direction}");

		// Usar o preço de fechamento da vela para determinar o preço
		var price = deltaCandle.ClosePrice;
		var volume = Volume;

		// Se já temos uma posição na direção oposta,
		// aumentar o volume para fechar a posição existente
		if ((Position < 0 && direction == Sides.Buy) ||
			(Position > 0 && direction == Sides.Sell))
		{
			volume = Math.Max(volume, Math.Abs(Position) + volume);
		}

		// Registrar uma ordem
		RegisterOrder(this.CreateOrder(direction, price, volume));
	}
}
```

## Pontos Importantes ao Criar Tipos de Vela Personalizados

1. **Exclusividade de MessageTypes** — certifique-se de que o identificador `MessageTypes` escolhido não entre em conflito com os tipos existentes no StockSharp. Recomenda-se usar valores maiores que 10000 para tipos personalizados.

2. **Registo do tipo de vela** — o registo por meio de `Extensions.RegisterCandleType` é necessário para a integração correta com os controlos gráficos e o armazenamento de dados do StockSharp. Sem o registo, o tipo de vela funcionará apenas no código, mas não estará disponível na interface do utilizador.

3. **Parâmetro da vela** — implemente a propriedade `ArgType` que retorna o tipo do argumento da vela. Isso é usado para a exibição correta dos parâmetros na interface gráfica.

4. **Sistema de Arquivos** — o parâmetro `fileName` no método `RegisterCandleType` é usado para salvar as velas no sistema de arquivos quando você usa o armazenamento de dados do StockSharp.

5. **Validação de Parâmetros** — o método de validação de parâmetros é usado no StockSharp para verificar a correção dos valores antes de criar uma assinatura.

Assim, criamos um tipo de vela totalmente personalizado que se integra corretamente a todo o ecossistema StockSharp (incluindo a interface do usuário e o armazenamento de dados) e pode ser usado para construir estratégias de negociação baseadas na análise de delta de volume.
