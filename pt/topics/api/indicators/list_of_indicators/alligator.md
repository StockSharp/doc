# Alligator

﻿# Alligator

O indicador **Alligator** é composto por um grupo de três médias móveis. Estas médias móveis têm períodos diferentes e também são deslocadas para a frente no gráfico.

Para usar o indicador, deve ser usada a classe [Alligator](xref:StockSharp.Algo.Indicators.Alligator).
##### Funcionalidades do indicador "Alligator"
  
O indicador é composto por três linhas de cores diferentes:
  
- Azul, denominada mandíbula, com um período de cálculo de 13 e um deslocamento de 8 barras. Quando está localizada abaixo da curva de preço, indica um potencial movimento ascendente do preço. Se a "mandíbula" subir acima dela, espera-se uma queda.

- Vermelha, chamada dentes do alligator, com um período de 8 e um deslocamento de 5 barras. Pertence às médias rápidas e mostra o comportamento do preço no intervalo horário.
  
- Verde, chamada lábios, com um período de 5 e um deslocamento de 3 barras. Analisa tendências de mercado de doze minutos.

Os valores são apresentados para o indicador clássico e, nas definições, é sempre possível especificar os seus próprios parâmetros.

![Gráfico do indicador Alligator](../../../../images/indicatoralligator.png)

## Ver também

[ADX](adx.md)
