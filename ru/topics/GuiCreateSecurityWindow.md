# Создание инструмента

Компонент [SecurityCreateWindow](../api/StockSharp.Xaml.SecurityCreateWindow.html) \- это окно для создания и редактирования инструмента. Компонент состоит из двух основных элементов: специального текстового поля [SecurityIdTextBox](../api/StockSharp.Xaml.SecurityIdTextBox.html) и сетки редактирования свойств [PropertyGridEx](../api/StockSharp.Xaml.PropertyGrid.PropertyGridEx.html). Получить доступ к созданному (отредактированному) инструменту можно при помощи свойства [Security](../api/StockSharp.Xaml.SecurityCreateWindow.Security.html). 

Ниже показан внешний вид компонента и фрагмент кода с его использованием. 

![Gui SecurityCreateWindow](../images/Gui_SecurityCreateWindow.png)

```cs
private void Button\_Click(object sender, RoutedEventArgs e)
{
    var dlg \= new SecurityCreateWindow();
    var result \= dlg.ShowDialog();
    if (result \!\= null && (bool)result)
    {
        var security \= dlg.Security;
    }
}
	
```
