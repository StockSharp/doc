# EFT

**transformação de Fisher de Ehlers (EFT)** é um indicador técnico desenvolvido por John Ehlers que usa a transformação estatística de Fisher para converter dados de preço numa forma normalmente distribuída.

Para usar o indicador, deve ser usada a classe [EhlersFisherTransform](xref:StockSharp.Algo.Indicators.EhlersFisherTransform).

## Descrição

O transformação de Fisher de Ehlers baseia-se no conceito de que os preços de mercado não têm uma distribuição normal (gaussiana). Em vez disso, demonstram frequentemente distribuições assimétricas. O indicador aplica uma fórmula matemática de transformação de Fisher para converter estas distribuições assimétricas em valores normalmente distribuídos.

Esta transformação torna os movimentos extremos do preço mais visíveis e ajuda a identificar com maior clareza pontos de reversão do mercado. Quando a transformação de Fisher é aplicada, os valores de pico aumentam acentuadamente, tornando os extremos do comportamento do mercado mais óbvios.

EFT é particularmente útil para:
- Determinar potenciais pontos de reversão do mercado
- Identificar condições de sobrecompra e sobrevenda
- Detetar divergências ocultas entre o preço e o indicador
- Gerar sinais de entrada e saída mais precisos

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 10)

## Cálculo

O cálculo do transformação de Fisher de Ehlers envolve vários passos:

1. Transformar dados de preço em valores entre -1 e +1 (normalmente usando ranking de preço normalizado ou outro oscilador):
   ```
   Valor = (2 * ((Price - Min) / (Max - Min))) - 1
   ```
   onde Min e Max são os preços mínimo e máximo ao longo do período Length.

2. Aplicar a transformação de Fisher:
   ```
   Se Valor >= 0.999, então Valor = 0.999
   Se Valor <= -0.999, então Valor = -0.999

   Fisher = 0.5 * ln((1 + Valor) / (1 - Valor))
   ```
   onde ln é o logaritmo natural.

3. Suavizar para reduzir ruído:
   ```
   EFT = EMA(Fisher, Period)
   ```
   onde EMA é a média móvel exponencial.

## Interpretação

O transformação de Fisher de Ehlers pode ser interpretado da seguinte forma:

1. **Valores extremos**:
   - Valores acima de +2 indicam frequentemente condições de sobrecompra no mercado
   - Valores abaixo de -2 indicam frequentemente condições de sobrevenda no mercado

2. **Cruzamentos da linha zero**:
   - O cruzamento da linha zero de baixo para cima pode ser visto como um sinal altista
   - O cruzamento da linha zero de cima para baixo pode ser visto como um sinal baixista

3. **Reversão do indicador**:
   - A reversão do indicador a partir de valores extremos precede frequentemente a reversão do preço

4. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o EFT forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o EFT forma um máximo mais baixo

5. **Inclinação da linha do indicador**:
   - Inclinação acentuada para cima indica forte momentum ascendente
   - Inclinação acentuada para baixo indica forte momentum descendente

O transformação de Fisher de Ehlers difere de muitos outros osciladores porque pode atingir valores extremos e permanecer aí durante algum tempo sem necessariamente reverter imediatamente. Isto torna-o útil para identificar movimentos de tendência fortes.

![Gráfico do indicador EFT](../../../../images/indicator_ehlers_fisher_transform.png)

## Ver também

[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[SineWave](sine_wave.md)
[HarmonicOscillator](harmonic_oscillator.md)
[RSI](rsi.md)
