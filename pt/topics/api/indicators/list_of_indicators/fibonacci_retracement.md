# FR

**retração de Fibonacci (FR)** é um indicador técnico baseado nos números de Fibonacci que ajuda a identificar potenciais níveis de suporte e resistência com base no movimento anterior do preço.

Para utilizar o indicador, é necessário usar a classe [FibonacciRetracement](xref:StockSharp.Algo.Indicators.FibonacciRetracement).

## Descrição

retração de Fibonacci é uma ferramenta técnica popular que usa linhas horizontais para indicar áreas de possível suporte ou resistência num gráfico de preços. Estes níveis baseiam-se nos números de Fibonacci e nos respetivos rácios percentuais.

O indicador baseia-se na sequência matemática de Fibonacci, em que cada número é a soma dos dois anteriores (1, 1, 2, 3, 5, 8, 13, 21, 34...). A partir desta sequência derivam-se rácios-chave usados na análise técnica: 23,6%, 38,2%, 50%, 61,8% e 78,6%.

retração de Fibonacci é aplicado a um movimento significativo de preço (tendência) e mostra os níveis onde pode ocorrer uma correção (pullback) antes da continuação na direção da tendência principal.

O indicador é particularmente útil para:
- Identificar potenciais níveis de suporte numa tendência de alta
- Identificar potenciais níveis de resistência numa tendência de baixa
- Definir objetivos de entrada após uma correção
- Determinar níveis para colocação de stop-loss

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Período** - período para determinar um movimento significativo do preço (o valor predefinido depende do período)

## Cálculo

O cálculo dos níveis de retração de Fibonacci envolve os seguintes passos:

1. Determinar um movimento significativo do preço (tendência):
   - Numa tendência de alta: do mínimo para o máximo
   - Numa tendência de baixa: do máximo para o mínimo

2. Calcular níveis de correção com base no intervalo deste movimento:
   ```
   Range = |High - Low|

   Nível 0% = High (para tendência de alta) ou Low (para tendência de baixa)
   Nível 23.6% = High - (Range * 0.236) ou Low + (Range * 0.236)
   Nível 38.2% = High - (Range * 0.382) ou Low + (Range * 0.382)
   Nível 50.0% = High - (Range * 0.5) ou Low + (Range * 0.5)
   Nível 61.8% = High - (Range * 0.618) ou Low + (Range * 0.618)
   Nível 78.6% = High - (Range * 0.786) ou Low + (Range * 0.786)
   Nível 100% = Low (para tendência de alta) ou High (para tendência de baixa)
   ```

## Interpretação

Os níveis de retração de Fibonacci são interpretados da seguinte forma:

1. **Principais Níveis de Correção**:
   - 23,6% - nível fraco, frequentemente rompido durante uma tendência forte
   - 38,2% - nível moderado, corresponde a uma correção típica
   - 50,0% - nível psicologicamente significativo (embora não seja um número de Fibonacci)
   - 61,8% - "rácio dourado", o nível de Fibonacci mais importante
   - 78,6% - correção profunda, pode sinalizar uma potencial inversão da tendência

2. **Numa Tendência de Alta**:
   - Os níveis de Fibonacci servem como potenciais níveis de suporte
   - Uma reação a partir de um nível de Fibonacci pode ser um sinal para entrar numa posição longa
   - O rompimento em baixa de vários níveis de Fibonacci pode indicar enfraquecimento da tendência

3. **Numa Tendência de Baixa**:
   - Os níveis de Fibonacci servem como potenciais níveis de resistência
   - Uma rejeição num nível de Fibonacci pode ser um sinal para entrar numa posição curta
   - O rompimento em alta de vários níveis de Fibonacci pode indicar enfraquecimento da tendência

4. **Coincidência com Outros Níveis**:
   - Os níveis de Fibonacci são mais significativos quando coincidem com outros níveis importantes (máximos/mínimos anteriores, médias móveis, etc.)

5. **Traçado em diferentes períodos**:
   - Níveis de Fibonacci traçados em diferentes períodos podem criar zonas de concentração onde a probabilidade de inversão aumenta

![Gráfico do indicador FR](../../../../images/indicator_fibonacci_retracement.png)

## Ver Também

[PivotPoints](pivot_points.md)
