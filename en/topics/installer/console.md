# Console installer

The `Installer.Console` application is a cross-platform version of StockSharp Installer. It allows you to download, update and remove products without using the graphical interface. The tool runs on any OS where the [.NET 6](https://dotnet.microsoft.com/) runtime is available.

## Running

1. Install the .NET 6 SDK or runtime for your platform.
2. Download `StockSharp.Installer.Console.zip` from the [Download page](https://stocksharp.com/products/download/).
3. Unpack the archive and run the utility from the command line:
   
   ```bash
   dotnet StockSharp.Installer.Console.dll <Command> [product] [dir] [options]
   ```

`<Command>` is one of the following:

- `Install` – install a product.
- `Update` – update an installed product.
- `Repair` – repair existing installation.
- `Remove` – uninstall a product.
- `License` – show license for a product.
- `Licenses` – list available licenses.
- `HddId` – print hard drive identifier.
- `Products` – list available products.
- `Updates` – show available updates.
- `Installed` – list installed programs.
- `Sign` – sign a DLL file.

The optional `[product]` parameter is the product ID from the [Store](https://stocksharp.com/store/). You can find this ID on the product page—for example on the [Hydra server page](https://stocksharp.com/store/hydra-server/)—or by running `StockSharp.Installer.Console.exe Products -s hydra`. `[dir]` specifies the installation directory.

## Options

The utility accepts the following options:

- `-s`, `--search` – filter products by name.
- `-r`, `--run` – automatically run an application (for example, `StockSharp.Hydra.Server.exe`) after installation.
- `-c`, `--cache` – use the NuGet cache.
- `-f`, `--force` – force update check ignoring the configured interval.
- `-p`, `--pre` – allow installing pre-release versions.
- `-e`, `--noerror` – suppress any errors.
- `-b`, `--backup` – backup previous settings before repair or update.
- `-l`, `--clear` – clear the target directory before installation.
- `-t`, `--fw` – specify the target .NET framework.
- `-d`, `--data` – remove application data folder.
- `-i`, `--in` – DLL to sign.
- `-o`, `--out` – result DLL after signing.

Example command:

```bash
dotnet StockSharp.Installer.Console.dll Install 1269 /home/user/stocksharp -p -r StockSharp.Hydra.Server.exe
```

This installs product **1269** (used here just as an example) into the specified directory, allows pre-release builds and launches `StockSharp.Hydra.Server.exe` after completion.
