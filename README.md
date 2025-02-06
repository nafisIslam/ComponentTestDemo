# ComponentTestDemo

This Repo shows an example to write ComponentTest For Apis using F# and [Execto](https://github.com/haf/expecto) library.

## Prerequisites
1.	.NET SDK: Ensure you have the .NET 8 SDK installed.
2.	SQL Server: Ensure you have SQL Server installed and running.

## Configure the Database
1.	Open appsettings.json and update the DefaultConnection string with your SQL Server details(Create a new database from SQL server and update the connection string here).

## Restore NuGet Packages
Restore the NuGet packages required for the project
`dotnet restore`

## Apply Migrations
Apply the Entity Framework Core migrations to set up the database schema.
`dotnet ef database update --project ComponentTestDemo`

## Build the Project
Build the project to ensure all dependencies are correctly set up
`dotnet build`

## Run the Project 
Open 2 Cmds and run followings

`dotnet run --project ComponentTestDemo.Api`

`dotnet run --project ComponentTestDemo.Test`

## Explore the code base for both Api and Test