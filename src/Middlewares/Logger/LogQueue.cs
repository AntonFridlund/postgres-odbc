using System.Text.Json;
using System.Threading.Channels;

namespace Middlewares.Logger;

public class LogQueue<T>(BoundedChannelOptions channelOptions, JsonSerializerOptions? jsonOptions = null) : BackgroundService {
  private readonly Channel<T> channel = Channel.CreateBounded<T>(channelOptions);
  public ChannelWriter<T> Writer => channel.Writer;

  protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    await foreach (var entry in channel.Reader.ReadAllAsync(stoppingToken)) {
      var json = JsonSerializer.Serialize(entry, jsonOptions);
      await Console.Out.WriteLineAsync(json).ConfigureAwait(false);
    }
  }
}
