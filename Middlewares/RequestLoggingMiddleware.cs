using System.Diagnostics;

namespace BookManagement.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var method = context.Request.Method;
            var path = context.Request.Path.ToString();

            Console.WriteLine($"[{time}] Method: {method} - Path: {path}");

            // Chặn Book/Detail/0 và Book/Detail/-1 trước khi request vào Controller.
            if (path.Equals("/Book/Detail/0", StringComparison.OrdinalIgnoreCase) ||
                path.Equals("/Book/Detail/-1", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Book id không hợp lệ");
                stopwatch.Stop();
                Console.WriteLine($"Status Code: {context.Response.StatusCode} - Time: {stopwatch.ElapsedMilliseconds} ms");
                return;
            }

            // Cho request tiếp tục đến middleware/controller tiếp theo.
            await _next(context);

            stopwatch.Stop();
            Console.WriteLine($"Status Code: {context.Response.StatusCode} - Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
