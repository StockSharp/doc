# Identifier \*@ALL

If the instrument is traded on several boards, then as the board code you can use the "ALL". For example, the instrument with the AAPL@ALL identifier means that it collects data from all boards for ticker AAPL (AAPL@BATS, AAPL@ICE, etc.) and these data also have a common order book. 

The board code is used to send the order in a specific trading system. If you specify "ALL", then the broker or the exchange will determine the order route. In this case, the broker or the exchange independently decides on which board it is better to match an order. On the contrary, if you specify a particular board, such as BATS, then the order will be sent only to BATS.
