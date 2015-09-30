using FacebookSample;
using FacebookSample.iOS;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FacebookSample.iOS {
    public class ExWebViewRenderer : WebViewRenderer {

        protected override void OnElementChanged(VisualElementChangedEventArgs e) {
            base.OnElementChanged(e);

            //Xamarin.Formのコントロール(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //ネイティブコントロール(MonoTouch.UIKit.UIWebView)
            //var webView = this;

            if (exWebView.DeleteCookie) {
                //クッキー（ログイン情報）の削除
                var storage = NSHttpCookieStorage.SharedStorage;
                foreach (var cookie in storage.Cookies) {
                    storage.DeleteCookie(cookie);
                }
            }

        }
    }
}