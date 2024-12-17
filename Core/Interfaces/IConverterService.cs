namespace Core.Interfaces;

public interface IConverterService
{
    public Task<byte[]> ConvertAsync(Stream stream, bool closeStream, CancellationToken cancellationToken);
}
