# Libro de órdenes filtrado

Un libro de órdenes filtrado es una herramienta especializada en StockSharp que permite a traders y estrategias automatizadas operar en el mercado excluyendo sus propias órdenes de la consideración. Esto es críticamente importante al usar varias estrategias en paralelo para evitar situaciones en las que una estrategia empieza a "operar" con otra, sin darse cuenta de que el volumen en el libro de órdenes proviene de otro participante del mercado o es el resultado de acciones de otra estrategia ejecutándose en paralelo.

## Ventajas del libro de órdenes filtrado

- **Evitar self-trading:** Las estrategias no ejecutarán órdenes contra sí mismas ni entre sí cuando se ejecuten en paralelo.
- **Pureza del análisis:** Permite a las estrategias analizar condiciones de mercado basándose únicamente en órdenes externas, sin distorsiones causadas por sus propias órdenes.
- **Eficiencia de ejecución:** Ayuda a mejorar la calidad de ejecución de órdenes minimizando el impacto de las órdenes propias sobre el precio de mercado.

## Ejemplo de suscripción

El enfoque para trabajar con el libro de órdenes filtrado usa el mismo método que [suscribirse a un libro de órdenes normal](subscriptions.md), pero con un valor [DataType](xref:StockSharp.Messages.DataType) distinto. A continuación se muestra un ejemplo que ilustra la suscripción a un libro de órdenes filtrado para un instrumento específico:

1. **Suscripción al evento de actualización del libro de órdenes:** [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) para recibir actualizaciones del libro de órdenes. Este evento se usa tanto para el libro de órdenes normal como para el filtrado.

    Al procesar el evento, compruebe [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) en el objeto `subscription` asociado con el evento. Si [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) es [DataType](xref:StockSharp.Messages.DataType.FilteredMarketDepth), indica que el libro de órdenes recibido está filtrado:

    ```cs
    connector.OrderBookReceived += (sender, subscription, orderBook) =>
    {
        if (subscription.DataType == DataType.FilteredMarketDepth)
        {
            // Lógica de manejo para el libro de órdenes filtrado
            Console.WriteLine($"Received filtered order book for {orderBook.SecurityId}.");
        }
    };
    ```

2. **Envío de la suscripción:** Forme un objeto [Subscription](xref:StockSharp.BusinessEntities.Subscription) y envíelo al conector:

    ```cs
    var subscription = new Subscription(DataType.FilteredMarketDepth, security);
    connector.Subscribe(subscription);
    
    // o así
    //var subscription = connector.SubscribeFilteredMarketDepth(security);
    ```

## Conclusión

El uso del libro de órdenes filtrado en StockSharp proporciona a traders y desarrolladores de estrategias una herramienta flexible para análisis de mercado, permitiéndoles evitar la autointeracción no deseada entre estrategias ejecutadas simultáneamente y simplificando la toma de decisiones basada en datos de órdenes del mercado.
