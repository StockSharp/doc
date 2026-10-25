# Shell

**Shell** ist ein fertiges grafisches Framework eines Handelsterminals mit vollständig offenem C#-Quellcode. Es ist mit [Avalonia](https://avaloniaui.net/) gebaut und läuft unter Windows, Linux und macOS.

Sie müssen keine Zeit mit der Erstellung einer GUI (grafische Benutzeroberfläche) verbringen: die Verbindung zu Datenquellen, Tabellen für Portfolios, Instrumente, Orders und Trades, Charts, Tests auf historischen Daten, die Optimierung von Parametern und das Arbeitsprotokoll sind bereits enthalten. Es bleibt nur, die Strategie selbst zu schreiben.

![Shell Bildschirmfoto](../images/shell_title_00.png)

## Hauptfunktionen

1. Vollständiger Quellcode, geeignet für eigene Handelsroboter und individuelle Lösungen.
2. **Unterstützung für mehr als 70 Verbindungen** zu Börsen und Brokern über [Konnektoren](api/connectors.md).
3. Vier Arbeitsbereiche, zwischen denen die Seitenleiste umschaltet: [Allgemein](shell/user_interface/common.md), [Echtzeit](shell/user_interface/real_time.md), [Emulation](shell/user_interface/emulation.md) und [Protokolle](shell/user_interface/log.md).
4. Eine flexible Oberfläche: Alle Panels innerhalb eines Arbeitsbereichs können verschoben, geteilt, in ein eigenes Fenster abgekoppelt und wieder angedockt werden. Die Anordnung wird zwischen den Starts gespeichert.
5. Tests von Strategien auf historischen Daten mit Statistik, Trade-Chart und Equity-Kurve.
6. [Optimierung](shell/user_interface/optimization.md) - Durchlaufen der Strategieparameter auf derselben Historie.
7. Speichern und Wiederherstellen von Strategieeinstellungen, einschließlich der Risikoregeln.
8. Paralleler Start mehrerer Strategien: Jede lebt ihr eigenes Leben und hat eigene Orders, Trades und Statistiken.
9. Start von Strategien, die im [Designer](designer.md) erstellt wurden, ohne Code zu schreiben.
10. [Fernsteuerung](shell/remote_control.md) - Starten und Stoppen von Strategien über die Website oder den Telegram-Bot.

## Mitgelieferte Strategien

Shell wird mit vier Strategien ausgeliefert, die direkt nach dem Start ausgeführt werden können:

| Strategie | Was sie tut |
|---|---|
| **SMA-Strategie** | Kreuzung des kurzen und des langen gleitenden Durchschnitts |
| **SMA-Index-Strategie** | dasselbe, aber auf einem synthetischen Instrument, das durch einen Ausdruck definiert ist |
| **StairsTrend-Strategie** | Einstieg mit dem Trend nach mehreren Kerzen in Folge in eine Richtung |
| **StairsCountertrend-Strategie** | Einstieg gegen den Trend in derselben Situation |

## Empfohlene Inhalte

[Shell installieren](shell/installing_shell.md)

[Schnellstart](shell/quick_start.md)
