# Connection IQFeed

The connection to the IQ servers is established using the IQConnect application, which can be installed both on the local computer and on the remote one. The communication between the client application and the IQConnect, as well as between IQConnect and servers is carried out by the TCP\/IP protocol. 

To get the data the client application uses four connections through various ports: 

1. Level1 (port 5009) – used for real time data on instruments (ticks, the opening price, the closing price, volatility, etc.) and news.
2. Level2 (port 9200) – used to get extended quotes on instruments, the best couple of quotes can get for each ECN.
3. Lookup (port 9100) – used for instruments search, historical data getting, obtaining extended information on the news.
4. Admin (port 9300) – used to obtain general information about the connection and the settings change.

The numbers of ports in brackets are used by default to connect to the IQConnect. For client connections, you can change the port numbers in the registry, for example, for Level1 on the following path: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. The ports numbers to connect to IQ servers can not be changed. 
