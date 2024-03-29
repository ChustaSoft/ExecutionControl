# ExecutionControl

![GitHub](https://img.shields.io/github/license/ChustaSoft/ExecutionControl)


## Packages table
| Package                                           | Status                                                                    | Pipeline                                                                                                                                                                                                                                                                                         |  NuGet version                                                                                                                                                             |    Downloads                                                                                                      |
|---------------------------------------------------|---------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------|
| ChustaSoft.Tools.ExecutionControl                 | ![](https://img.shields.io/badge/-production--ready-green)                | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/ExecutionControl/%5BRELEASE%5D%20-%20ChustaSoft%20ExecutionControl%20(NuGet)?branchName=main)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=12&branchName=main)            | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.ExecutionControl)](https://www.nuget.org/packages/ChustaSoft.Tools.ExecutionControl)                             | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.ExecutionControl?style=for-the-badge)                   |
| ChustaSoft.Tools.ExecutionControl.SqlServer       | ![](https://img.shields.io/badge/-production--ready-green)                | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/ExecutionControl/%5BRELEASE%5D%20-%20ChustaSoft%20ExecutionControl.SqlServer%20(NuGet)?branchName=main)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=40&branchName=main)  | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.ExecutionControl.SqlServer)](https://www.nuget.org/packages/ChustaSoft.Tools.ExecutionControl.SqlServer)         | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.ExecutionControl.SqlServer?style=for-the-badge)         |
| ChustaSoft.Tools.ExecutionControl.MySql           | ![](https://img.shields.io/badge/-production--ready-green)                | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/ExecutionControl/%5BRELEASE%5D%20-%20ChustaSoft%20ExecutionControl.MySql%20(NuGet)?branchName=main)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=41&branchName=main)      | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.ExecutionControl.MySql)](https://www.nuget.org/packages/ChustaSoft.Tools.ExecutionControl.MySql)                 | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.ExecutionControl.MySql?style=for-the-badge)             |

### Packages compatibility table

| Framework              | SqlServer From   | SqlServer Latest   | MySql/MariaDb From   | MySql/MariaDb Latest  |
|------------------------|------------------|--------------------|----------------------|-----------------------|
| .Net Core 2.1          | 1.0.0            | :heavy_check_mark: | :x:                  | :x:                   |
| .Net Core 2.2          | 1.0.0            | 1.3.0 (Main pck)   | :x:                  | :x:                   |
| .Net Core 3.0          | 1.0.0            | 1.3.0 (Main pck)   | :x:                  | :x:                   |
| .Net Core 3.1          | 1.0.0            | :heavy_check_mark: | 1.0.0                | :heavy_check_mark:    | 
| .Net 5.0               | 1.0.0            | :heavy_check_mark: | 1.0.0                | :heavy_check_mark:    | 
| .Net 6.0               | 1.1.0            | :heavy_check_mark: | 1.1.0                | :heavy_check_mark:    | 


## Description:

Tool for define and control backend processes.
ExecutionControl allow an application to manage processes by defining using an Enum Type.



## Getting started:

1. 
	Install the required package depending on you infrastructure (MySql or SqlServer)
	- _Install-Package ChustaSoft.Tools.ExecutionControl.SqlServer_ or _Install-Package ChustaSoft.Tools.ExecutionControl.MySql_
	- _dotnet add package ChustaSoft.Tools.ExecutionControl.SqlServer_ or _dotnet add package ChustaSoft.Tools.ExecutionControl.MySql_
	- _paket add ChustaSoft.Tools.ExecutionControl.SqlServer_ or _paket add ChustaSoft.Tools.ExecutionControl.MySql_
	
2. Define Enum Type. Enum members must correspond to an specific process inside the solution

	Example:
	public enum TestDefinedProcesses
	{
		[Description("Execution Process 1")]
		Process1,
		[Description("Execution Process 2")]
		Process2,
		[Description("Execution Process 3")]
		Process3
	}

	At this step, define enum members is mandatory, but not the Description. If Description attribute is added, ExecutionControl will generate the different ProcessDefinition with a custom description in DB, otherwise, the Description will be the same than the name.


3. Configure NuGet on Startup:
	
	· On ConfigureServices method:
	
	services.RegisterExecutionControl<TestDefinedProcesses>(Configuration.GetConnectionString("Connection"));
	
	a) The configuration requires to specify the Enum type (TestDefinedProcesses in the example above)
	b) As main parameter, the built-in connection string is mandatory, the tool will generate automatically de DB Schema if you add the configuration step on Configuration Startup method
	c) By default, the tool specifies 60 minutes to abort a process, to specify a different value, it is possible to add it as second parameter (10 minutes in the example below):
		services.RegisterExecutionControl<TestDefinedProcesses>(Configuration.GetConnectionString("Connection"), 10);
		
	· On Configure method:
	
	app.ConfigureExecutionControl<TestDefinedProcesses>(serviceProvider);
	
	a) As mentioned on previous point, section b, this line will automatically deploy de DB Schema on the selected database, and also will load the Process Definitions depending on the Enum type
	
	
4. Inside the service, to manage the execution, the following steps are needed:

	a. Inject IExecutionService on constructor, in the example below, we are using Guid type as PK (recommended)
		private readonly IExecutionService<Guid, TestDefinedProcesses> executionService;
		
	b. Call Execute method, specifying the Enum member as the Process name for the executed method, at this point we have two different possibilities, in both cases, the execution management is done by the tool:
	
		a. Simpliest way:
		
			- Define the process method:
			
			public bool TestProcess1Method()
			{
				//Process logic goes there

				return true;
			}
			
			- Run it using IExecutionService
			
			executionService.Execute(TestDefinedProcesses.Process1, () => TestProcess1Method());
			
			
		b. Having context for adding checkpoint events. 
		
			- Define the process method:
			
			public bool TestProcess2Method(ExecutionContext<Guid> executionContext)
			{
				//Process logic goes there

				return true;
			}
			
			- Run it using IExecutionService
			
				executionService.Execute(TestDefinedProcesses.Process2, (x) => TestProcess2Method(x));
			
			- By this, during the execution, the process has the possibility to invoke:
			
				executionContext.AddCheckpoint("Test checkpoint");
            executionContext.AddEndSummary("Test process finished overall summary");
	    
	    
	    c. Defining an always running process:
		
			- Add this Decorator to the Process Enum type:
				[ProcessDefinition(ProcessType.Background,  "Background process description")]
			
		        - Define the process method
			
			- Run it using IExecutionService, in the example, also with context
			
				executionService.Execute(ProcessExamplesEnum.BackgroundTestProcess, (ec) => TestAlwaysRunning(ec));
			

*Thanks for using and contributing*
---

		
Thanks for using ChustaSoft ExecutionControl in your project. Feel free to contribute, more info in issues section, ChustaSoft Twitter account or contact us via email

[![Twitter Follow](https://img.shields.io/twitter/follow/ChustaSoft?label=Follow%20us&style=social)](https://twitter.com/ChustaSoft)
