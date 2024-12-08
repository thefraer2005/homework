
using System.Net;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using Listener_client_ajax;
var httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:5000/");
httpListener.Start();

Console.WriteLine("запуск");
var hockeyNewsApiClient = new HockeyNewsApiClient();
while (httpListener.IsListening)
{
    var context = await httpListener.GetContextAsync();
    var response = context.Response;
    var request = context.Request;
    var ctx = new CancellationTokenSource();
   

    _ = Task.Run(async () =>
    {
        switch (request.Url?.LocalPath)
        {
            case "/home" when request.HttpMethod == "GET":
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";
                var file = await File.ReadAllBytesAsync("public/index.html", ctx.Token);
                await context.Response.OutputStream.WriteAsync(file, ctx.Token);
                Console.WriteLine("пошла вода горячая");
                break;

            case "/nhl/news" when request.HttpMethod == "GET":
                try
                {
                    var newsJson = await hockeyNewsApiClient.GetHockeyNewsAsync();

                    if (!string.IsNullOrEmpty(newsJson))
                    {
                       var newsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Root>>(newsJson);
                        response.StatusCode = 200;
                        response.ContentType = "application/json";
                        var newsBytes = System.Text.Encoding.UTF8.GetBytes(newsJson);
                        await response.OutputStream.WriteAsync(newsBytes, ctx.Token);
                    }
                    else
                    {
                        response.StatusCode = 204; 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при получении новостей: {ex.Message}");
                    response.StatusCode = 500; 
                }
                break;
            default:
                await ShowResourseFile(context, ctx.Token);
                break;
        }

        response.OutputStream.Close();
        response.Close();
    });
}
httpListener.Stop();
httpListener.Close();
async Task ShowResourseFile(HttpListenerContext context, CancellationToken token)
{
    if (context.Request.Url is null)
    {
        context.Response.StatusCode = 404;
        return;
    }
    var path = context.Request.Url?.LocalPath.Split('/')[^1];


    context.Response.StatusCode = 200;
    context.Response.ContentType = path!.Split('.')[^1] switch
    {
        "html" => "text/html",
        "css" => "text/css",
        "js" => "text/javascript",
        "svg" => "image/svg+xml",
        _ => throw new ArgumentOutOfRangeException()
    };

    var file = await File.ReadAllBytesAsync($"public/{path}", token);
    await context.Response.OutputStream.WriteAsync(file, token);
}
