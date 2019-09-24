# ExecutionControl

· Description:

Tool for define and control backend processes.
ExecutionControl allow an application to manage processes by defining using an Enum Type.



· Getting started:

1. 
	Install-Package ChustaSoft.Tools.ExecutionControl
	dotnet add package ChustaSoft.Tools.ExecutionControl
	paket add ChustaSoft.Tools.ExecutionControl
	
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

	a) Inject IExecutionService on constructor, in the example below, we are using Guid type as PK (recommended)
		private readonly IExecutionService<Guid, TestDefinedProcesses> executionService;
		
	b) Call Execute method, specifying the Enum member as the Process name for the executed method, at this point we have two different possibilities, in both cases, the execution management is done by the tool:
	
		1) Simpliest way:
		
			- Define the process method:
			
			public bool TestProcess1Method()
			{
				//Process logic goes there

				return true;
			}
			
			- Run it using IExecutionService
			
			executionService.Execute(TestDefinedProcesses.Process1, () => TestProcess1Method());
			
			
		2) Having context for adding checkpoint events. 
		
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
			
		
Thanks for using ChustaSoft ExecutionControl in your project. Feel free to contribute, more info in issues section, ChustaSoft Twitter account oe contact us via email

-	https://twitter.com/ChustaSoft
		