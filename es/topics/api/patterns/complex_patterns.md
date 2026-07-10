# Patrones de velas complejos

## Descripción general

La clase `ComplexCandlePattern` permite crear patrones de velas complejos combinando varios patrones simples (`ICandlePattern`) en uno. Al reconocer un patrón complejo, cada patrón interno se comprueba secuencialmente en su segmento de velas. El patrón se considera reconocido solo si todos los patrones internos coinciden.

## ICandlePattern

La interfaz base para todos los patrones de velas:

```csharp
public interface ICandlePattern : IPersistable
{
    // Nombre del patrón
    string Name { get; }

    // Número de velas requeridas para el reconocimiento
    int CandlesCount { get; }

    // Comprobar si el patrón se reconoce en las velas dadas
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

El registro `CandlePatternRegistry` contiene un conjunto de patrones integrados: `Flat`, `White`, `Black`, `Hammer`, `BullishEngulfing`, `MorningStar`, `ThreeWhiteSoldiers` y otros.

## ComplexCandlePattern

La clase `ComplexCandlePattern` implementa `ICandlePattern` y combina varios patrones internos:

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // Crear un patrón vacío
    public ComplexCandlePattern() { }

    // Crear un patrón con un nombre y conjunto de patrones internos
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // Nombre del patrón complejo
    public string Name { get; }

    // Patrones internos
    public IEnumerable<ICandlePattern> Inner { get; }

    // Número total de velas (suma de CandlesCount para todos los patrones internos)
    public int CandlesCount { get; }
}
```

Cuando se llama a `Recognize`, el array de velas se divide en segmentos secuenciales según el `CandlesCount` de cada patrón interno. Si al menos un patrón interno no coincide, el método devuelve `false`.

## Ejemplo: creación de un patrón complejo

```csharp
using StockSharp.Algo.Candles.Patterns;

// Crear un patrón complejo: primero una vela bajista, después Bullish Engulfing
var complex = new ComplexCandlePattern(
    "Reversal Up",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 vela: bajista
        CandlePatternRegistry.BullishEngulfing,  // 2 velas: Bullish Engulfing
    }
);

// Se requieren 3 velas para el reconocimiento (1 + 2)
Console.WriteLine($"Velas requeridas: {complex.CandlesCount}"); // 3
```

## ICandlePatternProvider

La interfaz `ICandlePatternProvider` gestiona el almacenamiento y búsqueda de patrones:

```csharp
public interface ICandlePatternProvider
{
    // Eventos de creación, reemplazo y eliminación de patrones
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // Inicializar almacenamiento
    ValueTask InitAsync(CancellationToken cancellationToken);

    // Todos los patrones disponibles
    IEnumerable<ICandlePattern> Patterns { get; }

    // Buscar un patrón por nombre
    bool TryFind(string name, out ICandlePattern pattern);

    // Eliminar un patrón
    bool Remove(ICandlePattern pattern);

    // Guardar (crear o reemplazar) un patrón
    void Save(ICandlePattern pattern);
}
```

### Implementaciones

- `InMemoryCandlePatternProvider` -- almacena patrones en memoria. Durante la inicialización, carga todos los patrones integrados desde `CandlePatternRegistry.All`.
- `CandlePatternFileStorage` -- guarda patrones personalizados en un archivo (JSON). Los patrones integrados de `InMemoryCandlePatternProvider` también están disponibles mediante este proveedor.

## Ejemplo: trabajo con ICandlePatternProvider

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

// Crear almacenamiento de patrones basado en archivo
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// Inicializar (carga patrones integrados + personalizados desde archivo)
await provider.InitAsync(CancellationToken.None);

// Suscribirse al evento de creación de nuevo patrón
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"Pattern created: {pattern.Name}");
};

// Crear y guardar un patrón complejo
var myPattern = new ComplexCandlePattern(
    "My Pattern",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// Buscar un patrón por nombre
if (provider.TryFind("My Pattern", out var found))
{
    Console.WriteLine($"Encontrado: {found.Name}, velas: {found.CandlesCount}");
}
```

## ExpressionCandlePattern

Para crear patrones basados en fórmulas, se usa `ExpressionCandlePattern`. Cada vela del patrón se describe mediante una expresión `CandleExpressionCondition`, con las siguientes variables disponibles:

| Variable | Descripción |
|----------|-------------|
| `O` | Precio de apertura |
| `H` | Precio máximo |
| `L` | Precio mínimo |
| `C` | Precio de cierre |
| `V` | Volumen |
| `B` | Cuerpo de la vela |
| `LEN` | Longitud de la vela |
| `BS` | Sombra inferior |
| `TS` | Sombra superior |

El prefijo `p` se refiere a la vela anterior (`pO`, `pC`), `pp` -- a dos velas atrás, y así sucesivamente.

Todos los patrones integrados en `CandlePatternRegistry` se construyen usando `ExpressionCandlePattern`.
