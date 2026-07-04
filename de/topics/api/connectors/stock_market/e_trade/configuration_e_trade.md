# Konfiguration E\*TRADE

Um mit dem Connector zu arbeiten, muessen Sie **Login** und **Password** angeben. **Login** und **Password** werden vom Broker bereitgestellt. Fuer den API-Zugriff wird empfohlen, den Broker zu kontaktieren.

Der Interaktionsmechanismus ist in dieser Abbildung dargestellt:

![ETrade](../../../../../images/etrade.png)

[E\*TRADE](../e_trade.md) verwendet das OAuth-1.0a-Autorisierungsprotokoll, das eine Anmeldung mit Login und Passwort ueber den Browser auf der Website von [E\*TRADE](https://etrade.com/) erfordert. Die vollstaendige Abfolge des Autorisierungsverfahrens ist in der folgenden Abbildung dargestellt:

![etrade authorization](../../../../../images/etrade_autoriazation.png)

Ein vollstaendiges Autorisierungsverfahren sollte nur einmal pro Tag durchgefuehrt werden (der Server von [E\*TRADE](../e_trade.md) setzt zuvor ausgegebene AccessTokens um Mitternacht EST zurueck). Wenn das vollstaendige Autorisierungsverfahren am aktuellen Tag nach EST bereits ausgefuehrt wurde, laedt der [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) automatisch das AccessToken, das in einem Unterverzeichnis des [E\*TRADE](../e_trade.md)-Algorithmus gespeichert ist.
