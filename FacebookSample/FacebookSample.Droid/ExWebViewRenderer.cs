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
            var webView = this.Control; //Ver1.4.x

            //WebViewではNavigateのイベントを拾えないためWebViewClientを上書きする
            webView.SetWebViewClient(new MyWebViewClient(exWebView));


            //クッキー（ログイン情報）の削除
            CookieManager.Instance.RemoveAllCookie();
        }

    }

    public class MyWebViewClient : WebViewClient {
        private readonly ExWebView _exWebView;

        public MyWebViewClient(ExWebView exWebView) {
            _exWebView = exWebView;
        }
        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url) {
            //イベントをForms側に送る
            _exWebView.OnNavigate(url);
            return false;
        }
    }
}
