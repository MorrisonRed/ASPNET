using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ASPNET.Models;

using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace ASPNET
{
    public partial class Startup
    {
        const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            // Microsoft : Create application
            // https://account.live.com/developers/applications
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("MicrosoftClientId")))
            {
                var msaccountOptions = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationOptions()
                {
                    ClientId = ConfigurationManager.AppSettings.Get("MicrosoftClientId"),
                    ClientSecret = ConfigurationManager.AppSettings.Get("MicrosoftClientSecret"),
                    Provider = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationProvider()
                    {
                        OnAuthenticated = (context) =>
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim("urn:microsoftaccount:access_token", context.AccessToken, XmlSchemaString, "Microsoft"));

                            return Task.FromResult(0);
                        }
                    }
                };

                app.UseMicrosoftAccountAuthentication(msaccountOptions);
            }

            // Twitter : Create a new application
            // https://dev.twitter.com/apps
            //app.UseTwitterAuthentication(
            //   consumerKey: ConfigurationManager.AppSettings.Get("TwitterConsumerKey"),
            //   consumerSecret: ConfigurationManager.AppSettings.Get("TwitterConsumerSecret"));
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TwitterConsumerKey")))
            {
                var twitterOptions = new Microsoft.Owin.Security.Twitter.TwitterAuthenticationOptions
                {
                    ConsumerKey = ConfigurationManager.AppSettings.Get("TwitterConsumerKey"),
                    ConsumerSecret = ConfigurationManager.AppSettings.Get("TwitterConsumerSecret"),
                    Provider = new Microsoft.Owin.Security.Twitter.TwitterAuthenticationProvider
                    {
                        OnAuthenticated = (context) =>
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim("urn:twitter:access_token", context.AccessToken, XmlSchemaString, "Twitter"));
                            return Task.FromResult(0);
                        }
                    }
                };

                app.UseTwitterAuthentication(twitterOptions);
            }

            // Facebook : Create New App
            // https://developers.facebook.com/apps
            //app.UseFacebookAuthentication(
            //   appId: ConfigurationManager.AppSettings.Get("FacebookAppId"),
            //   appSecret: ConfigurationManager.AppSettings.Get("FacebookAppSecret"));
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("FacebookAppId")))
            {
                var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
                {
                    AppId = ConfigurationManager.AppSettings.Get("FacebookAppId"),
                    AppSecret = ConfigurationManager.AppSettings.Get("FacebookAppSecret"),
                    Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider
                    {
                        OnAuthenticated = (context) =>
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
                            foreach (var x in context.User)
                            {
                                var claimType = string.Format("urn:facebook:{0}", x.Key);
                                string claimValue = x.Value.ToString();
                                if (!context.Identity.HasClaim(claimType, claimValue))
                                    context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, XmlSchemaString, "Facebook"));

                            }
                            return Task.FromResult(0);
                        }
                    }
                };
                facebookOptions.Scope.Add("email");
                app.UseFacebookAuthentication(facebookOptions);
            }

            // Google Plus : Create New App
            // https://console.developers.google.com
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings.Get("GooglePlusClientID"),
                ClientSecret = ConfigurationManager.AppSettings.Get("GooglePlusClientSecret")
            });
        }
    }
}