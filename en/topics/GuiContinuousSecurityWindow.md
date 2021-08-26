# Continuous futures

[ContinuousSecurityWindow](../api/StockSharp.Xaml.ContinuousSecurityWindow.html) \- is a visual editor for creating *continuous* ([ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html)) instruments. See [Continuous futures](SecurityContinuous.md). 

![HydraGluingCSCustom](../images/HydraGluingCSCustom.png)

This component includes: 

- Special [SecurityIdTextBox](../api/StockSharp.Xaml.SecurityIdTextBox.html) text field, whivh generates a *continuous* instrument with the input of Id \- \[Code\]@\[Board\]. 
- The [SecurityJumpsEditor](../api/StockSharp.Xaml.SecurityJumpsEditor.html) component is a special DataGrid for working with instruments that are part of a *continuous* instrument. The component instruments are wrapped in the [SecurityJump](../api/StockSharp.Xaml.SecurityJump.html), class, which has two properties: [Security](../api/StockSharp.Xaml.SecurityJump.Security.html) and [Date](../api/StockSharp.Xaml.SecurityJump.Date.html) (roll forward). The added instruments are stored in the [Jumps](../api/StockSharp.Xaml.SecurityJumpsEditor.Jumps.html). list. The component has the [Validate](../api/StockSharp.Xaml.SecurityJumpsEditor.Validate.html) function to check the correctness of the component instruments. 
- Buttons for adding\/removing instruments. 
- **Auto** button allows you to automatically create a *continuous* instrument. 
- **Ok** button completes the creation of a *continuous* instrument. 

**Main properties**

- [Security](../api/StockSharp.Xaml.ContinuousSecurityWindow.Security.html) – continuous instrument
- [SecurityStorage](../api/StockSharp.Xaml.ContinuousSecurityWindow.SecurityStorage.html) – provider of information about instruments.

Below is the code snippet with its use. 

```cs
private void CreateContinuousSecurity\_OnClick(object sender, RoutedEventArgs e)
{
	\_continuousSecurityWindow \= new ContinuousSecurityWindow
	{
		SecurityStorage \= \_entityRegistry.Securities,
		Security \= new ContinuousSecurity { Board \= ExchangeBoard.Associated }
	};
	if (\!\_continuousSecurityWindow.ShowModal(this))
		return;
	\_continuousSecurity \= \_continuousSecurityWindow.Security;
	ContinuousSecurity.Content \= \_continuousSecurity.Id;
	var first \= \_continuousSecurity.InnerSecurities.First();
	var gluingSecurity \= new Security
	{
		Id \= \_continuousSecurity.Id,
		Code \= \_continuousSecurity.Code,
		Board \= ExchangeBoard.Associated,
		Type \= \_continuousSecurity.Type,
		VolumeStep \= first.VolumeStep,
		PriceStep \= first.PriceStep,
		ExtensionInfo \= new Dictionary\<object, object\> { { "GluingSecurity", true } }
	};
	if (\_entityRegistry.Securities.ReadById(gluingSecurity.Id) \=\= null)
	{
		\_entityRegistry.Securities.Save(gluingSecurity);
	}
}
```

## Recommended content

[Continuous futures](HydraGluingData.md)
