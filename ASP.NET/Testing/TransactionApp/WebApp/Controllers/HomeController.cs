using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BitPayLight;
using BitPayLight.Models;
using BitPayLight.Models.Bill;
using BitPayLight.Models.Invoice;
using BitPayLight.Models.Rate;
using DataLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private IDataAccess _data;
        private IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IDataAccess data, IWebHostEnvironment environment)
        {
            _logger = logger;
            _configuration = configuration;
            _data = data;
            _environment = environment;
        }

        public string getConnectionString(string name = "Default")
        {
            string connectionString = _configuration.GetConnectionString(name);
            return connectionString;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListUsersAsync()
        {
            string sql = "SELECT * FROM users";
            string connectionString = getConnectionString();

            List<UserModel> users = await _data.LoadData<UserModel, dynamic>(sql, new { }, connectionString);
            foreach (var x in users)
            {
                var uid = x.uid;
                var name = x.name;
                Debug.WriteLine("uid: " + uid + " - name: " + name);
            }

            ListUsersModel pageModel = new ListUsersModel()
            {
                users = users
            };

            return View(pageModel);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(UserModel user)
        {
            if (ModelState.IsValid)
            {
                string sql = @"INSERT INTO users (uid, name) VALUES (@uid, @name)";
                string connectionString = getConnectionString();

                await _data.SaveData(sql, user, connectionString);

                return RedirectToAction("ListUsers");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Bitpay()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Bitpay(string email)
        {
            var bitpayToken = _configuration["bitpayToken"];
            BitPay bitpay = new BitPay(token: bitpayToken, environment: Env.Test);

            var buyerData = new Buyer()
            {
                Email = email,
                Notify = true
            };

            var baseUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.Path);
            var invoice = new Invoice()
            {
                Price = 1,
                Currency = Currency.USD,
                PaymentCurrencies = new List<string> {
                    Currency.BTC,
                    Currency.BCH,
                    Currency.ETH
                },
                Buyer = buyerData,
                RedirectUrl = baseUrl                
            };

            invoice = bitpay.CreateInvoice(invoice).Result;

            return Redirect(invoice.Url);
        }

        private Contract getContract()
        {
            var web3 = new Web3("http://127.0.0.1:7545");
            var jsonString = System.IO.File.ReadAllText(Path.Combine(_environment.WebRootPath, "Contracts", "SimpleStorage.json")); ;
            var json = JObject.Parse(jsonString);
            var abi = json["abi"].ToString(Formatting.None);
            var address = json["networks"]["5777"]["address"].ToString();
            var SimpleContract = web3.Eth.GetContract(abi, address);
            return SimpleContract;
        }

        public async Task<IActionResult> EthereumAsync()
        {
            var SimpleContract = getContract();
            var get = SimpleContract.GetFunction("get");

            var number = await get.CallAsync<uint>();
            Debug.WriteLine("Value: " + number);

            EthereumModel pageModel = new EthereumModel()
            {
                number = (int)number
            };

            return View(pageModel);
        }

        [HttpPost]
        public async Task<IActionResult> EthereumAsync(int newNumber)
        {
            var SimpleContract = getContract();
            var get = SimpleContract.GetFunction("get");
            var set = SimpleContract.GetFunction("set");

            var account = "0xf2600db53C0281d6c4761b55846Bbd86D8e1C97D";            
            var gas = await set.EstimateGasAsync(newNumber);
            var result = await set.SendTransactionAndWaitForReceiptAsync(account, gas, null, null, null, newNumber);

            var number = await get.CallAsync<uint>();
            Debug.WriteLine("New Value: " + number);

            EthereumModel pageModel = new EthereumModel()
            {
                number = (int)number
            };

            return View(pageModel);
        }
    }
}
