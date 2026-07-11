# TMF

**fluxo monetário de Twiggs (TMF)** é um indicador de volume desenvolvido por Colin Twiggs como uma versão melhorada do indicador fluxo monetário de Chaikin. O TMF é mais sensível a alterações no sentimento do mercado e tem menos sinais falsos.

Para usar o indicador, é necessário usar a classe [TwiggsMoneyFlow](xref:StockSharp.Algo.Indicators.TwiggsMoneyFlow).

## Descrição

O fluxo monetário de Twiggs analisa a relação entre preço e volume para determinar a direção do fluxo de dinheiro para dentro ou para fora do mercado. Ao contrário dos indicadores de volume tradicionais, o TMF elimina ruído normalizando os valores entre -1 e +1.

Características principais do TMF:
- Valores positivos indicam entrada de dinheiro no instrumento (sentimento altista)
- Valores negativos indicam saída de dinheiro do instrumento (sentimento baixista)
- Um valor de 0 mostra equilíbrio entre oferta e procura

O indicador é útil para:
- Confirmar a tendência atual ou identificar fraqueza da tendência
- Detetar divergências entre preço e fluxo de dinheiro
- Identificar potenciais pontos de inversão do mercado

## Parâmetros

- **Length** - período de cálculo da média móvel exponencial, usando tipicamente um valor de 21.

## Cálculo

O cálculo do fluxo monetário de Twiggs é efetuado em vários passos:

1. Calcular o intervalo verdadeiro:
   ```
   TR = Max(High - Low, |High - fecho anterior|, |Low - fecho anterior|)
   ```

2. Determinar o volume de fluxo monetário de Twiggs (TMFV):
   ```
   TMFV = Volume * ((Close - Low - (High - Close)) / TR)
   ```
   Quando (High - Low = 0), TMFV = 0

3. Calcular a média móvel exponencial de TMFV e volume:
   ```
   EMA_TMFV = EMA(TMFV, Length)
   EMA_Volume = EMA(Volume, Length)
   ```

4. Valor final do TMF:
   ```
   TMF = EMA_TMFV / EMA_Volume
   ```

Os valores do TMF variam de -1 (sinal fortemente baixista) a +1 (sinal fortemente altista).

![Gráfico do indicador TMF](../../../../images/indicator_twiggs_money_flow.png)

## Ver também

[ADL](accumulation_distribution_line.md)
[Índice de fluxo monetário](money_flow_index.md)
[OBV](on_balance_volume.md)
