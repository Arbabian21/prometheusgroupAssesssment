# AlphaVantage Intraday Aggregation API

A lightweight .NET 8 Web API that consumes Alpha Vantage’s Intraday endpoint, aggregates the last month of 15-minute candles by day, and returns daily averages in a custom format.

---

## Features

* Fetches **15-minute intraday stock data** for any symbol
* Filters values to the **last month**
* Groups values by **day**
* Computes:

  * average intraday low per day
  * average intraday high per day
  * total volume per day
* Returns output in this format:

```json
[
  {
    "day": "2025-01-30",
    "lowAverage": 40.2958,
    "highAverage": 49.7534,
    "volume": 49073348
  }
]
```

* Includes a **Swagger UI** for easy testing
* Follows standard .NET practices (Models / Services / Controllers / DI)

---

# 1. Install .NET 8

### macOS / Windows / Linux

Download and install .NET 8 SDK from the official Microsoft website:

👉 [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

To verify installation:

```bash
dotnet --version
```

You should see something like:

```
8.0.x
```

---

# 2. Clone This Repository

Open your terminal and run:

```bash
git clone https://github.com/Arbabian21/prometheusgroupAssesssment
```

Then enter the project directory:

```bash
cd prometheusgroupAssesssment
cd Backend
```

---

# 3. Get a Free Alpha Vantage API Key

1. Go to: [https://www.alphavantage.co/support/#api-key](https://www.alphavantage.co/support/#api-key)
2. Sign up (it’s free)
3. Check your email for your API key
4. Copy your API key
   (It’ll look something like: `ABCDEF1234567890`)

---

# 4. Add Your API Key Using .NET User Secrets

**This project does NOT store API keys in source code.**

From inside the project directory (where the `.csproj` file is):

```bash
dotnet user-secrets init
```

Then set your Alpha Vantage API key:

```bash
dotnet user-secrets set "AlphaVantage:ApiKey" "YOUR_API_KEY_HERE"
```

To confirm it was saved:

```bash
dotnet user-secrets list
```

You should see:

```
AlphaVantage:ApiKey = YOUR_API_KEY_HERE
```

---

# 5. Run the API

Start the application:

```bash
dotnet run
```

You should see something like:

```
Now listening on: http://localhost:5057
Application started. Press Ctrl+C to shut down.
```

---

# 6. Access Swagger UI

Open your browser and visit:

```
http://localhost:5057/swagger
```

You will see:

* `GET /api/AlphaVantageIntradayData/summary`

Click **Try it out**, and test with any symbol:

* `AAPL`
* `MSFT`
* `IBM`
* `GOOGL`
* `TSLA`

---

# Example Swagger Request

```
GET /api/AlphaVantageIntradayData/summary?symbol=AAPL
```

You will get a response similar to:

```json
[
  {
    "day": "2025-11-13",
    "lowAverage": 272.81,
    "highAverage": 273.70,
    "volume": 40893786
  }
]
```

---

# Project Structure

```
Backend/
│
├── Models/
│   ├── IntradayEndpointDataShape.cs
│   └── DaySummaryDataShape.cs
│
├── Services/
│   ├── IAlphaVantageIntradayDataService.cs
│   └── AlphaVantageIntradayDataService.cs
│
├── Controllers/
│   └── AlphaVantageIntradayDataController.cs
│
└──Program.cs
```


<sub>This README was generated using AI</sub>