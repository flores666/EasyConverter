namespace Core.Models;

public class FileModel
{
    public string ContentType { get; set; }
    public string FileName { get; set; }
    public long Length { get; set; }
    public byte[] Content { get; set; }
}
