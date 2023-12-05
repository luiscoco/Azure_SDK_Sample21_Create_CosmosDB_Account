using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;

//1. Obtaine Azure credentials
TokenCredential cred = new DefaultAzureCredential();

//2. Azure Authentication
ArmClient client = new ArmClient(cred);

//3. Set your Azure subscription number
string subscriptionId = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

//4. Set the ResourceGroup name where to create the new Azure CosmosDB account
//It is mandatory to create this ResourceGroup before creating the ComosDB account
string resourceGroupName = "rg1";

//5. Create the ResourceGroup Identifier
ResourceIdentifier resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);

//6. Get the ResourceGroup
ResourceGroupResource resourceGroupResource = client.GetResourceGroupResource(resourceGroupResourceId);

//7. Get the collection of this CosmosDBAccountResource
CosmosDBAccountCollection collection = resourceGroupResource.GetCosmosDBAccounts();

//8. Set the data input for creating the CosmosDB account: accountName, account location, etc
string accountName = "newcosmosdbwithazuresdk";
CosmosDBAccountCreateOrUpdateContent content = new CosmosDBAccountCreateOrUpdateContent(new AzureLocation("westeurope"), new CosmosDBAccountLocation[]
{
new CosmosDBAccountLocation()
{
LocationName = new AzureLocation("westeurope"),
FailoverPriority = 0,
IsZoneRedundant = false,
}
})
{
    CreateMode = CosmosDBAccountCreateMode.Default,
};

//9. Create/Update a CosmosDB account
ArmOperation<CosmosDBAccountResource> lro = await collection.CreateOrUpdateAsync(WaitUntil.Completed, accountName, content);

//10. Get the ComosDB Account Resource
CosmosDBAccountResource result = lro.Value;

//11. Get the CosmosDB Account Data
CosmosDBAccountData resourceData = result.Data;

//12. We print the new CosmosDB account Id
Console.WriteLine($"Succeeded on id: {resourceData.Id}");