# Modos de Negociação da Estratégia

## Visão Geral

A propriedade `TradingMode` permite restringir a atividade de negociação de uma estratégia sem a parar completamente. Isto é útil para a gestão de risco -- por exemplo, proibir a abertura de novas posições permitindo apenas fechar posições existentes, ou bloquear completamente o envio de ordens.

O modo é definido usando a enumeração `StrategyTradingModes` e pode ser alterado enquanto a estratégia está em execução.

## Enumeração StrategyTradingModes

| Valor | Descrição |
|-------|-----------|
| `Full` | Acesso total à negociação. Sem restrições sobre ordens. Valor predefinido. |
| `Disabled` | A negociação é completamente proibida. Todas as tentativas de colocação de ordens serão rejeitadas. |
| `CancelOrdersOnly` | Apenas o cancelamento de ordens é permitido. Novas ordens e a modificação de ordens existentes são proibidas. |
| `ReducePositionOnly` | Apenas são permitidas ordens que reduzam a posição atual. A abertura de novas posições e o aumento das existentes são proibidos. |
| `LongOnly` | Apenas são permitidas posições longas. A venda só é permitida para fechar uma posição longa existente (o volume de venda não pode exceder a posição atual). A abertura de posições curtas é proibida. |

## Definir o Modo

```csharp
// Ao criar a estratégia
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Alteração dinâmica durante a operação
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## Lógica de Verificação do Modo

Ao tentar registar uma ordem, a estratégia verifica o modo atual:

- **`Disabled`** -- a ordem é rejeitada com o motivo "negociação proibida".
- **`ReducePositionOnly`** -- a ordem é rejeitada se a posição atual for zero, se a direção da ordem corresponder à direção da posição, ou se o volume da ordem exceder o valor absoluto da posição.
- **`LongOnly`** -- uma ordem de venda é rejeitada se a posição atual não for positiva ou se o volume de venda exceder a posição atual.
- **`Full`** -- sem restrições.
- **`CancelOrdersOnly`** -- apenas o cancelamento de ordens é permitido.

## Método IsFormedAndOnlineAndAllowTrading

O método de extensão `IsFormedAndOnlineAndAllowTrading` verifica se a estratégia está formada (`IsFormed`), está num estado online (`IsOnline`) e se o modo de negociação permite a ação necessária:

```csharp
// Verificar permissão para negociação completa (predefinição)
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// Verificar permissão apenas para cancelamento de ordens
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// Verificar permissão para redução de posição
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

Lógica de permissões quando chamado com um parâmetro `required`:

| TradingMode atual \ required | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|------------------------------|--------|--------------------|----------------------|
| `Full` | sim | sim | sim |
| `Disabled` | não | não | não |
| `CancelOrdersOnly` | não | sim | não |
| `ReducePositionOnly` | não | sim | sim |
| `LongOnly` | não | sim | sim |

## Exemplo de Utilização

```csharp
public class TradingModeStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TradingModeStrategy()
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
        // Verificar se a estratégia está pronta para negociação completa
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.ClosePrice > candle.OpenPrice)
        {
            BuyMarket(Volume);
        }
        else if (candle.ClosePrice < candle.OpenPrice)
        {
            SellMarket(Volume);
        }
    }
}

// Iniciar a estratégia com uma restrição -- apenas posições longas
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// Mais tarde -- mudar para o modo de fecho de posição
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Bloqueio completo da negociação
strategy.TradingMode = StrategyTradingModes.Disabled;
```

Neste exemplo, a estratégia opera inicialmente no modo `LongOnly`, que permite apenas compras e o fecho de posições longas. Quando as condições de mercado mudam, o modo pode ser alterado para `ReducePositionOnly` para o fecho gradual da posição, e depois para `Disabled` para uma paragem completa da atividade de negociação.
