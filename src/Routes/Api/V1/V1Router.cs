using Routes.Api.V1.Users;

namespace Routes.Api.V1;

public static class V1Router {
  public static void Register(RouteGroupBuilder group) {
    var v1Group = group.MapGroup("/v1");
    UserRouter.Register(v1Group);
  }
}
