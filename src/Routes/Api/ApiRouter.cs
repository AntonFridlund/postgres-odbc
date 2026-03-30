using Routes.Api.Users;

namespace Routes.Api;

public class ApiRouter {
  public void Register(WebApplication app) {
    var apiGroup = app.MapGroup("/api");
    var userRouter = new UserRouter();
    userRouter.Register(apiGroup);
  }
}
