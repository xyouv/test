namespace PhoneManagement.Extension {
    public class ApiKeyAuthenExtension {
        private readonly RequestDelegate _requestDelegate;

        private readonly string KeyName = "x-api-key";
        private readonly string KeyValue = "qwertyuiopasdfghjjkl";

        public ApiKeyAuthenExtension(RequestDelegate requestDelegate) {
            _requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context) {
            var api_token = context.Request.Headers[KeyName].FirstOrDefault();
            if (api_token == null) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("ApiKey Name is not authorization!");
                return;
            }
            if (api_token != KeyValue) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("ApiKey Value is not authorization!");
                return;
            }
            await _requestDelegate(context);
        }

    }
}
