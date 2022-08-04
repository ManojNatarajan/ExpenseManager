
MS_ConsoleLogger uses below packages: 
	<PackageReference Include="Microsoft.Extensions.Logging" Version="6.0.0" />
	<PackageReference Include="Microsoft.Extensions.Logging.Console" Version="6.0.0" />

Serilog_Logger and Serilog_Logger<T> uses below packages: 
(Note: Config driven)
	<PackageReference Include="Serilog.AspNetCore" Version="5.0.0" />
	<PackageReference Include="Serilog.Enrichers.Environment" Version="2.2.0" />
	<PackageReference Include="Serilog.Enrichers.Process" Version="2.0.2" />
	<PackageReference Include="Serilog.Enrichers.Thread" Version="3.1.0" />
	<PackageReference Include="Serilog.Settings.Configuration" Version="3.3.0" />

Serilog_Logger_Alt uses below package: 
	<PackageReference Include="Serilog.Extensions.Logging" Version="3.1.0" />

--------------------------------------------------------------------------------------------------

can be used by injecting any of the below classes
//builder.Services.AddSingleton((IServiceProvider arg) => "ExpenseManager");
//builder.Services.AddSingleton<GE.ILogger, GE.Serilog_Logger>();
//builder.Services.AddSingleton<GE.ILogger, GE.Serilog_Logger<UserController>>();
//builder.Services.AddSingleton<GE.ILogger, GE.Serilog_Logger_Alt>(); //not recommended as it does not follow the generic pattern and description not included in log statement
//builder.Services.AddSingleton<GE.ILogger, GE.MS_ConsoleLogger>();

--------------------------------------------------------------------------------------------------
Example Log Statements: 
2022-07-03T20:05:56.6392065+05:30 [INF] (ExpenseManager) UserController: Getting All Users!
2022-07-03T20:05:56.8125859+05:30 [INF] (ExpenseManager) Expenses.DAL.Repo.Repository.UserRepository(Expenses.Domain.Model.Models.UserDTO):GetAllEntityList
2022-07-03T20:05:56.8156159+05:30 [INF] (ExpenseManager) Expenses.DAL.Models.User: GetAll

