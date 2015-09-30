using Android.Webkit;
using FacebookSample;
using FacebookSample.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FacebookSample.Droid {
    public class ExWebViewRenderer : WebViewRenderer {

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e) {
            base.OnElementChanged(e);

            //Xamarin.Formのコントロール(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //ネイティブコントロール(Android.Webkit.WebView)
            //var webView = ResourceBundle.Control;

            if (exWebView.DeleteCookie) {
                //クッキー（ログイン情報）の削除
                CookieManager.Instance.RemoveAllCookie();
            }
        }

    }
}

