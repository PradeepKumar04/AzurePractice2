using AzurePractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzurePractice2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {

        [HttpPost("{id}")]
        public async Task<IActionResult> AddCart(int id)
        {
            Cart cart = new Cart();
            cart.Id = 1;
            cart.MenuItemId = id;
            cart.MenuItemName = await GetName(id);
            cart.UserId = 1;
            return Ok(cart);
        }

        [HttpGet("{id}")]
        public async Task<string> GetName(int id)
        {
            MenuItem item = null;
            string url = String.Format("https://practicecheck.azurewebsites.net/api/MenuItem/GetById/" + id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var responseTask =  client.GetAsync($"{id}");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var result1 = result.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<MenuItem>(result1);
                }
            }
            return item.Name;
        }
    }
}
