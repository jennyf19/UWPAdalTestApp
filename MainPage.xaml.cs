using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MakeUWPGreatAgainTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string ClientId = "f6d51ae5-e244-4bed-9fe6-4fcc4c3873ff";
        private const string NullRedirectUri = "None - WEB redirect uri";
        private ObservableCollection<string> _redirectUris = new ObservableCollection<string>();

        public MainPage()
        {
            this.InitializeComponent();
            _redirectUris.Add(NullRedirectUri);
            var x = Windows.Security.Authentication.Web.WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
        }

        private Uri GetRedirectUri()
        {
            if (redirectUriCbx.SelectedValue == null)
            {
                return null;
            }

            string selectedRedirectUri = redirectUriCbx.SelectedValue.ToString();
            if (selectedRedirectUri == NullRedirectUri)
            {
                return null;
            }

            return new Uri(selectedRedirectUri);
        }

        private async void AccessTokenButton_Click(object sender, RoutedEventArgs e)
        {
            this.AccessToken.Text = string.Empty;
            AuthenticationContext ctx = new AuthenticationContext("https://login.microsoftonline.com/common");

            try
            {
                AuthenticationResult result = await ctx.AcquireTokenAsync(
                    "https://graph.windows.net",
                    ClientId,
                    GetRedirectUri(),
                    new PlatformParameters(PromptBehavior.Auto, false)).ConfigureAwait(false);

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        AccessToken.Text = "Signed in User - " + result.UserInfo.DisplayableId + "\nAccessToken: \n" + result.AccessToken;
                    });
            }
            catch (Exception exc)
            {
                await ShowErrorAsync(exc).ConfigureAwait(false);
            }
        }

        private async void ClearCache(object sender, RoutedEventArgs e)
        {
            this.AccessToken.Text = string.Empty;
            AuthenticationContext ctx = new AuthenticationContext("https://login.microsoftonline.com/common");
            try
            {
                ctx.TokenCache.Clear();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    AccessToken.Text = "Cache was cleared";
                });
            }
            catch (Exception exc)
            {
                await ShowErrorAsync(exc).ConfigureAwait(false);
            }
        }

        private async void ShowCacheCount(object sender, RoutedEventArgs e)
        {
            this.AccessToken.Text = string.Empty;
            AuthenticationContext ctx = new AuthenticationContext("https://login.microsoftonline.com/common");
            try
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    AccessToken.Text = "Token Cache count is " + ctx.TokenCache.Count;
                });
            }
            catch (Exception exc)
            {
                await ShowErrorAsync(exc).ConfigureAwait(false);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void Button_Click_2(object sender, RoutedEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.AccessToken.Text = string.Empty;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void AcquireTokenClientCred_Click(object sender, RoutedEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.AccessToken.Text = string.Empty;
            AuthenticationContext ctx = new AuthenticationContext("https://login.microsoftonline.com/common");

            try
            {
                AuthenticationResult result = await ctx.AcquireTokenAsync(
                    "https://graph.windows.net",
                    ClientId,
                    new UserCredential()).ConfigureAwait(false); // can add a

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        AccessToken.Text = "Signed in User - " + result.UserInfo.DisplayableId + "\nAccessToken: \n" + result.AccessToken;
                    });
            }
            catch (Exception exc)
            {
                await ShowErrorAsync(exc).ConfigureAwait(false);
            }
        }

        private async void AccessTokenSilentButton_Click(object sender, RoutedEventArgs e)
        {
            this.AccessToken.Text = string.Empty;
            AuthenticationContext context = new AuthenticationContext("https://login.microsoftonline.com/common");
            try
            {
                AuthenticationResult authResult = await context.AcquireTokenSilentAsync("https://graph.microsoft.com", ClientId).ConfigureAwait(false);

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                       () =>
                       {
                           AccessToken.Text = "Acquire Token Silent:\nSigned in User - " + authResult.UserInfo.DisplayableId + "\nAccessToken: \n" + authResult.AccessToken;
                       });
            }
            catch (Exception exc)
            {
                await ShowErrorAsync(exc).ConfigureAwait(false);
            }
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        private async void AcquireTokenIWA_Click(object sender, RoutedEventArgs e) // make sure to use a client id that is configured, such as the one from the .net sample
        {
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

            this.AccessToken.Text = string.Empty;
            AuthenticationContext context = new AuthenticationContext("https://login.microsoftonline.com/common");
            try
            {
                AuthenticationResult authResult = await context.AcquireTokenAsync(
                    "https://graph.microsoft.com",
                    ClientId, new UserCredential())
                        .ConfigureAwait(false);

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                       () =>
                       {
                           AccessToken.Text = "Acquire Token Silent:\nSigned in User - " + authResult.UserInfo.DisplayableId + "\nAccessToken: \n" + authResult.AccessToken;
                       });
            }
            catch (Exception exc)
            {
                await ShowErrorAsync(exc).ConfigureAwait(false);
            }
        }

        private async System.Threading.Tasks.Task ShowErrorAsync(Exception exc)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                  () =>
                  {
                      this.AccessToken.Text = "Auth failed: " + exc.Message;
                  });
        }
    }
}
