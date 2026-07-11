# MMI

**Índice de adversidade do mercado (MMI)** é um indicador técnico desenvolvido para determinar se o mercado está num estado tendencial ou lateral (caótico).

Para utilizar o indicador, é necessário usar a classe [MarketMeannessIndex](xref:StockSharp.Algo.Indicators.MarketMeannessIndex).

## Descrição

O Índice de adversidade do mercado (MMI) é uma ferramenta que ajuda os traders a determinar a natureza do mercado actual - se está tendencial ou lateral. O nome "Meanness" reflecte a ideia de que o mercado por vezes se comporta de forma "má" ou imprevisível para os traders, especialmente quando está em movimento lateral.

O MMI baseia-se na contagem do número de pares de valores de preço (normalmente preços de fecho) que não seguem um padrão linear simples, e no seu rácio relativamente ao número total de pares analisados. O indicador mede o "caos" ou a "aleatoriedade" do movimento do preço durante um período específico.

O índice oscila de 0 a 100:
- Valores baixos (normalmente abaixo de 50) indicam predominância de movimento tendencial
- Valores altos (normalmente acima de 50) indicam predominância de movimento lateral ou caótico

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 20)

## Cálculo

O cálculo do Índice de adversidade do mercado envolve os seguintes passos:

1. Criar um conjunto de pares consecutivos de preços de fecho (Close) dentro do período Length indicado.

2. Contar o número de pares "não sequenciais". Um par é considerado não sequencial se não seguir o padrão linear típico de uma tendência. Se dois pares consecutivos (P1, P2) e (P2, P3) tiverem direcções opostas (sinais de diferença diferentes), o par é considerado não sequencial.

3. Calcular o MMI como rácio percentual:
   ```
   MMI = (número de pares não sequenciais / número total de pares) * 100
   ```

Formalmente, isto pode ser representado como:
1. Para cada trio de preços consecutivos (Close[i-2], Close[i-1], Close[i]), verificar:
   - Se (Close[i-1] - Close[i-2]) * (Close[i] - Close[i-1]) < 0, o par é considerado não sequencial
   - Contar o número total desses pares

2. MMI = (Número de pares não sequenciais / (Length - 2)) * 100

## Interpretação

O Índice de adversidade do mercado pode ser interpretado da seguinte forma:

1. **Níveis do Indicador**:
   - MMI > 50: o mercado está num estado lateral ou caótico
   - MMI < 50: o mercado está num estado tendencial
   - Quanto mais próximo o MMI estiver de 100, mais caótico é o mercado
   - Quanto mais próximo o MMI estiver de 0, mais pronunciada é a tendência

2. **Aplicação em Estratégias de Negociação**:
   - Quando o MMI é alto (>50), usar estratégias orientadas para mercado lateral (por exemplo, negociação em intervalo, osciladores)
   - Quando o MMI é baixo (<50), usar estratégias de tendência (por exemplo, seguimento de tendência)

3. **Dinâmica das Alterações**:
   - A diminuição do MMI a partir de níveis altos pode sinalizar a formação de uma nova tendência
   - O aumento do MMI a partir de níveis baixos pode indicar conclusão da tendência e transição para consolidação

4. **Valores Extremos**:
   - Valores muito baixos (MMI < 20) podem indicar uma tendência forte, mas também potenciais condições de sobrecompra/sobrevenda
   - Valores muito altos (MMI > 80) indicam um mercado extremamente caótico onde é difícil aplicar qualquer estratégia

5. **Filtragem de Sinais**:
   - O MMI é frequentemente usado como filtro para outros indicadores:
     - Sinais de indicadores de tendência (MA, MACD) são mais fiáveis com MMI baixo
     - Sinais de osciladores (RSI, Estocástico) são mais fiáveis com MMI alto

6. **Combinação com Outros Indicadores**:
   - O MMI funciona bem em combinação com ADX (Average Directional Index)
   - MMI baixo e ADX alto confirmam uma tendência forte
   - MMI alto e ADX baixo confirmam um mercado lateral

7. **Períodos**:
   - O MMI pode ser usado em diferentes períodos para determinar o carácter do mercado
   - O MMI de longo prazo ajuda a determinar o estado primário do mercado
   - O MMI de curto prazo ajuda a escolher uma estratégia adequada para as condições actuais

![indicator_market_meanness_index](../../../../images/indicator_market_meanness_index.png)

## Ver Também

[ChoppinessIndex](choppiness_index.md)
[ADX](adx.md)
[VHF](vhf.md)
[BalanceOfPower](balance_of_power.md)
