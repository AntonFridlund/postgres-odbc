using Routes.Api;

namespace Routes;

public class MainRouter {
  public void Register(WebApplication app) {
    var apiRouter = new ApiRouter();
    apiRouter.Register(app);
  }
}
