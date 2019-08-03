using Polly.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class Compress : AsyncWorkerBase
    {
        private IProductRepository _productRepository;

        public Compress(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override string ToString() { return ""; }

        protected override Task DoWorkInternalAsync(CancellationToken token)
        {
            var ids = new List<long>() { 3767 };

            //await GetAllProductIds();
            var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return null;
            //using (SqlConnection con = new SqlConnection(connString))
            //{
            //    await con.OpenAsync();
            //    SqlTransaction tran = con.BeginTransaction();
            //    for (int i = 0; i < ids.Count; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            tran.Commit();
            //            tran = con.BeginTransaction();
            //        }

            //        using (SqlCommand com = new SqlCommand("select [Description] from Product where Id = " + ids[i], con, tran))
            //        {
            //            string description = (string)await com.ExecuteScalarAsync();
            //            var compbtyes = await CompressString(description);
            //            using (SqlCommand comup = new SqlCommand("update Product set [Description] = @comp where Id = @id", con, tran))
            //            {
            //                comup.Parameters.AddWithValue("@comp", compbtyes);
            //                comup.Parameters.AddWithValue("@id", ids[i]);
            //                await comup.ExecuteNonQueryAsync();
            //            }
            //        }

            //        tran.Commit();
            //    }
            //}
        }

        public async Task<byte[]> CompressString(string input)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(input);
            using (var output = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(output, CompressionMode.Compress))
                {
                    await compressionStream.WriteAsync(encoded, 0, encoded.Length);
                }

                return output.ToArray();
            }
        }

        private static async Task<List<long>> GetAllProductIds()
        {
            var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            List<long> ids = new List<long>(5100000);

            using (SqlConnection con = new SqlConnection(connString))
            {
                await con.OpenAsync();
                using (SqlCommand com = new SqlCommand("select Id from Product", con))
                {
                    com.CommandTimeout = 600;
                    var reader = await com.ExecuteReaderAsync();
                    ids.Add(reader.GetInt64(0));
                }
            }

            return ids;
        }
    }
}
