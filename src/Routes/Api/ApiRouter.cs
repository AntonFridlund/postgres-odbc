using Routes.Api.Users;

namespace Routes.Api;

// Routing for api calls
public class ApiRouter {
  public void Register(WebApplication app) {
    var apiGroup = app.MapGroup("/api");
    var userRouter = new UserRouter();
    userRouter.Register(apiGroup);
  }
}
