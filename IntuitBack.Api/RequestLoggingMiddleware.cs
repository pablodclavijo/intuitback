using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IntuitBack.Api.Middleware
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
            // Habilita la lectura múltiple del body
            context.Request.EnableBuffering();

            // Lee el body (si existe)
            string bodyAsText = string.Empty;
            if (context.Request.ContentLength > 0)
            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true);

                bodyAsText = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset para que el pipeline pueda leerlo luego
            }

            Console.WriteLine("=== REQUEST INCOMING ===");
            Console.WriteLine($"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
            Console.WriteLine("Headers:");
            foreach (var header in context.Request.Headers)
                Console.WriteLine($"  {header.Key}: {header.Value}");
            if (!string.IsNullOrWhiteSpace(bodyAsText))
                Console.WriteLine($"Body:\n{bodyAsText}");
            Console.WriteLine("========================");

            await _next(context); // Continua con el pipeline
        }
    }
}
