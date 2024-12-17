using Core.Interfaces;
using FFMpegCore;
using FFMpegCore.Pipes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ConverterService : IConverterService
{
    private readonly ILogger<ConverterService> _logger;

    public ConverterService(ILogger<ConverterService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> ConvertAsync(Stream stream, bool closeStream, CancellationToken cancellationToken)
    {
        // var analysis = await FFProbe.AnalyseAsync(stream, null, cancellationToken); // для проверок?
        //todo cancellation token

        using var memoryStream = new MemoryStream();
        try
        {
            await FFMpegArguments.FromPipeInput(new StreamPipeSource(stream))
                .OutputToPipe(new StreamPipeSink(memoryStream), options => options
                    .WithVideoCodec("vp9")
                    .WithConstantRateFactor(21)
                    .WithVariableBitrate(4)
                    // .WithAudioCodec("none") // ?
                    .ForceFormat("webm"))
                .ProcessAsynchronously();
        }
        catch (Exception e)
        {
            _logger.LogError("ConverterService -> ConvertAsync, Exception {E}", e);
            memoryStream.Close();
        }

        if (closeStream) stream.Close();

        return memoryStream.ToArray();
    }
}
