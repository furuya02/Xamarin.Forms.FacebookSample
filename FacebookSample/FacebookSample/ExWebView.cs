using Xamarin.Forms;

namespace FacebookSample {
    public class ExWebView : WebView {

        public bool DeleteCookie { get; set; }
        public ExWebView(bool deleCookie) {
            DeleteCookie = deleCookie;
        }

    }
}

