using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFunctionsApp.ServiceInterfaces
{
    public interface IBlobService
    {
        Task UploadImageAsync(string fileName, string imageUrl);
    }
}
