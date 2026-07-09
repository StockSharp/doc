# Importación automática

La tarea realiza la importación automática de datos bursátiles desde archivos del directorio especificado de acuerdo con la máscara de archivo especificada.

Para cada tipo de datos de mercado seleccionado, la plantilla se configura en la pestaña [Importación](../importing.md).

![hydra tasks import](../../../images/hydra_tasks_import.png)

En la parte inferior del panel, puede seleccionar los instrumentos por los que se importarán datos, así como el tipo de datos que se importará.

Para cada instrumento, puede especificar las siguientes propiedades de importación de datos:

![hydra tasks proper import](../../../images/hydra_tasks_proper_import.png)

**Import (auto)**

**Configuración**

- **Data type** - tipo de datos importados.
- **Filename** - ruta completa al archivo.
- **Data directory** - directorio de datos.
- **File mask** - máscara de archivo que se usa al escanear el directorio. Por ejemplo, candles\*.csv.
- **Subdirectories** - incluir subdirectorios.
- **Column separator** - separador de columnas. La tabulación se indica como TAB.
- **Indent from the beginning** - número de líneas que se omitirán desde el inicio del archivo (si contienen metainformación).
- **Time zone** - zona horaria.
- **Interval** - frecuencia de actualización de datos.
- **Extended information** - guardar los campos importados extendidos en el almacenamiento de información extendida.
- **Duplicates** - indica si los instrumentos duplicados deben actualizarse si ya existen.
- **Ignore without ID** - ignorar instrumentos sin identificador.

**General**

- **Header** - Converter.
- **Working hours** - configuración del horario de trabajo del mercado. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - intervalo de operación.
- **Data directory** - directorio de datos desde el que se recibirán los datos para la conversión.
- **Format** - formato de los datos convertidos: BIN\/CSV.
- **Max. errors** - número máximo de errores tras el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
- **Dependency** - tarea que debe ejecutarse antes de iniciar la actual.

**Logging**

- **Identifier** - identificador.
- **Logging level** - nivel de logging.
