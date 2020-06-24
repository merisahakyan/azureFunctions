using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFunctionsApp.ServiceInterfaces
{
    public interface ISqlService
    {
        Task<int> GetByUrlCountAsync(string url);
        Task InsertToDbAsync(string imageUrl, string fileName);
    }
}
