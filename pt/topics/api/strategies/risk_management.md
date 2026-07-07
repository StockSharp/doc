# Gestão de Risco

## Visão Geral

Cada estratégia no StockSharp tem um `RiskManager` integrado que permite o controlo automático dos riscos de negociação. Quando uma regra é acionada, o gestor pode fechar posições, cancelar ordens ativas ou parar completamente a negociação da estratégia.

O gestor de risco processa todas as mensagens de negociação que passam pela estratégia e, quando as condições das regras são cumpridas, executa automaticamente a ação especificada sem qualquer código adicional na lógica da estratégia.

## Propriedades da Estratégia

### RiskManager

O objeto `IRiskManager` que gere a lista de regras. É criado automaticamente quando a estratégia é inicializada:

```csharp
// Aceder ao gestor de risco
IRiskManager manager = strategy.RiskManager;
```

### RiskRules

Uma propriedade conveniente para ler e definir a lista de regras:

```csharp
// Ler regras atuais
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// Definir novas regras
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## Ações de Acionamento

A enumeração `RiskActions` define as ações possíveis:

| Ação | Descrição |
|------|-----------|
| `ClosePositions` | Fechar todas as posições abertas com uma ordem de mercado |
| `StopTrading` | Bloquear a negociação da estratégia |
| `CancelOrders` | Cancelar todas as ordens ativas |

Quando uma regra é acionada, a estratégia regista um aviso com o nome da regra, os seus parâmetros e a ação executada.

## Regras Disponíveis

### RiskPnLRule -- controlo de lucro/perda

Aciona quando o nível de P&L especificado é atingido. Um valor positivo controla o lucro, um valor negativo controla a perda:

```csharp
// Parar a negociação com uma perda superior a 5000
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### RiskPositionSizeRule -- controlo do tamanho da posição

Aciona quando o tamanho de posição especificado é atingido:

```csharp
// Cancelar ordens quando posição >= 100
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- controlo da frequência de ordens

Aciona quando o número de ordens dentro do intervalo especificado excede o limite:

```csharp
// Parar com mais de 50 ordens por minuto
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### RiskOrderVolumeRule -- controlo do volume da ordem

Aciona quando o volume de uma única ordem é excedido.

### RiskOrderPriceRule -- controlo do preço da ordem

Aciona quando o preço da ordem ultrapassa os limites estabelecidos.

### RiskTradeVolumeRule -- controlo do volume da transação

Aciona quando o volume de uma única transação é excedido.

### RiskTradePriceRule -- controlo do preço da transação

Aciona quando o preço da transação ultrapassa os limites estabelecidos.

### RiskTradeFreqRule -- controlo da frequência de transações

Aciona quando o número de transações dentro do intervalo é excedido.

### RiskPositionTimeRule -- controlo do tempo de manutenção da posição

Aciona quando uma posição foi mantida durante mais tempo do que o estabelecido.

### RiskCommissionRule -- controlo da comissão

Aciona quando a comissão total é excedida.

### RiskSlippageRule -- controlo do slippage

Aciona quando o nível de slippage é excedido.

### RiskErrorRule -- controlo de erros

Aciona quando se acumulou um determinado número de erros.

### RiskOrderErrorRule -- controlo de erros de registo de ordens

Aciona quando se acumularam erros de registo de ordens.

## Exemplo de Utilização

```csharp
public class RiskAwareStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public RiskAwareStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configurar regras de gestão de risco
        RiskRules = new IRiskRule[]
        {
            // Fechar posições com uma perda superior a 10000
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // Parar a negociação quando a posição excede 500 contratos
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // Cancelar ordens quando a frequência excede 100 por minuto
            new RiskOrderFreqRule
            {
                Count = 100,
                Interval = TimeSpan.FromMinutes(1),
                Action = RiskActions.CancelOrders
            },
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

        // Lógica de negociação
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

Neste exemplo, as regras de gestão de risco são definidas quando a estratégia inicia. Quando é atingida uma perda de 10000, as posições são fechadas automaticamente. Quando o tamanho da posição excede 500 contratos, a negociação é bloqueada. Quando as ordens são colocadas com demasiada frequência, as ordens ativas são canceladas. Todas estas verificações são executadas automaticamente sem qualquer código adicional no método `ProcessCandle`.
