# Horario de trabajo

![Designer tiempo de trabajo 00](../../../../../../images/designer_working_time_00.png)

Este bloque se usa para determinar el horario de trabajo de la estrategia. Por ejemplo, para definir cuándo se negocia un instrumento específico o cuándo la estrategia tiene permitido operar.
#### Sockets de entrada

- **Cualquier dato** - el bloque acepta cualquier valor, pero toma de él la marca de tiempo, que luego se compara con los parámetros del bloque.
#### Sockets de salida

- **Indicador** - bandera que determina si la marca de tiempo cumple los parámetros del bloque (true) o no (false).
#### Parámetros

- **Hora desde** - hora de inicio del horario de trabajo.
- **Hora hasta** - hora de fin del horario de trabajo.

El bloque puede usarse para determinar cuándo se realiza trading para varios instrumentos de distintas plataformas de trading.

![Designer tiempo de trabajo 01](../../../../../../images/designer_working_time_01.png)

## Véase también

[Trading permitido](trade_allow.md)
