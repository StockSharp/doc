# Panel de órdenes rápidas

![Captura de pantalla: panel de órdenes rápidas](../../../../images/gui_quickorderpanel.png)

[QuickOrderPanel](xref:StockSharp.Xaml.QuickOrderPanel) - un panel compacto para registrar órdenes con un clic. Muestra los mejores precios de compra y venta, el spread y el volumen de la orden; al pulsar un lado se forma la orden de inmediato.

**Propiedades principales**

- [QuickOrderPanel.Security](xref:StockSharp.Xaml.QuickOrderPanel.Security) - instrumento para el que se registran las órdenes.
- [QuickOrderPanel.Volume](xref:StockSharp.Xaml.QuickOrderPanel.Volume) - volumen de la orden.
- [QuickOrderPanel.BuyBackground](xref:StockSharp.Xaml.QuickOrderPanel.BuyBackground) - fondo del lado de compra.
- [QuickOrderPanel.SellBackground](xref:StockSharp.Xaml.QuickOrderPanel.SellBackground) - fondo del lado de venta.

El panel no registra órdenes por sí mismo: solo forma un objeto [Order](xref:StockSharp.BusinessEntities.Order) y lo pasa al evento `RegisterOrder`. La cartera y las comprobaciones adicionales se añaden en el manejador. Cambiar el volumen o el aspecto genera `SettingsChanged`, punto cómodo para guardar la configuración.

A continuación se muestran fragmentos de código con su uso:

```xaml
<Window x:Class="Sample.QuickOrderWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Height="300" Width="260">
	<xaml:QuickOrderPanel x:Name="QuickOrderPanel" Volume="10" />
</Window>
```

```cs
// Establecemos el instrumento: el panel se suscribe a sus mejores precios
QuickOrderPanel.Security = _security;

// El panel forma la orden, la registramos nosotros
QuickOrderPanel.RegisterOrder += order =>
{
	order.Portfolio = _portfolio;
	_connector.RegisterOrder(order);
};

// Guardamos la configuración cuando cambia
QuickOrderPanel.SettingsChanged += () => SaveSettings();
```

## Ver también

[Operaciones](../trading.md)
