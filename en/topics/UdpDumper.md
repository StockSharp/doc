# S\#.UDP Dumper

The app **S\#.UDPDumper** is designed to accumulate UDP packets. Using this program, you can check the valid of the network settings made by a broker or exchange. And also, accumulate data for further tests of the connector.

To install need use [S\#.Installer](SharpInstaller.md).

## Setup and run

1. On first launch, the app shows the following:![Dumper 1](../images/Dumper_1.png)
2. To add network feeds, you can add both manually and automatically load all feeds from the config files of exchanges. To do this, click on the button:![Dumper 2](../images/Dumper_2.png)
3. In the window that appears, you need to find the desired config file from the exchange and open it:![Dumper 3](../images/Dumper_3.png)
4. All feeds with IP addresses and ports settings will be loaded from a file:![Dumper 4](../images/Dumper_4.png)
5. It is necessary to select the necessary feeds, and click on the download start button:![Dumper 5](../images/Dumper_5.png)
6. В случае успешных настроек программа начнет получать UDP датаграммы и записывать на диск. Программа будет писать кол\-во полученных байтов для каждого потока:![Dumper 6](../images/Dumper_6.png)

   > [!CAUTION]
   > If the settings are successful, the program will start receiving UDP datagrams and writing them to disk. The app will write the number of bytes received for each feed.
7. The app **S\#.UDPDumper** is written with a graphical interface. If the program is launched without a graphical interface (as well as running under Linux operating systems, etc.) you can use the **S\#.UDPDumper.Console**, program, which is a console and cross\-platform version.

   The app **S\#.UDPDumper.Console** takes as a parameter the path to the file created by the UI version (exactly the UI version, and **not an exchange config**):

   ```cs
   		StockSharp.UdpDumper.Console.exe settings.xml
   		
   ```
8. To test on the accumulated data of the connector, you can use the dump mode. More details [Dump mode](FastDump.md).
