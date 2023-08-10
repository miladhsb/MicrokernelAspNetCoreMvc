using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin01.Controllers
{
    public class SamplePlugin01Controller: Controller
    {
        private readonly ILogger<SamplePlugin01Controller> _logger;

        public SamplePlugin01Controller(ILogger<SamplePlugin01Controller> logger)
        {
            this._logger = logger;
        }


        public IActionResult Configure()
        {
            return View();
        }
    }
}
