using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;
using DotNetEnv;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Env.Load();

            var outputPath = Environment.GetEnvironmentVariable("OUTPUT_PATH");
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            var tweetPath = Environment.GetEnvironmentVariable("TWEET_PATH");

            string trumpPost = await FetchLatestTrumpPost();
            Console.WriteLine("Trump's latest post:\n" + trumpPost);

            string mood = await GenerateDescriptor(apiKey, trumpPost, "Summarize the mood of this post in one word:");
            string vibeLevel = await GenerateDescriptor(apiKey, trumpPost, "Describe the vibe level of this post in one dramatic word:");

            // Call the function to generate a haiku
            string haiku = await GenerateHaiku(apiKey, trumpPost);

            var templatePath = Path.Combine(AppContext.BaseDirectory, "template.html");
            var html = File.ReadAllText(templatePath);
            html = html.Replace("{{HAIKU}}",
                HttpUtility.HtmlEncode(haiku).ReplaceLineEndings("<br/>"));
            html = html.Replace("{{MOOD}}", mood);
            html = html.Replace("{{VIBE_LEVEL}}", vibeLevel);
            html = html.Replace("{{LAST_UPDATED}}", DateTime.UtcNow.ToString("u"));
            File.WriteAllText(outputPath, html);

            Console.WriteLine("Generated Haiku:\n" + haiku);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static async Task<string> GenerateDescriptor(string apiKey, string trumpPost, string prompt)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

        var data = new
        {
            model = "gpt-4o",
            input = $"{prompt} {trumpPost}"
        };

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/responses", content);
        string responseString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString)
            ?? throw new Exception("Failed to generate descriptor.");

        return ((string)responseJson.output[0].content[0].text).Trim();
    }

    static async Task<string> GenerateHaiku(string apiKey, string trumpPost)
    {
        // Set up the HTTP client
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

        // Prepare the request payload
        var data = new
        {
            model = "gpt-4o", // You can change the model if needed
            input = "Generate a haiku. Use the latest unhinged tweet from Donald Trump as inspiration. If possible, make the haiku subtly mock the tweet." +
                     "The tweet is: " + trumpPost,
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

    static async Task<string> FetchLatestTrumpPost()
    {
        var tweetPath = Environment.GetEnvironmentVariable("TWEET_PATH")
            ?? throw new Exception("Please set the TWEET_PATH environment variable.");

        if (!File.Exists(tweetPath))
        {
            throw new FileNotFoundException("Tweet file not found at: " + tweetPath);
        }

        string tweetContent = await File.ReadAllTextAsync(tweetPath);
        return tweetContent.Trim();
    }
}