# CGO

**Oscilador do centro de gravidade (CGO)** é um indicador técnico desenvolvido por John Ehlers, baseado no conceito de centro de gravidade da física e aplicado à análise do movimento dos preços no mercado.

Para usar o indicador, deve ser usada a classe [CenterOfGravityOscillator](xref:StockSharp.Algo.Indicators.CenterOfGravityOscillator).

## Descrição

O Oscilador do centro de gravidade (CGO) é um indicador avançado que tenta identificar pontos de reversão do mercado tratando a série de preços como um sistema físico e determinando o seu "centro de gravidade". O indicador calcula onde está o "equilíbrio" nos movimentos atuais dos preços e usa esta informação para prever futuras alterações na direção da tendência.

CGO é particularmente útil para:
- Identificar potenciais pontos de reversão antes de aparecerem no gráfico de preços
- Revelar a força e a fraqueza da tendência atual
- Detetar divergências ocultas entre o preço e o indicador
- Criar sistemas de trading baseados em sinais avançados

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 10)

## Cálculo

O cálculo do Oscilador do centro de gravidade (CGO) baseia-se na fórmula:

```
CGO = - Sum(Price(i) * (i + 1)) / Sum(Price(i))
```

Onde:
- i - índice do valor do preço no período de 0 a (Length-1)
- Price(i) - preço (normalmente o preço de fecho) para o índice i correspondente
- Sum - soma de todos os valores no período Length

Nesta fórmula, cada preço é ponderado pela sua posição na série temporal e depois normalizado pela soma total dos preços. O sinal de menos antes da fórmula é adicionado para fazer o indicador subir quando o preço sobe, tornando-o mais intuitivo.

## Interpretação

- **Cruzamento da linha zero**: quando o CGO cruza a linha zero de baixo para cima, isto pode ser visto como um sinal altista. O cruzamento de cima para baixo pode indicar um sinal baixista.

- **Extremos do indicador**: quando o CGO atinge extremos (máximos ou mínimos), isto pode indicar uma potencial reversão da tendência.

- **Divergências**: 
  - Divergência altista: quando o preço forma um novo mínimo, mas o CGO não o confirma, formando um mínimo mais alto.
  - Divergência baixista: quando o preço atinge um novo máximo, mas o CGO forma um máximo mais baixo.

- **Movimento do indicador**: um movimento rápido do CGO numa direção pode indicar o início de uma nova tendência. Se o indicador se mover lentamente ou oscilar em torno da linha zero, isto pode indicar consolidação do mercado.

Como o CGO é um indicador avançado, os seus sinais aparecem frequentemente antes das alterações correspondentes no gráfico de preços, dando aos traders uma vantagem na tomada de decisões de trading.

![CGO](../../../../images/indicator_center_of_gravity_oscillator.png)

## Ver também

[SineWave](sine_wave.md)
[HarmonicOscillator](harmonic_oscillator.md)
[FisherTransform](ehlers_fisher_transform.md)
[RVI](rvi.md)
