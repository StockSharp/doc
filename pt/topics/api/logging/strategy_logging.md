# Registo de estratégia

A classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) implementa a interface [ILogSource](xref:Ecng.Logging.ILogSource). Por isso, as estratégias podem ser passadas para [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources), e todas as suas mensagens chegarão automaticamente a [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners).

## Pré-requisitos

[Estratégias de negociação](../strategies.md)

## Registo num ficheiro de teste

1. Primeiro, tem de criar o gestor especial:

   ```cs
   var logManager = new LogManager();
   ```
2. Depois, tem de criar um registador de ficheiro, passando-lhe o nome do ficheiro, e adicioná-lo a [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners):

   ```cs
   var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
   logManager.Listeners.Add(fileListener);
   ```
3. Para registar mensagens, tem de adicionar uma estratégia a [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources):

   ```cs
   logManager.Sources.Add(lkohSmaStrategy);
   ```
4. Depois de adicionar a estratégia ao gestor de registo, todas as suas mensagens serão gravadas no ficheiro.

## Reprodução de som

1. Criar um registador e passar-lhe o nome do ficheiro de som:

   ```cs
   var soundListener = new SoundLogListener("error.mp3");
   						
   logManager.Listeners.Add(soundListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Definir o filtro para que o som seja reproduzido apenas quando o tipo de mensagem for [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error):

   ```cs
   soundListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   ```

## Envio de email

1. Crie o registador e passe-lhe os parâmetros para as mensagens enviadas:

   ```cs
   var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
   logManager.Listeners.Add(emailListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Definir o filtro para o envio de mensagens dos tipos [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) e [LogLevels.Warning](xref:Ecng.Logging.LogLevels.Warning):

   ```cs
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Warning);
   ```

## Registo na LogWindow

1. Criar o registador [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener):

   ```cs
    // cada estratégia terá a sua própria janela
   var guiListener = new GuiLogListener();
   logManager.Listeners.Add(guiListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Esta é a janela de registo quando a estratégia está a funcionar: ![Captura de tela de registo de estratégia](../../../images/strategy_logging.png)

## Conteúdo recomendado

[Componentes visuais de registo](../graphical_user_interface/logging.md)
