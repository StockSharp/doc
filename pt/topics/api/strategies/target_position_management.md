# Gestão da Posição-Alvo

## Visão Geral

O sistema de gestão da posição-alvo permite que uma estratégia especifique declarativamente o tamanho de posição pretendido, enquanto a plataforma coloca automaticamente as ordens necessárias para atingir esse nível. Em vez de calcular manualmente o volume e a direção de um negócio, basta chamar `SetTargetPosition(10)` -- e o gestor determinará se deve comprar ou vender, e em que volume.

O componente principal é a classe `PositionTargetManager`, que automaticamente:

- Calcula a diferença entre a posição atual e a posição-alvo
- Determina a direção e o volume da ordem
- Trata a execução, cancelamento e erros das ordens
- Suporta novas tentativas em caso de falhas

## Métodos da Estratégia

### SetTargetPosition

Define a posição-alvo. Estão disponíveis duas variantes de chamada:

```csharp
// Para o instrumento e portefólio principais da estratégia
SetTargetPosition(decimal target);

// Para um instrumento e portefólio arbitrários
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

Quando `target` é maior do que a posição atual, o gestor colocará uma ordem de compra. Quando é menor -- uma ordem de venda. Se a posição já for igual ao alvo (tendo em conta `PositionTolerance`), nenhuma ação é tomada.

### CancelTargetPosition

Cancela uma posição-alvo definida anteriormente e para todas as ordens ativas relacionadas:

```csharp
// Para o instrumento e portefólio principais da estratégia
CancelTargetPosition();

// Para um instrumento e portefólio arbitrários
CancelTargetPosition(Security security, Portfolio portfolio);
```

### GetTargetPosition

Devolve o valor atual da posição-alvo, ou `null` se nenhum alvo estiver definido:

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## Propriedade TargetPositionManager

A propriedade `TargetPositionManager` fornece acesso direto ao objeto `PositionTargetManager` para ajuste fino:

```csharp
// Número máximo de novas tentativas em caso de erro da ordem (a predefinição é 3)
TargetPositionManager.MaxRetries = 5;

// Tolerância para determinar se a posição-alvo foi atingida
TargetPositionManager.PositionTolerance = 0.01m;

// Tipo de ordem (a predefinição é Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

O gestor gera os seguintes eventos:

- `TargetReached` -- a posição-alvo foi atingida
- `Error` -- ocorreu um erro durante a execução da ordem
- `OrderRegistered` -- o gestor registou uma ordem

## Propriedade TargetAlgoFactory

A propriedade `TargetAlgoFactory` permite definir uma fábrica para algoritmos de alteração de posição. Por predefinição, é usado `MarketOrderAlgo`, que cria ordens de mercado:

```csharp
// Usar um algoritmo personalizado em vez de ordens de mercado
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## Exemplo de Utilização

```csharp
public class TargetPositionStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TargetPositionStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configurar o gestor da posição-alvo
        TargetPositionManager.MaxRetries = 5;
        TargetPositionManager.TargetReached += (sec, pf) =>
        {
            this.AddInfoLog("Posição alvo alcançada: {0}, {1}", sec, pf);
        };

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice)
        {
            // Vela de alta -- definir posição-alvo para compra
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // Vela de baixa -- definir posição-alvo para venda
            SetTargetPosition(-Volume);
        }
    }
}
```

Neste exemplo, a estratégia não lida com cálculos manuais de volume e direção. Simplesmente declara o tamanho de posição pretendido, e `PositionTargetManager` trata de todo o trabalho de colocação de ordens.

