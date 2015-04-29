using FacebookSample;
using FacebookSample.WinPhone;
using Microsoft.Phone.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FacebookSample.WinPhone {
    public class ExWebViewRenderer : WebViewRenderer {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e) {
            base.OnElementChanged(e);

            //Xamarin.Formのコントロール(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //ネイティブコントロール(Microsoft.Phone.Controls.WebBrowser)
            var webBrowser = Control;


            webBrowser.IsScriptEnabled = true; //デフォルトでOFFになっているJavaScriptをONにしないと認証できない

            webBrowser.Navigating += (s, a) => {
                //イベントをForms側に送る
                exWebView.OnNavigate(a.Uri.AbsoluteUri);
            };

            //クッキー（ログイン情報）の削除
            webBrowser.ClearCookiesAsync();
        }

    }

}
