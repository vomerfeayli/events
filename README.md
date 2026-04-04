Для запуска выполнить:
```
dotnet build
```
```
dotnet run --project Events.WebApi --launch-profile https
```

Приложение будет жоступно по этим адресам:  
[https://localhost:7262](https://localhost:7262)  
[http://localhost:5033](http://localhost:5033)  
Если запускать без флага ```--launch-profile https```, то приложение будет доступно только для http на порту 5033

Swagger доступен по этим адресам:  
[https://localhost:7262/swagger](https://localhost:7262/swagger)  
[http://localhost:5033/swagger](http://localhost:5033/swagger)  