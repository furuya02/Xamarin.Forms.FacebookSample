using System.ComponentModel;
using FacebookSample;
using FacebookSample.iOS;
using MonoTouch.Foundation;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MonoTouch.UIKit;


[assembly: ExportRenderer(typeof(ExWebView), typeof(ExWebViewRenderer))]
namespace FacebookSample.iOS {
    public class ExWebViewRenderer : WebViewRenderer {


        protected override void OnElementChanged(VisualElementChangedEventArgs e) {
            base.OnElementChanged(e);

            //Xamarin.Form�̃R���g���[��(ExWebView)
            var exWebView = e.NewElement as ExWebView;
            //�l�C�e�B�u�R���g���[��(MonoTouch.UIKit.UIWebView)
            var webView = this;

            webView.ShouldStartLoad = (w, request, naviType) => {
                //�C�x���g��Forms���ɑ���
                exWebView.OnNavigate(request.Url.AbsoluteString);
                return true;
            };

            //�N�b�L�[�i���O�C�����j�̍폜
            var storage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in storage.Cookies) {
                storage.DeleteCookie(cookie);
            }

        }
    }
}