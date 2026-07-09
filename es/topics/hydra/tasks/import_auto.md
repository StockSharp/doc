# Importación automática

La tarea realiza la importación automática de datos bursátiles desde archivos del directorio especificado de acuerdo con la máscara de archivo especificada.

Para cada tipo de datos de mercado seleccionado, la plantilla se configura en la pestaña [Importación](../importing.md).

![hydra tasks import](../../../images/hydra_tasks_import.png)

En la parte inferior del panel, puede seleccionar los instrumentos por los que se importarán datos, así como el tipo de datos que se importará.

Para cada instrumento, puede especificar las siguientes propiedades de importación de datos:

![hydra tasks proper import](../../../images/hydra_tasks_proper_import.png)

**Importación (automática)**

**Configuración**

- **Tipo de datos** - tipo de datos importados.
- **Nombre de archivo** - ruta completa al archivo.
- **Directorio de datos** - directorio de datos.
- **Máscara de archivo** - máscara de archivo que se usa al escanear el directorio. Por ejemplo, candles\*.csv.
- **Subdirectorios** - incluir subdirectorios.
- **Separador de columnas** - separador de columnas. La tabulación se indica como TAB.
- **Sangría desde el inicio** - número de líneas que se omitirán desde el inicio del archivo (si contienen metainformación).
- **Zona horaria** - zona horaria.
- **Intervalo** - frecuencia de actualización de datos.
- **Información extendida** - guardar los campos importados extendidos en el almacenamiento de información extendida.
- **Duplicados** - indica si los instrumentos duplicados deben actualizarse si ya existen.
- **Ignorar sin ID** - ignorar instrumentos sin identificador.

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
