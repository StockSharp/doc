# Konfiguration E\*TRADE

Um mit dem Connector zu arbeiten, müssen Sie **Benutzername** und **Passwort** angeben. **Benutzername** und **Passwort** werden vom Broker bereitgestellt. Für den API-Zugriff wird empfohlen, den Broker zu kontaktieren.

Der Interaktionsmechanismus ist in dieser Abbildung dargestellt:

![Konfiguration E\TRADE Bildschirmfoto](../../../../../images/etrade.png)

[E\*TRADE](../e_trade.md) verwendet das OAuth-1.0a-Autorisierungsprotokoll, das eine Anmeldung mit Login und Passwort über den Browser auf der Website von [E\*TRADE](https://etrade.com/) erfordert. Die vollständige Abfolge des Autorisierungsverfahrens ist in der folgenden Abbildung dargestellt:

![etrade authorization](../../../../../images/etrade_autoriazation.png)

Ein vollständiges Autorisierungsverfahren sollte nur einmal pro Tag durchgeführt werden (der Server von [E\*TRADE](../e_trade.md) setzt zuvor ausgegebene AccessTokens um Mitternacht EST zurück). Wenn das vollständige Autorisierungsverfahren am aktuellen Tag nach EST bereits ausgeführt wurde, lädt der [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) automatisch das AccessToken, das in einem Unterverzeichnis des [E\*TRADE](../e_trade.md)-Algorithmus gespeichert ist.
