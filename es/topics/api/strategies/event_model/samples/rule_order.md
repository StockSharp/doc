# Regla para órdenes

## Descripción general

`SimpleOrderRulesStrategy` es una estrategia que demuestra el uso de reglas para procesar eventos relacionados con órdenes en StockSharp. Se suscribe a operaciones y crea reglas para procesar eventos de registro de órdenes.

## Componentes principales

```cs
// Componentes principales
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## Método OnStarted

Se llama cuando se inicia la estrategia:

- Crea una suscripción a ticks
- Crea dos conjuntos de reglas para procesar eventos de registro de órdenes

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 1);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Orden №1 registrada"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Orden №1: registro fallido"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 10000000);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Orden №2 registrada"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Orden №2: registro fallido"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	// Envío de solicitud para suscribirse a datos de mercado.
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## Lógica

### Primer conjunto de reglas

- Cuando se recibe un tick, crea una orden para comprar 1 unidad
- La orden se crea mediante el método `CreateOrder`, especificando dirección, precio (default = mercado) y volumen
- Establece reglas para procesar el registro correcto y los errores de registro
- Las reglas son mutuamente excluyentes y se activan solo una vez

### Segundo conjunto de reglas

- Cuando se recibe el siguiente tick, crea una orden para comprar 10 000 000 unidades
- De forma similar, establece reglas para procesar el registro correcto y los errores de registro
- Las reglas también son mutuamente excluyentes y se activan solo una vez

## Características

- Demuestra la creación de reglas para procesar eventos de registro de órdenes
- Usa el mecanismo de reglas mutuamente excluyentes (`Exclusive`)
- Muestra un ejemplo de logging de información sobre eventos de órdenes mediante el método `LogInfo`
- Ilustra el uso de `Once()` para limitar la activación de reglas
- Crea órdenes con volúmenes diferentes para demostrar varios escenarios (registro correcto y error de registro)
