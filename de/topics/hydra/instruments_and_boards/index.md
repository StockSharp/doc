# Index

Mit [Hydra](../../hydra.md) koennen Sie einen eigenen Index erstellen.

Waehlen Sie auf der Registerkarte **Common** den Punkt **Securities**, sodass die Registerkarte **All Securities** angezeigt wird.

Pruefen Sie vor dem Erstellen des **Index**, welche Marktdaten verfuegbar sind. Waehlen Sie den Pfad aus, in dem die Daten gespeichert sind, und betrachten Sie nacheinander die Instrumente, die in die Berechnung des Index eingehen sollen. Wenn Luecken vorhanden sind, laden Sie die erforderlichen Marktdaten aus einer unterstuetzten Datenquelle herunter.

![HydraGluingCheckData](../../../images/hydragluingcheckdata.png)

Als Beispiel betrachten wir den Instrumentenverhaeltnis-Index AAPL@NYSE\/GOOG@NYSE.

1. Der erste Schritt besteht darin, den **Index** zu erstellen. Klicken Sie auf der Registerkarte **All Securities** auf **Create security \=\> Index** ![hydra index sec 00](../../../images/hydra_index_sec_00.png).
2. Das folgende Fenster erscheint: ![hydra index sec](../../../images/hydra_index_sec.png)
3. Um das **Index**-Instrument zu erstellen, geben Sie einen Namen an und fuegen die mathematische Formel fuer eine Kombination mehrerer Instrumente hinzu. Neben den Standardoperatoren der Mathematik koennen Sie die folgenden Funktionen verwenden:
   - **abs(a)** - Gibt den Absolutwert einer Zahl zurueck.
   - **acos(a)** - Gibt den Winkel zurueck, dessen Kosinus der angegebenen Zahl entspricht.
   - **asin(a)** - Gibt den Winkel zurueck, dessen Sinus der angegebenen Zahl entspricht.
   - **atan(a)** - Gibt den Winkel zurueck, dessen Tangens der angegebenen Zahl entspricht.
   - **ceiling(a)** - Gibt die kleinste ganze Zahl zurueck, die groesser oder gleich der angegebenen Zahl ist.
   - **cos(a)** - Gibt den Kosinus des angegebenen Winkels zurueck.
   - **exp(a)** - Gibt den Wert von **e** potenziert mit dem angegebenen Exponenten zurueck.
   - **floor(a)** - Gibt die groesste ganze Zahl zurueck, die kleiner oder gleich der angegebenen Zahl ist.
   - **log(a)** - Gibt den natuerlichen Logarithmus (Basis **e**) der angegebenen Zahl zurueck.
   - **log10(a)** - Gibt den Logarithmus zur Basis 10 der angegebenen Zahl zurueck.
   - **max (a, b)** - Gibt die groessere von zwei Dezimalzahlen zurueck.
   - **min(a, b)** - Gibt die kleinere von zwei Dezimalzahlen zurueck.
   - **pow(a, b)** - Gibt die angegebene Zahl potenziert mit dem angegebenen Exponenten zurueck.
   - **sign(a)** - Gibt eine ganze Zahl zurueck, die das Vorzeichen der angegebenen Zahl angibt.
   - **sin(a)** - Gibt den Sinus des angegebenen Winkels zurueck.
   - **sqrt (a)** - Gibt die Quadratwurzel der angegebenen Zahl zurueck.
   - **tan(a)** - Gibt den Tangens des angegebenen Winkels zurueck.
   - **truncate(a)** - Berechnet den ganzzahligen Anteil der angegebenen Zahl.
4. Geben Sie die mathematische Operation ein, mit der der Index berechnet werden soll. ![hydra index sec 01](../../../images/hydra_index_sec_01.png)
5. Klicken Sie danach auf der Registerkarte **Common** auf [Candles](../working_with_data/view_and_export/candles.md), waehlen Sie das erstellte **Index**-Instrument und den Datenzeitraum aus, setzen Sie im Feld **Create From:** den Wert **Composite Element** und klicken Sie dann auf ![hydra find](../../../images/hydra_find.png). ![hydra index candle](../../../images/hydra_index_candle.png)

Die erzeugten Daten koennen in die Formate Excel, XML oder TXT exportiert werden. Der Export erfolgt ueber die Dropdown-Liste.

![hydra export](../../../images/hydra_export.png)
