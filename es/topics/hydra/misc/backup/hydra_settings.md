# Configuración de Hydra

A continuación se describe cómo crear y configurar una tarea de copia de seguridad.

1. Para crear una tarea, haga clic en el botón **Add tasks...**; en la ventana que se abre, seleccione el elemento **Backup** y haga clic en el botón **OK**.![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. Después debe configurar la tarea.![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Backup**
   - **Service** - dirección del servicio.
  - **Address** - dirección de la región. Es la dirección de la región especificada en la configuración del bucket. Consulte [Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region).
   - **Storage** - nombre del bucket.
   - **Login** - login. Access Key ID.
   - **Password** - contraseña. **Secret Access Key**.
   - **Start date** - fecha desde la que iniciar la copia de seguridad.
   - **Time offset** - desplazamiento en días desde la fecha actual.

   **General**
   - **Header** - Converter.
   - **Working hours** - configuración del horario de trabajo del mercado. ![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **Interval of operation** - intervalo de operación.
  - **Data directory** - directorio de datos desde el que se recibirán los datos para la conversión.
   - **Format** - formato de los datos convertidos: BIN\/CSV.
   - **Max. errors** - número máximo de errores tras el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
   - **Dependency** - tarea que debe ejecutarse antes de iniciar la actual.

   **Logging**
   - **Identifier** - identificador.
   - **Logging level** - nivel de logging.
3. Después de configurar la tarea, añada los instrumentos que deben guardarse en el almacenamiento de copia de seguridad y haga clic en el botón **Iniciar**.

## Contenido recomendado

[Creación y configuración de una cuenta](setup.md)
