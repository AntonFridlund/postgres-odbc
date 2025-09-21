using Routes.Api;

namespace Routes;

public static class MainRouter {
  public static void Register(WebApplication app) {
    ApiRouter.Register(app);
  }
}
