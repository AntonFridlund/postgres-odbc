using System.Data.Common;

namespace Persistence.Odbc;

// Database response reader
public sealed class ResponseReader(DbDataReader reader) {
  public T Required<T>(int ordinal) {
    if (reader.IsDBNull(ordinal)) {
      var error = $"Column {ordinal} is required but got null";
      throw new InvalidOperationException(error);
    }
    return reader.GetFieldValue<T>(ordinal);
  }

  public T? Nullable<T>(int ordinal) {
    if (reader.IsDBNull(ordinal)) return default;
    return reader.GetFieldValue<T>(ordinal);
  }
}
