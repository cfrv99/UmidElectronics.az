using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UmidShop.Entities;

namespace UmidShop.ADOConfig
{
    public static class Databases
    {
        public static async Task<DataTable> Select(string query,AppDbContext context)
        {
            var conn = context.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            command.CommandText = query;
            var reader = await command.ExecuteReaderAsync();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
    }
}
