param name string
param location string = resourceGroup().location

param apiManagementNameValueName string
param apiManagementNameValueDisplayName string
@secure()
param apiManagementNameValueValue string

param apiManagementApiName string
param apiManagementApiDisplayName string
param apiManagementApiDescription string
param apiManagementApiSubscriptionRequired bool = false
param apiManagementApiServiceUrl string
param apiManagementApiPath string
@allowed([
    'swagger-json'
    'swagger-link-json'
    'openapi'
    'openapi+json'
    'openapi+json-link'
    'openapi-link'
    'wadl-link-json'
    'wadl-xml'
    'wsdl'
    'wsdl-link'
    'graphql-link'
])
param apiManagementApiFormat string = 'openapi+json-link'
param apiManagementApiValue string
@allowed([
    'rawxml'
    'rawxml-link'
    'xml'
    'xml-link'
])
param apiManagementApiPolicyFormat string = 'xml'
param apiManagementApiPolicyValue string = '<!--\r\n  IMPORTANT:\r\n  - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n  - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n  - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n  - To add a policy, place the cursor at the desired insertion point and select a policy from the sidebar.\r\n  - To remove a policy, delete the corresponding policy statement from the policy document.\r\n  - Position the <base> element within a section element to inherit all policies from the corresponding section element in the enclosing scope.\r\n  - Remove the <base> element to prevent inheriting policies from the corresponding section element in the enclosing scope.\r\n  - Policies are applied in the order of their appearance, from the top down.\r\n  - Comments within policy elements are not supported and may disappear. Place your comments between policy elements or at a higher level scope.\r\n-->\r\n<policies>\r\n  <inbound>\r\n    <base />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>'

param apiManagementApiOperations array = [
    {
        name: ''
        policy: {
            format: ''
            value: ''
        }
    }
]


module apimapi './apiManagementApi.bicep' = {
    name: 'ApiManagementApi_APIM_${apiManagementApiName}'
    params: {
        name: name
        location: location
        apiManagementNameValueName: apiManagementNameValueName
        apiManagementNameValueDisplayName: apiManagementNameValueDisplayName
        apiManagementNameValueValue: apiManagementNameValueValue
        apiManagementApiName: apiManagementApiName
        apiManagementApiDisplayName: apiManagementApiDisplayName
        apiManagementApiDescription: apiManagementApiDescription
        apiManagementApiSubscriptionRequired: apiManagementApiSubscriptionRequired
        apiManagementApiServiceUrl: apiManagementApiServiceUrl
        apiManagementApiPath: apiManagementApiPath
        apiManagementApiFormat: apiManagementApiFormat
        apiManagementApiValue: apiManagementApiValue
        apiManagementApiPolicyFormat: apiManagementApiPolicyFormat
        apiManagementApiPolicyValue: apiManagementApiPolicyValue
        apiManagementApiOperations: apiManagementApiOperations
    }
}

output id string = apimapi.outputs.id
output name string = apimapi.outputs.name
output path string = apimapi.outputs.path
