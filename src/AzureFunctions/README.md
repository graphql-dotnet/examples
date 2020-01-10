# Azure Functions GraphQL Example

This sample is based on the work from [tpeczek](https://github.com/tpeczek/Demo.Azure.Functions.GraphQL/). He published a blog post about this: [Serverless GraphQL with Azure Functions, GraphQL for .NET, and Cosmos DB](https://www.tpeczek.com/2019/05/serverless-graphql-with-azure-functions.html)

## Preparation

Install [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools) in version 3 for your plattform

## Run

switch to the `\src\AzureFunctions` folder

Bash:

```bash
func host start
curl --location --request POST 'http://localhost:7071/api/graphql' \
  --header 'Content-Type: application/json' \
  --data-raw '{"query":"query humans {human(id: \"1\") {appearsIn}}","variables":{}}'
```

Powershell:

```powershell
func host start
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")

$body = '{"query":"query humans {human(id: \"1\") {appearsIn}}","variables":{}}'

$response = Invoke-RestMethod 'http://localhost:7071/api/graphql' -Method 'POST' -Headers $headers -Body $body
$response | ConvertTo-Json
```

or run the task `func: host start` in VSCode.
