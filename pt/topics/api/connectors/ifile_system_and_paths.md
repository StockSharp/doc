# IFileSystem e Paths.FileSystem

## Visão geral

`IFileSystem` é uma abstração do sistema de ficheiros utilizada em todo o StockSharp. Em vez de aceder diretamente a `System.IO.File` e `System.IO.Directory`, os componentes da plataforma trabalham através desta interface. Isto proporciona:

- **Testabilidade** -- a capacidade de substituir o sistema de ficheiros em testes unitários
- **Portabilidade** -- uma interface unificada para diferentes ambientes (disco local, armazenamento na cloud, memória)
- **Consistência** -- todos os componentes utilizam a mesma abordagem para trabalhar com ficheiros

A interface `IFileSystem` está definida no pacote `Ecng.Common` e fornece métodos para trabalhar com ficheiros e diretórios: criação, leitura, escrita, eliminação, verificação de existência e enumeração de ficheiros.

## Paths.FileSystem

A classe `Paths` (namespace `StockSharp.Configuration`) fornece a propriedade estática `FileSystem` -- a implementação predefinida de `IFileSystem`:

```csharp
public static class Paths
{
    // Standard file system (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` referencia `LocalFileSystem.Instance` -- um singleton que trabalha com o disco local através dos recursos padrão de `System.IO`.

## Principais métodos de IFileSystem

A interface `IFileSystem` inclui métodos para operações típicas:

| Método | Descrição |
|--------|-------------|
| `FileExists(path)` | Verificar se um ficheiro existe |
| `DirectoryExists(path)` | Verificar se um diretório existe |
| `CreateDirectory(path)` | Criar um diretório |
| `OpenRead(path)` | Abrir um ficheiro para leitura (`Stream`) |
| `OpenWrite(path)` | Abrir um ficheiro para escrita (`Stream`) |
| `MoveFile(src, dst)` | Mover um ficheiro |
| `DeleteFile(path)` | Eliminar um ficheiro |
| `EnumerateFiles(path, mask)` | Enumerar ficheiros num diretório |
| `WriteAllTextAsync(path, text)` | Escrever texto num ficheiro de forma assíncrona |

## Onde é utilizado

Praticamente todos os componentes StockSharp que trabalham com o sistema de ficheiros aceitam `IFileSystem` no respetivo construtor. Abaixo estão os casos mais comuns.

### LocalMarketDataDrive

Armazenamento de dados de mercado em disco local:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Forma recomendada -- passar IFileSystem explicitamente
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// Forma obsoleta (usa Paths.FileSystem internamente)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### CsvEntityRegistry

Registo de entidades (bolsas, instrumentos, carteiras) em formato CSV:

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### SnapshotRegistry

Registo de snapshots de dados de mercado:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### Serialização e desserialização

Métodos de extensão em `Paths` para trabalhar com configurações JSON:

```csharp
using StockSharp.Configuration;

var fs = Paths.FileSystem;

// Serializar objeto para um arquivo
settings.Serialize(fs, @"C:\config.json");

// Desserializar objeto de um arquivo
var loaded = @"C:\config.json".Deserialize<SettingsStorage>(fs);

// Desserialização assíncrona
var data = await @"C:\data.json".DeserializeAsync<MyData>(fs, cancellationToken);

// Verificar se um arquivo de configuração existe
if (@"C:\config.json".IsConfigExists(fs))
{
    // ...
}
```

### CandlePatternFileStorage

Armazenamento de padrões de candles:

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage, SecurityMappingStorage e outros

A maioria dos armazenamentos de mapeamento e identificadores também aceita `IFileSystem`:

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## Padrão típico de utilização

Em aplicações StockSharp, recomenda-se manter uma referência a `IFileSystem` e passá-la a todos os componentes:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Obter sistema de arquivos
var fs = Paths.FileSystem;

// Criar armazenamento de dados
var drive = new LocalMarketDataDrive(fs, Paths.StorageDir);

// Criar conector e configurar armazenamento
var connector = new Connector();
connector.Adapter.StorageSettings.Drive = drive;

// Carregar configurações de um arquivo
var configFile = Path.Combine(Paths.AppDataPath, "connector_config.json");

if (configFile.IsConfigExists(fs))
{
    var config = configFile.Deserialize<SettingsStorage>(fs);
    connector.Load(config);
}
```

## Overloads obsoletos

Muitas classes mantêm construtores sem `IFileSystem` por compatibilidade retroativa, mas estão marcados com o atributo `[Obsolete]`. Estes construtores utilizam `Paths.FileSystem` internamente:

```csharp
// Forma obsoleta
[Obsolete("Use IFileSystem overload.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// Forma recomendada
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

Recomenda-se utilizar sempre overloads com passagem explícita de `IFileSystem`, uma vez que os construtores obsoletos serão removidos em versões futuras.
