# Aroon Oscillator

﻿# Aroon Oscillator

O **Aroon Oscillator** mede a diferença entre as linhas Aroon Up e Aroon Down. Destaca que lado do mercado é dominante e quão forte é a tendência atual.

Use a classe [AroonOscillator](xref:StockSharp.Algo.Indicators.AroonOscillator) para trabalhar com este indicador.

## Descrição

O oscilador oscila entre −100 e +100:

- valores positivos mostram que Aroon Up está acima de Aroon Down e que o mercado é dominado por compradores;
- valores negativos indicam que Aroon Down está a liderar e que os vendedores estão no controlo;
- leituras em torno de zero refletem equilíbrio ou consolidação.

Quanto mais o valor se afasta de zero, mais forte é o movimento direcional.

## Parâmetros

- **Length** — período usado para os cálculos Aroon subjacentes. Valores maiores fornecem leituras mais suaves com resposta mais lenta.

## Cálculo

1. Calcule as séries Aroon Up e Aroon Down com o `Length` selecionado.
2. Subtraia as duas linhas:  
   `Aroon Oscillator = Aroon Up − Aroon Down`.

## Interpretação

- **Aroon Oscillator > 0** — dominância compradora.
- **Aroon Oscillator < 0** — dominância vendedora.
- **Cruzamento da linha zero** — potencial alteração da tendência predominante.
- **Valores extremos** — tendência direcional forte, frequentemente usada como filtro direcional.

O oscilador é frequentemente analisado em conjunto com o indicador base [Aroon](aroon.md) para observar tanto os níveis absolutos como a sua diferença.

![indicator_aroon_oscillator](../../../../images/indicator_aroon_oscillator.png)

## Ver também

[Aroon](aroon.md)
[ADX](adx.md)
[DMI](dmi.md)
