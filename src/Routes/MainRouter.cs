using Routes.Api;

namespace Routes;

// Top level router
public class MainRouter {
  public void Register(WebApplication app) {
    var apiRouter = new ApiRouter();
    apiRouter.Register(app);
  }
}
