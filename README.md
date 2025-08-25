To run this app, clone it and run it with your prefered editor, eg Visual Studio
Run it via docker compose - you can do that easily from visual studio
for easy testing purpose, I have included the appsettings.json file - won't be there in a normal production codebase
Logs are written to Seq - the credentials are: url: http://localhost:8081/#/login username: admin  password Passw@rd123

Note: I seeded Two default users for testing purpose. Their login credentials are below
User 1: email: victorblaze@gmail.com  password: 123Pa$$word!
User 2: email: victorblaze2010@gmail.com  password: 123Pa$$word!


#The architecture/Setup for this api includes

-.NET 8
-Clean Architecture
-Hangfire for handling backround jobs
-SignalR - uses authenticated SignalR conenctions to send notifications to specific authenticated users
-Caching using Memory Cache
-Seq/Serilog for logging
-Entity Framework Core
-MSSQL
-Docker 
-Unit Tests
-Architecture Tests
-Rate Limiting
-JWT Authentication/Authorization
-CORS

happy testing.
