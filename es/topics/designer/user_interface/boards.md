# Mercados de negociación

En el panel **Editor de mercados**, puede crear **Mercados** y **Bolsas**, y ver o configurar los existentes.

![Captura de Mercados de negociación](../../../images/designer_boards.png)

En [S#](../../api.md), los instrumentos de distintas fuentes usan un identificador unificado que consta del código del instrumento y el código del board. La sintaxis es [**código del instrumento**]@[código del board]. Por ejemplo, para las acciones **AAPL** de la bolsa **NASDAQ**, el identificador es **AAPL@NASDAQ**. Cada instrumento está vinculado a un board específico en el que se negocia. Sin embargo, el instrumento puede negociarse en distintos boards. En este caso, los códigos de board serán diferentes. Para cada board puede configurar un horario de trabajo con días laborables y fines de semana.

El exchange puede tener varios boards con diferentes condiciones de negociación (hora de sesión, comisión, etc.). Pero cada board está vinculado a un exchange específico. Por lo tanto, en el **Editor de mercados**, cuando selecciona un board, la información sobre el exchange cambiará automáticamente al exchange en el que se encuentra el board. Para cada exchange puede establecer el código del exchange, país, nombres en ruso e inglés.
