# Escribir una estrategia con IA

Una guía paso a paso para crear una estrategia de negociación de StockSharp usando herramientas de IA.

## Preparación

### 1. Instale una herramienta de IA

Elija una de las herramientas disponibles:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (requiere Node.js)
- **Cursor** — descargue desde [cursor.com](https://cursor.com)
- **GitHub Copilot** — instale el plugin para su IDE

### 2. Cree un proyecto

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Prepare el contexto

Cree un archivo `CLAUDE.md` (o `.cursorrules`) en la raíz del proyecto:

```markdown
# Reglas del proyecto

- Marco de trabajo: StockSharp 5.x, .NET 10
- Las estrategias heredan de la clase Strategy
- Suscribirse a velas mediante Connector.Subscribe(subscription)
- Registrar órdenes mediante RegisterOrder(order)
- Registro: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Indicadores: crearlos con new y llamar a indicator.Process(candle)
- Gestionar siempre connector.Error y los errores de la estrategia
```

## Ejemplo paso a paso: estrategia SMA

### Paso 1: Describa la tarea a la IA

Ejemplo de prompt:

```
Cree una estrategia de negociación con StockSharp que:
- herede de Strategy
- use dos medias móviles simples (SMA): rápida (periodo 10) y lenta (periodo 30)
- compre cuando la SMA rápida cruza por encima de la SMA lenta
- venda cuando la SMA rápida cruza por debajo de la SMA lenta
- tamaño de posición: 1 lote
- use velas de 5 minutos
- se suscriba a las velas en OnStarted()
- procese las velas mediante reglas de suscripción
```

### Paso 2: Revise el código generado

La IA generará algo como esto:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _fastPeriod;
    private readonly StrategyParam<int> _slowPeriod;
    private readonly StrategyParam<DataType> _candleType;

    private SimpleMovingAverage _fastSma;
    private SimpleMovingAverage _slowSma;

    private bool _prevFastAbove;
    private bool _isFirstCandle = true;

    public SmaCrossStrategy()
    {
        _fastPeriod = Param(nameof(FastPeriod), 10);
        _slowPeriod = Param(nameof(SlowPeriod), 30);
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(5)));
    }

    public int FastPeriod
    {
        get => _fastPeriod.Value;
        set => _fastPeriod.Value = value;
    }

    public int SlowPeriod
    {
        get => _slowPeriod.Value;
        set => _slowPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _fastSma = new SimpleMovingAverage { Length = FastPeriod };
        _slowSma = new SimpleMovingAverage { Length = SlowPeriod };
        _isFirstCandle = true;

        var subscription = new Subscription(CandleType, Security);

        this
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var fastValue = _fastSma.Process(candle);
        var slowValue = _slowSma.Process(candle);

        if (!_fastSma.IsFormed || !_slowSma.IsFormed)
            return;

        var fastAbove = fastValue.GetValue<decimal>() > slowValue.GetValue<decimal>();

        if (!_isFirstCandle)
        {
            if (fastAbove && !_prevFastAbove)
            {
                // La SMA rápida cruza por encima de la lenta — comprar
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // La SMA rápida cruza por debajo de la lenta — vender
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### Paso 3: Qué comprobar

Repase esta lista de verificación:

- **Herencia**: la clase hereda de `Strategy` ✓
- **Parámetros**: usa `StrategyParam<T>` para la optimización ✓
- **Suscripción a velas**: mediante `Subscribe(new Subscription(...))` ✓
- **Procesamiento de velas**: mediante la regla `WhenCandlesFinished` ✓
- **Comprobación de IsFormed**: se verifica que los indicadores estén listos ✓
- **Órdenes**: mediante `RegisterOrder()` con `BuyAtMarket` / `SellAtMarket` ✓
- **Posición**: `Position` se comprueba antes de colocar órdenes ✓

### Paso 4: Pida a la IA que añada pruebas históricas

```
Agregue código de pruebas históricas para esta estrategia usando datos históricos.
Use HistoryEmulationConnector, cargue datos desde el almacenamiento local
y muestre estadísticas resumidas (PnL, número de operaciones, reducción máxima).
```

## Ejemplos de prompts

### Estrategia de bandas de Bollinger

```
Cree una estrategia StockSharp que opere con Bandas de Bollinger:
- comprar cuando el precio toca la banda inferior
- vender cuando el precio toca la banda superior
- periodo 20, multiplicador 2,0
- stop-loss: 1 % desde el precio de entrada
- take-profit: 2 % desde el precio de entrada
- usar StrategyParam para todos los parámetros
```

### Estrategia de arbitraje

```
Cree una estrategia de arbitraje de pares en StockSharp:
- dos instrumentos (especificados mediante parámetros)
- calcular el spread entre los precios
- entrar cuando el spread se desvíe 2 desviaciones estándar
- salir cuando el spread vuelva a la media
- neutralización de volumen (posiciones equivalentes en términos monetarios)
```

### Scalping con libro de órdenes

```
Cree una estrategia de scalping en StockSharp:
- suscribirse al libro de órdenes (MarketDepth) mediante Subscribe
- analizar el desequilibrio bid/ask
- entrar con un desequilibrio fuerte (> 3:1)
- salida rápida por take-profit (5 ticks)
- stop-loss: 3 ticks
- máximo 1 posición a la vez
```

## Errores comunes de la IA

### 1. Eventos obsoletos

**Incorrecto** (API antigua):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Correcto** (API actual):
```csharp
// Usar suscripciones
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Creación de órdenes sin helpers

**Incorrecto**:
```csharp
var order = new Order
{
    Security = Security,
    Portfolio = Portfolio,
    Side = Sides.Buy,
    Type = OrderTypes.Market,
    Volume = 1,
};
```

**Correcto** (usando los helpers de la estrategia):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Falta la comprobación de IsFormed

**Incorrecto**:
```csharp
var value = _sma.Process(candle);
// Usar el valor inmediatamente — puede no estar listo
```

**Correcto**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Consejos

1. **Proporcione documentación a la IA** — indíquele [doc.stocksharp.com](https://doc.stocksharp.com) o copie ejemplos de código de `Samples/`
2. **Use CLAUDE.md** — un archivo de reglas del proyecto reduce considerablemente el número de errores
3. **Empiece de forma simple** — cree primero una estrategia básica y luego añada filtros y gestión de riesgo
4. **Pruebe con el histórico** — ejecute siempre una prueba histórica antes de operar en real
5. **Clone el repositorio** — si la IA tiene acceso a los fuentes de StockSharp, usará la API con mayor precisión
