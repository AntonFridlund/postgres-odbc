using static System.Text.Json.Serialization.JsonIgnoreCondition;
using System.Threading.Channels;
using System.Text.Json;

namespace Middlewares.Logging;

// Processes queued log entries
public static class LogQueue<T> {
  private static readonly JsonSerializerOptions jsonOptions = new() { DefaultIgnoreCondition = WhenWritingNull };
  private static readonly BoundedChannelOptions channelOptions = new(100) { SingleReader = true };
  private static readonly Channel<T> channel = Channel.CreateBounded<T>(channelOptions);
  public static ChannelWriter<T> Writer => channel.Writer;

  public static async Task RunAsync() {
    await foreach (var entry in channel.Reader.ReadAllAsync()) {
      var json = JsonSerializer.Serialize(entry, jsonOptions);
      await Console.Out.WriteLineAsync(json);
    }
  }
}
