# LRSI

**RSI de Laguerre (LRSI)** é um indicador técnico baseado nos princípios matemáticos do filtro de Laguerre, desenvolvido por John Ehlers como uma versão avançada do índice de força relativa (RSI) tradicional.

Para utilizar o indicador, é necessário usar a classe [LaguerreRSI](xref:StockSharp.Algo.Indicators.LaguerreRSI).

## Descrição

O RSI de Laguerre (LRSI) é um oscilador inovador que usa a matemática do filtro de Laguerre para criar um indicador mais sensível e com menos atraso em comparação com o RSI tradicional. John Ehlers desenvolveu este indicador para resolver o problema do atraso de sinal inerente a muitos indicadores técnicos.

O LRSI combina princípios de polinómios de Laguerre com o conceito do índice de força relativa. Isto permite que o indicador responda mais rapidamente a alterações de tendência e forme sinais de negociação mais claros. Tal como o RSI tradicional, o LRSI oscila entre 0 e 1 (ou 0 a 100 quando multiplicado por 100), mas tem uma estrutura menos ruidosa e viragens mais distintas.

A principal vantagem do LRSI é a sua capacidade de identificar rapidamente alterações de tendência mantendo a estabilidade do sinal. Isto torna-o particularmente útil para negociação de curto prazo e para determinar pontos de entrada e saída.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Gamma** - coeficiente de filtragem (valor predefinido: 0,4, intervalo de 0,1 a 0,9)

O parâmetro Gamma determina o grau de filtragem e afecta a sensibilidade do indicador. Valores Gamma mais baixos resultam num indicador mais suave e menos sensível, enquanto valores mais altos tornam o indicador mais sensível a alterações de preço, mas potencialmente mais ruidoso.

## Cálculo

O cálculo do RSI de Laguerre envolve vários passos:

1. Inicializar quatro valores do filtro de Laguerre (L0, L1, L2, L3) na primeira execução:
   ```
   L0 = L1 = L2 = L3 = 0
   ```

2. Actualizar os valores do filtro para cada novo preço:
   ```
   L0_new = (1 - Gamma) * Price + Gamma * L0_old
   L1_new = -Gamma * L0_new + L0_old + Gamma * L1_old
   L2_new = -Gamma * L1_new + L1_old + Gamma * L2_old
   L3_new = -Gamma * L2_new + L2_old + Gamma * L3_old
   ```

3. Calcular a "multiplicação cumulativa" dos valores filtrados:
   ```
   CU = (L0_new + L1_new + L2_new + L3_new) / 4
   ```

4. Separar em componentes "up" e "down":
   ```
   Se CU >= CU_old, então:
     UP = CU - CU_old
     DN = 0
   Otherwise:
     UP = 0
     DN = CU_old - CU
   ```

5. Cálculo final do LRSI:
   ```
   LRSI = UP / (UP + DN)
   ```

   Se (UP + DN) for zero, o LRSI é definido como o valor anterior.

Onde:
- Price - preço de entrada (normalmente o preço de fecho)
- Gamma - parâmetro de filtragem
- CU - "multiplicação cumulativa"

## Interpretação

O RSI de Laguerre é interpretado de forma semelhante ao RSI tradicional, mas com a sua sensibilidade acrescida:

1. **Níveis de Sobrecompra e Sobrevenda**:
   - Valores acima de 0,8 (ou 80) são normalmente considerados sobrecompra
   - Valores abaixo de 0,2 (ou 20) são normalmente considerados sobrevenda
   - Devido às características do filtro de Laguerre, estes níveis podem ser ajustados com base na volatilidade do mercado

2. **Cruzamentos da Linha Central**:
   - Cruzar o nível 0,5 (ou 50) de baixo para cima pode ser visto como um sinal altista
   - Cruzar o nível 0,5 (ou 50) de cima para baixo pode ser visto como um sinal baixista

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o LRSI forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o LRSI forma um máximo mais baixo

4. **Ressaltos dos Extremos**:
   - A inversão do LRSI a partir de níveis de sobrecompra ou sobrevenda pode servir como sinal de entrada no mercado

5. **Confirmação da Tendência**:
   - Valores do LRSI acima de 0,5 confirmam uma tendência ascendente
   - Valores do LRSI abaixo de 0,5 confirmam uma tendência descendente

6. **Ajuste do Parâmetro Gamma**:
   - Para sinais mais rápidos - aumentar Gamma (mais próximo de 0,9)
   - Para sinais mais suaves - diminuir Gamma (mais próximo de 0,1)

![indicator_laguerre_rsi](../../../../images/indicator_laguerre_rsi.png)

## Ver Também

[RSI](rsi.md)
[AdaptiveLaguerreFilter](adaptive_laguerre_filter.md)
[ConnorsRSI](connors_rsi.md)

