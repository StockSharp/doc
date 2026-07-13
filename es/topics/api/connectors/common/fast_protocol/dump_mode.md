# Modo de volcado

El conector FAST está disponible en modo de volcado. En este caso, los datos no provienen de una conexión de red real, sino de los archivos acumulados de la aplicación [UDP Dumper](../../../../udp_dumper.md).

Para habilitar el modo de volcado, pase los archivos a través del método de volcado [IFastDialect.Dump](xref:StockSharp.Fix.Dialects.IFastDialect.Dump(System.Collections.Generic.IDictionary{Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable{System.IO.Stream}}))**(**[System.Collections.Generic.IDictionary\<Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable\<System.IO.Stream\>\>](xref:System.Collections.Generic.IDictionary`2) dumpFiles **)** antes de conectar:

```cs
// ... inicialización del conector
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

Después de esto, el trabajo con el conector continúa como de costumbre, como si estuviera recibiendo datos de la red.
