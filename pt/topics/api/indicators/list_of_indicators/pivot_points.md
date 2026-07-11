# PP

**pontos pivô (PP)** é um indicador técnico que usa os preços máximo, mínimo e de fecho anteriores para determinar potenciais níveis de suporte e resistência para o período de negociação atual.

Para usar o indicador, é necessário usar a classe [PivotPoints](xref:StockSharp.Algo.Indicators.PivotPoints).

## Descrição

pontos pivô (pontos pivô) são um dos métodos mais antigos e mais usados para identificar níveis-chave do mercado. O indicador calcula o ponto pivô central (PP) e vários níveis de suporte (S1, S2, S3) e resistência (R1, R2, R3) com base nos dados do período anterior.

Originalmente, os pontos pivô eram usados por traders nos pregões das bolsas para determinar níveis-chave para o dia de negociação atual com base nos dados do dia anterior. Hoje, este método foi adaptado a vários períodos - desde intradiário até mensal.

A ideia principal dos pontos pivô é que o mercado tende a reagir a estes níveis pré-calculados, usando-os como pontos de reversão ou zonas onde pode ocorrer consolidação. Os traders usam estes níveis para tomar decisões sobre entrada e saída do mercado, bem como para definir níveis-alvo e stop-losses.

## Cálculo

Existem vários métodos para calcular pontos pivô, incluindo padrão, Fibonacci, Woodie, Camarilla e DeMark. Abaixo está o método de cálculo padrão:

1. Calcular o ponto pivô principal (PP):
   ```
   PP = (High + Low + Close) / 3
   ```

2. Calcular os primeiros níveis de resistência (R1) e suporte (S1):
   ```
   R1 = (2 * PP) - Low
   S1 = (2 * PP) - High
   ```

3. Calcular os segundos níveis de resistência (R2) e suporte (S2):
   ```
   R2 = PP + (High - Low)
   S2 = PP - (High - Low)
   ```

4. Calcular os terceiros níveis de resistência (R3) e suporte (S3):
   ```
   R3 = High + 2 * (PP - Low)
   S3 = Low - 2 * (High - PP)
   ```

Onde:
- High - preço mais alto do período anterior
- Low - preço mais baixo do período anterior
- Close - preço de fecho do período anterior

## Interpretação

pontos pivô podem ser interpretados da seguinte forma:

1. **Ponto pivô principal (PP)**:
   - O PP serve como referência principal para determinar o sentimento geral do mercado
   - Se o preço estiver acima do PP, indica um sentimento altista
   - Se o preço estiver abaixo do PP, indica um sentimento baixista
   - O PP também pode servir como nível de suporte ou resistência

2. **Níveis de resistência (R1, R2, R3)**:
   - Estes níveis representam potenciais zonas de resistência num mercado altista
   - Uma rutura de um nível pode levar à continuação do movimento até ao nível seguinte
   - Um ressalto a partir de um nível pode levar a uma reversão descendente

3. **Níveis de suporte (S1, S2, S3)**:
   - Estes níveis representam potenciais zonas de suporte num mercado baixista
   - Uma rutura de um nível pode levar à continuação do movimento até ao nível seguinte
   - Um ressalto a partir de um nível pode levar a uma reversão ascendente

4. **Estratégias de trading**:
   - **Negociação por ressalto**: Entrar numa posição quando há ressalto a partir de um nível de suporte ou resistência
   - **Negociação de rutura**: Entrar numa posição após uma rutura confirmada de um nível
   - **Negociação em intervalo**: Comprar em níveis de suporte e vender em níveis de resistência
   - **Definição de alvos**: Usar o nível seguinte como alvo de realização de lucro
   - **Colocação de stop-loss**: Colocar stop-losses para lá dos níveis correspondentes

5. **Combinação com outros indicadores**:
   - pontos pivô são frequentemente usados em conjunto com outros indicadores técnicos para confirmar sinais
   - São particularmente eficazes quando combinados com indicadores de momentum (RSI, Estocástico) e indicadores de tendência (MA, MACD)

6. **Períodos**:
   - pontos pivô diários são calculados com base no dia de negociação anterior
   - pontos pivô semanais são calculados com base na semana anterior
   - pontos pivô mensais são calculados com base no mês anterior
   - A seleção do período depende do estilo de trading e do horizonte temporal

![PP](../../../../images/indicator_pivot_points.png)

## Ver também

[FibonacciRetracement](fibonacci_retracement.md)
