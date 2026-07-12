# Protokoll-Panel

[LogControl](xref:StockSharp.Xaml.LogControl) ist eine Tabelle zur Anzeige von Logmeldungen. Mit den Schaltflächen der Symbolleiste können Sie Meldungen nach verschiedenen Protokollierungsstufen filtern.

## LogControl

![GUI-Protokollanzeige](../../../../images/gui_logcontrol.png)

Beispielcode

```xaml
<Window x:Class="LoggingControls.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		Title="MainWindow" Height="350" Width="525">
	<Grid>
		<sx:LogControl x:Name="LogControl"/>
	</Grid>
</Window>

```
```cs
// Neue Instanz von LogManager erstellen
_logManager = new LogManager();
// .NET Tracing als Protokollquelle hinzufügen.
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// LogControl als Protokollempfänger hinzufügen.
_logManager.Listeners.Add(new GuiLogListener(LogControl));
..........................
// Testmeldungen aus der TraceSource senden:
Trace.TraceInformation("Infotestmeldung");
Trace.TraceWarning("Warnungstestmeldung");
Trace.TraceError("Fehlertestmeldung");

```
