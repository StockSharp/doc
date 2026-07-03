# Conexión de un cliente Hydra

En modo servidor, es posible conectar otro programa Hydra, que actuará como cliente y descargará datos para sí mismo. A diferencia de [Conexión mediante protocolo FIX](fix_fast_connectivity.md), los datos se transmiten en forma de archivos en formato StockSharp. Esto hace que la fuente sea adecuada para transferir un gran volumen de datos históricos.

Para la conexión se usa una fuente especial:

![hydra tasks server](../../../images/hydratasksserver_1.png)

**Configuración**

![hydra tasks server](../../../images/hydratasksserver_2.png)

- **Address** - dirección del servidor Hydra.
- **Login** - login (obligatorio si el servidor requiere autorización).
- **Password** - contraseña (obligatoria si el servidor requiere autorización).
- **Time Offset** - desplazamiento de tiempo en días desde la fecha actual, necesario para evitar descargar datos incompletos de la sesión de negociación actual.
- **Weekends** - indica si se deben descargar datos de fines de semana.

**Main**

- **Title** - nombre de la tarea.
- **Working Hours** - configuración del funcionamiento de la plataforma.
- **Interval of Operation** - intervalo de operación.
- **Data Directory** - directorio con datos donde se guardarán los archivos finales en formato [S#](../../api.md).
- **Format** - formato de datos: BIN/CSV.
- **Max. Errors** - número máximo de errores al alcanzarse el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
- **Dependency** - tarea que debe completarse antes de iniciar la actual.

**Logging**

- **Identifier** - identificador.
- **Logging Level** - nivel de logging.
