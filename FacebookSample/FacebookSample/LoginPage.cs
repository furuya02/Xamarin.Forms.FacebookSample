using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace FacebookSample {
    internal class LoginPage : ContentPage {

        private readonly FacebookClient _fb;
        private readonly MyPage _parent;

        public LoginPage(MyPage parent, string appId, string extendedPermissions,bool deleteCookie) {

            _parent = parent;

            Title = "Login"; //ページのタイトル

            _fb = new FacebookClient();

            var exWebView = new ExWebView(deleteCookie) {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //Uri遷移のイベントを処理する
            exWebView.Navigated += (s, e) => {
                //リクエストUriから認証の状態を判断する
                FacebookOAuthResult oauthResult;
                if (!_fb.TryParseOAuthCallbackUrl(new Uri(e.Url), out oauthResult)) {
                    return; //認証継続中
                }
                if (oauthResult.IsSuccess) {
                    //認証成功
                    LoginSucceded(oauthResult.AccessToken);
                } else {
                    //認証失敗
                    LoginSucceded(string.Empty);
                }
            };

            //認証URLへ移動 (https://www.facebook.com/dialog/oauth)
            exWebView.Source = _fb.GetLoginUrl(appId, extendedPermissions).AbsoluteUri;

            Content = exWebView;
        }

        private async void LoginSucceded(string accessToken) {
            try {
                var fb = new FacebookClient(accessToken);
                var json = await fb.GetTaskAsync("me?fields=id");
                var o = JObject.Parse(json);
                var id = (string)o["id"];
                _parent.SetStatus(true, accessToken, id, "");//ログイン成功
            } catch (Exception ex) {
                _parent.SetStatus(false, accessToken, "", ex.Message);//ログイン失敗
            }
            await Navigation.PopAsync();//メインビューへ戻る
        }
    }
}
