To run this app, clone it and run it with your prefered editor, eg Visual Studio
Run it via docker compose - you can do that easily from visual studio

for easy testing purpose, I have included the appsettings.json file - I won't include it normally

Logs are written to Seq - the credentials are: 

url: http://localhost:8081/#/login 

username: admin  

password Passw@rd123


Note: I seeded Two default users for testing purpose. Their login credentials are below

User 1: email: victorblaze@gmail.com  password: 123Pa$$word!

User 2: email: victorblaze2010@gmail.com  password: 123Pa$$word!


#The architecture/Setup for this api includes

-.NET 8

-Clean Architecture

-CQRS

-Hangfire for handling backround jobs

-SignalR - used strongly typed hubs, JWT Authentication to make sure messages are sent to specific users

-Caching using Memory Cache

-Seq/Serilog for logging

-Fluent Validation

-Entity Framework Core

-MSSQL

-Docker 

-Unit Tests

-Architecture Tests

-Rate Limiting

-JWT Authentication/Authorization

-CORS

-Global error handling + Problem Details

happy testing.
