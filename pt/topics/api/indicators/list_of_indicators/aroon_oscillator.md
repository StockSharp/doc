# oscilador Aroon

﻿# oscilador Aroon

O **oscilador Aroon** mede a diferença entre as linhas Aroon ascendente e Aroon descendente. Destaca que lado do mercado é dominante e quão forte é a tendência atual.

Use a classe [AroonOscillator](xref:StockSharp.Algo.Indicators.AroonOscillator) para trabalhar com este indicador.

## Descrição

O oscilador oscila entre −100 e +100:

- valores positivos mostram que Aroon ascendente está acima de Aroon descendente e que o mercado é dominado por compradores;
- valores negativos indicam que Aroon descendente está a liderar e que os vendedores estão no controlo;
- leituras em torno de zero refletem equilíbrio ou consolidação.

Quanto mais o valor se afasta de zero, mais forte é o movimento direcional.

## Parâmetros

- **Length** — período usado para os cálculos Aroon subjacentes. Valores maiores fornecem leituras mais suaves com resposta mais lenta.

## Cálculo

1. Calcule as séries Aroon ascendente e Aroon descendente com o `Length` selecionado.
2. Subtraia as duas linhas:
   `oscilador Aroon = Aroon ascendente − Aroon descendente`.

## Interpretação

- **oscilador Aroon > 0** — dominância compradora.
- **oscilador Aroon < 0** — dominância vendedora.
- **Cruzamento da linha zero** — potencial alteração da tendência predominante.
- **Valores extremos** — tendência direcional forte, frequentemente usada como filtro direcional.

O oscilador é frequentemente analisado em conjunto com o indicador base [Aroon](aroon.md) para observar tanto os níveis absolutos como a sua diferença.

![Gráfico do indicador oscilador Aroon](../../../../images/indicator_aroon_oscillator.png)

## Ver também

[Aroon](aroon.md)
[ADX](adx.md)
[DMI](dmi.md)
