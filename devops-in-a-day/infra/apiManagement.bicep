param name string
param location string = resourceGroup().location

param appInsightsId string
@secure()
param appInsightsInstrumentationKey string

param apiManagementPublisherName string
param apiManagementPublisherEmail string

@allowed([
    'rawxml'
    'rawxml-link'
    'xml'
    'xml-link'
])
param apiManagementPolicyFormat string = 'xml'
param apiManagementPolicyValue string = '<!--\r\n    IMPORTANT:\r\n    - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n    - Only the <forward-request> policy element can appear within the <backend> section element.\r\n    - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n    - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n    - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n    - To remove a policy, delete the corresponding policy statement from the policy document.\r\n    - Policies are applied in the order of their appearance, from the top down.\r\n-->\r\n<policies>\r\n  <inbound></inbound>\r\n  <backend>\r\n    <forward-request />\r\n  </backend>\r\n  <outbound></outbound>\r\n</policies>'

var appInsights = {
    id: appInsightsId
    name: 'appins-${name}'
    instrumentationKey: appInsightsInstrumentationKey
}

var apiManagement = {
    name: 'apim-${name}'
    location: location
    skuName: 'Consumption'
    skuCapacity: 0
    publisherName: apiManagementPublisherName
    publisherEmail: apiManagementPublisherEmail
    policyFormat: apiManagementPolicyFormat
    policyValue: apiManagementPolicyValue
}

resource apim 'Microsoft.ApiManagement/service@2021-08-01' = {
    name: apiManagement.name
    location: apiManagement.location
    identity: {
        type: 'SystemAssigned'
    }
    sku: {
        name: apiManagement.skuName
        capacity: apiManagement.skuCapacity
    }
    properties: {
        publisherName: apiManagement.publisherName
        publisherEmail: apiManagement.publisherEmail
    }
}

resource apimlogger 'Microsoft.ApiManagement/service/loggers@2021-08-01' = {
    name: appInsights.name
    parent: apim
    properties: {
        loggerType: 'applicationInsights'
        resourceId: appInsights.id
        credentials: {
            instrumentationKey: appInsights.instrumentationKey
        }
    }
}

resource apimpolicy 'Microsoft.ApiManagement/service/policies@2021-08-01' = {
    name: 'policy'
    parent: apim
    properties: {
        format: apiManagement.policyFormat
        value: apiManagement.policyValue
    }
}

resource apimproduct 'Microsoft.ApiManagement/service/products@2021-08-01' = {
    name: 'default'
    parent: apim
    properties: {
        displayName: 'Default Product'
        description: 'This is the default product created by the template, which includes all APIs.'
        state: 'published'
        subscriptionRequired: false
    }
}

output id string = apim.id
output name string = apim.name
