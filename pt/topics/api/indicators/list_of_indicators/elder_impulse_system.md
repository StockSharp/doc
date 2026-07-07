# EIS

**Elder Impulse System (EIS)** é um indicador técnico desenvolvido pelo Dr. Alexander Elder que combina um indicador de tendência e um oscilador de momentum para determinar a direção e a força do movimento do mercado.

Para usar o indicador, deve ser usada a classe [ElderImpulseSystem](xref:StockSharp.Algo.Indicators.ElderImpulseSystem).

## Descrição

O Elder Impulse System (EIS) é uma ferramenta simples mas poderosa para visualizar o momentum do mercado. Combina dois indicadores:
1. **Exponential Moving Average (EMA)** - para determinar a direção da tendência
2. **MACD Histogram** - para medir a força e o momentum do movimento do preço

O EIS classifica cada vela no gráfico de preços numa de três categorias (normalmente indicadas por cores diferentes):
- **Verde (forte impulso bullish)** - quando ambos os indicadores estão a subir
- **Vermelho (forte impulso bearish)** - quando ambos os indicadores estão a descer
- **Azul ou neutro (sem impulso claro)** - quando os indicadores se movem em direções opostas

EIS é particularmente útil para:
- Determinar rapidamente de forma visual a direção e a força da tendência
- Identificar pontos de entrada e saída na direção da tendência principal
- Identificar potenciais pontos de reversão
- Filtrar sinais falsos

## Cálculo

O cálculo do Elder Impulse System envolve os seguintes passos:

1. Calcular a média móvel exponencial de 13 períodos (EMA):
   ```
   EMA = EMA(Close, 13)
   ```

2. Calcular o MACD Histogram (valores padrão: 12, 26, 9):
   ```
   MACD Line = EMA(Close, 12) - EMA(Close, 26)
   Signal Line = EMA(MACD Line, 9)
   MACD Histogram = MACD Line - Signal Line
   ```

3. Determinar a classificação por cor para a vela atual:
   ```
   If EMA[current] > EMA[previous] AND MACD Histogram[current] > MACD Histogram[previous], then Green (Bullish Impulse)
   If EMA[current] < EMA[previous] AND MACD Histogram[current] < MACD Histogram[previous], then Red (Bearish Impulse)
   Otherwise Blue (No Impulse)
   ```

## Interpretação

O Elder Impulse System é interpretado da seguinte forma:

1. **Velas verdes (forte impulso bullish)**:
   - Indicam forte momentum ascendente
   - Melhor momento para comprar ou manter posições long
   - Uma série de velas verdes indica uma forte tendência ascendente

2. **Velas vermelhas (forte impulso bearish)**:
   - Indicam forte momentum descendente
   - Melhor momento para vender ou manter posições short
   - Uma série de velas vermelhas indica uma forte tendência descendente

3. **Velas azuis (sem impulso claro)**:
   - Indicam incerteza ou consolidação
   - Sinalizam uma possível desaceleração ou reversão da tendência
   - Aparecem frequentemente durante períodos de consolidação ou antes de uma alteração de tendência

4. **Estratégias de trading**:
   - Comprar quando as velas mudam de azul para verde
   - Vender quando as velas mudam de azul para vermelho
   - Fechar posições long quando as velas mudam de verde para qualquer outra cor
   - Fechar posições short quando as velas mudam de vermelho para qualquer outra cor

5. **Confirmação de tendência**:
   - Uma sequência de velas verdes confirma uma tendência ascendente
   - Uma sequência de velas vermelhas confirma uma tendência descendente
   - A alternância de cores indica uma tendência lateral ou incerteza

![indicator_elder_impulse_system](../../../../images/indicator_elder_impulse_system.png)

## Ver também

[EMA](ema.md)
[MACD](macd.md)
[MACDHistogram](macd_histogram.md)
[ForceIndex](force_index.md)
