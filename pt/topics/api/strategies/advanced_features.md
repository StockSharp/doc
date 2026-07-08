# Funcionalidades Avançadas de Estratégia

## Visão Geral

A classe `Strategy` fornece várias propriedades adicionais para ajustar o comportamento: comentário automático de ordens, horário de negociação, taxa livre de risco para estatísticas, fonte de dados para indicadores e gestão do período histórico.

## CommentMode -- Comentários de Ordens

A propriedade `CommentMode` controla o preenchimento automático do campo `Order.Comment` para todas as ordens submetidas pela estratégia. Isto permite identificar que estratégia criou uma ordem, o que é especialmente útil ao executar várias estratégias na mesma conta em simultâneo.

### Enumeração StrategyCommentModes

| Valor | Descrição |
|-------|-------------|
| `Disabled` | O comentário não é preenchido automaticamente. Valor predefinido. |
| `Id` | O comentário é definido como `Strategy.Id` (identificador GUID único). |
| `Name` | O comentário é definido como `Strategy.Name` (nome da estratégia). |

### Exemplo

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Todas as ordens serão marcadas com o nome da estratégia
        CommentMode = StrategyCommentModes.Name;

        // Ou com o identificador para vinculação exata
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

Com o valor `Name` e um nome de estratégia "SMA Crossover", todas as ordens receberão o comentário "SMA Crossover", permitindo filtrar as ordens desta estratégia no diário de negociação.

## WorkingTime -- Horário de Trabalho

A propriedade `WorkingTime` define o horário durante o qual a estratégia está ativa. Fora dos intervalos de tempo especificados, a estratégia pode restringir automaticamente a sua atividade.

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configurar horário de trabalho
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // Negociar das 10:00 às 18:00
                        new Range<TimeSpan>(
                            TimeSpan.FromHours(10),
                            TimeSpan.FromHours(18))
                    }
                }
            }
        };
    }
}
```

A propriedade `TotalWorkingTime` (só de leitura) mostra o tempo total de trabalho da estratégia desde o seu início. É calculada automaticamente quando a estratégia é parada e reiniciada.

## RiskFreeRate -- Taxa Livre de Risco

A propriedade `RiskFreeRate` define a taxa anual livre de risco usada nos cálculos estatísticos -- principalmente o rácio de Sharpe e o rácio de Sortino.

```csharp
var strategy = new MyStrategy();

// Taxa livre de risco de 5% ao ano
strategy.RiskFreeRate = 0.05m;
```

O valor é passado automaticamente para todos os parâmetros estatísticos que implementam `IRiskFreeRateStatisticParameter` quando o gestor de estatísticas da estratégia é inicializado.

## IndicatorSource -- Fonte de Dados dos Indicadores

A propriedade `IndicatorSource` define o valor predefinido para a propriedade `IIndicator.Source` de todos os indicadores da estratégia que não tenham uma fonte especificada explicitamente. Define que campo de `Level1Fields` deve ser usado como dados de entrada do indicador.

```csharp
var strategy = new MyStrategy();

// Todos os indicadores usarão o preço da última negociação por padrão
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// Ou o preço médio
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

Se a propriedade for `null` (valor predefinido), os indicadores usam a sua própria fonte de dados.

## HistoryCalculated -- Período Histórico Calculado

A propriedade virtual `HistoryCalculated` permite que uma estratégia determine programaticamente o período de dados históricos necessário para aquecimento dos indicadores. Devolve `TimeSpan?` -- a duração do período histórico, ou `null` se não for especificado nenhum período.

```csharp
public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _longPeriod;

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public SmaCrossStrategy()
    {
        _longPeriod = Param(nameof(LongPeriod), 50);
    }

    // Cálculo automático do período histórico necessário
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` é a versão calculada por código da propriedade `HistorySize`. A diferença é que `HistorySize` é definida pelo utilizador como um parâmetro da estratégia, enquanto `HistoryCalculated` é calculada programaticamente com base nos parâmetros da estratégia (por exemplo, períodos de indicadores).

## Exemplo: Estratégia com Todas as Definições Avançadas

```csharp
public class AdvancedStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<int> _smaPeriod;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public int SmaPeriod
    {
        get => _smaPeriod.Value;
        set => _smaPeriod.Value = value;
    }

    public AdvancedStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _smaPeriod = Param(nameof(SmaPeriod), 20);
    }

    // Cálculo automático do período histórico
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Comentários das ordens -- nome da estratégia
        CommentMode = StrategyCommentModes.Name;

        // Taxa livre de risco para cálculo de Sharpe
        RiskFreeRate = 0.05m;

        // Fonte de dados para indicadores
        IndicatorSource = Level1Fields.LastTradePrice;

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Lógica de negociação...
    }
}
```

Neste exemplo, a estratégia usa todas as funcionalidades descritas: comenta ordens automaticamente, define a taxa livre de risco para estatísticas, estabelece a fonte de dados para indicadores e calcula o período histórico necessário.
