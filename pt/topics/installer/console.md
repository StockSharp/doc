# Instalador de consola

A aplicação `Installer.Console` é uma versão multiplataforma do StockSharp Installer. Permite descarregar, atualizar e remover produtos sem usar a interface gráfica. A ferramenta é executada em qualquer SO onde o runtime [.NET 6](https://dotnet.microsoft.com/) esteja disponível.

## Execução

1. Instale o SDK ou runtime .NET 6 para a sua plataforma.
2. Descarregue `StockSharp.Installer.Console.zip` a partir da [página de transferência](https://stocksharp.com/products/download/).
3. Descompacte o arquivo e execute o utilitário a partir da linha de comandos:
   
   ```bash
   dotnet StockSharp.Installer.Console.dll <Command> [product] [dir] [options]
   ```

`<Command>` é um dos seguintes:

- `Install` - instalar um produto.
- `Update` - atualizar um produto instalado.
- `Repair` - reparar uma instalação existente.
- `Remove` - desinstalar um produto.
- `License` - mostrar a licença de um produto.
- `Licenses` - listar as licenças disponíveis.
- `HddId` - imprimir o identificador do disco rígido.
- `Products` - listar os produtos disponíveis.
- `Updates` - mostrar as atualizações disponíveis.
- `Installed` - listar os programas instalados.
- `Sign` - assinar um ficheiro DLL.

O parâmetro opcional `[product]` é o ID do produto na [loja](https://stocksharp.com/store/). Pode encontrar este ID na página do produto, por exemplo na [página do Hydra Server](https://stocksharp.com/store/hydra-server/), ou executando `StockSharp.Installer.Console.exe Products -s hydra`. `[dir]` especifica o diretório de instalação.

## Opções

O utilitário aceita as seguintes opções:

- `-s`, `--search` - filtrar produtos por nome.
- `-r`, `--run` - executar automaticamente uma aplicação (por exemplo, `StockSharp.Hydra.Server.exe`) após a instalação.
- `-c`, `--cache` - usar a cache NuGet.
- `-f`, `--force` - forçar a verificação de atualizações ignorando o intervalo configurado.
- `-p`, `--pre` - permitir a instalação de versões preliminares.
- `-e`, `--noerror` - suprimir quaisquer erros.
- `-b`, `--backup` - efetuar uma cópia de segurança das definições anteriores antes de reparar ou atualizar.
- `-l`, `--clear` - limpar o diretório de destino antes da instalação.
- `-t`, `--fw` - especificar o framework .NET de destino.
- `-d`, `--data` - remover a pasta de dados da aplicação.
- `-i`, `--in` - DLL a assinar.
- `-o`, `--out` - DLL resultante após a assinatura.

Comando de exemplo:

```bash
dotnet StockSharp.Installer.Console.dll Install 1269 /home/user/stocksharp -p -r StockSharp.Hydra.Server.exe
```

Isto instala o produto **1269** (usado aqui apenas como exemplo) no diretório especificado, permite builds preliminares e inicia `StockSharp.Hydra.Server.exe` após a conclusão.

## Assinatura

O comando `Sign` assina digitalmente a DLL de um robô. Use-o ao distribuir o seu próprio robô baseado na API, para que todos os destinatários, mesmo no [plano gratuito](https://stocksharp.com/pricing/), possam executá-lo.

Compile previamente o robô e adicione o atributo `[assembly: ProductId(9)]` do namespace `StockSharp.Configuration`. Assine a DLL (por exemplo, `MyRobot.dll`) em vez do ficheiro `.exe`.

Exemplo:

```bash
StockSharp.Installer.Console.exe Sign -i "MyRobot.dll"
```

A recompilação do projeto remove a assinatura, por isso assine o assembly no fim do desenvolvimento, quando não se espera mais nenhuma compilação.
