# T3MA

**Média móvel T3 (T3MA)** é um tipo avançado de média móvel desenvolvido por Tim Tillson. A T3 representa uma média móvel exponencial (EMA) suavizada três vezes com um fator de volume, tornando-a mais suave e menos propensa a sinais falsos em comparação com médias móveis tradicionais.

Para utilizar o indicador, é necessário usar a classe [T3MovingAverage](xref:StockSharp.Algo.Indicators.T3MovingAverage).

## Descrição

A média móvel T3 foi desenvolvida para eliminar as desvantagens das médias móveis tradicionais, como o atraso e os sinais falsos. Através de múltiplas suavizações e de um fator de volume ajustável, a T3MA fornece uma curva mais suave que acompanha a tendência do preço com maior precisão.

Principais vantagens da T3MA:
- Menor atraso em comparação com médias móveis comuns
- Curva mais suave com menos sinais falsos
- Adaptabilidade a várias condições de mercado devido ao fator de volume ajustável

A T3MA pode ser utilizada para:
- Determinar a direção da tendência
- Encontrar pontos de entrada e saída quando o preço cruza a linha do indicador
- Construir sistemas de negociação baseados em cruzamentos de várias T3MA com períodos diferentes

## Parâmetros

- **Fator de volume** - fator de volume que determina o grau de suavização (normalmente um valor entre 0 e 1; o valor recomendado é 0,7).
- **Período** - período de cálculo, semelhante ao período em médias móveis comuns.

## Cálculo

O cálculo da média móvel T3 é efetuado em vários passos:

1. Calcular seis médias móveis exponenciais consecutivas com o mesmo período:
   ```
   EMA1 = EMA(Price, Length)
   EMA2 = EMA(EMA1, Length)
   EMA3 = EMA(EMA2, Length)
   EMA4 = EMA(EMA3, Length)
   EMA5 = EMA(EMA4, Length)
   EMA6 = EMA(EMA5, Length)
   ```

2. Calcular a T3 com base nas EMAs obtidas e no fator de volume:
   ```
   c1 = -VolumeFactor^3
   c2 = 3 * VolumeFactor^2 + 3 * VolumeFactor^3
   c3 = -6 * VolumeFactor^2 - 3 * VolumeFactor - 3 * VolumeFactor^3
   c4 = 1 + 3 * VolumeFactor + VolumeFactor^3 + 3 * VolumeFactor^2
   
   T3 = c1 * EMA6 + c2 * EMA5 + c3 * EMA4 + c4 * EMA3
   ```

Quando VolumeFactor = 0, a T3 torna-se equivalente à EMA3 (EMA tripla). Quando VolumeFactor = 1, a T3 fica maximamente suavizada.

![Gráfico do indicador T3MA](../../../../images/indicator_t3_moving_average.png)

## Ver também

[EMA](ema.md)
[DEMA](dema.md)
[TEMA](tema.md)
