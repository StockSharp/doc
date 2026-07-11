# Conexión de un cliente Hydra

En modo servidor, es posible conectar otro programa Hydra, que actuará como cliente y descargará datos para sí mismo. A diferencia de [Conexión mediante protocolo FIX](fix_fast_connectivity.md), los datos se transmiten en forma de archivos en formato StockSharp. Esto hace que la fuente sea adecuada para transferir un gran volumen de datos históricos.

Para la conexión se usa una fuente especial:

![Hydra tarea del servidor (1)](../../../images/hydratasksserver_1.png)

**Configuración**

![Hydra tarea del servidor (2)](../../../images/hydratasksserver_2.png)

- **Dirección** - dirección del servidor Hydra.
- **Inicio de sesión** - inicio de sesión (obligatorio si el servidor requiere autorización).
- **Contraseña** - contraseña (obligatoria si el servidor requiere autorización).
- **Desfase temporal** - desplazamiento de tiempo en días desde la fecha actual, necesario para evitar descargar datos incompletos de la sesión de negociación actual.
- **Fines de semana** - indica si se deben descargar datos de fines de semana.

**Principal**

- **Título** - nombre de la tarea.
- **Horario de trabajo** - configuración del funcionamiento de la plataforma.
- **Intervalo de operación** - intervalo de operación.
- **Directorio de datos** - directorio con datos donde se guardarán los archivos finales en formato [S#](../../api.md).
- **Formato** - formato de datos: BIN/CSV.
- **Máx. errores** - número máximo de errores al alcanzarse el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
- **Dependencia** - tarea que debe completarse antes de iniciar la actual.

**Registro**

- **Identificador** - identificador.
- **Nivel de registro** - nivel de registro.
