


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading;
using System.Web.Http.Controllers;

namespace SampleWebApiB
{
    public class OktaAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {


        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool isAuthorized = base.IsAuthorized(actionContext);
            if (isAuthorized)
            {
                if (Thread.CurrentPrincipal != null)
                {
                    string strScope = System.Web.Configuration.WebConfigurationManager.AppSettings["okta.ScopeRequired"];
                    if (!string.IsNullOrEmpty(strScope))
                    {
                        ClaimsPrincipal principal = Thread.CurrentPrincipal as ClaimsPrincipal;// HttpContext.Current.User as ClaimsPrincipal;
                        IEnumerable<Claim> claimEnum = principal.Claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/scope");
                        List<Claim> scopeClaims = null;
                        if (claimEnum != null)
                        {
                            scopeClaims = claimEnum.ToList();

                        }
                        try
                        {
                            if (scopeClaims != null && scopeClaims.Count > 0)
                            {

                                Claim scopeClaim = scopeClaims.Find(g => g.Value == strScope);
                                if (scopeClaim == null)
                                {
                                    isAuthorized = false;
                                }
                                
                            }
                            else
                            {
                                isAuthorized = false;
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    else
                    {
                        //we specified no group on the method or class, so we'll assume the user is authorized
                        isAuthorized = true;
                    }

                }
                else
                {
                    isAuthorized = false;
                }
            }

            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var tokenHasExpired = false;
            base.HandleUnauthorizedRequest(actionContext);

            var owinContext = actionContext.Request.GetOwinContext();
            if (owinContext != null)
            {
                tokenHasExpired = owinContext.Environment.ContainsKey("oauth.token_expired");
            }
            if (tokenHasExpired)
            {
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        error = "invalid_token",
                        error_message = "The Token has expired"
                    });
            }
            else
            {
                if (owinContext.Authentication.User != null)
                {
                    if (owinContext.Authentication.User.Claims.Count() != 0)
                    {
                        actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                            new
                            {
                                error = "validation_error",
                                error_message = string.Format("The user could be found in the JWT claims (userid: {0}) but the JWT itself is invalid, most likely because it doesn't contain the proper groups claim value.", owinContext.Authentication.User.Claims.ElementAt(4))
                            });
                    }
                    else //the user has no claim so it looks like the JWT token couldn't be validated at all
                    {
                        actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                        new
                        {
                            error = "validation_error",
                            error_message = string.Format("The bearer token could not be validated, most likely  because the WebApi project's okta:OIDC_Issuer value in its web.config does not match the okta:TenantUrl value of the SinglePageWebApp project's web.config file.")
                        });

                    }
                }

                else
                {
                    actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                        new
                        {
                            error = "invalid_user",
                            error_message = "The user could not be found, so most likely the user claims could not be extracted from the token you sent"
                        });
                }
            }
        }

    }

    public class AuthenticationFailureMessage : HttpResponseMessage
    {
        public AuthenticationFailureMessage(string reasonPhrase, HttpRequestMessage request, object responseMessage)
            : base(HttpStatusCode.Unauthorized)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

            Content = new ObjectContent<object>(responseMessage, jsonFormatter);
            RequestMessage = request;
            ReasonPhrase = reasonPhrase;
        }
    }


}
