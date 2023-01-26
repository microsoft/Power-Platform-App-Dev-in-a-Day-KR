# Basic Auth #

## Assumptions ##

* Make sure all the apps have been deployed to Azure and integrated with Azure APIM. Detailed deployment instructions can be found at [this document](../../README.md).
* You have an account on [Atlassian Jira](https://www.atlassian.com/software/jira).
  * If you don't have one, create it. Then, get the API token from your [account settings](https://id.atlassian.com/manage-profile/security/api-tokens).
  * Your Atlassian instance name has been stored as GitHub secret, in the name of `ATLASSIAN_INSTANCE_NAME`.


## Getting Started ##

1. Make sure `BASIC-AUTH` on API Management disables the following options:

   * `subscription required`

2. Make sure `BASIC-AUTH` on API Management selects the following options:

   * User authorization: `None`

3. Export OpenAPI v2 (JSON) from `BASIC-AUTH` on API Management with the filename of `apim-swagger-basicauth.json`.
4. Update the OpenAPI document by replacing both "securityDefinitions" and "security" attributes to allow basic auth:

   ```jsonc
   {
     "swagger": "2.0",
     ...
     // Replace both "securityDefinitions" and "security" attributes:
     "securityDefinitions": {
       "apiKeyHeader": {
         "type": "apiKey",
         "name": "Ocp-Apim-Subscription-Key",
         "in": "header"
       },
       "apiKeyQuery": {
         "type": "apiKey",
         "name": "subscription-key",
         "in": "query"
       }
     },
     "security": [
       {
         "apiKeyHeader": []
       },
       {
         "apiKeyQuery": []
       }
     ],
   
     // with these ones:
     "securityDefinitions": {
       "basic_auth": {
         "type": "basic"
       }
     },
     "security": [
       {
         "basic_auth": []
       }
     ],
     ...
   }
   ```

5. Create a new custom connector on either Power Apps or Power Platform by importing the OpenAPI file.

   * Connector name: `Basic Key Auth`

6. Make sure the authentication type.

   * Authentication type: `Basic authentication`
   * Parameter label: `username` and `password`

7. Remove all operations except `Profile`.
8. Create the connector.
9. Create a new connection by entering your Jira username and password. Your username will be your email address and password will be the API token.
10. Test the connector directly on the connector, a Power Apps app or a Power Automate workflow.
