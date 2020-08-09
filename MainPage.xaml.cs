using System;
using System.Collections.Generic;
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
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Resources;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorldUWPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var x = AppDataPaths.GetDefault();
            Debug.WriteLine(x.LocalAppData);
            var uri = new Uri("ms-appdata:///local/HelloWorldMsAppData.html");
            StreamUriWinRTResolver resolver = new StreamUriWinRTResolver();
            var lsuri = MyWebView.BuildLocalStreamUri(@"MyWebView", uri.PathAndQuery);
            Debug.WriteLine($@"WebView: Navigating to URI: {lsuri.AbsoluteUri}");
            // Pass the resolver object to the navigate call.
            MyWebView.NavigateToLocalStreamUri(lsuri, resolver);
        }
    }

    public sealed class StreamUriWinRTResolver : IUriToStreamResolver
    {
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new Exception();
            }
            string path = uri.AbsolutePath;

            // Because of the signature of the this method, it can't use await, so we 
            // call into a separate helper method that can use the C# await pattern.
            return GetContent(path).AsAsyncOperation();
        }

        private async Task<IInputStream> GetContent(string path)
        {
            Debug.WriteLine("GetContent invoked");
            // We use a package folder as the source, but the same principle should apply
            // when supplying content from other locations
            try
            {
                // Don't use "ms-appdata:///" on the scheme string, because inside the path
                // will contain "/local/MyFolderOnLocal/index.html"
                string scheme = @"ms-appdata://" + path;

                Uri localUri = new Uri(scheme);
                StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(localUri);
                IRandomAccessStream stream = await f.OpenAsync(FileAccessMode.Read);
                return stream.GetInputStreamAt(0);
            }
            catch (Exception) { throw new Exception("Invalid path"); }
        }
    }


}
