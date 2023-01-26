# OAuth2 &ndash; Authorisation Code Auth #

## Assumptions ##

* Make sure all the apps have been deployed to Azure and integrated with Azure APIM. Detailed deployment instructions can be found at [this document](../../README.md).


## Azure AD App Registration ##

To apply OAuth2 &ndash; Authorisation Code Auth approach to your custom connector through APIM, you need to create an app onto Azure AD first.


### For API Management ###

1. Go to Azure Portal, and navigate to "Azure Active Directory ➡️ App registrations".
2. Create a new app.

   * Give it a name. Let's say `spn-ppim-authcodeauth`.
   * Choose account type of "Accounts in this organizational directory only (Microsoft only - Single tenant)".
   * Enter a redirect URL for now: `https://oauth.pstmn.io/v1/callback` (this points to Postman).
   * Click "Register".

3. Once created, navigate to the "Authentication" blade.
4. Add more redirect URLs. Make sure you need to replace `{{APIM_NAME}}` with your APIM instance name:

   * `https://{{APIM_NAME}}.azure-api.net/signin-oauth/code/callback/oauth2-auth-code-flow`

5. Click "Save".
6. Navigate to the "API permissions" blade.
7. Make sure a delegation permission for "Microsoft Graph ➡️ User.Read" has been created.
8. Navigate to the "Certification & secrets" blade.
9. Create a new client secret.
10. Note your application ID (client ID) and client secret.


### For Power Platform Custom Connector ###

1. Go to Azure Portal, and navigate to "Azure Active Directory ➡️ App registrations".
2. Create a new app.

   * Give it a name. Let's say `spn-ppim-pp`.
   * Choose account type of "Accounts in this organizational directory only (Microsoft only - Single tenant)".
   * Enter a redirect URL for now: `https://oauth.pstmn.io/v1/callback` (this points to Postman).
   * Click "Register".

3. Once created, navigate to the "Authentication" blade.
4. Add more redirect URLs:

   * `https://global.consent.azure-apim.net/redirect`

5. Click "Save".
6. Navigate to the "API permissions" blade.
7. Make sure a delegation permission for "Microsoft Graph ➡️ User.Read" has been created.
8. Navigate to the "Certification & secrets" blade.
9. Create a new client secret.
10. Note your application ID (client ID) and client secret.


## Getting Started ##

1. Open your APIM instance and navigate to the "OAuth 2.0 + OpenID Connect" blade.
2. Add a new OAuth 2.0 service.

   * Give it a display name of `AuthCode Auth`.
   * Enter the "Client registration page URL" of any arbitrary one. For example, `http://localhost`.
   * Make sure the `Authorization code` is selected under the "Authorization grant types".
   * Enter the "Authorization endpoint URL" of `https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/authorize`. Make sure you need to replace `{{TENANT_ID}}` with your tenant ID.
   * Enter the "Token endpoint URL" of `https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/token`. Make sure you need to replace `{{TENANT_ID}}` with your tenant ID.
   * Enter the default scope of `https://graph.microsoft.com/.default`.
   * Enter the "Client ID" of your client ID noted above for `spn-ppapim-authcodeauth`.
   * Enter the "Client secret" of your client secret noted above for `spn-ppapim-authcodeauth`.
   * Client "Create".

3. Make sure `AUTH-CODE-AUTH` on API Management disables the following options:

   * `subscription required`

4. Make sure `AUTH-CODE-AUTH` on API Management selects the following options:

   * User authorization: `OAuth 2.0`
   * OAuth 2.0 server: `AuthCode Auth`

5. Export OpenAPI v2 (JSON) from `AUTH-CODE-AUTH` on API Management with the filename of `apim-swagger-authcodeauth.json`.
6. Update the OpenAPI document by removing `apiKeyHeader` and `apiKeyQuery` from both "securityDefinitions" and "security" attributes to allow auth-code auth. Make sure you need to replace `{{TENANT_ID}}` with your tenant ID:

   ```jsonc
   {
     "swagger": "2.0",
     ...
     // Leave only "oauth2AuthCode Auth" on both "securityDefinitions" and "security" attributes:
     "securityDefinitions": {
       "oauth2AuthCode Auth": {
         "type": "oauth2",
         "scopes": {
           "https://graph.microsoft.com/.default": ""
         },
         "flow": "accessCode",
         "authorizationUrl": "https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/authorize",
         "tokenUrl": "https://login.microsoftonline.com/{{TENANT_ID}}/oauth2/v2.0/token"
       }
     },
     "security": [
      {
        "oauth2AuthCode Auth": [
          "https://graph.microsoft.com/.default"
        ]
      }
     ],
     ...
   }
   ```

7. Create a new custom connector on either Power Apps or Power Platform by importing the OpenAPI file.

   * Connector name: `AuthCode Auth`

8. Make sure the authentication type.

   * Authentication type: `OAuth 2.0`
   * Identity provider: `Generic Oauth 2`
   * Client ID: noted from above for `spn-ppapim-pp`
   * Client secret: noted from above for `spn-ppapim-pp`
   * Refresh URL: same as the token URL

9. Remove all operations except `Profile`.
10. Create the connector.
11. Create a new connection. No need to enter client ID or client secret this time.
12. Test the connector directly on the connector, a Power Apps app or a Power Automate workflow.
