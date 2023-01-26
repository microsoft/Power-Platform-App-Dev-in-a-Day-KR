# API Key Auth #

## Assumptions ##

* Make sure all the apps have been deployed to Azure and integrated with Azure APIM. Detailed deployment instructions can be found at [this document](../../README.md).


## Getting Started ##

1. Make sure `API-KEY-AUTH` on API Management enables the following options:

   * `subscription required`

2. Make sure `API-KEY-AUTH` on API Management selects the following options:

   * User authorization: `None`

3. Export OpenAPI v2 (JSON) from `API-KEY-AUTH` on API Management with the filename of `apim-swagger-apikeyauth.json`.
4. Create a new custom connector on either Power Apps or Power Platform by importing the OpenAPI file.

   * Connector name: `API Key Auth`

5. Make sure the authentication type.

   * Authentication type: `API Key`
   * Parameter label: `API Key`
   * Parameter name: `Ocp-Apim-Subscription-Key`
   * Parameter location: `Header`

6. Remove all operations except `Greeting`.
7. Create the connector.
8. Create a new connection by entering the APIM subscription key.
9. Test the connector directly on the connector, a Power Apps app or a Power Automate workflow.
