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

            //Xamarin.Form�̃R���g���[��(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //�l�C�e�B�u�R���g���[��(Android.Webkit.WebView)
            //var webView = ResourceBundle.Control;

            if (exWebView.DeleteCookie) {
                //�N�b�L�[�i���O�C�����j�̍폜
                CookieManager.Instance.RemoveAllCookie();
            }
        }

    }
}

