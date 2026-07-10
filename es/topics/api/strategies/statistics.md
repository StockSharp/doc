# Estadísticas de estrategia

## Descripción general

La plataforma StockSharp proporciona un sistema completo para el análisis estadístico de estrategias de trading, que ayuda a los traders a evaluar la eficacia, optimizar parámetros y tomar decisiones informadas. El sistema de estadísticas recopila y procesa datos de varios aspectos del trading, incluidas órdenes, operaciones, posiciones e indicadores de beneficios/pérdidas.

## Propósito y beneficios

El análisis estadístico en estrategias de trading cumple varias funciones importantes:

1. **Medición de rendimiento**: evaluación cuantitativa del éxito de su estrategia mediante métricas como beneficio neto, drawdown máximo y factor de recuperación.

2. **Gestión de riesgos**: comprensión del perfil de riesgo de su estrategia mediante métricas como porcentaje de drawdown máximo y estadísticas de tamaño de posición.

3. **Optimización**: búsqueda de parámetros óptimos de estrategia comparando indicadores estadísticos entre distintos conjuntos de parámetros.

4. **Análisis de calidad de operaciones**: análisis de distribución de operaciones, proporción entre operaciones rentables y perdedoras, beneficio medio por operación.

5. **Métricas operativas**: seguimiento de métricas operativas como estadísticas de latencia y tasas de error de órdenes para identificar problemas de ejecución.

## Indicadores estadísticos disponibles

La interfaz [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) en StockSharp proporciona acceso a numerosos parámetros estadísticos organizados en varias categorías:

### Estadísticas de beneficios y pérdidas

- Beneficio neto
- Beneficio neto (%)
- Beneficio máximo
- Drawdown máximo
- Drawdown máximo (%)
- Drawdown relativo máximo
- Factor de recuperación

### Estadísticas de operaciones

- Número de operaciones rentables
- Número de operaciones perdedoras
- Número total de operaciones
- Beneficio medio por operación
- Operación rentable media
- Operación perdedora media
- Número de operaciones por mes/día

### Estadísticas de posición

- Posición larga máxima
- Posición corta máxima

### Estadísticas de órdenes

- Número de órdenes
- Número de errores de órdenes
- Retraso máximo/mínimo de registro
- Retraso máximo/mínimo de cancelación

## Integración con la clase Strategy

La clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) recopila y calcula estadísticas automáticamente durante la ejecución. El gestor de estadísticas está disponible mediante la propiedad `StatisticManager`, que implementa la interfaz [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager).

Los valores estadísticos clave también se representan directamente como propiedades de la clase Strategy:

- `PnL`: valor de beneficios y pérdidas
- `Commission`: comisión total pagada
- `Slippage`: slippage total
- `Latency`: latencia media de operación de órdenes

## Visualización

StockSharp proporciona un componente gráfico especial para visualizar estadísticas de estrategia llamado `StatisticParameterGrid`, disponible en el espacio de nombres `StockSharp.Xaml`. Esta cuadrícula muestra todos los parámetros estadísticos en un formato cómodo para el usuario.

Para más información sobre el componente gráfico, consulte la documentación sobre [Estadísticas](../graphical_user_interface/strategies/statistics.md).

## Ejemplo de uso

A continuación se muestra un ejemplo de trabajo con estadísticas de estrategia en su código:

```csharp
// Crear una estrategia
var strategy = new SmaStrategy
{
	// Configurar parámetros de estrategia
	Security = security,
	Portfolio = portfolio,
	Volume = 1,
	// Establecer parámetros SMA
	LongSma = 200,
	ShortSma = 50,
};

// Conectar la estrategia a un gráfico para visualización
var chart = new ChartPanel();
strategy.SetChart(chart);

// Acceder al gestor de estadísticas
var statisticManager = strategy.StatisticManager;

// Mostrar estadísticas de estrategia en la interfaz de usuario
// Suponiendo que tiene un StatisticParameterGrid definido en XAML como 'StatisticsGrid'
StatisticsGrid.Parameters.Clear();
StatisticsGrid.Parameters.AddRange(statisticManager.Parameters);

// Iniciar la estrategia
strategy.Start();

// Cuando necesite reaccionar a cambios en estadísticas
strategy.PnLChanged += () =>
{
	Console.WriteLine($"Current PnL: {strategy.PnL}");
	
	// También puede acceder a parámetros estadísticos individuales
	var netProfit = statisticManager.Parameters
		.OfType<NetProfitParameter>()
		.FirstOrDefault();
		
	if (netProfit != null)
	{
		Console.WriteLine($"Net Profit: {netProfit.Value}");
	}
};

// Para seguir estadísticas de posición
strategy.PositionChanged += () =>
{
	Console.WriteLine($"Current Position: {strategy.Position}");
};
```

## Estadísticas personalizadas

También puede crear sus propios parámetros estadísticos implementando las interfaces adecuadas:

- [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter): interfaz base para todos los parámetros estadísticos
- [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter): para parámetros relacionados con beneficios/pérdidas
- [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter): para parámetros relacionados con operaciones
- [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter): para parámetros relacionados con posiciones
- [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter): para parámetros relacionados con órdenes

A continuación se muestra un ejemplo simple de un parámetro estadístico personalizado:

```csharp
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = "Mi indicador personalizado",
	Description = "Descripción de mi indicador personalizado",
	GroupName = "Custom Parameters",
	Order = 1000
)]
public class MyCustomParameter : BasePnLStatisticParameter<decimal>
{
	public MyCustomParameter()
		: base(StatisticParameterTypes.Custom)
	{
	}

	public override void Add(DateTimeOffset marketTime, decimal pnl, decimal? commission)
	{
		// Lógica de cálculo personalizada
		Value = /* your custom calculation */;
	}
}

// Luego agregarlo al StatisticManager de su estrategia
strategy.StatisticManager.Parameters.Add(new MyCustomParameter());
```

## Conclusión

El sistema de análisis estadístico en StockSharp proporciona a los traders herramientas potentes para evaluar y optimizar sus estrategias de trading. Al usar estas estadísticas, puede obtener información valiosa sobre el rendimiento de su estrategia, identificar áreas de mejora y tomar decisiones basadas en datos para mejorar sus resultados de trading.
