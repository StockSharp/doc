# EWO

**Elliot Wave Oscillator (EWO)** é um indicador técnico baseado na Teoria das Ondas de Elliott que ajuda os traders a determinar a estrutura das ondas e potenciais pontos de inversão do mercado.

Para utilizar o indicador, é necessário usar a classe [ElliotWaveOscillator](xref:StockSharp.Algo.Indicators.ElliotWaveOscillator).

## Descrição

O Elliot Wave Oscillator (EWO) foi desenvolvido para ajudar os traders a aplicar a Teoria das Ondas de Elliott na análise de mercado. A Teoria das Ondas de Elliott assume que os mercados se movem em ciclos previsíveis compostos por cinco ondas na direção da tendência (ondas impulsivas) e três ondas contra a tendência (ondas corretivas).

O EWO baseia-se na diferença entre médias móveis rápida e lenta e foi concebido para identificar ondas impulsivas e corretivas segundo a teoria de Elliott. Ajuda a determinar quando o mercado está numa fase impulsiva ou corretiva e sugere potenciais pontos de inversão.

O Elliot Wave Oscillator é particularmente útil para:
- Identificar a estrutura atual das ondas de Elliott
- Determinar potenciais finais de ondas impulsivas e corretivas
- Confirmar a análise manual de ondas
- Prever potenciais pontos de inversão

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período da média móvel curta (valor predefinido: 5)
- **LongPeriod** - período da média móvel longa (valor predefinido: 35)

## Cálculo

O cálculo do Elliot Wave Oscillator é bastante simples:

```
EWO = EMA(Close, ShortPeriod) - EMA(Close, LongPeriod)
```

Onde:
- EMA - média móvel exponencial
- Close - preço de fecho
- ShortPeriod - período curto (normalmente 5)
- LongPeriod - período longo (normalmente 35)

## Interpretação

O Elliot Wave Oscillator pode ser interpretado da seguinte forma:

1. **Valores Positivos e Negativos**:
   - Valores positivos (EWO acima de zero) indicam que a EMA curta está acima da EMA longa, correspondendo frequentemente a uma tendência de alta ou a uma onda impulsiva ascendente
   - Valores negativos (EWO abaixo de zero) indicam que a EMA curta está abaixo da EMA longa, correspondendo frequentemente a uma tendência de baixa ou a uma onda impulsiva descendente

2. **Cruzamentos da Linha Zero**:
   - O cruzamento da linha zero de baixo para cima pode sinalizar o início de uma nova onda impulsiva ascendente
   - O cruzamento da linha zero de cima para baixo pode sinalizar o início de uma nova onda impulsiva descendente

3. **Extremos do Oscilador**:
   - Picos e fundos do oscilador podem corresponder ao fim de ondas impulsivas
   - Uma fase corretiva segue frequentemente o alcance de um extremo

4. **Divergências**:
   - Divergência de alta (o preço forma um novo mínimo, enquanto o EWO forma um mínimo mais alto) pode indicar um potencial fim de uma onda impulsiva descendente
   - Divergência de baixa (o preço forma um novo máximo, enquanto o EWO forma um máximo mais baixo) pode indicar um potencial fim de uma onda impulsiva ascendente

5. **Estrutura de Ondas**:
   - Nas ondas impulsivas (ondas 1, 3, 5), o EWO normalmente mostra valores fortes na direção da tendência
   - Nas ondas corretivas (ondas 2, 4, A, B, C), o EWO normalmente mostra valores mais fracos ou move-se numa direção oposta à tendência principal

6. **Identificação da Onda 3**:
   - A onda 3, que normalmente é a onda impulsiva mais forte na Teoria das Ondas de Elliott, é frequentemente caracterizada pelos valores mais elevados do EWO

![indicator_elliot_wave_oscillator](../../../../images/indicator_elliot_wave_oscillator.png)

## Ver Também

[EMA](ema.md)
[MACD](macd.md)
[ZigZag](zigzag.md)
[WaveTrendOscillator](wave_trend_oscillator.md)

