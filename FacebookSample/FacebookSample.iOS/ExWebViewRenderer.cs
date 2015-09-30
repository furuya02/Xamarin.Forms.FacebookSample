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

            //Xamarin.Form�̃R���g���[��(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //�l�C�e�B�u�R���g���[��(MonoTouch.UIKit.UIWebView)
            //var webView = this;

            if (exWebView.DeleteCookie) {
                //�N�b�L�[�i���O�C�����j�̍폜
                var storage = NSHttpCookieStorage.SharedStorage;
                foreach (var cookie in storage.Cookies) {
                    storage.DeleteCookie(cookie);
                }
            }

        }
    }
}