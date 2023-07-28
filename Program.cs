using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
var httpClient = new HttpClient();
var memoryCache = app.Services.GetRequiredService<IMemoryCache>();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/api/BestStories/{n}", async (int n) =>
{
    if (!memoryCache.TryGetValue("BestStories", out List<BestStory> bestStories))
    {
        string bestStoriesUri = "https://hacker-news.firebaseio.com/v0/beststories.json";
        string bestStoriesJson = await httpClient.GetStringAsync(bestStoriesUri);
        int[] bestStoryIds = JsonConvert.DeserializeObject<int[]>(bestStoriesJson);

        bestStories = new List<BestStory>();

        for (int i = 0; i < Math.Min(n, bestStoryIds.Length); i++)
        {
            string storyUri = $"https://hacker-news.firebaseio.com/v0/item/{bestStoryIds[i]}.json";
            string storyJson = await httpClient.GetStringAsync(storyUri);
            var storyDetails = JsonConvert.DeserializeObject<StoryDetails>(storyJson);

            var bestStory = new BestStory
            {
                Title = storyDetails.Title,
                Uri = storyDetails.Url,
                PostedBy = storyDetails.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(storyDetails.Time).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                Score = storyDetails.Score,
                CommentCount = storyDetails.Descendants
            };

            bestStories.Add(bestStory);
        }

        // Sorting the best stories by score in descending order
        bestStories = bestStories.OrderByDescending(s => s.Score).ToList();

        // Caching the sorted best stories for 5 minutes
        memoryCache.Set("BestStories", bestStories, TimeSpan.FromMinutes(5));
    }

    // Return the sorted list from the cache without taking n again.
    return Results.Json(bestStories);
});


app.Run();

public class BestStory
{
    public string Title { get; set; }
    public string Uri { get; set; }
    public string PostedBy { get; set; }
    public string Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }
}

public class StoryDetails
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string By { get; set; }
    public long Time { get; set; }
    public int Score { get; set; }
    public int Descendants { get; set; }
}






