# Sistema de Temporizadores

## Visão Geral

As estratégias em StockSharp suportam um sistema de temporizadores incorporado que permite executar ações em intervalos de tempo especificados. Os temporizadores baseiam-se no mecanismo `WhenIntervalElapsed` do conector e funcionam corretamente tanto em negociação real como durante backtesting (usando o tempo virtual do emulador).

Os temporizadores são convenientes para:

- Verificações periódicas das condições de mercado
- Rebalanceamento do portefólio em intervalos fixos
- Fecho de posições por tempo
- Monitorização do estado da estratégia

## Interface ITimerHandler

O temporizador é gerido através da interface `ITimerHandler`, que fornece os seguintes membros:

| Membro | Descrição |
|--------|-----------|
| `Start()` | Inicia o temporizador. Devolve `ITimerHandler` para encadeamento de métodos |
| `Stop()` | Para o temporizador. Devolve `ITimerHandler` para encadeamento de métodos |
| `Interval` | Intervalo de acionamento do temporizador (leitura e escrita) |
| `IsStarted` | Indica se o temporizador está em execução |
| `Dispose()` | Liberta recursos e para o temporizador |

## Métodos de Criação de Temporizadores

### CreateTimer

Cria um temporizador, mas não o inicia. O arranque é feito por uma chamada separada a `Start()`:

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### StartTimer

Cria e inicia imediatamente um temporizador. Equivale a `CreateTimer(...).Start()`:

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

O intervalo deve ser um valor positivo (`> TimeSpan.Zero`), caso contrário será lançada uma exceção.

## Gestão de Temporizadores

Um temporizador pode ser parado e reiniciado:

```csharp
var timer = CreateTimer(TimeSpan.FromMinutes(1), MyCallback);

// Iniciar
timer.Start();

// Parar
timer.Stop();

// Alterar intervalo
timer.Interval = TimeSpan.FromMinutes(5);

// Iniciar novamente
timer.Start();

// Libertar recursos
timer.Dispose();
```

## Exemplo de Utilização

```csharp
public class TimerStrategy : Strategy
{
    private ITimerHandler _checkTimer;
    private ITimerHandler _closeTimer;

    private readonly StrategyParam<TimeSpan> _checkInterval;
    private readonly StrategyParam<TimeSpan> _maxHoldTime;

    public TimeSpan CheckInterval
    {
        get => _checkInterval.Value;
        set => _checkInterval.Value = value;
    }

    public TimeSpan MaxHoldTime
    {
        get => _maxHoldTime.Value;
        set => _maxHoldTime.Value = value;
    }

    public TimerStrategy()
    {
        _checkInterval = Param(nameof(CheckInterval), TimeSpan.FromMinutes(1));
        _maxHoldTime = Param(nameof(MaxHoldTime), TimeSpan.FromHours(4));
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Temporizador para verificações periódicas das condições de mercado
        _checkTimer = StartTimer(CheckInterval, OnCheckTimer);

        // Temporizador para fecho forçado da posição (criado mas não iniciado)
        _closeTimer = CreateTimer(MaxHoldTime, OnCloseTimer);

        var subscription = SubscribeCandles(TimeSpan.FromMinutes(5));

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void OnCheckTimer()
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        this.AddInfoLog("Checking market conditions at {0}", CurrentTime);

        // Verificação periódica do estado da posição
        if (Position != 0)
        {
            this.AddInfoLog("Current position: {0}", Position);
        }
    }

    private void OnCloseTimer()
    {
        if (Position != 0)
        {
            this.AddInfoLog("Position hold time expired, closing");
            ClosePosition();

            // Parar o temporizador de fecho depois de ser acionado
            _closeTimer.Stop();
        }
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice && Position == 0)
        {
            BuyMarket();

            // Iniciar o temporizador de fecho forçado
            _closeTimer.Start();
        }
    }
}
```

Neste exemplo, são usados dois temporizadores: um para verificação periódica do estado (`StartTimer` -- criado e iniciado imediatamente), e outro para limitar o tempo de manutenção da posição (`CreateTimer` -- criado, mas iniciado apenas ao entrar numa posição).

