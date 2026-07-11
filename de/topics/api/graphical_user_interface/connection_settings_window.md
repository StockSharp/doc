# Fenster für Verbindungseinstellungen

[ConnectorWindow](xref:StockSharp.Xaml.ConnectorWindow) ist ein spezielles Fenster zum Konfigurieren von Adaptern für die Verbindung eines Connectors.

![API-GUI-Verbindungsfenster](../../../images/api_gui_connectorwindow.png)

Dies ist das Fenster für Verbindungseinstellungen. Aus der Dropdown-Liste, die über die Schaltfläche "+" geöffnet wird, müssen Sie die erforderlichen Adapter auswählen und deren Eigenschaften im rechts angeordneten Eigenschaftenfenster konfigurieren.

Dieses Fenster sollte über die Erweiterungsmethode [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner **)** aufgerufen werden, an die der [Connector](xref:StockSharp.Algo.Connector) und das übergeordnete Fenster übergeben werden. Wenn die Konfiguration erfolgreich ist, gibt die Erweiterungsmethode [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner **)** den Wert `true` zurück. Unten sehen Sie den Code zum Aufrufen des Fensters für Connector-Verbindungseinstellungen und zum Speichern der Einstellungen in einer Datei.

```cs
		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			if (_connector.Configure(this))
			{
				new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
			}
		}

```

> [!TIP]
> Die Korrektheit der Verbindung kann über die Schaltfläche **Prüfen** geprüft werden.

Das Ergebnis dieses Fensters ist das Erstellen und Hinzufügen von Adaptern zur Liste der *inneren Adapter* der Eigenschaft [Connector.Adapter](xref:StockSharp.Algo.Connector.Adapter).

Weitere Informationen zum Speichern und Laden von Connector-Einstellungen finden Sie unter [Einstellungen speichern und laden](../connectors/save_and_load_settings.md).
