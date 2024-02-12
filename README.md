# Recipe Book



### By Gabriel Tucker, Noah Kise, Teddy Peterschmidt



## Description

an app that allows users to keep track of recipes

## Technologies Used

* C#
* .NET 6 SDK
* Entity Framework Core


## Setup/Installation Requirements

* Clone this repository.
* If needed, download and configure MySQL Workbench for your operating system by following the instructions in [this lesson.](https://full-time.learnhowtoprogram.com/c-and-net/getting-started-with-c/installing-and-configuring-mysql) 
* Navigate to the production directory Recipe.
* Within the production directory "Recipe", create a new file called `appsettings.json`.
* Within `appsettings.json`, put in the following code, replacing the `database`, `uid`, and `pwd` values with your own username and password for MySQL.
* Or use sql dump file
```json  
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Port=3306;database=[DATABASE-NAME];uid=[YOUR-USERNAME-HERE];pwd=[YOUR-PASSWORD-HERE];"
  }
}
```
* In the command line, run "dotnet restore" to download and install packages.
* If needed, add `dotnet-ef` to your device by running "dotnet tool install --global dotnet-ef --version 6.0.0"
* In the command line run "dotnet ef database update" to update your database.
* In the command line, run the command "dotnet run" to compile and execute the application.
* Optionally, you can run "dotnet build" to compile this application without running it.

## Known Bugs