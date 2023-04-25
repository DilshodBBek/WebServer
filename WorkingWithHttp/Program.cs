using Microsoft.Extensions.Primitives;

namespace WorkingWithHttp
{
    public class Program
    {
        public static List<Student> list;
        static HttpClient client;
        public static void Main(string[] args)
        {
            client = new HttpClient();
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            app.MapPost("/file", async (HttpRequest req) =>
            {
                try
                {
                    int id = 1;
                    await Console.Out.WriteLineAsync("Request:" + id++);
                    string path = @"C:\Users\User\Desktop\PDP\";
                    string fileName = @"Nature" + Guid.NewGuid().ToString();
                    string FullPath = path + fileName + ".jpg";
                    using FileStream reader = new(FullPath, FileMode.Create);
                    await req.Body.CopyToAsync(reader);
                    return Results.Ok("File Uploaded!");
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            });
            app.MapPost("/data", (HttpRequest req) =>
            {
                StringValues response1 = req.Form["1"];
                StringValues response2 = req.Form["2"];
                StringValues response3 = req.Form["3"];
                Console.WriteLine(response1);
                Console.WriteLine(response2);
                Console.WriteLine(response3);
                return Results.Ok("Success");

            });
            app.MapPost("/", async (HttpRequest request) =>
            {
                if (request.Headers.TryGetValue("Students", out StringValues res))
                {
                    await Console.Out.WriteLineAsync(res);
                    return Results.Ok("Ish bitdi!");
                }
                else
                {
                    return Results.BadRequest("Ishlamadi");
                }
            });


            app.MapPost("/upload", async (HttpRequest req) =>
            {
                string path = @"C:\Users\User\Desktop\PDP\";

                using StreamReader streamReader = new StreamReader(req.Body);
                string filename1 = await streamReader.ReadToEndAsync();
                string format = Path.GetExtension(filename1);

                string fileName = Guid.NewGuid().ToString() + format;

                using (var fs = new FileStream(path + fileName, FileMode.Create))
                {
                    await req.Body.CopyToAsync(fs);
                }
                return Results.Ok("File uploaded:)");

            });

            app.MapPost("/downloads", async (HttpRequest context) =>
            {

                IFormFileCollection files = context.Form.Files;
                string path = @"C:\Users\User\Desktop\PDP\";
                foreach (var file in files)
                {
                    using FileStream fs = new(path + file, FileMode.Create);
                    await file.CopyToAsync(fs);
                }

            });
            app.MapGet("/", (HttpContext context) =>
            {
                var res=context.Request.Cookies.FirstOrDefault(x => x.Key == "Login" && x.Value == "Elyor_bek");
                context.Response.Cookies.Append("Login", "Elyor_bek");
                context.Response.Cookies.Append("email", "Elyor@localhost.com");
                context.Response.Cookies.Append("Password", "Elyor1@34");                
            });

            app.MapGet("/test", (HttpContext context) =>
            {
                foreach (var item in context.Request.Cookies)
                {
                    Console.WriteLine(item.Key+"="+item.Value);
                }
            });


            app.Run();
        }
    }
}