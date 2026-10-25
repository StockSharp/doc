# Crear estrategia

Las estrategias incluidas en [Shell](../shell.md) están en `Shell.Avalonia/Shell.Core/Strategies/`. Una propia se añade en el mismo sitio y pasa por tres pasos: la clase de la estrategia, la entrada en el catálogo y, si hace falta, un panel propio.

## Clase de la estrategia

Una estrategia es un descendiente normal de `Strategy` de StockSharp. No tiene nada específico de Shell:

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "Mi estrategia";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// El grupo se toma de los recursos para que el parámetro quede en el mismo grupo que los estándar.
			.SetDisplay("Longitud", "Longitud de la media móvil", LocalizedStrings.Parameters)
			.SetOptimize(10, 50, 5);
	}

	public int Length
	{
		get => _length.Value;
		set => _length.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// suscripciones a datos y lógica de negociación
	}
}
```

El punto de entrada es `OnStarted2(DateTime)`, no `OnStarted`: cuando se llama, la estrategia ya ha recibido el instrumento, la cartera y la conexión, por lo que puede suscribirse a los datos aquí mismo.

Un parámetro declarado con `Param(...)` y con `SetOptimize(...)` definido aparece automáticamente en la pestaña [Optimización](user_interface/optimization.md): la lista de parámetros que se iteran y los límites de la iteración se leen de la propia estrategia, no se definen por separado.

## Catálogo de estrategias

Para que la estrategia aparezca en la ventana de selección, hay que añadirla en dos sitios.

El primero es la fábrica de `BuiltInStrategies`, que crea una instancia por el identificador de tipo:

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

El segundo es la descripción de la plantilla en `LegacyStrategyTemplates`, de donde se toman el nombre de la estrategia en la lista y el conjunto de parámetros con sus tipos y valores predeterminados:

```cs
new StrategyTemplateDefinition(MyTypeId, "Mi estrategia", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "Longitud", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` devuelve el instrumento, la cartera y el volumen, que tiene cada estrategia; el resto son sus propios parámetros. Los campos de la descripción van en este orden: clave, etiqueta, tipo, valor predeterminado, obligatoriedad, mínimo y máximo.

`StrategyTemplateKind` solo distingue los tipos para los que Shell tiene su propio panel (`Sma`, `SmaIndex`, `StairsTrend`, `StairsCountertrend`, `Designer`); una estrategia sin panel propio es `Generic`.

Los valores que el usuario define en el panel **Estrategia** se aplican a la instancia en `StrategyMaterializer`: allí mismo se añade el análisis de los nuevos parámetros.

Shell guarda y restaura la configuración de la estrategia: no hace falta escribir código de guardado aparte.

## Panel propio

Si basta con el panel estándar de parámetros, no hay que hacer nada: Shell lo muestra por sí solo. Un panel propio hace falta cuando la estrategia tiene algo que mostrar además de los parámetros: cómo configurarlo y cómo observarla se describe en la sección [Crear panel de estrategia](create_strategy_panel.md).

## Contenido recomendado

[Crear panel de estrategia](create_strategy_panel.md)

[Ejecutar estrategias desde Designer](run_strategies_from_designer.md)
