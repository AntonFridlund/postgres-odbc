using System.Data.Common;

namespace Persistence;

// Database response reader with error handling
public sealed class ResponseReader(DbDataReader reader) {
  public T Required<T>(int ordinal) {
    var value = reader.GetValue(ordinal);
    if (value is DBNull) throw new InvalidOperationException();
    return (T)value;
  }

  public T? Nullable<T>(int ordinal) {
    var value = reader.GetValue(ordinal);
    if (value is DBNull) return default;
    return (T)value;
  }
}
