using Routes.Api.V1;

namespace Routes.Api;

public static class ApiRouter {
  public static void Register(WebApplication app) {
    var apiGroup = app.MapGroup("/api");
    V1Router.Register(apiGroup);
  }
}
