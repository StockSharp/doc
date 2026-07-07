# Drives de Armazenamento

Os drives de armazenamento no StockSharp são responsáveis pela colocação física dos dados de mercado — num disco local ou num servidor remoto. A interface base [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) define o contrato comum para todas as implementações.

## IMarketDataDrive — Interface Base

A interface [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) fornece as seguintes capacidades principais:

- **Path** — caminho para o armazenamento de dados.
- **GetAvailableSecuritiesAsync()** — obter uma lista de todos os instrumentos disponíveis no armazenamento.
- **GetAvailableDataTypesAsync()** — obter uma lista de tipos de dados disponíveis para um instrumento específico.
- **GetStorageDrive()** — obter um drive de armazenamento para um instrumento e tipo de dados específicos.
- **VerifyAsync()** — verificar a integridade do armazenamento.
- **LookupSecuritiesAsync()** — pesquisar instrumentos por critérios especificados.

## LocalMarketDataDrive — Armazenamento Local em Ficheiros

A classe [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) é a implementação principal de drive que armazena dados no disco local no sistema de ficheiros.

### Funcionalidades Principais

- **Sistema de ficheiros** — os dados são organizados numa estrutura hierárquica de directórios por instrumentos e datas.
- **Sistema de índice** — a classe interna `Index` fornece acesso rápido aos dados sem analisar o sistema de ficheiros. Os ficheiros de índice são armazenados no formato `{instrument_path}{file_name}Dates2.bin`.
- **Segurança de threads** — o acesso aos dados é protegido por mecanismos de bloqueio para funcionamento correcto em aplicações multithread.
- **Construção de índice** — o método `BuildIndexAsync()` permite reconstruir índices para melhorar o desempenho após operações de dados em massa.

### Exemplo de Utilização

```cs
// Criar um drive local com um caminho especificado
var localDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));

// Utilizar com o registo de armazenamento
var storageRegistry = new StorageRegistry
{
    DefaultDrive = localDrive,
};

// Obter a lista de instrumentos disponíveis
await foreach (var secId in localDrive.GetAvailableSecuritiesAsync())
{
    Console.WriteLine(secId);
}
```

## RemoteMarketDataDrive — Armazenamento Remoto

A classe [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) permite ligar a um servidor Hydra remoto para aceder a dados de mercado através da rede.

### Definições de Ligação

- **Address** — endereço do servidor remoto. O valor predefinido é `127.0.0.1:5002`.
- **Credentials** — credenciais de autenticação (Email e Password).
- **TargetCompId** — identificador do componente de destino, predefinido como `"StockSharpHydraMD"`.
- **SecurityBatchSize** — tamanho do lote ao carregar instrumentos, predefinido como 1000.
- **Timeout** — tempo limite de ligação, predefinido como 2 minutos.

### Exemplo de Utilização

```cs
// Criar um drive remoto
var remoteDrive = new RemoteMarketDataDrive
{
    Address = "192.168.1.100:5002".To<EndPoint>(),
    Credentials = { Email = "user", Password = "pass".Secure() }
};

// Obter tipos de dados disponíveis para um instrumento
var secId = "AAPL@NASDAQ".ToSecurityId();
await foreach (var dataType in remoteDrive.GetAvailableDataTypesAsync(secId, StorageFormats.Binary))
{
    Console.WriteLine(dataType);
}
```

Para mais detalhes sobre o trabalho com armazenamento remoto, consulte a secção [Trabalhar com Armazenamento Remoto](remote.md).

## DriveCache — Gestão de Drives

A classe [DriveCache](xref:StockSharp.Algo.Storages.DriveCache) gere uma colecção de drives de armazenamento e fornece cache para reutilização.

### Métodos e Propriedades Principais

- **GetDrive(path)** — obter um drive existente por caminho ou criar um novo.
- **DeleteDrive(drive)** — remover um drive da cache.
- **TryDefaultDrive** — o primeiro [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) disponível.
- **NewDriveCreated** — evento para criação de novo drive.
- **DriveDeleted** — evento para eliminação de drive.
- **Changed** — evento para alterações na colecção de drives.

A classe implementa a interface `IPersistable`, que permite guardar e carregar configurações de drives.

### Exemplo de Utilização

```cs
// Criar uma cache com um drive local predefinido
var defaultDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));
var driveCache = new DriveCache(defaultDrive);

// Obter ou criar um drive por caminho
var anotherDrive = driveCache.GetDrive(@"D:\MarketData");

// Subscrever eventos
driveCache.NewDriveCreated += drive =>
    Console.WriteLine($"Drive created: {drive.Path}");
```

## Ver Também

- [Trabalhar com a API](api.md)
- [Trabalhar com Armazenamento Remoto](remote.md)
- [Formatos de Armazenamento](formats.md)
