# Window

The [SecurityCreateWindow](xref:StockSharp.Xaml.SecurityCreateWindow) component is a window for creating and editing an instrument. The component consists of two main elements: the special text field [SecurityIdTextBox](xref:StockSharp.Xaml.SecurityIdTextBox) the property editing grid [PropertyGridEx](xref:StockSharp.Xaml.PropertyGrid.PropertyGridEx). You can access the created (edited) instrument with the [SecurityCreateWindow.Security](xref:StockSharp.Xaml.SecurityCreateWindow.Security) property. 

Below is the appearance of the component and the code snippet with its use. 

![Gui SecurityCreateWindow](../images/Gui_SecurityCreateWindow.png)

```cs
private void Button_Click(object sender, RoutedEventArgs e)
{
    var dlg = new SecurityCreateWindow();
    var result = dlg.ShowDialog();
    if (result != null && (bool)result)
    {
        var security = dlg.Security;
    }
}
	
```
