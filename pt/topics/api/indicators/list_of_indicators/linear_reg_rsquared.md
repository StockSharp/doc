# R-quadrado

**R-quadrado na regressão linear (R-quadrado da regressão linear)** é um indicador técnico que mede quão bem a regressão linear aproxima os dados de preço e determina a força da tendência do mercado.

Para utilizar o indicador, é necessário usar a classe [LinearRegRSquared](xref:StockSharp.Algo.Indicators.LinearRegRSquared).

## Descrição

R-quadrado na regressão linear (R²) é uma medida estatística usada para avaliar o grau de correspondência entre os dados de preço e uma linha de regressão linear traçada através desses dados. No contexto da análise técnica, R² mostra quão bem o movimento actual do preço corresponde a uma tendência linear.

Os valores de R² variam de 0 a 1 (ou de 0% a 100%):
- Um valor próximo de 1 (100%) indica que os preços se alinham muito bem ao longo da linha de tendência, significando uma tendência forte
- Um valor próximo de 0 indica ausência de uma tendência linear e é característico de mercados laterais, caóticos ou cíclicos

O indicador ajuda os traders a distinguir entre períodos de tendências fortes e períodos de consolidação ou movimentos laterais, permitindo escolher uma estratégia de negociação adequada.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período para o cálculo da regressão linear (valor predefinido: 14)

## Cálculo

O cálculo de R-quadrado na regressão linear envolve os seguintes passos:

1. Construir uma linha de regressão linear para os dados de preço durante o período Length:
   ```
   y = a + b*x
   ```
   Onde:
   - y - preço (variável dependente)
   - x - número sequencial do período (variável independente)
   - a - termo livre (intercepção no eixo y)
   - b - coeficiente de inclinação

2. Calcular a soma dos desvios quadráticos da regressão (SSE):
   ```
   SSE = Sum((Actual Price - Predicted Price)^2)
   ```
   Onde:
   - Actual Price - preço real
   - Predicted Price - preço previsto pela equação de regressão

3. Calcular a soma total dos quadrados (SST):
   ```
   SST = Sum((Actual Price - Average Price)^2)
   ```
   Onde Average Price é o preço médio durante o período Length

4. Calcular R²:
   ```
   R² = 1 - (SSE / SST)
   ```

## Interpretação

R-quadrado na regressão linear pode ser interpretado da seguinte forma:

1. **Avaliação da Força da Tendência**:
   - Valores acima de 0,7 (70%) indicam uma tendência forte
   - Valores entre 0,3 e 0,7 (30-70%) indicam uma tendência moderada
   - Valores abaixo de 0,3 (30%) indicam uma tendência fraca ou ausência de tendência

2. **Selecção da Estratégia de Negociação**:
   - Com valores altos de R² (tendência forte), as estratégias de seguimento de tendência são eficazes
   - Com valores baixos de R² (movimento lateral), as estratégias de negociação em intervalo são eficazes

3. **Procura de Pontos de Transição**:
   - O aumento de R² pode sinalizar a formação de uma nova tendência
   - A diminuição de R² pode sinalizar enfraquecimento da tendência e possível consolidação ou inversão

4. **Filtragem de Sinais**:
   - Sinais de indicadores de tendência são mais fiáveis com valores altos de R²
   - Sinais de osciladores são mais fiáveis com valores baixos de R²

5. **Combinação com Outros Indicadores**:
   - R² é frequentemente usado para determinar o modo do mercado, após o qual são aplicados indicadores adequados
   - Por exemplo, usar médias móveis com R² alto, e o oscilador estocástico com R² baixo

6. **Avaliação da Previsibilidade do Mercado**:
   - Valores altos de R² indicam movimento de preço mais previsível no curto prazo
   - Valores baixos de R² indicam movimento mais caótico e imprevisível

7. **Períodos**:
   - R² pode produzir resultados diferentes em períodos diferentes
   - Comparar R² entre períodos pode fornecer informação adicional sobre a estrutura do mercado

![indicator_linear_reg_r_squared](../../../../images/indicator_linear_reg_rsquared.png)

## Ver Também

[LinearRegression](lrc.md)
[StandardError](standard_error.md)
[ChoppinessIndex](choppiness_index.md)
