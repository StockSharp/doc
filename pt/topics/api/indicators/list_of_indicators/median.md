# Mediana móvel

O indicador **Mediana móvel** calcula a mediana dos N valores mais recentes. Em comparação com as médias móveis, é menos
sensível a outliers e preserva alterações abruptas de preço, o que o torna útil em ambientes ruidosos.

Use a classe [Median](xref:StockSharp.Algo.Indicators.Median) para aceder ao indicador.

## Descrição

Um filtro de mediana ordena os preços dentro da janela móvel e selecciona o valor central. Como resultado:

- Picos ou quedas isoladas não distorcem a saída.
- O atraso é menor do que em muitos filtros de suavização.
- A forma do sinal permanece mais angular, ajudando a capturar inversões.

## Parâmetros

- **Length** — tamanho da janela usado para calcular a mediana. Janelas maiores proporcionam uma suavização mais forte, mas aumentam o atraso.

## Utilização

- Aplique a Mediana móvel como alternativa às médias móveis quando os dados de preço contêm ruído significativo.
- Cruzamentos entre o preço e a mediana podem ser tratados como sinais de alteração de tendência.
- Combine a mediana com outros filtros para extrair tendências mantendo saltos importantes do preço.

![Gráfico do indicador Mediana móvel](../../../../images/indicator_median.png)

## Ver também

[SMA](sma.md)
[EMA](ema.md)
[Média móvel suavizada](smoothed_ma.md)
