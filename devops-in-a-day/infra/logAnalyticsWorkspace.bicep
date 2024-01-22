param name string
param location string = resourceGroup().location

var workspace = {
    name: 'wrkspc-${name}'
    location: location
}

resource wrkspc 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
    name: workspace.name
    location: workspace.location
    properties: {
        sku: {
            name: 'PerGB2018'
        }
        retentionInDays: 30
        workspaceCapping: {
            dailyQuotaGb: -1
        }
        publicNetworkAccessForIngestion: 'Enabled'
        publicNetworkAccessForQuery: 'Enabled'
    }
}

output id string = wrkspc.id
output name string = wrkspc.name
