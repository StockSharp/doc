# Estratégias

## Visão Geral

A classe `Strategy` é a classe base para criar estratégias de negociação em StockSharp. Fornece um conjunto completo de ferramentas para subscrever dados de mercado, gerir ordens e posições, calcular estatísticas e gerar relatórios.

Principais capacidades da classe `Strategy`:

- Subscrever velas, livros de ordens, ticks e outros dados de mercado
- Colocar, modificar e cancelar ordens
- Gestão de posição alvo
- Cálculo de PnL, comissão e estatísticas
- Gestão de risco
- Sistema de temporizador e regras
- Alertas
- Geração de relatórios

> [!WARNING]
> A funcionalidade de estratégias filhas (`ChildStrategies`) foi declarada obsoleta e deixou de ser suportada. A propriedade `ChildStrategies` está marcada com o atributo `[Obsolete("Child strategies no longer supported.")]`. Se o seu código usa estratégias filhas, recomenda-se refatorar -- execute cada estratégia como uma instância independente.

## Secções da Documentação

- [Gestão de Posição Alvo](target_position_management.md) -- gestão declarativa do tamanho da posição através de `SetTargetPosition`
- [Modos de Negociação](trading_modes.md) -- restrição da atividade de negociação através de `StrategyTradingModes`
- [Sistema de Alertas](alert_system.md) -- envio de notificações (popup, som, log, Telegram)
- [Sistema de Temporizador](timer_system.md) -- execução periódica de ações
- [Gestão de Risco](risk_management.md) -- regras de gestão de risco
- [Subscrições de Alto Nível](high_level_subscriptions.md) -- subscrições simplificadas de dados de mercado
- [Relatórios de Estratégia](reporting.md) -- geração de relatórios de resultados de transações
- [Funcionalidades Avançadas](advanced_features.md) -- comentários de ordens, horários, taxa sem risco, fonte de indicadores

## Estratégia Mínima

```csharp
public class MyStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public MyStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Lógica de negociação
    }
}
```

## Ciclo de Vida da Estratégia

1. **Criação** -- construtor, declaração de parâmetros através de `Param<T>`.
2. **Configuração** -- definição de `Security`, `Portfolio`, `Connector` e parâmetros.
3. **Início** -- chamada de `Start()`, transição para o estado `ProcessStates.Started`, invocação de `OnStarted2(DateTime)`.
4. **Execução** -- processamento de dados de mercado, colocação de ordens.
5. **Paragem** -- chamada de `Stop()`, transição através de `ProcessStates.Stopping` para `ProcessStates.Stopped`, invocação de `OnStopped()`.
