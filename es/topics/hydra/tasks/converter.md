# Convertidor

La tarea convierte datos bursátiles. Por ejemplo, de registros de órdenes a ticks o de ticks a velas, etc.

![hydra tasks converter](../../../images/hydra_tasks_converter.png)

**Convertidor**

- **Convertidor** - convertidor.
- **From** - qué tipo de datos se convertirá.
- **Data format** - formato de los datos convertidos.
- **Fecha inicial** - fecha desde la que iniciar la conversión de datos.
- **Desfase temporal** - desplazamiento de tiempo en días desde la fecha en que se inició la tarea. Esto evita convertir un día incompleto. Si está configurada la conversión de datos en tiempo real, el intervalo de actualización puede dejar el día actual solo parcialmente convertido. Use el desplazamiento de tiempo para evitarlo.
- **Where** - directorio de datos donde se guardarán los datos convertidos.

**Libros de órdenes**

- **Intervalo** - intervalo de generación del libro de órdenes.
- **Depth** - profundidad máxima de generación del libro de órdenes.
- **Registro de órdenes** - cómo construir libros de órdenes a partir del registro de órdenes.

  Cada bolsa tiene su propio formato de **registro de órdenes**; el programa [Hydra](../../hydra.md) admite tres formatos:
  - **By default** - se usa en la mayoría de los casos.
  - **ITCH** - se usa para el protocolo ITCH (bolsas: LSE y Nasdaq).

**General**

- **Encabezado** - Converter.
- **Horario de trabajo** - configuración del horario de trabajo del mercado. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Intervalo de operación** - intervalo de operación.
- **Directorio de datos** - directorio de datos desde el que se recibirán los datos para la conversión.
- **Formato** - formato de los datos convertidos: BIN\/CSV.
- **Máx. errores** - número máximo de errores tras el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
- **Dependencia** - tarea que debe ejecutarse antes de iniciar la actual.

**Registro**

- **Identificador** - identificador.
- **Nivel de registro** - nivel de logging.

Consideremos un ejemplo de conversión de datos.

1. Vaya a la tarea **Convertidor**. ![hydra tasks converter 00](../../../images/hydra_tasks_converter_00.png)
2. Seleccione el instrumento y, en la ventana que aparece, establezca el tipo de datos que debemos recibir durante la conversión, así como el tipo de datos desde el que debemos convertir. Por ejemplo, necesita convertir Ticks en velas con marco temporal de 15 minutos.

   > [!TIP]
> ¡IMPORTANTE\! El período de datos solicitado debe coincidir con el período disponible para la conversión; de lo contrario, los datos no se convertirán. En la configuración, especifique el formato correcto de datos de origen para que coincida con el formato de los datos convertidos.
3. Especifique los directorios necesarios. Desplazamiento de tiempo. Intervalo de operación.
4. Inicie la conversión.![hydra tasks converter 01](../../../images/hydra_tasks_converter_01.png)

Se puede ver que los datos se han convertido. [Revisemos](../working_with_data/view_and_export.md) los datos resultantes.

![hydra tasks converter 02](../../../images/hydra_tasks_converter_02.png)

Esta función es similar a [obtener los datos de mercado necesarios](../working_with_data/any_market_data_types.md) desde otro tipo de datos.

**Ver [tutorial en video](../videos/converter_task.md)**
