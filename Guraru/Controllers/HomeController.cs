using Guraru.Models;
using GuraruRepository.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Guraru.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GuraruContext _context;

        public HomeController(GuraruContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            GodownWrapper godownWrapper = new GodownWrapper();
            List<GodownStatus> godownStatuses = new List<GodownStatus>();

            var allQualities = _context.RawQualities.ToList();
            foreach(var eachQuality in allQualities)
            {
                var particularQualityThread = _context.RawThreads.Where(m => m.Quality == eachQuality.QualityId);
                int? threadWeightInGodown = particularQualityThread.Where(m=>m.ThWeight!=m.UsedWeight).Sum(a=>(a.ThWeight??0)-(a.UsedWeight??0));
                int? threadWeightProcessed = particularQualityThread.Sum(m => m.UsedWeight);

                var particularQualityOnMachine = _context.ManufacturingStage1s.Where(m => m.Quality==eachQuality.QualityId);
                int ? currentWeightsOnMachines = particularQualityOnMachine.Where(m=>m.CompletedWeight==null||m.CompletedWeight==0).Sum(m => m.SubmittedWeght);
                int? completedOnMachines = particularQualityOnMachine.Where(m=>m.CompletedDate!=null&&m.CompletedWeight!=0).Sum(m=>m.CompletedWeight);

                var particularQualityAtFinal = _context.ManufacturingStage2s.Where(m => m.Quality.Equals(eachQuality.QualityId));
                int? machineProductAtGodown = completedOnMachines - particularQualityAtFinal.Sum(m => m.SubmittedWeight);
                int ? currentWeightOnFinalStage = particularQualityAtFinal.Where(m => m.CompletedWeight == null || m.CompletedWeight == 0).Sum(m => m.SubmittedWeight);
                int? finalCompletedWeight = particularQualityAtFinal.Where(m => m.CompletedWeight != null || m.CompletedWeight != 0).Sum(m => m.CompletedWeight);

                
                int? exitFromGodown = _context.GuraruExits.Where(m=>m.Quality.Equals(eachQuality.QualityId)).Sum(m=>m.ExitWeight);

                int? finalProductAtGodown = finalCompletedWeight - exitFromGodown;

                GodownStatus godownStatus = new GodownStatus();
                godownStatus.Quality = eachQuality;
                godownStatus.ThreadWtGodown = threadWeightInGodown??0;
                godownStatus.ThreadWtprocessed = threadWeightProcessed??0;
                godownStatus.CurrentWeightOnMachine = currentWeightsOnMachines??0;
                godownStatus.MachineCompletedWt = completedOnMachines??0;
                godownStatus.MachineProductGodown = machineProductAtGodown ?? 0;
                godownStatus.CurrentFinalStageWt = currentWeightOnFinalStage ?? 0;
                godownStatus.FinalStageCompleted = finalCompletedWeight??0;
                godownStatus.GodownExit = exitFromGodown??0;
                godownStatus.FinalProductGodown = finalProductAtGodown??0;
                godownStatuses.Add(godownStatus);
            }

            godownWrapper.GodownStatuses = godownStatuses;
            godownWrapper.RawQualities = _context.RawQualities.ToList();

            return View(godownWrapper);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
