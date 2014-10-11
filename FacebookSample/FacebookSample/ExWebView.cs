using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FacebookSample {
    public class ExWebView :WebView{
        public event NavigateHandler Navigate;

        public delegate void NavigateHandler(string request);

        public void OnNavigate(string request) {
            if (Navigate != null) {
                Navigate(request);
            }
        }
    }
}
