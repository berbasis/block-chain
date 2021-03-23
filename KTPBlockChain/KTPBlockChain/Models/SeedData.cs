using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace KTPBlockChain.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new KTPBlockChainContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<KTPBlockChainContext>>()))
            {
                if (context.Block.Any())
                {
                    return;
                }

                context.Block.AddRange(
                    new Block
                    {
                        Id = 1,
                        TimeStamp = DateTime.Now,
                        DataHash = "0",
                        PrevHash = "0",
                        Hash = "0",
                        Nonce = 0
                    }
                );
                context.SaveChanges();

                context.KTP.AddRange(
                    new KTP
                    {
                        ID = 1,
                        NIK = "0",
                        Nama = "0",
                        TempatLahir = "0",
                        TanggalLahir = DateTime.Now,
                        JenisKelamin = "0",
                        Alamat = "0",
                        Agama = "0",
                        StatusKawin = "0",
                        Pekerjaan = "0",
                        Kewarganegaraan = "0",
                        BerlakuHingga = DateTime.Now,
                        Foto = "0",
                        TimeStamp = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}