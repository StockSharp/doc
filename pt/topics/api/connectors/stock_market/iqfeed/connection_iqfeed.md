# Ligação IQFeed

A ligação aos servidores IQ é estabelecida através da aplicação IQConnect, que pode ser instalada num computador local ou remoto. A comunicação entre a aplicação cliente e o IQConnect, bem como entre o IQConnect e os servidores, é efetuada através do protocolo TCP\/IP.

Para obter os dados, a aplicação cliente utiliza quatro ligações através de várias portas:

1. Level1 (porta 5009) - utilizado para dados em tempo real sobre instrumentos (ticks, preço de abertura, preço de fecho, volatilidade, etc.) e notícias.
2. Level2 (porta 9200) - utilizado para obter cotações alargadas de instrumentos; o melhor par de cotações pode ser obtido para cada ECN.
3. Lookup (porta 9100) - utilizado para pesquisa de instrumentos, obtenção de dados históricos e obtenção de informação alargada sobre notícias.
4. Admin (porta 9300) - utilizado para obter informação geral sobre a ligação e a alteração das definições.

Os números das portas entre parênteses são utilizados por predefinição para ligar ao IQConnect. Para ligações de cliente, pode alterar os números de porta no registo, por exemplo, para Level1 no seguinte caminho: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. Os números de porta para ligação aos servidores IQ não podem ser alterados.
