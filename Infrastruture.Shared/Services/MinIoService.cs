
using Core.Application.Interfaces.Services.System;
using Core.Application.SystemSettings;
using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruture.Shared.Services
{
    public class MinIoService : IMinIoService
    {

        private readonly MinIoSetting _minIO;

        public MinIoService(IOptions<MinIoSetting> minIO)
        {
            _minIO = minIO.Value;

        }

        public string BucketBaseUrl => _minIO.Protocol + ":" + "//" + _minIO.EndPoint + "/" + _minIO.Bucket;

        private MinioClient ConnectedMinioClient => new MinioClient().WithEndpoint(_minIO.EndPoint)
            .WithCredentials(_minIO.AccessKey, _minIO.SecretKey).Build();

        public async Task<string> GetBase64File(string fileName)
        {
            try
            {
                var client = ConnectedMinioClient;
                var ms = new MemoryStream();
                var minIoArguments = new GetObjectArgs().
                     WithBucket(_minIO.Bucket)
                    .WithObject(fileName)
                    .WithCallbackStream(x => x.CopyTo(ms));

                await client.GetObjectAsync(minIoArguments);
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> GetByteArray(string fileName)
        {
            try
            {
                var client = ConnectedMinioClient;
                var ms = new MemoryStream();
                var minIoArguments = new GetObjectArgs().
                     WithBucket(_minIO.Bucket)
                    .WithObject(fileName)
                    .WithCallbackStream(x => x.CopyTo(ms));

                await client.GetObjectAsync(minIoArguments);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task ListBuckets()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFile(string FileName)
        {
            try
            {
                var client = ConnectedMinioClient;

                var minIoArguments = new RemoveObjectArgs()
                    .WithBucket(_minIO.Bucket)
                    .WithObject(FileName);

                await client.RemoveObjectAsync(minIoArguments);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UploadFile(string Filebase64, string FileName)
        {
            try
            {
                var client = ConnectedMinioClient;
                var fileBytes = Convert.FromBase64String(Filebase64);
                var ms = new MemoryStream(fileBytes);

                var minIoArguments = new PutObjectArgs()
                    .WithBucket(_minIO.Bucket)
                    .WithObject(FileName)
                    .WithObjectSize(ms.Length)
                    .WithStreamData(ms);
                await client.PutObjectAsync(minIoArguments);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
