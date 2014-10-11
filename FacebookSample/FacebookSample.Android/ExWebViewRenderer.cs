using Android.Webkit;
using FacebookSample;
using FacebookSample.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FacebookSample.Droid {
    public class ExWebViewRenderer : WebRenderer {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e) {
            base.OnElementChanged(e);

            //Xamarin.Formのコントロール(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //ネイティブコントロール(Android.Webkit.WebView)
            var webView = Control;

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
