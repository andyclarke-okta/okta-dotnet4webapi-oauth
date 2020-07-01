# okta-dotnet4webapi-oauth

> Okta OAUTH integration example using using ASP.NET 4 Web Api OWIN

![Version](https://img.shields.io/badge/version-v0.3.0.beta-blue.svg)

Okta provides an API Access Management feature which allows you to enable OAUTH 2.0 for protecting your backend Web Api's.
Please refer to [Implementing OAuth 2.0 Authentication](https://developer.okta.com/authentication-guide/implementing-authentication/)

## System Requirements ##
This sample was built using Visual Studio 2017 with .Net Framework 4.7.2

## Usage ##
if you do not have access to an Okta Tenant you can get a free Okta Developer Tenant [here](https://developer.okta.com/signup/)
The Okta tenent requires the API Access management feature and some configuration to integrate your backend Web Api to use OAUTH for authorization.
Please refer to [Customizing Your Authorization Server](https://developer.okta.com/authentication-guide/implementing-authentication/set-up-authz-server)

The defined endpoints are in the ValueController. For testing purposes there is both a protected and a unprotected endpoint. The API call requesting access
to the protected endpoints need to include the Authorization header with contents of Bearer accessToken. The access token shall have been minted by the same
Okta Tenant, as specified in the web.config settings.


The web.config file needs to be edited to point to your configuration and your Okta tenent.

For example;

  <appSettings>
    <!-- Okta API AM Authorization Server Required-->
    <add key="oidc.Audience" value="api://testapps"/>
    <add key="oidc.Issuer" value="https://dev-assurant.oktapreview.com/oauth2/ausfw010erYbq9d5O0h7"/>
    <add key="okta.RequiredGroupMembership" value="TST Users"/>
    <add key="okta.ScopeRequired" value="groups"/>
  </appSettings>

### Sample Access Token ###
```javascript
{
 "ver": 1,
 "jti": "AT.pauseckwfpxYhmJ9k1U160GFfhEKsJl53WEkkhR0AaI",
 "iss": "https://dev-assurant.oktapreview.com/oauth2/ausfw010erYbq9d5O0h7",
 "aud": "api://testapps",
 "iat": 1545238757,
 "exp": 1545242357,
 "cid": "0oadt4xjwdPgIrMP00h7",
 "uid": "00ui33011eHJ4SbMj0h7",
 "scp": [
  "groups",
  "openid",
  "profile"
 ],
 "sub": "dick.tracy",
 "groups": [
  "Everyone",
  "TST External Users",
  "TST Users"
 ]
}  
``` 
  
## Features ##
This sample .Net MVC API application using OAUTH 2.0 OWIN middleware to protect selected endpoints. 
It is necessary to supply only a few parameters. The remaining configuration is dynamically retreived using /.well-known/oauth-authorization-server endpoint.
There are multiple types of authorization possible depending on the use case. These are;
* Requiring that only a valid access token is present
	* [Authorize] annotation
* Require specific scopes
	* this can equate to a specific role or permission
	* [OktaAuthorize] annotation
* Require a specific value for a claim
	* [OktaGroupAuthorize] annotation

The sample application also demostrates; 
* How to access user related claims from main code. 
	
	
## Contributing

1. Fork it
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

**Note**: Contributing is very similar to the Github contribution process as described in detail 
[here](https://guides.github.com/activities/forking/).

## Contacts

- [Wayne Carson](https://assurhub.assurant.com/yc6235) – [wayne.carson@assurant.com](mailto:wayne.carson@assurant.com)
- [Andy Clarke](https://assurhub.assurant.com/fz6302) - [andy.clarke@assurant.com](mailto:andy.clarke@assurant.com)
