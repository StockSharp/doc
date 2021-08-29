# Включение и выключение Quik

Для включения (с автоматической авторизацией) и выключения терминала [Quik](Quik.md) предоставляется [QuikTerminal](xref:StockSharp.Quik.QuikTerminal). Подобный сценарий может потребоваться, если необходимо на ночь выключать терминал, или в случае восстановления соединения (подробнее, в [Настройки переподключения](Reconnect.md)), когда оно было потеряно из\-за закрытия [Quik](Quik.md) (в нем произошла ошибка). 

### Варианты использования:

1. Включение [Quik](Quik.md) (с проверкой, запущен ли уже через свойство [QuikTerminal.GetTerminals](xref:StockSharp.Quik.QuikTerminal.GetTerminals)): 

   ```cs
   Console.Write("Введите путь к директории с Quik: ");
   var path = Console.ReadLine();
   var terminal = QuikTerminal.Get(path);
   if (!terminal.IsLaunched)
   {
   	Console.WriteLine("Запускается Quik...");
   	terminal.Launch();
   	Console.WriteLine("Quik запущен.");
   }
   else
   	Console.WriteLine("Quik найден.");
   ```
2. После включения [Quik](Quik.md) (запустится его процесс), можно производить авторизацию к серверу брокера через метод [Login](xref:StockSharp.Quik.QuikTerminal.Login): 

   ```cs
   Console.Write("Введите логин: ");
   var login = Console.ReadLine();
   Console.Write("Введите пароль: ");
   var password = Console.ReadLine();
   if (!terminal.IsConnected)
   {
   	terminal.Login(login, password);
   	Console.WriteLine("Авторизация произведена.");
   }
   ```
3. Для выключения [Quik](Quik.md) (например, по окончанию работы робота), делается следующее: 

   ```cs
   terminal.Logout();
   Console.WriteLine("Quik отключен от торговли.");
   terminal.Exit();
   Console.WriteLine("Quik выключен.");
   ```

### Следующие шаги

[Управление окном Quik](QuikWindow.md)
