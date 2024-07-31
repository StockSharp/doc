# OAuth авторизация

Для использования OAuth авторизации в коннекторах в вашей программе необходимо зарегистрировать несколько сервисов:

```csharp
// Подключение хранилища паролей для доступа на StockSharp WebAPI.
// Требуется предварительная авторизация. Например, через Installer.
ConfigManager.RegisterService<ICredentialsProvider>(new DefaultCredentialsProvider());

// Подключение провайдера сервисов для доступа к StockSharp WebAPI
ConfigManager.RegisterService<IApiServiceProvider>(new ApiServiceProvider());
                           
// Сервис OAuth авторизации, который будут использовать коннекторы
ConfigManager.RegisterService<IOAuthProvider>(new OAuthProvider());
```

- [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) реализует интерфейс [ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider)
- [ApiServiceProvider](xref:StockSharp.Web.Api.Client.ApiServiceProvider) реализует интерфейс [IApiServiceProvider](xref:StockSharp.Web.Api.Client.IApiServiceProvider)
- `OAuthProvider` реализует интерфейс [IOAuthProvider](xref:Ecng.Net.IOAuthProvider)

Для [IOAuthProvider](xref:Ecng.Net.IOAuthProvider) существует два варианта реализации:

1. [WebApiOAuthProvider](xref:StockSharp.Studio.WebApi.WebApiOAuthProvider) - для консольных приложений, где не требуется показ окна авторизации.

2. [OAuthProvider](xref:StockSharp.Studio.Controls.OAuthProvider) - для WPF-приложений, где необходимо показать окно авторизации:

   ![OAuth Start](../../../images/oauth_start.png)

Выбор конкретной реализации `OAuthProvider` зависит от типа вашего приложения и требований к процессу авторизации.