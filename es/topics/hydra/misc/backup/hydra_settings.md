# Configuración de Hydra

A continuación se describe cómo crear y configurar una tarea de copia de seguridad.

1. Para crear una tarea, haga clic en el botón **Añadir tareas...**; en la ventana que se abre, seleccione el elemento **Copia de seguridad** y haga clic en el botón **Aceptar**.![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. Después debe configurar la tarea.![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Copia de seguridad**
   - **Servicio** - dirección del servicio.
   - **Dirección** - dirección de la región. Es la dirección de la región especificada en la configuración del bucket. Consulte [Regiones y endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region).
   - **Almacenamiento** - nombre del bucket.
   - **Inicio de sesión** - inicio de sesión. **Access Key ID**.
   - **Contraseña** - contraseña. **Secret Access Key**.
   - **Fecha inicial** - fecha desde la que iniciar la copia de seguridad.
   - **Desfase temporal** - desplazamiento en días desde la fecha actual.

   **General**
   - **Encabezado** - título de la tarea.
   - **Horario de trabajo** - configuración del horario de trabajo del mercado. ![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **Intervalo de operación** - intervalo de operación.
   - **Directorio de datos** - directorio de datos desde el que se recibirán los datos para la conversión.
   - **Formato** - formato de los datos convertidos: BIN\/CSV.
   - **Máx. errores** - número máximo de errores tras el cual la tarea se detendrá. De forma predeterminada, 0: se ignora el número de errores.
   - **Dependencia** - tarea que debe ejecutarse antes de iniciar la actual.

   **Registro**
   - **Identificador** - identificador.
   - **Nivel de registro** - nivel de registro.
3. Después de configurar la tarea, añada los instrumentos que deben guardarse en el almacenamiento de copia de seguridad y haga clic en el botón **Iniciar**.

## Contenido recomendado

[Creación y configuración de una cuenta](setup.md)
