# Scripts de análisis

[S#](../api.md) implementa un subsistema de scripts de análisis que permite realizar un análisis arbitrario de datos de mercado con visualización de resultados. Las clases se encuentran en el espacio de nombres `StockSharp.Algo.Analytics`.

## IAnalyticsScript — Interfaz principal

La interfaz [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) define un único método:

```cs
Task Run(
    ILogReceiver logs,
    IAnalyticsPanel panel,
    SecurityId[] securities,
    DateTime from,
    DateTime to,
    IStorageRegistry storage,
    IMarketDataDrive drive,
    StorageFormats format,
    DataType dataType,
    CancellationToken cancellationToken);
```

Parámetros:

- **logs** — receptor de registros para mostrar mensajes de diagnóstico.
- **panel** — panel para mostrar los resultados del análisis.
- **securities** — array de instrumentos a analizar.
- **from** / **to** — rango de tiempo.
- **storage** — registro de almacenamiento de datos de mercado.
- **drive** — fuente de datos.
- **format** — formato de almacenamiento de datos.
- **Tipo de datos** — tipo de datos a analizar.
- **cancellationToken** — token de cancelación.

## IAnalyticsPanel — Panel de resultados

La interfaz [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) proporciona métodos para crear diversas visualizaciones:

- **CreateGrid(params string[] columns)** — crea una tabla [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) con las columnas especificadas.
- **CreateChart\<X, Y\>()** — crea un gráfico bidimensional [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2).
- **CreateChart\<X, Y, Z\>()** — crea un gráfico tridimensional [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3).
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — dibuja un mapa de calor.
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — dibuja una visualización 3D.

## IAnalyticsChart — Gráficos

La interfaz [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) proporciona un método para agregar series de datos:

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

Estilos de dibujo disponibles ([DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)):

- **Línea** — gráfico de líneas.
- **Línea discontinua** — línea discontinua.
- **Histograma** — histograma.
- **Burbujas** — gráfico de burbujas.

## IAnalyticsGrid — Tablas

La interfaz [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) permite mostrar datos tabulares:

- **SetSort(string column, bool ascending)** — establece el orden por columna.
- **SetRow(params object[] values)** — agrega una fila de datos.

## Scripts integrados

El paquete `StockSharp.Algo.Analytics.CSharp` incluye scripts ya preparados:

- **IndicatorScript** — calcula y visualiza indicadores en un gráfico.
- **ChartDrawScript** — muestra la construcción de varios tipos de gráficos.
- **PriceVolumeScript** — analiza la distribución del volumen por niveles de precio.

## Ejemplo: script de análisis personalizado

A continuación se muestra un script de ejemplo que carga velas para una lista de instrumentos y muestra los precios de cierre en un gráfico de líneas:

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // crear un gráfico bidimensional
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // obtener el almacenamiento de velas
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // cargar datos del periodo
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"No hay datos para {secId}");
                continue;
            }

            // añadir una serie al gráfico
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: {candles.Length} velas cargadas");
        }
    }
}
```

## Ejemplo: tabla de volumen

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Instrument", "Velas totales",
            "Volumen total", "Volumen medio");
        grid.SetSort("Volumen total", false);

        foreach (var secId in securities)
        {
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
                continue;

            var totalVolume = candles.Sum(c => c.TotalVolume);
            var avgVolume = totalVolume / candles.Length;

            grid.SetRow(secId.ToString(), candles.Length,
                totalVolume, avgVolume);
        }
    }
}
```

## Véase también

[Indicadores](indicators.md)

[Almacenamiento de datos](market_data_storage.md)
