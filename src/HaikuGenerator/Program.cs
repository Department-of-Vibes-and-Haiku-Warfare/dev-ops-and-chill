using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DotNetEnv;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var outputPath = args!.FirstOrDefault() ?? throw new Exception("Please provide the output path as a command line argument.");

            // Set your OpenAI API key here
            Env.Load();
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("Please set the OPENAI_API_KEY environment variable.");

            // Call the function to generate a haiku
            string haiku = await GenerateHaiku(apiKey);

            var templatePath = Path.Combine(AppContext.BaseDirectory, "template.html");
            var html = File.ReadAllText(templatePath);
            html = html.Replace("{{DATE}}", DateTime.Now.ToString("yyyy-MM-dd"));
            html = html.Replace("{{HAIKU}}",
                HttpUtility.HtmlEncode(haiku).ReplaceLineEndings("<br/>"));
            File.WriteAllText(outputPath, html);

            Console.WriteLine("Generated Haiku:\n" + haiku);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static async Task<string> GenerateHaiku(string apiKey)
    {
        // Set up the HTTP client
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

        // Prepare the request payload
        var data = new
        {
            model = "gpt-4o", // You can change the model if needed
            input = "Generate a haiku."
        };

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        // Send the request and get the response
        var response = await client.PostAsync("https://api.openai.com/v1/responses", content);

        // Get the response content
        string responseString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString)
            ?? throw new Exception("Failed to generate haiku.");

        // Extract the haiku text from the response
        string haiku = responseJson.output[0].content[0].text;

        return haiku;
    }
}