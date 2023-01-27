# Weather API

## Prerequisites
Before you begin, ensure you have installed the following
* Install the .NET Core `SDK 6`. Download link can be found [here](https://docs.microsoft.com/en-us/dotnet/core/install/sdk?pivots=os-windows) - Choose an operating system of your choice to download SDK.
* Install the `ASP.NET Core Runtime 6`. Download link can be found [here](https://dotnet.microsoft.com/download/dotnet-core) - Choose runtime for your OS

## Install development tools. 
Visual Studio 2022 community edition or Visaul Code is the preferred IDE
* Windows: Download `Viual Studio 2022` community or `Visual Studio Code` [here](https://visualstudio.microsoft.com/downloads/?utm_medium=microsoft&utm_source=docs.microsoft.com&utm_campaign=button+cta&utm_content=download+vs2019)
* Mac or Linux: Download `Visual Studio Code` [here](https://visualstudio.microsoft.com/downloads/?utm_medium=microsoft&utm_source=docs.microsoft.com&utm_campaign=button+cta&utm_content=download+vs2019) You can also use rider
* The installation guide for Visual Studio can be followed [here](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019)
* The setting up of Visual Studio Code can be followed [here](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019)


## Setup project

1. Clone the repository
	`git clone https://github.com/TomisinFagbola/weather-api`
2. Create appsettings.Development.json and add the following 
3. 
   ```json
   {
        "ConnectionStrings": {
            "DefaultConnection": "Server=;Database=;User Id=;Password=;" 
            "DefaultConnection1": "Server=;Database=;Trusted_Connection=True;" 
            
        },
   
        "SENDGRID_KEY": "", 
        "SENDER_EMAIL": "" 
   }
   ```

4. `cd API`
5. Run `dotnet restore` to install package dependencies
6. Run `dotnet build` to build the application


## Database Migration
### Entity Framework Core tools
1.  `dotnet ef` must be installed as a global or local tool. Install `dotnet ef` as a global tool with the following command:
    `dotnet tool install --global dotnet-ef`
2. In the terminal change directory into the `API` project directory
3. Apply the migration to the database to create the schema.
    `dotnet ef database update`

Visit this link [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) for more information on using `ef core tool`

## Running the application locally on your machine
Make sure all the above instructions are done completely before running the application
1. If you are using `Visual Studio`
    * Right click on the `API` project and select `Set as Startup Project`
    * Then press the `F5` to run the application or click on the `IIS Express` run button on the standard toolbar to run the application

2. If you are using `Visual Studio Code`
    * In your terminal, change directory into src/API directory where the solution file is located and then run the following: 
        `dotnet restore`
        `dotnet run`
    * The above commands will restore missing application packages, build and then run the application
3. Either ways will make the project web APIs accessible on Swagger through this `http://localhost:5000/swagger/index.html`

## Migrations 
```bash
cd src/API
dotnet ef migrations add [NextMigrationName]
dotnet ef migrations script [LastMigrationName] --idempotent
```

LastMigrationName = Initial
