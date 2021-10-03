using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MakonisApplication.Models;
using Newtonsoft.Json;
using System.IO;

namespace MakonisApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string folderPath = @"D:\temp";
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                   
                    string path = @"D:\temp\file.json";
                                       
                    if (!System.IO.File.Exists(path))                                         
                        System.IO.File.WriteAllText(path, null);
                    
                    List<User> items = new List<User>();                  
                    using (StreamReader r = new StreamReader(path))
                    {
                        string objUser = r.ReadToEnd();
                        if (objUser != "")
                        {
                            items = JsonConvert.DeserializeObject<List<User>>(objUser);
                        }
                    }
                    items.Add(user);

                    string json = JsonConvert.SerializeObject(items);                                        
                    System.IO.File.WriteAllText(path, json);

                    ViewBag.Message = "Successfully submitted";                   
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Failed " + ex.Message;
            }

            ModelState.Clear();
            return View();
        }       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
