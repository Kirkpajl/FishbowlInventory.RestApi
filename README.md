![Fishbowl](https://raw.githubusercontent.com/Kirkpajl/FishbowlInventory.RestApi/master/fishbowl-logo.png "Fishbowl")

# Fishbowl Inventory REST API implementation in C#.NET

#### IMPORTANT NOTE:
* I am not actively maintaining this repo project.
* If you find issues - please reach out with a PR that would solve it. I would try to push NuGET with updates to help as soon as I can.
* If anyone is interested to take over as maintainer - please contact

## Latest NuGet Release:
This library can be used from NuGet channel:

* [Fishbowl Inventory REST API Package](https://www.nuget.org/packages/FishbowlInventory.RestApi/) - Version 1.0.0: `Install-Package FishbowlInventory.RestApi`

**Please note:** This source and nuget are a work-in-progress.  Not all API methods/calls have been included/tested.

## Example Usage

### Get user permissions

```C#
// Initialize the Fishbowl Inventory REST API client
using var client = new FishbowlInventoryApiClient("https://localhost:80", "REST API Test Client", "Tests the new REST API endpoints", 1234, "admin", "admin");

// Authenticate with the Fishbowl Inventory server
var userInfo = await client.LoginAsync();

if (userInfo == null) return false;

// Output User details
Console.WriteLine($"User Name:  {userInfo.FullName}");
Console.WriteLine($"Allowed Modules ({userInfo.AllowedModules.Length}):");
foreach (var module in userInfo.AllowedModules) Console.WriteLine($"  * {module}");
Console.WriteLine($"Server Version:  {userInfo.ServerVersion}");

// Terminate the Fishbowl Inventory user session
await fishbowl.LogoutAsync();
```

## Documentation:
For further details on how to use/integrate the FishbowlInventory.RestApi package, please refer to the repository wiki page.

[Fishbowl Inventory REST API .NET SDK WIKI](https://github.com/Kirkpajl/FishbowlInventory.RestApi/wiki)

## Issues / Bugs:
If you have a query, issues or bugs, it means that you have shown interest in this project, and I thank you for that.
Feel free to ask, suggest, report issue or post a bug [here](https://github.com/Kirkpajl/FishbowlInventory.RestApi/issues) in context of this library use.

**Please note:** If your query/issue/bug is related to Fishbowl Inventory REST API, I recommend posting it to the official [Fishbowl Support](https://help.fishbowlinventory.com/s/) forum.

You can find all of the methods to connect with me at my [blog](https://joshuakirkpatrick.com/contact) (ref. footer)

## References:

* [Fishbowl Inventory REST API](https://help.fishbowlinventory.com/s/article/Fishbowl-API) - Official documentation
* [My Blog](https://joshuakirkpatrick.com/) - My personal blog

## Credits / Disclaimer:

* Fishbowl Advanced logo used in this readme file is owned by and copyright of Fishbowl.
* I am not affiliated with Fishbowl, this work is solely undertaken by me.
* This library is not or part of the official set of libraries from Fishbowl and hence can be referred as Third party library for Fishbowl using .NET.

## License

This work is [licensed](https://github.com/Kirkpajl/FishbowlInventory.RestApi/blob/master/LICENSE) under:

The MIT License (MIT)
Copyright (c) 2023 Josh Kirkpatrick