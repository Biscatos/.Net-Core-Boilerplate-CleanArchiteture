namespace Core.Application.Interfaces.Services.System
{
    public interface IMinIoService
    {
        Task ListBuckets();
        Task UploadFile(string Filebase64, string FileName);
        Task<bool> RemoveFile(string FileName);
        public string BucketBaseUrl { get; }
        Task<string> GetBase64File(string fileName);
        Task<byte[]> GetByteArray(string fileName);
    }
}
