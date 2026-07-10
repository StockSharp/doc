# Guardado y carga de ajustes

En StockSharp, el mecanismo para guardar y cargar ajustes de estrategia se implementa mediante los métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) y [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)).

## Manejo automático de parámetros

En la mayoría de los casos, **no es necesario** sobrescribir los métodos `Save` y `Load`, ya que la clase base [Strategy](xref:StockSharp.Algo.Strategies.Strategy) guarda y carga automáticamente los parámetros de estrategia creados con [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1).

El enfoque recomendado es usar el mecanismo de parámetros de estrategia descrito en detalle en la sección [Parámetros de estrategia](parameters.md). Con este enfoque, todos los parámetros se guardan y cargan automáticamente:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Longitud de la SMA larga", string.Empty, "Configuración básica");
	}
}
```

## Sobrescritura para casos especiales

Sobrescribir los métodos `Save` y `Load` solo es necesario en casos especiales cuando necesita guardar o cargar datos que no forman parte del conjunto estándar de parámetros de estrategia. Por ejemplo, para guardar estado interno, estructuras de datos no estándar o valores en caché.

Si sobrescribe estos métodos, **debe llamar a los métodos de la clase base**:

```cs
public override void Save(SettingsStorage settings)
{
	// Primero llamar al método base para guardar parámetros estándar
	base.Save(settings);
	
	// Luego agregar su lógica de guardado específica
	settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
	// Primero llamar al método base para cargar parámetros estándar
	base.Load(settings);
	
	// Luego agregar su lógica de carga específica
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
}
```

## Guardar y cargar desde un archivo

Para guardar ajustes en un archivo o cargarlos desde un archivo, puede usar la serialización y deserialización implementadas en StockSharp:

```cs
// Guardar ajustes en un archivo
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// Cargar ajustes desde un archivo
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
	var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
	newStrategy.Load(loadedSettings);
}
```

## Recomendaciones

1. Siempre que sea posible, use [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) para todos los parámetros configurables de la estrategia.
2. Sobrescriba los métodos `Save` y `Load` solo cuando necesite guardar/cargar datos no estándar.
3. Llame siempre a los métodos base `base.Save()` y `base.Load()` al sobrescribir.
4. Use las herramientas estándar de serialización de StockSharp para guardar ajustes en un archivo o cargarlos desde un archivo.

## Ver también

[Parámetros de estrategia](parameters.md)
