# Dump mode

The FAST connector is available in dump mode. In this case, the data does not come from a real network connection, but from the accumulated files of the app [S\#.UDP Dumper](UdpDumper.md).

In order to enable dump mode, it is necessary to transfer files through the dump method [IFastDialect.Dump](xref:StockSharp.Fix.Dialects.IFastDialect.Dump(System.Collections.Generic.IDictionary{Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable{System.IO.Stream}})) before process connecting:

```cs
// ... connector initialization
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

After that, the work with the connector proceeds as usual, as if it was receiving data from the network.
