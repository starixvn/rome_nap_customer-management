using System;
using System.Web.Mvc;
using System.Text;
using System.Diagnostics;
using System.Net;
using ONEbook.UM.AspNetMVC.Filters;

namespace Fanex.NAP.SignIn
{
    [AllowAnonymous]
    public class HealthCheckController : Controller
    {
        private static bool alive = true;

        [HttpGet]
        [SkipUMFilter]
        public ActionResult Index()
        {
            if (alive)
            {
                return Content(Environment.MachineName);
            }
            
            Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return Content("Alive is false");
        }

        [HttpGet]
        [SkipUMFilter]
        public ActionResult Kill()
        {
            alive = false;
            return Content($"{Environment.MachineName} - Alive: {alive}");
        }

        [HttpGet]
        [SkipUMFilter]
        public ActionResult Redis()
        {
            Session["Now"] = DateTimeOffset.Now.ToString();
            return Content(Session["Now"]?.ToString() ?? string.Empty);
        }

        [HttpGet]
        [SkipUMFilter]
        public ActionResult Gc()
        {
            const int megabyte = 1024 * 1024;
            var stringBuilder = new StringBuilder();

            var sw = Stopwatch.StartNew();
            sw.Start();

            using (var currentProcess = Process.GetCurrentProcess())
            {
                currentProcess.Refresh();
                var memory = currentProcess.WorkingSet64 / megabyte;
                var heapSize = GC.GetTotalMemory(false) / megabyte;

                stringBuilder.Append($"Before GC.Collect: WorkingSet64 = {memory} Mb. HeapSize = {heapSize} Mb.\r\n");

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();

                currentProcess.Refresh();
                memory = currentProcess.WorkingSet64 / megabyte;
                heapSize = GC.GetTotalMemory(false) / megabyte;

                var duration = sw.ElapsedMilliseconds;
                stringBuilder.Append($"After GC.Collect. WorkingSet64 = {memory} Mb. HeapSize = {heapSize}.\r\n");
                stringBuilder.Append($"Duration: {duration} milliseconds.\r\n");
            }

            return Content(stringBuilder.ToString());
        }
    }
}