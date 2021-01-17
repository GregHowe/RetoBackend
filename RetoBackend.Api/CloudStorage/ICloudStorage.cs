using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RetoBackend.Api.CloudStorage
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(byte[] file, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}
