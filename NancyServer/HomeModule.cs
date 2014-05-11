using Nancy;

namespace NancyServer
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = x =>
            {
                var environment = new OwinEnvironment(this.Context);
                return environment.GetRequestMethodWithUri();
            };
        }
    }
}