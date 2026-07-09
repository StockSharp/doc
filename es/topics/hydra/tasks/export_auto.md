# Exportación automática

La tarea exporta datos bursátiles a varios formatos: Excel, xml, sql, bin, Json o txt.

![hydra tasks export](../../../images/hydra_tasks_export.png)

**Database**

- **Connection** - conexión a la base de datos. Se usa en caso de exportación mediante SQL.
- **Packet** - tamaño del paquete de datos transmitido. De forma predeterminada, el tamaño es de 50 elementos. Se usa en caso de exportación mediante SQL.
- **Uniqueness** - comprobar la unicidad de los datos en la base de datos. Afecta al rendimiento. Está habilitado de forma predeterminada. Se usa en caso de exportación mediante SQL.

> [!TIP]
> Al usar exportación mediante SQL, debe establecer los parámetros de la cadena de conexión.

**Nueva cadena de conexión**

![hydra tasks connstring](../../../images/hydra_tasks_connstring.png)

- **Provider** - configuración del proveedor.
- **Server** - dirección del servidor o ruta a la base de datos.
- **Database** - nombre de la base de datos. No se usa para SQLite.
- **Login** - login para acceder a la base de datos. No se usa para acceso anónimo.
- **Password** - contraseña para acceder a la base de datos. No se usa para acceso anónimo.
- **Windows** - usar la cuenta actual de Windows para conectarse a la base de datos.
- **Connection** - cadena de conexión preparada.

> [!TIP]
> Puede comprobar la conexión a la base de datos con el botón **Comprobar**.

**General**

- **Header** - Converter.
- **Working hours** - configuración del horario de trabajo del mercado. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - intervalo de operación.
- **Data directory** - directorio de datos desde el que se recibirán los datos para la conversión.
- **Format** - formato de los datos convertidos: BIN\/CSV.
- **Max. errors** - número máximo de errores tras el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
- **Dependency** - tarea que debe ejecutarse antes de iniciar la actual.

**CSV**

- **Templates** - plantillas para cada tipo de datos exportados.
- **Header** - encabezado en la primera línea. Si se pasa una cadena vacía, el encabezado no se añadirá al archivo.
- **Name format** - formato para escribir el nombre del archivo exportado.

**Export (auto)**

- **Type** - tipo de exportación (formato).
- **Start date** - fecha desde la que iniciar la exportación de datos.
- **Time offset** - desplazamiento de tiempo en días.
- **Export directory** - directorio donde se exportarán los datos.
- **Format** - formato de datos.
- **Split** - tipo de división.

**Logging**

- **Identifier** - identificador.
- **Logging level** - nivel de logging.

Consideremos un ejemplo de exportación automática:

1. Seleccione el instrumento.
2. Configure los datos de mercado que deben exportarse.![hydra tasks export 00](../../../images/hydra_tasks_export_00.png)
3. Establezca el período de exportación. Si está configurada la descarga de datos de mercado en tiempo real, puede omitir la fecha final del período. En este caso, los datos se exportarán en tiempo real, de acuerdo con el intervalo de trabajo (actualización de datos). ![hydra tasks export 01](../../../images/hydra_tasks_export_01.png)
4. Configure directorios. Intervalo de operación. Tipo de datos. Formato de datos.
5. Inicie la exportación.![hydra tasks export 02](../../../images/hydra_tasks_export_02.png)

Veamos los datos exportados.

![hydra tasks export 03](../../../images/hydra_tasks_export_03.png)

**Ver [tutorial en video](../videos/export_task.md)**
