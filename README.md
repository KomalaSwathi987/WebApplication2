# WebApplication2
# Hacker News API - Best Stories

## Overview
This project implements a RESTful API to retrieve the details of the first n "best stories" from the Hacker News API, where n is specified by the caller to the API. The "best stories" are sorted by their score in descending order.

The Hacker News API is documented here: [https://github.com/HackerNews/API](https://github.com/HackerNews/API).

## How to Run the Application
To run the application locally, follow these steps:

1. Clone this repository to your local machine.
2. Open the solution in your preferred development environment (e.g., Visual Studio, Visual Studio Code).
3. Build the solution to restore NuGet packages and resolve dependencies.
4. Run the application using the `dotnet run` command from the root directory of the project.
5. The API will be hosted at `https://localhost:5001` by default.

## Assumptions
1. I used new minimal API approach in ASP.NET Core, which does not require a traditional Startup class. I directly configured the services and middleware in the Program.cs file itself.
2. The development server uses a self-signed SSL certificate, which may cause SSL-related warnings in web browsers. 
3. The API uses an in-memory cache (provided by MemoryCache) to store the fetched best stories. The cache is set to expire after 5 minutes. 

## Enhancements and Changes
1.Implement proper error handling and error responses to provide better feedback to clients. 
2.Add unit tests to verify the functionality of the API and ensure robustness. 
3.Implement API versioning to allow for future backward compatibility with breaking changes. 
4.Add logging to track API usage and identify potential issues. 
5.Add authentication and authorization to secure the API if necessary. 
6.Implement paging support to allow clients to fetch stories in batches.

## API Endpoint
The API provides the following endpoint:

### GET /api/BestStories/{n}
Retrieves the details of the first n "best stories" from the Hacker News API.

#### Parameters
- `n` (integer): The number of best stories to retrieve.

#### Response
The API returns an array of "best stories" sorted by their score in descending order. Each story has the following format:

```json
[
    {
        "title": "A uBlock Origin update was rejected from the Chrome Web Store",
        "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
        "postedBy": "ismaildonmez",
        "time": "2019-10-12T13:43:01+00:00",
        "score": 1716,
        "commentCount": 572
    },
    { ... },
    { ... },
    ...
]
