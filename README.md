# Resiliency
This Repo contains the sample on how to use Microsoft.Extensions.Http.Resiliency use cases.

The Sample solution has 2 projects, One as client application to invoke and  Api and another is a API that can be invoked the Client App. The example demonstrates how to inject Resiliency pipeline to the IHTTPClienFactory that will be used to creat HttpClient instance to invoke an API and executes the resiliency pipeline if the call to APi has failed.


Here the links from the Microsoft.
-> https://learn.microsoft.com/en-us/dotnet/core/resilience/?tabs=dotnet-cli
-> https://www.pollydocs.org/strategies/
