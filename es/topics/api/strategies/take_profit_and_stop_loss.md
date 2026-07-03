# Protección de posiciones

## Introducción

Esta modificación de la estrategia SMA implementa un mecanismo para proteger posiciones abiertas mediante un controlador protector local. Este enfoque permite una gestión flexible del riesgo y el cierre automático de posiciones cuando se cumplen determinadas condiciones.

## Componentes clave de la protección de posiciones

### Controlador protector

La estrategia usa dos objetos clave para la protección de posiciones:

```cs
// Declaración de controladores protectores
private readonly ProtectiveController _protectiveController = new();
private IProtectivePositionController _posController;

// Este código inicializa el controlador protector principal y crea un marcador de posición para
// un controlador de posición específico. ProtectiveController gestiona todas las posiciones,
// mientras que IProtectivePositionController es responsable de una posición específica.
```

- `_protectiveController`: el controlador principal que gestiona la protección de todas las posiciones.
- `_posController`: controlador para una posición específica.

### Inicialización de protección

Al abrir una nueva posición o modificar una existente, se inicializa el controlador protector:

```cs
// Inicialización del controlador protector para una nueva posición
this.WhenOwnTradeReceived()
	.Do(t =>
	{
		// ... (otro código)

		if (TakeValue.IsSet() || StopValue.IsSet())
		{
			_posController ??= _protectiveController.GetController(
				security.ToSecurityId(),
				portfolio.Name,
				new LocalProtectiveBehaviourFactory(security.PriceStep, security.Decimals),
				TakeValue, StopValue, true, default, default, true);
		}

		var info = _posController?.Update(t.Trade.Price, t.GetPosition());

		if (info is not null)
			ActiveProtection(info.Value);
	})
	.Apply(this);

// Este código crea e inicializa un controlador protector para una nueva posición
// al recibir información sobre una nueva operación. También actualiza la información
// sobre la posición en el controlador y activa la protección si es necesario.
```

Esto crea un controlador para una posición específica con parámetros de take-profit y stop-loss dados.

### Actualización de información de posición

```cs
var info = _posController?.Update(t.Trade.Price, t.GetPosition());

if (info is not null)
	ActiveProtection(info.Value);
```

Esto permite al controlador realizar seguimiento del estado actual de la posición y ajustar las órdenes protectoras según sea necesario.

### Comprobación de condiciones de activación de protección

En el método que procesa nuevos datos (por ejemplo, al recibir una nueva vela), se comprueban las condiciones para activar órdenes protectoras:

```cs
// Comprobación de condiciones de activación de protección en el método ProcessCandle
var info = _posController?.TryActivate(candle.ClosePrice, CurrentTime);

if (info is not null)
	ActiveProtection(info.Value);

// Este código comprueba si debe activarse una orden protectora en función del
// precio actual (en este caso, el precio de cierre de la vela) y del tiempo.
// Si se cumplen las condiciones, se llama al método ActiveProtection.
```

Aquí, el precio de cierre de la vela se usa como precio actual, pero puede ser cualquier valor de precio relevante (por ejemplo, el precio de la última operación o el spread actual en el libro de órdenes).

### Activación de una orden protectora

Si se cumplen las condiciones para activar una orden protectora, se desencadena la lógica correspondiente:

```cs
// Método para activar una orden protectora
private void ActiveProtection((bool isTake, Sides side, decimal price, decimal volume, OrderCondition condition) info)
{
	// enviar una orden protectora (de cierre de posición) como una orden normal
	RegisterOrder(this.CreateOrder(info.side, info.price, info.volume));
}

// Este método crea y registra una orden para cerrar la posición
// basándose en la información recibida del controlador protector.
```

Este método crea y registra una orden para cerrar la posición según los parámetros devueltos por el controlador protector.

## Comparación con stop orders del lado del servidor

### Ventajas de las stop orders del lado del servidor

1. Las stop orders (stop loss y take profit) se envían directamente al broker.
2. El broker supervisa de forma independiente el cumplimiento de las condiciones stop.
3. Cuando se activa un stop, el broker coloca automáticamente una orden de mercado o límite.

### Ventajas del enfoque local

1. **Flexibilidad**: posibilidad de implementar lógica de protección compleja no disponible en stops estándar del lado del servidor.
2. **Confidencialidad**: la información sobre niveles stop no se transmite al broker, lo que puede ser importante en algunos mercados.
3. **Velocidad de reacción**: reacción potencialmente más rápida ante cambios en las condiciones de mercado.
4. **Adaptabilidad**: posibilidad de ajustar dinámicamente los niveles de protección según datos de mercado o lógica de estrategia.
5. **Independencia de la implementación del broker/exchange**: el enfoque local funciona igual independientemente de si el broker o exchange admite todos los tipos necesarios de órdenes protectoras.
6. **Pruebas con datos históricos**: posibilidad de probar completamente la estrategia con protección de posiciones sobre datos históricos, lo que es imposible con stops del lado del servidor.

### Desventajas del enfoque local

1. **Dependencia de la funcionalidad del terminal de trading**: si el terminal se desconecta, la protección no funcionará.
2. **Carga del sistema**: requiere cálculos constantes en el lado del cliente.
3. **Retrasos**: posibles retrasos al colocar una orden después de que se activen las condiciones de protección.

### Desventajas de las stop orders del lado del servidor

1. **Dependencia de la implementación del broker/exchange**: no todos los brokers o exchanges admiten todos los tipos de órdenes protectoras, lo que puede limitar la funcionalidad de la estrategia.
2. **Imposibilidad de realizar pruebas completas con datos históricos**: los stops del lado del servidor no se pueden modelar con precisión al probar con datos históricos, lo que dificulta evaluar la eficacia real de la estrategia.
3. **Flexibilidad limitada**: normalmente solo están disponibles tipos básicos de stop orders, lo que limita las posibilidades de implementar mecanismos protectores complejos.

## Conclusión

El uso de un controlador protector local en la estrategia SMA permite una gestión eficaz del riesgo de posiciones abiertas. Este enfoque proporciona flexibilidad al configurar parámetros de protección y una reacción rápida a cambios en la situación del mercado, lo cual es crítico para un trading exitoso.
