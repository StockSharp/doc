# Ciclo de vida das subscrições

As subscrições no StockSharp passam por fases específicas do ciclo de vida. A interface [ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) fornece eventos para acompanhar cada fase.

## Eventos do ciclo de vida

### SubscriptionStarted

```cs
event Action<Subscription> SubscriptionStarted;
```

Chamado quando uma subscrição foi iniciada com êxito: o adaptador aceitou o pedido e começou a transmitir dados. Para subscrições históricas, isto significa que o carregamento de dados começou. Para subscrições em tempo real, significa que o servidor aceitou o pedido.

### SubscriptionOnline

```cs
event Action<Subscription> SubscriptionOnline;
```

Chamado quando a subscrição passou para o modo em tempo real. Para subscrições em tempo real, isto significa que a recuperação de dados históricos (se existir) foi concluída e os dados estão agora a chegar em tempo real. Este é um sinal importante para as estratégias perceberem que os indicadores estão "aquecidos" e que a negociação pode começar.

### SubscriptionStopped

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

Chamado quando a subscrição terminou. O parâmetro `Exception` contém o motivo da paragem:
- `null` -- conclusão normal (o utilizador cancelou a subscrição ou os dados históricos terminaram).
- Um objeto de exceção -- um erro (perda de ligação, erro do lado do servidor, etc.).

### SubscriptionFailed

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

Chamado quando ocorre um erro de subscrição. O terceiro parâmetro `bool` indica se foi uma operação de subscrição (`true`) ou de cancelamento de subscrição (`false`).

## Ordem dos eventos

Sequência típica para uma subscrição em tempo real:

1. Chamar `Subscribe(subscription)`
2. `SubscriptionStarted` -- subscrição aceite
3. Chegada de dados (velas, livros de ordens, negócios, etc.)
4. `SubscriptionOnline` -- transição para o modo em tempo real
5. Continuação da chegada de dados em tempo real
6. Chamar `UnSubscribe(subscription)` ou perda de ligação
7. `SubscriptionStopped` -- subscrição terminada

Para uma subscrição histórica (com um intervalo de datas especificado):

1. Chamar `Subscribe(subscription)`
2. `SubscriptionStarted` -- subscrição aceite
3. Chegada de dados históricos
4. `SubscriptionStopped` com `null` -- todos os dados recebidos

## SubscriptionsOnConnect

A propriedade [Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) define o conjunto de subscrições que são enviadas automaticamente ao ligar:

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

Por predefinição, estão incluídas subscrições para pesquisas de instrumentos, pesquisas de portfólios e pesquisas de ordens:

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

Pode adicionar as suas próprias subscrições, que serão iniciadas automaticamente em cada ligação:

```cs
// Adicionar subscrição automática de dados Level1
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// Remover a pesquisa automática de ordens na ligação
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## Eventos de ligação por adaptador

Ao trabalhar com múltiplas ligações (múltiplos adaptadores), são úteis os eventos que indicam qual adaptador específico se ligou ou desligou:

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

Chamado na ligação bem-sucedida de um adaptador específico. O parâmetro é o adaptador que iniciou o evento.

### DisconnectedEx

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

Chamado na desconexão de um adaptador específico.

### ConnectionErrorEx

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

Chamado quando ocorre um erro de ligação para um adaptador específico.

Também estão disponíveis os eventos agregados `Connected`, `Disconnected` e `ConnectionError`, que são disparados sem especificar um adaptador concreto.

## Exemplo

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // Acompanhar o ciclo de vida da subscrição
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"Subscrição iniciada: {subscription.DataType}, " +
            $"Instrumento: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"Subscrição online: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"Subscrição concluída: {subscription.DataType}");
        else
            Console.WriteLine($"Subscrição interrompida: {subscription.DataType}, " +
                $"Erro: {error.Message}");
    };

    // Acompanhar ligações de adaptadores individuais
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"Adaptador ligado: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"Adaptador desligado: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"Erro de ligação do adaptador {adapter.Name}: {error.Message}");
    };

    // Ligar
    _connector.Connect();

    // Após a ligação -- criar uma subscrição
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## Ver também

[Ligação](../connectors.md)
