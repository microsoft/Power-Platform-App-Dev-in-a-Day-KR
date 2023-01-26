# BFF &ndash; Combination of API Key Auth and Basic Auth #

## Assumptions ##

* Make sure all the apps have been deployed to Azure and integrated with Azure APIM. Detailed deployment instructions can be found at [this document](../../README.md).


## Getting Started ##

### BFF ###

1. Open `./custom-connectors-in-a-day/infra/apim-openapi-bff.yaml` and replace `{{AZURE_ENV_NAME}}` with your resource name.
2. Add a new API on API Management.
3. Select "OpenAPI" under the "Create from definition" section.
4. Toggle to "Full".
5. Select `./custom-connectors-in-a-day/infra/apim-openapi-bff.yaml`.
6. Enter `bff` into the "API URL suffix" field.
7. Click "Create".
8. Select the "BFF" API then the "Greeting" operation.
9. Replace the backend service URL with `https://apim-{{AZURE_ENV_NAME}}.azure-api.net/apikeyauth`. Make sure you tick the "override" checkbox.
10. Select the "Profile" operation.
11. Replace the backend service URL with `https://apim-{{AZURE_ENV_NAME}}.azure-api.net/basicauth`. Make sure you tick the "override" checkbox.
12. Test both endpoints
    * Give the name parameter to the "Greeting" operation.
    * Give the basic auth token to the "Profile" operation.


### Custom Connector ###

TBD
