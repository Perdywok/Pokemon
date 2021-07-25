# Pokemon API
The API has two main endpoints:
1. http://localhost:8000/api/pokemon/{name}  
Returns basic Pokemon information. Gets pokemon info from https://pokeapi.co/docs/v2#pokemon-species
2. http://localhost:8000/api/pokemon/translated/{name}  
Returns basic Pokemon information but with a "fun" translation of the Pokemon description. Gets pokemon info from https://pokeapi.co/docs/v2#pokemon-species and translates description via https://api.funtranslations.com/translate/shakespeare.json? or https://api.funtranslations.com/translate/yoda.json
## How to run
1. Install docker if you don't already have it.  
https://docs.docker.com/engine/install/
2. Run command below from the root folder.
```powershell
docker compose up -d
```
3. Open http://localhost:8000

## How to test
Run command below from the root folder.

Linux 
```powershell
dotnet test ./Pokemon.Tests
```

Windows 
```powershell
dotnet test .\Pokemon.Tests
```

## Possible improvements for production
1. Use distributed cache storage for responses from third party APIs.
2. Use database to store types of translation. Current implementation stores it in appsettings.json.
3. Add logging of httpclient request/responses and log requests to the API in general.
