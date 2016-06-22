Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.Google
Imports Owin

Partial Public Class Startup
    ' For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    Public Sub ConfigureAuth(app As IAppBuilder)
        ' Configure the db context, user manager and signin manager to use a single instance per request
        app.CreatePerOwinContext(AddressOf ApplicationDbContext.Create)
        app.CreatePerOwinContext(Of ApplicationUserManager)(AddressOf ApplicationUserManager.Create)
        app.CreatePerOwinContext(Of ApplicationSignInManager)(AddressOf ApplicationSignInManager.Create)

        ' Enable the application to use a cookie to store information for the signed in user
        ' and to use a cookie to temporarily store inforation about a user logging in with a third party login provider
        ' Configure the sign in cookie
        ' OnValidateIdentity enables the application to validate the security stamp when the user logs in.
        ' This is a security feature which is used when you change a password or add an external login to your account.
        app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
            .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            .Provider = New CookieAuthenticationProvider() With {
                .OnValidateIdentity = SecurityStampValidator.OnValidateIdentity(Of ApplicationUserManager, ApplicationUser)(
                    validateInterval:=TimeSpan.FromMinutes(30),
                    regenerateIdentity:=Function(manager, user) user.GenerateUserIdentityAsync(manager))},
            .LoginPath = New PathString("/Account/Login")})

        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

        ' Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5))

        ' Enables the application to remember the second login verification factor such as phone or email.
        ' Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        ' This is similar to the RememberMe option when you log in.
        app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie)

        ' Uncomment the following lines to enable logging in with third party login providers
        app.UseMicrosoftAccountAuthentication(
            clientId:="f6853522-5578-4f43-9149-0154059758a9",
            clientSecret:="0qssET9O5c0h77jE1q1ZOWo"
        )

        Dim arrTwitterCerts() As String = {
        "A5EF0B11CEC04103A34A659048B21CE0572D7D47", ' VeriSign Class 3 Secure Server CA - G2
        "0D445C165344C1827E1D20AB25F40163D8BE79A5", ' VeriSign Class 3 Secure Server CA - G3
        "7FD365A7C2DDECBBF03009F34339FA02AF333133", ' VeriSign Class 3 Public Primary Certification Authority - G5
        "39A55D933676616E73A761DFA16A7E59CDE66FAD", ' Symantec Class 3 Secure Server CA - G4
        "‎add53f6680fe66e383cbac3e60922e3b4c412bed", ' Symantec Class 3 EV SSL CA - G3
        "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", ' VeriSign Class 3 Primary CA - G5
        "5168FF90AF0207753CCCD9656462A212B859723B", ' DigiCert SHA2 High Assurance Server C‎A 
        "B13EC36903F8BF4701D498261A0802EF63642BC3"  ' DigiCert High Assurance EV Root CA
        }
        app.UseTwitterAuthentication(New Security.Twitter.TwitterAuthenticationOptions() With {
            .ConsumerKey = "2itegvfvge99SWM3pZNiJypAa",
            .ConsumerSecret = "wzf3eN8ajvOIsQNaTBhdCdk76l9dDA2YcFzXjTaKDJyGxyAmfk",
            .BackchannelCertificateValidator = New Microsoft.Owin.Security.CertificateSubjectKeyIdentifierValidator(arrTwitterCerts)
        })

        app.UseFacebookAuthentication(
            appId:="296585730678175",
            appSecret:="bc5c40fb85b911d1b044a0ecc33accd5"
        )

        app.UseGoogleAuthentication(New GoogleOAuth2AuthenticationOptions() With {
            .ClientId = "557904966420-uq7m3ta8i4lsu515qsudrer02n7b7om0.apps.googleusercontent.com",
            .ClientSecret = "IY22da6j9ixJEJgfZ7J74QeT"
        })
    End Sub
End Class
