using Azure.Core;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.RateLimiting;

namespace CodeBase.API.Utils.RateLimiting
{
    public class CreateOrderPolicy : IRateLimiterPolicy<string>
    {
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.HttpContext.Response.WriteAsJsonAsync(new CommandResult<OrderDTO>
            {
                IsSucceed =false,
                Message ="Too Many Reqeust"
            }.ToString());
            return new();
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            var request = httpContext.Request;
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
           var s =  request.Body.ReadAsync(buffer, 0, buffer.Length).Result;
            //get body string here...
            var requestContent = Encoding.UTF8.GetString(buffer);
            var bodyModel = JsonConvert.DeserializeObject<CreateOrderItemDTO>(requestContent);
            string key = $"book_{bodyModel.BookId}";
            request.Body.Position = 0;  //rewinding the stream to 0

            return RateLimitPartition.GetFixedWindowLimiter(key, _ => new()
                {
                    AutoReplenishment = true,
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(3),
                    QueueLimit = 50,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
           
        }
    }
}
