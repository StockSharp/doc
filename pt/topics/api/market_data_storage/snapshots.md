# Sistema de Snapshots

No StockSharp, os snapshots representam um mecanismo para guardar o estado atual mais recente dos dados de mercado. Em vez de percorrer todo o histórico, os snapshots permitem obter instantaneamente o valor Level1 atual, o livro de ofertas, a posição ou a transação.

## Objetivo dos Snapshots

Ao trabalhar com dados em streaming, é frequentemente necessário conhecer o estado mais recente de um instrumento -- preço atual, livro de ofertas, posição aberta. Sem snapshots, obter esta informação exigiria carregar e processar todo o histórico. O sistema de snapshots resolve este problema guardando o estado mais recente de cada objeto e disponibilizando acesso ao mesmo no menor tempo possível.

## ISnapshotStorage -- Interface de Armazenamento de Snapshots

A interface [ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) define o contrato base para trabalhar com snapshots. A versão tipada `ISnapshotStorage<TKey, TMessage>` disponibiliza os seguintes métodos:

- **Update(message)** -- guardar ou atualizar um snapshot. Se já existir um snapshot para a chave indicada, será atualizado.
- **Get(key)** -- obter um snapshot pela chave (por exemplo, pelo identificador do instrumento).
- **GetAll(from, to)** -- obter todos os snapshots para o intervalo de datas especificado.
- **Clear(key)** -- eliminar o snapshot de uma chave específica.
- **ClearAll()** -- eliminar todos os snapshots.

## ISnapshotSerializer -- Serialização de Snapshots

A interface [ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) é responsável por converter snapshots para representação binária e vice-versa:

- **DataType** -- informação do tipo de dados do snapshot.
- **Version** -- versão do formato de serialização.
- **Serialize(version, message)** -- serializar uma mensagem para uma matriz de bytes.
- **Deserialize(version, buffer)** -- desserializar uma matriz de bytes de volta para uma mensagem.
- **GetKey(message)** -- extrair a chave de uma mensagem.
- **Update(message, changes)** -- aplicar alterações incrementais a um snapshot existente.

## SnapshotRegistry -- Registo de Snapshots

A classe [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) é o componente central para a gestão de snapshots. Implementa a interface `ISnapshotRegistry` e coordena o funcionamento de todos os armazenamentos de snapshots.

### Funcionalidades Principais

- **Gestão de armazenamento** -- disponibiliza acesso aos armazenamentos de snapshots para vários tipos de dados.
- **Escrita periódica** -- as alterações são descarregadas para disco a cada 10 segundos, garantindo um equilíbrio entre desempenho e fiabilidade.
- **Segurança entre threads** -- todas as operações são seguras para utilização a partir de várias threads.

### Organização dos Ficheiros

Os ficheiros de snapshot são armazenados no seguinte caminho:

```
{path}/{yyyy_MM_dd}/{serializer_name}.bin
```

## Serializadores Integrados

O StockSharp inclui quatro serializadores para os principais tipos de dados de mercado:

| Serializador | Tipo de Mensagem | Objetivo |
|------------|-------------|---------|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | Dados Level1 (preços, volumes, spreads) |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | Livro de ofertas |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | Posições |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | Transações (ordens e negócios) |

Cada serializador suporta versionamento do formato, garantindo compatibilidade retroativa ao atualizar o StockSharp.

## Exemplo de Código

### Criar um Registo de Snapshots

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Trabalhar com Snapshots Level1

```cs
// Get the Level1 snapshot storage
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Save a snapshot
var level1Msg = new Level1ChangeMessage
{
    SecurityId = "AAPL@NASDAQ".ToSecurityId(),
    ServerTime = DateTimeOffset.Now,
};
level1Msg.TryAdd(Level1Fields.LastTradePrice, 260.5m);
level1Msg.TryAdd(Level1Fields.BestBidPrice, 260.4m);
level1Msg.TryAdd(Level1Fields.BestAskPrice, 260.6m);

level1Snapshots.Update(level1Msg);
```

### Obter um Snapshot

```cs
// Get the latest snapshot for an instrument
var secId = "AAPL@NASDAQ".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Last price: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## Ver Também

- [Trabalhar com a API](api.md)
- [Formatos de Armazenamento](formats.md)
- [Unidades de Armazenamento](drives.md)
