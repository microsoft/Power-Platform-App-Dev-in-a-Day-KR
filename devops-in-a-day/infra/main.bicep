targetScope = 'subscription'

param name string
param location string = 'Korea Central'

param apiManagementPublisherName string = 'APIM Power Platform Connectors'
param apiManagementPublisherEmail string = 'apim@contoso.com'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: 'rg-${name}'
    location: location
}

module apim './provision-apiManagement.bicep' = {
    name: 'ApiManagement'
    scope: rg
    params: {
        name: name
        location: location
        apiManagementPublisherName: apiManagementPublisherName
        apiManagementPublisherEmail: apiManagementPublisherEmail
    }
}
