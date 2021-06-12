using ChustaSoft.Tools.ExecutionControl.Model;
using ChustaSoft.Tools.ExecutionControl.Services;
using ChustaSoft.Tools.ExecutionControl.TestAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.ExecutionControl.TestAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ProcessController : ControllerBase
    {

        private readonly IExecutionService<Guid, ProcessExamplesEnum> executionService;
        private readonly IReportingService<Guid> reportingService;


        public ProcessController(IExecutionService<Guid, ProcessExamplesEnum> executionService, IReportingService<Guid> reportingService)
        {
            this.executionService = executionService;
            this.reportingService = reportingService;
        }


        #region Synchronous executions examples

        /// <summary>
        /// Test example for timed scheduleded execution without using context for detailed logging
        /// </summary>
        /// <returns></returns>
        [HttpGet("sync/without-context")]
        public ActionResult<IEnumerable<string>> TestSyncWithoutContext()
        {
            executionService.Execute(ProcessExamplesEnum.Process1, () => TestMethod());

            return Ok();
        }

        /// <summary>
        /// Test example for timed scheduleded execution using context for detailed logging
        /// </summary>
        /// <returns></returns>
        [HttpGet("sync/with-context")]
        public ActionResult<IEnumerable<string>> TestSyncWithContext()
        {
            executionService.Execute(ProcessExamplesEnum.Process2, (ec) => TestMethodContext(ec));

            return Ok();
        }

        /// <summary>
        /// Test example for method background execution (Always running) injecting context for detailed logging
        /// </summary>
        /// <returns></returns>
        [HttpGet("sync/background")]
        public ActionResult<IEnumerable<string>> TestSyncBackground()
        {
            executionService.Execute(ProcessExamplesEnum.BackgroundTestProcess, (ec) => TestAlwaysRunning(ec));

            return Ok();
        }

        #endregion


        #region Asynchronous executions examples

        /// <summary>
        /// Test example for timed scheduleded execution without using context for detailed logging
        /// </summary>
        /// <returns></returns>
        [HttpGet("async/without-context")]
        public async Task<ActionResult<IEnumerable<string>>> TestAsyncWithoutContext()
        {
            var result = await executionService.ExecuteAsync(ProcessExamplesEnum.Process1, () => TestMethod());

            return Ok(result);
        }

        /// <summary>
        /// Test example for timed scheduleded execution using context for detailed logging
        /// </summary>
        /// <returns></returns>
        [HttpGet("async/with-context")]
        public async Task<ActionResult<IEnumerable<string>>> TestAsyncWithContext()
        {
            var result = await executionService.ExecuteAsync(ProcessExamplesEnum.Process2, (ec) => TestMethodContext(ec));

            return Ok(result);
        }

        /// <summary>
        /// Test example for method background execution (Always running) injecting context for detailed logging
        /// </summary>
        /// <returns></returns>
        [HttpGet("async/background")]
        public async Task<ActionResult<IEnumerable<string>>> TestAsyncBackground()
        {
            var result = await executionService.ExecuteAsync(ProcessExamplesEnum.BackgroundTestProcess, (ec) => TestAlwaysRunning(ec));

            return Ok(result);
        }

        #endregion


        private bool TestMethod()
        {
            var allData = reportingService.Daily(DateTime.Now);
            var processData = reportingService.Daily(ProcessExamplesEnum.Process1, DateTime.Now);

            return true;
        }

        private bool TestMethodContext(ExecutionContext<Guid> executionContext)
        {
            executionContext.AddCheckpoint("Test checkpoint");
            executionContext.AddEndSummary("Test process finished overall summary");

            return true;
        }

        private bool TestAlwaysRunning(ExecutionContext<Guid> executionContext)
        {

            return true;
        }

    }
}
