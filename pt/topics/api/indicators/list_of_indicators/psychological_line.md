# PSY

**Psychological Line (PSY)** é um indicador técnico que mede a proporção de períodos de subida (velas, barras) relativamente ao número total de períodos num intervalo de tempo especificado.

Para utilizar o indicador, é necessário usar a classe [PsychologicalLine](xref:StockSharp.Algo.Indicators.PsychologicalLine).

## Descrição

A Psychological Line (PSY) é um indicador simples, mas eficaz, que reflete o sentimento do mercado ao calcular a percentagem de períodos em que o preço subiu face ao número total de períodos considerados. O indicador baseia-se no pressuposto de que a psicologia do mercado e o sentimento dos investidores desempenham um papel crucial nos movimentos dos preços.

O PSY é um oscilador com valores entre 0 e 100, em que:
- Um valor de 100 significa que o preço subiu em todos os períodos considerados
- Um valor de 0 significa que o preço caiu em todos os períodos considerados
- Um valor de 50 significa um número igual de períodos de subida e de queda

O indicador PSY ajuda a determinar se o mercado está em condição de sobrecompra ou sobrevenda e pode antecipar potenciais inversões de tendência.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 12-14)

## Cálculo

O cálculo da Psychological Line é muito simples:

```
PSY = (Número de períodos de subida ao longo de Length períodos / Length) * 100
```

Onde:
- Um período de subida é definido como um período em que o preço de fecho é superior ao preço de fecho do período anterior
- Length - número de períodos considerados

## Interpretação

A Psychological Line pode ser interpretada da seguinte forma:

1. **Níveis de Sobrecompra e Sobrevenda**:
   - Valores acima de 70-80 indicam condições de sobrecompra no mercado (demasiados períodos foram de subida)
   - Valores abaixo de 20-30 indicam condições de sobrevenda no mercado (demasiados períodos foram de queda)
   - Valores extremos precedem frequentemente inversões de tendência

2. **Linha central (50)**:
   - Cruzar o nível 50 de baixo para cima pode ser visto como um sinal altista
   - Cruzar o nível 50 de cima para baixo pode ser visto como um sinal baixista
   - Movimento sustentado acima de 50 indica domínio dos compradores
   - Movimento sustentado abaixo de 50 indica domínio dos vendedores

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o PSY forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o PSY forma um máximo mais baixo

4. **Ressaltos a partir de níveis extremos**:
   - Uma inversão do PSY a partir da zona de sobrecompra pode sinalizar uma potencial inversão baixista
   - Uma inversão do PSY a partir da zona de sobrevenda pode sinalizar uma potencial inversão altista

5. **Análise de tendência**:
   - Numa tendência ascendente forte, o PSY permanece frequentemente acima de 50, com ressaltos periódicos a partir da zona de sobrecompra
   - Numa tendência descendente forte, o PSY permanece frequentemente abaixo de 50, com ressaltos periódicos a partir da zona de sobrevenda

6. **Ajuste do parâmetro Length**:
   - Períodos mais curtos (por exemplo, 5-8) tornam o PSY mais sensível e adequado para negociação de curto prazo
   - Períodos mais longos (por exemplo, 20-25) tornam o PSY mais suave e adequado para negociação de longo prazo

7. **Combinação com outros indicadores**:
   - O PSY é frequentemente utilizado em combinação com outros indicadores para confirmar sinais
   - É particularmente útil quando combinado com indicadores de tendência e indicadores de volume

![indicator_psychological_line](../../../../images/indicator_psychological_line.png)

## Ver também

[RSI](rsi.md)
[Oscilador estocástico](stochastic_oscillator.md)
[UltimateOscillator](uo.md)
[MomentumOscillator](momentum.md)
