# Índice

Con [Hydra](../../hydra.md), puede crear su propio índice.

En la pestaña **Común**, seleccione **Instrumentos** para que aparezca la pestaña **Todos los instrumentos**.

Antes de crear el **Índice**, compruebe qué datos de mercado están disponibles. Seleccione la ruta donde se almacenan los datos y revise secuencialmente los instrumentos que deben participar en el cálculo del índice. Si hay huecos, descargue los datos de mercado necesarios desde una fuente de datos compatible.

![Hydra comprobación de datos de futuros continuos](../../../images/hydragluingcheckdata.png)

Como ejemplo, consideremos el índice de relación de instrumentos AAPL@NYSE\/GOOG@NYSE.

1. El primer paso es crear el **Índice**. En la pestaña **Todos los instrumentos**, haga clic en **Crear instrumento \=\> Índice** ![hydra índice de instrumento 00](../../../images/hydra_index_sec_00.png).
2. Aparecerá la siguiente ventana: ![hydra índice de instrumento](../../../images/hydra_index_sec.png)
3. Para crear el instrumento **Índice**, especifique un nombre y añada la fórmula matemática para una combinación de varios instrumentos. Junto con los operadores matemáticos estándar, puede usar las siguientes funciones:
   - **abs(a)** - devuelve el valor absoluto de un número.
   - **acos(a)** - devuelve el ángulo cuyo coseno es igual al número especificado.
   - **asin(a)** - devuelve el ángulo cuyo seno es igual al número especificado.
   - **atan(a)** - devuelve el ángulo cuya tangente es igual al número especificado.
   - **ceiling(a)** - devuelve el entero más pequeño que es mayor o igual que el número especificado.
   - **cos(a)** - devuelve el coseno del ángulo especificado.
   - **exp(a)** - devuelve el valor de **e** elevado a la potencia especificada.
   - **floor(a)** - devuelve el entero más grande que es menor o igual que el número especificado.
   - **log(a)** - devuelve el logaritmo natural (base **e**) del número especificado.
   - **log10(a)** - devuelve el logaritmo en base 10 del número especificado.
   - **max (a, b)** - devuelve el mayor de dos números decimales.
   - **min(a, b)** - devuelve el menor de dos números decimales.
   - **pow(a, b)** - devuelve el número especificado elevado a la potencia especificada.
   - **sign(a)** - devuelve un entero que indica el signo del número especificado.
   - **sin(a)** - devuelve el seno del ángulo especificado.
   - **sqrt (a)** - devuelve la raíz cuadrada del número especificado.
   - **tan(a)** - devuelve la tangente del ángulo especificado.
   - **truncate(a)** - calcula la parte entera del número especificado.
4. Introduzca la operación matemática que se usará para calcular el índice. ![hydra índice de instrumento 01](../../../images/hydra_index_sec_01.png)
5. A continuación, haga clic en [Velas](../working_with_data/view_and_export/candles.md) en la pestaña **Común**, seleccione el instrumento **Índice** creado y el período de datos, establezca **Elemento compuesto** en el campo **Crear desde:** y luego haga clic en ![Hydra botón Buscar](../../../images/hydra_find.png). ![hydra índice de vela](../../../images/hydra_index_candle.png)

Los datos generados se pueden exportar a formatos Excel, XML o TXT. La exportación se realiza mediante la lista desplegable.

![Hydra exportación](../../../images/hydra_export.png)
