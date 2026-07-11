# GAPO

**índice de intervalo de Gopalakrishnan (GAPO)** é um indicador técnico desenvolvido por Tushar Gopalakrishnan para medir a volatilidade do mercado usando uma escala logarítmica.

Para utilizar o indicador, é necessário usar a classe [GopalakrishnanRangeIndex](xref:StockSharp.Algo.Indicators.GopalakrishnanRangeIndex).

## Descrição

O índice de intervalo de Gopalakrishnan (GAPO) é um indicador de volatilidade que usa uma escala logarítmica para medir o intervalo global de preços ao longo de um período específico. Foi desenvolvido por Tushar Gopalakrishnan e apresentado na revista "Technical Analysis of Stocks & Commodities".

O GAPO avalia movimentos extremos do mercado medindo o rácio logarítmico entre os preços máximo e mínimo ao longo de um determinado período. Esta abordagem permite ao indicador refletir com maior precisão o aumento da volatilidade, especialmente durante períodos de movimentos bruscos do preço.

O indicador GAPO é particularmente útil para:
- Identificar períodos de alta e baixa volatilidade
- Detetar potenciais pontos de inversão após movimentos extremos
- Ajustar parâmetros de outros indicadores baseados em volatilidade
- Adaptar estratégias de negociação às condições atuais do mercado

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 10)

## Cálculo

O cálculo do índice de intervalo de Gopalakrishnan é bastante simples:

```
GAPO = log(N) * log(Highest High - Lowest Low)
```

Onde:
- log - logaritmo natural
- N - número de períodos (Length)
- Highest High - máximo mais alto ao longo do período Length
- Lowest Low - mínimo mais baixo ao longo do período Length

## Interpretação

O índice de intervalo de Gopalakrishnan pode ser interpretado da seguinte forma:

1. **Valores Absolutos**:
   - Valores elevados do GAPO indicam períodos de alta volatilidade
   - Valores baixos do GAPO indicam períodos de baixa volatilidade
   - Valores extremamente elevados podem indicar possível extensão excessiva do mercado e potencial inversão

2. **Tendências do GAPO**:
   - Valores crescentes do GAPO indicam aumento da volatilidade
   - Valores decrescentes do GAPO indicam diminuição da volatilidade
   - Um salto acentuado do GAPO pode sinalizar o início de um novo movimento tendencial

3. **Níveis Relativos**:
   - Comparar o valor atual do GAPO com os seus níveis históricos permite avaliar a volatilidade relativa
   - Valores acima do percentil 95 do intervalo histórico podem indicar volatilidade extrema
   - Valores abaixo do percentil 5 do intervalo histórico podem indicar volatilidade invulgarmente baixa

4. **Estratégias de Negociação**:
   - Durante períodos de alta volatilidade (valores elevados do GAPO), pode ser adequado aumentar os tamanhos de stop-loss e objetivo de lucro
   - Durante períodos de baixa volatilidade (valores baixos do GAPO), estratégias de negociação em intervalo podem ser mais adequadas
   - Valores extremos do GAPO podem ser usados como indicadores contrários para encontrar pontos de inversão

5. **Combinação com Outros Indicadores**:
   - O GAPO pode ser usado para filtrar sinais de outros indicadores
   - Durante períodos de alta volatilidade, os sinais de indicadores de tendência podem ser mais fiáveis
   - Durante períodos de baixa volatilidade, os sinais de osciladores podem ser mais eficazes

![indicator_gopalakrishnan_range_index](../../../../images/indicator_gopalakrishnan_range_index.png)

## Ver Também

[ATR](atr.md)
[ChoppinessIndex](choppiness_index.md)
[Intervalo verdadeiro](true_range.md)
