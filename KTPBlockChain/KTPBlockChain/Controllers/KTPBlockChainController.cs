using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KTPBlockChain.Models;
using Hangfire;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace KTPBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KTPBlockChainController : ControllerBase
    {
        private readonly List<string> OtherNodes = new List<string>(new string[] { "node1", "node2" }); //insert address of other nodes here
        private readonly KTPBlockChainContext _context;

        public KTPBlockChainController(KTPBlockChainContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KTP>>> GetKTP()
        {
            if (IsValid() == false)
                await Restore();
            return await _context.KTP.Where(t => t.ID != 1).ToListAsync();
        }

        [HttpGet("{id}")]
        public KTP GetKTP(int id)
        {
            return _context.KTP.FirstOrDefault(m => m.ID == id);
        }

        [HttpGet("GetAllBlocks")]
        public List<Block> GetAllBlocks()
        {
            List<Block> blocks = _context.Block.ToList();
            return blocks;
        }

        [HttpGet("GetAllKTPs")]
        public List<KTP> GetAllKTPs()
        {
            List<KTP> ktps = _context.KTP.ToList();
            return ktps;
        }

        [HttpGet("IsValid")]
        public bool IsValid()
        {
            List<Block> blocks = GetAllBlocks();
            for (int i = 1; i < blocks.Count; i++)
            {
                if (!IsValidHash(blocks[i]) || !IsValidPrevHash(blocks[i], blocks[i - 1]) || !blocks[i].IsValidHash() || !IsValidDataHash(blocks[i]))
                    return false;
            }
            return true;
        }

        [HttpGet("Mine")]
        public async Task Mine()
        {
            Pool pool = GetFirstPool();
            if (pool != null)
            {
                int id = GetLastBlockId() + 1;
                if (GetLastKTP().TimeStamp < pool.TimeStamp)
                {
                    KTP ktp = new KTP(id, pool.NIK, pool.Nama, pool.TempatLahir, pool.TanggalLahir, pool.JenisKelamin, pool.Alamat, pool.Agama, pool.StatusKawin, pool.Pekerjaan, pool.Kewarganegaraan, pool.BerlakuHingga, pool.Foto, pool.TimeStamp);
                    Block block = CreateBlock(ktp);
                    if (GetLastKTP().TimeStamp < pool.TimeStamp)
                    {
                        foreach (string node in OtherNodes)
                        {
                            string jsonBlock = JsonConvert.SerializeObject(block);
                            await PostOtherNode(node, jsonBlock, "api/KTPBlockChain/AddBlock");
                            string jsonKtp = JsonConvert.SerializeObject(ktp);
                            await PostOtherNode(node, jsonKtp, "api/KTPBlockChain/AddKTP");
                        }
                        await PostKTP(ktp);
                        await PostBlock(block);
                    }
                }
                _context.Pool.Remove(GetFirstPool());
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Pool>> PostPool(Pool pool)
        {
            _context.Pool.Add(pool);
            await _context.SaveChangesAsync();
            BackgroundJob.Enqueue(() => Mine());

            return Ok();
        }

        [HttpPost("Addblock")]
        public async Task<ActionResult<Block>> PostBlock(Block block)
        {
            if(!_context.Block.Where(m => m.DataHash == block.DataHash).Any())
            {
                _context.Block.Add(block);
                await _context.SaveChangesAsync();
            }
         
            return Ok();
        }

        [HttpPost("AddKTP")]
        public async Task<ActionResult<KTP>> PostKTP(KTP ktp)
        {
            if (GetLastKTP().TimeStamp < ktp.TimeStamp)
            {
                _context.KTP.Add(ktp);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        private Block CreateBlock(KTP ktp)
        {
            string prevhash = (from n in _context.Block orderby n.Id descending select n.Hash).FirstOrDefault();

            Block block = new Block
            {
                Id = GetLastBlockId() + 1,
                TimeStamp = DateTime.Now,
                DataHash = ktp.CreateHash(),
                PrevHash = prevhash
            };
            block.Mine();
            return block;
        }

        private async Task<string> GetOtherNode(string node, string request)
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
                        EmpResponse = Res.Content.ReadAsStringAsync().Result;
                }
            }
            return EmpResponse;
        }

        private async Task<string> PostOtherNode(string node, string json, string request)
        {
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string result;

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(node);
                    HttpResponseMessage Res = await client.PostAsync(request, data);
                    result = Res.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }

        private async Task Restore()
        {
            _context.KTP.RemoveRange(_context.KTP);
            _context.Block.RemoveRange(_context.Block);
            _context.Pool.RemoveRange(_context.Pool);
            await _context.SaveChangesAsync();
            foreach (string node in OtherNodes)
            {
                var response = await GetOtherNode(node, "api/KTPBlockChain/IsValid");

                if (response.ToString() == "true")
                {
                    response = await GetOtherNode(node, "api/KTPBlockChain/GetAllBlocks");
                    List<Block> blocks = JsonConvert.DeserializeObject<List<Block>>(response.ToString());
                    response = await GetOtherNode(node, "api/KTPBlockChain/GetAllKTPs");
                    List<KTP> ktps = JsonConvert.DeserializeObject<List<KTP>>(response.ToString());
                    for (int i = 0; i < blocks.Count; i++)
                    {
                        _context.KTP.Add(ktps[i]);
                        _context.Block.Add(blocks[i]);
                    }
                    await _context.SaveChangesAsync();
                    break;
                }
            }
        }

        private Pool GetFirstPool()
        {
            return _context.Pool.OrderBy(e => e.TimeStamp).FirstOrDefault();
        }

        private KTP GetLastKTP()
        {
            return _context.KTP.OrderByDescending(e => e.ID).FirstOrDefault();
        }

        private int GetLastBlockId()
        {
            return _context.Block.Select(e => e.Id).Count();
        }

        private bool IsValidDataHash(Block block)
        {
            KTP ktp = _context.KTP.Where(e => e.ID == block.Id).FirstOrDefault();
            if (ktp != null)
                return block.DataHash == ktp.CreateHash();
            else
                return false;
        }

        private bool IsValidHash(Block block)
        {
            return block.Hash == block.CreateHash();
        }

        private bool IsValidPrevHash(Block block, Block prevblock)
        {
            if (prevblock != null)
            {
                string prevhash = prevblock.Hash;
                return block.PrevHash == prevhash;
            }
            return true;
        }

    }
}
