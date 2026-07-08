# Dump-Modus

Der FAST-Connector ist im Dump-Modus verfügbar. In diesem Fall stammen die Daten nicht aus einer echten Netzwerkverbindung, sondern aus den gesammelten Dateien der App [UDP Dumper](../../../../udp_dumper.md).

Um den Dump-Modus zu aktivieren, übergeben Sie die Dateien über die Dump-Methode [IFastDialect.Dump](xref:StockSharp.Fix.Dialects.IFastDialect.Dump(System.Collections.Generic.IDictionary{Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable{System.IO.Stream}}))**(**[System.Collections.Generic.IDictionary\<Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable\<System.IO.Stream\>\>](xref:System.Collections.Generic.IDictionary`2) dumpFiles **)** vor dem Verbinden:

```cs
// ... Connector-Initialisierung
var fastAdapter = (FastMessageAdapter)connector.Adapters.InnerAdapters.First();
IEnumerable<string> dumpFiles = Directory.GetFiles(dumpDir, "*.bin");
var dict = dumpFiles.Select(f =>
{
	var name = Path.GetFileNameWithoutExtension(f);
	var parts = name.Split('_').Skip(1).ToArray();
	var groupAddr = parts[0];
	var port = parts[1];
	var sourceAddr = parts[2];
	if (sourceAddr.IsEmpty())
		sourceAddr = null;
	return Tuple.Create(new MulticastSourceAddress
	{
		GroupAddress = groupAddr.To<IPAddress>(),
		Port = port.To<int>(),
		SourceAddress = sourceAddr.To<IPAddress>(),
	}, f);
}).GroupBy(t => t.Item1).ToDictionary(g => g.Key, g => (IEnumerable<Stream>)g.Select(p => File.OpenRead(p.Item2)).ToArray());

fastAdapter.DialectSettings.Dump(dict);
// ...
connector.Connect();
```

Danach verläuft die Arbeit mit dem Connector wie gewohnt, als ob er Daten aus dem Netzwerk empfangen würde.
