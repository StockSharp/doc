# Instrumentos de trabajo

## Descripción

El método `GetWorkingSecurities()` de la clase base `Strategy` se usa para obtener una lista de instrumentos y tipos de datos que la estrategia usa en su funcionamiento. Este método cumple un papel importante al trabajar con [Designer](../../designer.md).

## Propósito

El propósito principal del método es proporcionar a Designer información sobre qué instrumentos y tipos de datos son necesarios para que la estrategia funcione. Esto permite a Designer:

1. Comprobar la disponibilidad de los datos históricos necesarios en el almacenamiento antes de iniciar las pruebas
2. Cargar automáticamente los datos requeridos cuando estén disponibles
3. Configurar correctamente las suscripciones al lanzar la estrategia

## Implementación

En la clase base `Strategy`, el método devuelve una colección vacía. Para trabajar correctamente con Designer, se recomienda sobrescribirlo en su estrategia:

```cs
public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
{
	// Devolver una lista de pares (instrumento, tipo de datos) usados por la estrategia
	return new[] 
	{ 
		(Security, CandleType),
		// Otros pares instrumento-tipo de datos si la estrategia usa varios
	};
}
```

## Importancia de sobrescribir el método

Si el método `GetWorkingSecurities()` no se sobrescribe en su estrategia:

- Designer no podrá comprobar automáticamente los datos necesarios
- Si los datos históricos requeridos faltan en el almacenamiento, Designer no emitirá advertencias
- La estrategia puede lanzarse para pruebas, pero no se mostrarán resultados
- El usuario no recibirá información sobre la razón de la ausencia de resultados

## Ejemplo de uso

```cs
public class MySmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
	
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}
	
	public MySmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
	}
	
	// Sobrescribir el método para trabajar correctamente con Designer
	public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
	{
		return new[] { (Security, CandleType) };
	}
	
	// El resto del código de la estrategia...
}
```

## Conclusión

Aunque el método `GetWorkingSecurities()` no es obligatorio para implementar la funcionalidad básica de una estrategia, sobrescribirlo se recomienda encarecidamente para trabajar correctamente con StockSharp Designer. Esto ayuda a evitar situaciones en las que una estrategia se lanza para pruebas pero no muestra resultados por ausencia de los datos históricos necesarios.
