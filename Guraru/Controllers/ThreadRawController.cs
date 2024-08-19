using Guraru.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Interface;
using Repository.Models;

namespace Guraru.Controllers
{
    public class ThreadRawController : Controller
    {
        public IRepository<RawThreadDTO> rawThreadRepo { get; set; }
        public ThreadRawController(IRepository<RawThreadDTO> repository)
        {
            rawThreadRepo = repository;
        }
        public IActionResult Index()
        {

            List<RawThreadDTO> rawThreadsDto = rawThreadRepo.GetRecords().ToList();
            List<RawThread> rawThreads = rawThreadsDto.Select(m => new RawThread()
            {
                BillAmount = m.BillAmount,
                BillDate = m.BillDate,
                BillNo = m.BillNo,
                CompanyName = m.CompanyName,
                CreatedBy = m.CreatedBy,
                Quality = m.Quality,
                ThreadWeight = m.ThreadWeight
            }).ToList();
            return View(rawThreads);
        }

        public IActionResult AddThread()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult AddThread(RawThread thread)
        {
            RawThreadDTO rawThreadDTO = new RawThreadDTO()
            {
              ThreadWeight = thread.ThreadWeight,
              CreatedBy = thread.CreatedBy,
              Quality = thread.Quality,
              BillAmount= thread.BillAmount,
              BillDate= thread.BillDate,
              BillNo = thread.BillNo,
              CompanyName = thread.CompanyName,
              CreatedDate = DateTime.Now
            };
            bool isSuccess = rawThreadRepo.CreateRecord(rawThreadDTO);
            return RedirectToAction("Index");
        }

    }
}
