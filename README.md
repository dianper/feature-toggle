[![CircleCI](https://circleci.com/gh/dianper/feature-toggle.svg?style=shield)](https://circleci.com/gh/dianper/feature-toggle)

# On/Off Feature Toggle Core

The features can be enabled by

* Cookies
* Headers
* QueryString

## How to use

### Toggle Declaration

```javascript

// appsettings.json
"FeatureManagement": {
  "FeatureA": false,
  "FeatureB": false,
  "FeatureC": {
    "EnabledFor": [
      {
        "Name": "Cookies"
      },
      {
        "Name": "QueryString"
      }
    ]
  },
  "FeatureD": {
    "EnabledFor": [
      {
        "Name": "Cookies"
      }
    ]
  }
}
```

### Service Registration

```c#
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpContextAccessor();
    services.AddMvc();
    services.AddFeatureManagement()
        .AddFeatureFilter<CookiesFilter>()
        .AddFeatureFilter<HeadersFilter>()
        .AddFeatureFilter<QueryStringFilter>();
}
```

### Dependency Injection

```c#
private readonly IFeatureManager featureManager;

public DummyClass(IFeatureManager featureManager)
{
    this.featureManager = featureManager;
}
```

### Check

```c#
private const string FeatureName = "FeatureC";

public async Task DummyMethod()
{
    if(await this.featureManager.IsEnabledAsync(FeatureName))
    {
      // Do something
    }
}
```

### Build

```sh
dotnet build
```

### Test

```sh
dotnet test
```

### Running

Turn on by querystring

```
dotnet run --project .\src\Dianper.FeatureToggle\Dianper.FeatureToggle.Sample

http://localhost:55715/api/values?FeatureC=true
```

## Reference
[Microsoft.FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet)
