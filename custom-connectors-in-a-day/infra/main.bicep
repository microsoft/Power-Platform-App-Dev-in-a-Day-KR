targetScope = 'subscription'

param name string
param location string = 'Korea Central'

param apiManagementPublisherName string = 'APIM Power Platform Connectors'
param apiManagementPublisherEmail string = 'apim@contoso.com'

@secure()
param gitHubAccessToken string = ''
param gitHubUsername string
param gitHubRepositoryName string
param gitHubBranchName string = 'main'

var apps = [
    {
        suffix: 'api-key-auth'
        apiName: 'API-KEY-AUTH'
        apiPath: 'apikeyauth'
        apiFormat: 'openapi+json-link'
        apiExtension: 'json'
        apiSubscription: false
        apiOperations: [
            {
                name: 'Greeting'
                policy: {
                    format: 'xml-link'
                    value: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/custom-connectors-in-a-day/infra/apim-api-apikeyauth-operation-policy-greeting.xml'
                }
            }
        ]
    }
    {
        suffix: 'basic-auth'
        apiName: 'BASIC-AUTH'
        apiPath: 'basicauth'
        apiFormat: 'openapi+json-link'
        apiExtension: 'json'
        apiSubscription: false
        apiOperations: []
    }
    {
        suffix: 'auth-code-auth'
        apiName: 'AUTH-CODE-AUTH'
        apiPath: 'authcodeauth'
        apiFormat: 'openapi+json-link'
        apiExtension: 'json'
        apiSubscription: false
        apiOperations: []
    }
]
var storageContainerName = 'openapis'

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
        storageContainerName: storageContainerName
        gitHubUsername: gitHubUsername
        gitHubRepositoryName: gitHubRepositoryName
        apiManagementPublisherName: apiManagementPublisherName
        apiManagementPublisherEmail: apiManagementPublisherEmail
        apiManagementPolicyFormat: 'xml-link'
        apiManagementPolicyValue: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/custom-connectors-in-a-day/infra/apim-global-policy.xml'
    }
}

module fncapps './provision-functionApp.bicep' = [for (app, index) in apps: {
    name: 'FunctionApp_${app.suffix}'
    scope: rg
    dependsOn: [
        apim
    ]
    params: {
        name: name
        suffix: app.suffix
        location: location
        storageContainerName: storageContainerName
        apimApiPath: app.apiPath
    }
}]

// module apis './provision-apiManagementApi.bicep' = [for (app, index) in apps: {
//     name: 'ApiManagementApi_${app.suffix}'
//     scope: rg
//     dependsOn: [
//         apim
//         fncapps
//     ]
//     params: {
//         name: name
//         location: location
//         apiManagementNameValueName: 'X_FUNCTIONS_KEY_${replace(toUpper(app.apiName), '-', '_')}'
//         apiManagementNameValueDisplayName: 'X_FUNCTIONS_KEY_${replace(toUpper(app.apiName), '-', '_')}'
//         apiManagementNameValueValue: 'to_be_replaced'
//         apiManagementApiName: app.apiName
//         apiManagementApiDisplayName: app.apiName
//         apiManagementApiDescription: app.apiName
//         apiManagementApiSubscriptionRequired: app.apiSubscription
//         apiManagementApiServiceUrl: 'https://fncapp-${name}-${app.suffix}.azurewebsites.net/api'
//         apiManagementApiPath: app.apiPath
//         apiManagementApiFormat: app.apiFormat
//         apiManagementApiValue: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/custom-connectors-in-a-day/infra/openapi-${replace(toLower(app.apiName), '-', '')}.${app.apiExtension}'
//         apiManagementApiPolicyFormat: 'xml-link'
//         apiManagementApiPolicyValue: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/custom-connectors-in-a-day/infra/apim-api-policy-${replace(toLower(app.apiName), '-', '')}.xml'
//         apiManagementApiOperations: app.apiOperations
//     }
// }]

module depscrpt './deploymentScript.bicep' = {
    name: 'DeploymentScript'
    scope: rg
    dependsOn: [
        apim
        fncapps
    ]
    params: {
        name: name
        location: location
        gitHubAccessToken: gitHubAccessToken
        gitHubBranchName: gitHubBranchName
        gitHubUsername: gitHubUsername
        gitHubRepositoryName: gitHubRepositoryName
    }
}
