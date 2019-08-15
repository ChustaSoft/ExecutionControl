using ChustaSoft.Tools.ExecutionControl.Services;
using ChustaSoft.Tools.ExecutionControl.TestAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.TestAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ProcessController : ControllerBase
    {

        private readonly IExecutionService<Guid, ProcessExamplesEnum> executionService;


        public ProcessController(IExecutionService<Guid, ProcessExamplesEnum> executionService)
        {
            this.executionService = executionService;
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> Test()
        {
            executionService.Execute(ProcessExamplesEnum.Process1, () => TestMethod());

            return Ok();
        }


        public bool TestMethod()
        {
            return true;
        }

    }
}
