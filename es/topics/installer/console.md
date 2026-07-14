# Instalador de consola

La aplicación `Installer.Console` es una versión multiplataforma de StockSharp Installer. Permite descargar, actualizar y eliminar productos sin usar la interfaz gráfica. La herramienta se ejecuta en cualquier SO donde esté disponible el runtime [.NET 6](https://dotnet.microsoft.com/).

## Ejecución

1. Instale el SDK o runtime de .NET 6 para su plataforma.
2. Descargue `StockSharp.Installer.Console.zip` desde la [página de descarga](https://stocksharp.com/es/products/download/).
3. Descomprima el archivo y ejecute la utilidad desde la línea de comandos:

   ```bash
   dotnet StockSharp.Installer.Console.dll <Command> [product] [dir] [options]
   ```

`<Command>` es uno de los siguientes:

- `Install` – instalar un producto.
- `Update` – actualizar un producto instalado.
- `Repair` – reparar una instalación existente.
- `Remove` – desinstalar un producto.
- `License` – mostrar la licencia de un producto.
- `Licenses` – listar licencias disponibles.
- `HddId` – imprimir el identificador del disco duro.
- `Products` – listar productos disponibles.
- `Updates` – mostrar actualizaciones disponibles.
- `Installed` – listar programas instalados.
- `Sign` – firmar un archivo DLL.

El parámetro opcional `[product]` es el ID del producto en la [tienda](https://stocksharp.com/es/store/). Puede encontrar este ID en la página del producto —por ejemplo, en la [página de Hydra Server](https://stocksharp.com/es/store/hydra-server/)— o ejecutando `StockSharp.Installer.Console.exe Products -s hydra`. `[dir]` especifica el directorio de instalación.

## Opciones

La utilidad acepta las siguientes opciones:

- `-s`, `--search` – filtrar productos por nombre.
- `-r`, `--run` – ejecutar automáticamente una aplicación (por ejemplo, `StockSharp.Hydra.Server.exe`) después de la instalación.
- `-c`, `--cache` – usar la caché de NuGet.
- `-f`, `--force` – forzar la comprobación de actualizaciones ignorando el intervalo configurado.
- `-p`, `--pre` – permitir instalar versiones preliminares.
- `-e`, `--noerror` – suprimir cualquier error.
- `-b`, `--backup` – hacer copia de seguridad de la configuración anterior antes de reparar o actualizar.
- `-l`, `--clear` – limpiar el directorio de destino antes de instalar.
- `-t`, `--fw` – especificar el framework .NET de destino.
- `-d`, `--data` – eliminar la carpeta de datos de la aplicación.
- `-i`, `--in` – DLL que se debe firmar.
- `-o`, `--out` – DLL resultante después de la firma.

Comando de ejemplo:

```bash
dotnet StockSharp.Installer.Console.dll Install 1269 /home/user/stocksharp -p -r StockSharp.Hydra.Server.exe
```

Esto instala el producto **1269** (usado aquí solo como ejemplo) en el directorio especificado, permite builds preliminares y lanza `StockSharp.Hydra.Server.exe` al finalizar.

## Firma

El comando `Sign` firma digitalmente una DLL de robot. Úselo al distribuir su propio robot basado en API para que todos los destinatarios, incluso en el [plan gratuito](https://stocksharp.com/es/pricing/), puedan ejecutarlo.

Compile el robot previamente y añada el atributo `[assembly: ProductId(9)]` desde el namespace `StockSharp.Configuration`. Firme la DLL (por ejemplo, `MyRobot.dll`) en lugar del archivo `.exe`.

Ejemplo:

```bash
StockSharp.Installer.Console.exe Sign -i "MyRobot.dll"
```

Reconstruir el proyecto elimina la firma, por lo que debe firmar el ensamblado al final del desarrollo, cuando no se espere más compilación.
