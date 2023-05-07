namespace Core.Application.DTOS.System
{
    public class FileDTO
    {
        public string FileName { get; set; }
        public string Base64 { get; set; }
    }

    public class FileUpdateDTO<T>
    {
        public T Id { get; set; }
        public string FileName { get; set; }
        public string Base64 { get; set; }
    }
}
