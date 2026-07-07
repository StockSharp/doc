# Padrões de Velas Complexos

## Visão Geral

A classe `ComplexCandlePattern` permite criar padrões de velas complexos combinando vários padrões simples (`ICandlePattern`) num só. Ao reconhecer um padrão complexo, cada padrão interno é verificado sequencialmente no seu segmento de velas. O padrão só é considerado reconhecido se todos os padrões internos corresponderem.

## ICandlePattern

A interface base para todos os padrões de velas:

```csharp
public interface ICandlePattern : IPersistable
{
    // Nome do padrão
    string Name { get; }

    // Número de velas necessário para reconhecimento
    int CandlesCount { get; }

    // Verificar se o padrão é reconhecido nas velas fornecidas
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

O registo `CandlePatternRegistry` contém um conjunto de padrões integrados: `Flat`, `White`, `Black`, `Hammer`, `BullishEngulfing`, `MorningStar`, `ThreeWhiteSoldiers` e outros.

## ComplexCandlePattern

A classe `ComplexCandlePattern` implementa `ICandlePattern` e combina vários padrões internos:

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // Criar um padrão vazio
    public ComplexCandlePattern() { }

    // Criar um padrão com um nome e um conjunto de padrões internos
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // Nome do padrão complexo
    public string Name { get; }

    // Padrões internos
    public IEnumerable<ICandlePattern> Inner { get; }

    // Número total de velas (soma de CandlesCount de todos os padrões internos)
    public int CandlesCount { get; }
}
```

Quando `Recognize` é chamado, o array de velas é dividido em segmentos sequenciais de acordo com o `CandlesCount` de cada padrão interno. Se pelo menos um padrão interno não corresponder, o método devolve `false`.

## Exemplo: Criar um Padrão Complexo

```csharp
using StockSharp.Algo.Candles.Patterns;

// Criar um padrão complexo: primeiro uma vela bearish, depois bullish engulfing
var complex = new ComplexCandlePattern(
    "Reversal Up",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 vela: bearish
        CandlePatternRegistry.BullishEngulfing,  // 2 velas: bullish engulfing
    }
);

// São necessárias 3 velas para reconhecimento (1 + 2)
Console.WriteLine($"Candles required: {complex.CandlesCount}"); // 3
```

## ICandlePatternProvider

A interface `ICandlePatternProvider` gere o armazenamento e a pesquisa de padrões:

```csharp
public interface ICandlePatternProvider
{
    // Eventos para criação, substituição e eliminação de padrões
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // Inicializar o armazenamento
    ValueTask InitAsync(CancellationToken cancellationToken);

    // Todos os padrões disponíveis
    IEnumerable<ICandlePattern> Patterns { get; }

    // Encontrar um padrão por nome
    bool TryFind(string name, out ICandlePattern pattern);

    // Remover um padrão
    bool Remove(ICandlePattern pattern);

    // Guardar (criar ou substituir) um padrão
    void Save(ICandlePattern pattern);
}
```

### Implementações

- `InMemoryCandlePatternProvider` -- armazena padrões em memória. Na inicialização, carrega todos os padrões integrados de `CandlePatternRegistry.All`.
- `CandlePatternFileStorage` -- guarda padrões personalizados num ficheiro (JSON). Os padrões integrados de `InMemoryCandlePatternProvider` também estão disponíveis através deste fornecedor.

## Exemplo: Trabalhar com ICandlePatternProvider

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

// Criar armazenamento de padrões baseado em ficheiro
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// Inicializar (carrega padrões integrados + personalizados a partir do ficheiro)
await provider.InitAsync(CancellationToken.None);

// Subscrever o evento de criação de novo padrão
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"Pattern created: {pattern.Name}");
};

// Criar e guardar um padrão complexo
var myPattern = new ComplexCandlePattern(
    "My Pattern",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// Encontrar um padrão por nome
if (provider.TryFind("My Pattern", out var found))
{
    Console.WriteLine($"Found: {found.Name}, candles: {found.CandlesCount}");
}
```

## ExpressionCandlePattern

Para criar padrões com base em fórmulas, é utilizado `ExpressionCandlePattern`. Cada vela no padrão é descrita por uma expressão `CandleExpressionCondition`, com as seguintes variáveis disponíveis:

| Variável | Descrição |
|----------|-------------|
| `O` | Preço de abertura |
| `H` | Preço máximo |
| `L` | Preço mínimo |
| `C` | Preço de fecho |
| `V` | Volume |
| `B` | Corpo da vela |
| `LEN` | Comprimento da vela |
| `BS` | Sombra inferior |
| `TS` | Sombra superior |

O prefixo `p` refere-se à vela anterior (`pO`, `pC`), `pp` -- a duas velas atrás, e assim sucessivamente.

Todos os padrões integrados em `CandlePatternRegistry` são construídos utilizando `ExpressionCandlePattern`.
