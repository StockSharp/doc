# CM

**Composite Momentum (CM)** é um indicador que combina vários métodos de medição do momentum do preço para obter sinais mais fiáveis sobre a força e a direção da tendência.

Para usar o indicador, deve ser usada a classe [CompositeMomentum](xref:StockSharp.Algo.Indicators.CompositeMomentum).

## Descrição

O indicador Composite Momentum (CM) é uma ferramenta abrangente que integra vários aspetos do movimento do preço, incluindo a taxa de variação do preço, a força relativa e outros componentes de momentum. Através desta abordagem combinada, o CM fornece uma imagem mais completa do momentum atual do mercado em comparação com indicadores tradicionais de momentum unidimensionais.

CM é eficaz para:
- Determinar a força da tendência atual
- Identificar potenciais pontos de reversão
- Detetar divergências entre o preço e o momentum
- Filtrar sinais falsos de outros indicadores

Composite Momentum é particularmente útil em mercados voláteis, onde os indicadores tradicionais de momentum podem gerar numerosos sinais falsos.

## Cálculo

O cálculo do Composite Momentum envolve várias etapas e componentes:

1. Calcular componentes de momentum:
   - Alteração do preço relativamente a períodos anteriores
   - Relação entre máximos e mínimos recentes
   - Análise de volume que acompanha o movimento do preço

2. Normalizar cada componente para os colocar em escalas comparáveis.

3. Somar os componentes de forma ponderada para obter o valor final de CM.

O valor final de CM é um oscilador que pode flutuar em zonas positivas e negativas:
- Valores positivos indicam momentum ascendente
- Valores negativos indicam momentum descendente
- A magnitude do valor (valor absoluto) indica a força do momentum

## Interpretação

- **Cruzamento da linha zero**:
  - A transição da zona negativa para a positiva pode ser vista como um sinal altista
  - A transição da zona positiva para a negativa pode ser vista como um sinal baixista

- **Valores extremos**:
  - Valores positivos muito elevados podem indicar condições de sobrecompra no mercado
  - Valores negativos muito baixos podem indicar condições de sobrevenda no mercado

- **Divergências**:
  - Divergência altista: o preço forma um novo mínimo, enquanto o CM forma um mínimo mais alto
  - Divergência baixista: o preço forma um novo máximo, enquanto o CM forma um máximo mais baixo

- **Confirmação da tendência**:
  - Valores consistentemente positivos de CM confirmam a força de uma tendência ascendente
  - Valores consistentemente negativos de CM confirmam a força de uma tendência descendente

- **Perda de momentum**:
  - A diminuição do valor absoluto de CM na direção da tendência pode sinalizar perda de momentum e uma potencial reversão

![indicator_composite_momentum](../../../../images/indicator_composite_momentum.png)

## Ver também

[Momentum](momentum.md)
[RoC](roc.md)
[RSI](rsi.md)
[MACD](macd.md)
