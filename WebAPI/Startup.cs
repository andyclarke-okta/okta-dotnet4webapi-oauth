/*!
 * Copyright (c) 2016, Okta, Inc. and/or its affiliates. All rights reserved.
 * The Okta software accompanied by this notice is provided pursuant to the Apache License, Version 2.0 (the "License.")
 *
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *
 * See the License for the specific language governing permissions and limitations under the License.
 */


using Microsoft.Owin;
using Owin;
using System.Web.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;

[assembly: OwinStartup(typeof(SampleWebApiB.Startup))]

namespace SampleWebApiB
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            string audience = WebConfigurationManager.AppSettings["oidc.Audience"]; ;
            string issuer = WebConfigurationManager.AppSettings["oidc.Issuer"];


            TokenValidationParameters tvps = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidAudience = audience,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ClockSkew = System.TimeSpan.FromMinutes(5)
            };

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new JwtFormat(tvps,
                new OpenIdConnectCachingSecurityTokenProvider(issuer + "/.well-known/oauth-authorization-server")),
            });


        }
    }


}
