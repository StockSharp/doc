# Sistema de Alertas

## Visão Geral

As estratégias no StockSharp têm um sistema de alertas integrado que permite enviar notificações de vários tipos: janelas popup, sinais sonoros, entradas de log e mensagens Telegram. Os alertas são úteis para informar o trader sobre eventos importantes -- entradas em posição, quebras de níveis, erros e outros sinais de negociação.

Durante o backtesting, alertas que não sejam do tipo `Log` são ignorados automaticamente para evitar interferência durante os testes.

## Tipos de Alerta

A enumeração `AlertNotifications` define os tipos disponíveis:

| Tipo | Descrição |
|------|-------------|
| `Sound` | Sinal sonoro |
| `Popup` | Janela popup |
| `Log` | Entrada em ficheiro de log |
| `Telegram` | Mensagem Telegram |

## Métodos

### Alert

Método base para enviar um alerta com tipo, legenda e mensagem especificados:

```csharp
// Com título e mensagem
Alert(AlertNotifications type, string caption, string message);

// With automatic caption (uses the strategy name)
Alert(AlertNotifications type, string message);
```

### AlertPopup

Envia uma notificação popup. A legenda é o nome da estratégia:

```csharp
AlertPopup(string message);
```

### AlertSound

Envia uma notificação sonora:

```csharp
AlertSound(string message);
```

### AlertLog

Envia uma notificação para o log. Este tipo também funciona durante o backtesting:

```csharp
AlertLog(string message);
```

## Configurar o Serviço de Alertas

Para que os alertas funcionem, o serviço `IAlertNotificationService` tem de estar registado no ambiente da estratégia. Isto é feito através do método de extensão:

```csharp
strategy.SetAlertService(alertService);
```

O serviço atual pode ser obtido através de:

```csharp
var service = strategy.GetAlertService();
```

Em aplicações gráficas (Designer, terminal), o serviço é normalmente registado automaticamente.

## Exemplo de Utilização

```csharp
public class AlertStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<decimal> _priceLevel;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public decimal PriceLevel
    {
        get => _priceLevel.Value;
        set => _priceLevel.Value = value;
    }

    public AlertStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _priceLevel = Param(nameof(PriceLevel), 100m);
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();

        // Alerta sobre início da estratégia
        AlertLog("Strategy started, tracked level: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // O preço cruzou o nível de baixo para cima
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("Price crossed level " + PriceLevel + " upward!");
            AlertSound("Level breakout!");
            BuyMarket();
        }

        // O preço cruzou o nível de cima para baixo
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Trading signal",
                "Price broke level " + PriceLevel + " downward");
            SellMarket();
        }
    }
}
```

Neste exemplo, a estratégia usa diferentes tipos de alerta para diferentes situações: `AlertPopup` e `AlertSound` para captar imediatamente a atenção do trader, e `Alert` com o tipo `Telegram` para notificação remota.
