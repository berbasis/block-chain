using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KTPBlockChainClient.Models;
using System.Net.Http;  
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace KTPBlockChainClient.Controllers
{
    public class KTPController : Controller
    {
        List<string> NodesUrl = new List<string>(new string[] { "https://192.168.100.83:44359/", "https://192.168.100.108:44359/" }); //insert address of all nodes here

        private async Task<string> RequestNode(string node, string request)
        {
            var EmpResponse = "";
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(node);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync(request);

                    if (Res.IsSuccessStatusCode)
                    {
                        EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            return EmpResponse;
        }
        public async Task<ActionResult> Index()
        {
            string response = await RequestNode(NodesUrl[0], "api/KTPBlockChain");
            List <KTP> ktps = JsonConvert.DeserializeObject<List<KTP>>(response.ToString());
            return View(ktps);
        }

        public async Task<ActionResult> Details(int id)
        {
            string response = await RequestNode(NodesUrl[0], $"api/KTPBlockChain/{id + 1}");
            KTP ktp = JsonConvert.DeserializeObject<KTP>(response.ToString());
            if (ktp == null || ktp.ID == 1)
            {
                return RedirectToAction("Index", "KTP");
            }
            return View(ktp);
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Submit([Bind("ID,NIK,Nama,TempatLahir,TanggalLahir,JenisKelamin,Alamat,Agama,StatusKawin,Pekerjaan,Kewarganegaraan,BerlakuHingga,Foto,TimeStamp")] KTP kTP)
        {
            kTP.TimeStamp = DateTime.Now;
            var json = JsonConvert.SerializeObject(kTP);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            foreach (var node in NodesUrl)
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(node);
                        HttpResponseMessage Res = await client.PostAsync("api/KTPBlockChain", data);
                        string result = Res.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            return RedirectToAction("Index", "KTP");
        }
    }
}