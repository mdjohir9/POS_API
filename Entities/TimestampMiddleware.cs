namespace POS_API.Entities
{
    public class TimestampMiddleware
    {
        private readonly RequestDelegate _next;

        public TimestampMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentUrl = context.Request.Path.Value;
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"); 

            if (!currentUrl.Contains("?"))
            {
                context.Request.Path = currentUrl + "?t=" + timestamp;
            }
            else
            {
                context.Request.Path = currentUrl + "&t=" + timestamp;
            }

            await _next(context); 
        }
    }
}
