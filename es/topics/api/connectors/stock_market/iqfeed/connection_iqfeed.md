# Conexión IQFeed

La conexión a los servidores IQ se establece mediante la aplicación IQConnect, que puede instalarse tanto en el equipo local como en uno remoto. La comunicación entre la aplicación cliente e IQConnect, así como entre IQConnect y los servidores, se realiza mediante el protocolo TCP\/IP. 

Para obtener los datos, la aplicación cliente usa cuatro conexiones mediante distintos puertos: 

1. Level1 (puerto 5009) – se usa para datos en tiempo real sobre instrumentos (ticks, precio de apertura, precio de cierre, volatilidad, etc.) y noticias.
2. Level2 (puerto 9200) – se usa para obtener cotizaciones extendidas de instrumentos; se puede obtener el mejor par de cotizaciones para cada ECN.
3. Lookup (puerto 9100) – se usa para búsqueda de instrumentos, obtención de datos históricos y obtención de información extendida sobre noticias.
4. Admin (puerto 9300) – se usa para obtener información general sobre la conexión y cambiar la configuración.

Los números de puerto entre paréntesis se usan por defecto para conectarse a IQConnect. Para conexiones cliente, puede cambiar los números de puerto en el registro, por ejemplo, para Level1 en la siguiente ruta: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. Los números de puerto para conectarse a servidores IQ no pueden cambiarse. 
