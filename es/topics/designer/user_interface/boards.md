# Mercados de negociación

En el panel **Editor de mercados**, puede crear **Mercados** y **bolsas**, y ver o configurar los existentes.

![Captura de Mercados de negociación](../../../images/designer_boards.png)

En [S#](../../api.md), los instrumentos de distintas fuentes usan un identificador unificado que consta del código del instrumento y el código del mercado. La sintaxis es [**código del instrumento**]@[código del mercado]. Por ejemplo, para las acciones **AAPL** de la bolsa **NASDAQ**, el identificador es **AAPL@NASDAQ**. Cada instrumento está vinculado a un mercado específico en el que se negocia. Sin embargo, el instrumento puede negociarse en distintos mercados. En este caso, los códigos de mercado serán diferentes. Para cada mercado puede configurar un horario de trabajo con días laborables y fines de semana.

La bolsa puede tener varios mercados con diferentes condiciones de negociación (hora de sesión, comisión, etc.). Pero cada mercado está vinculado a una bolsa específica. Por lo tanto, en el **Editor de mercados**, cuando selecciona un mercado, la información sobre la bolsa cambiará automáticamente a la bolsa en la que se encuentra el mercado. Para cada bolsa puede establecer el código de la bolsa, país, nombres en ruso e inglés.
