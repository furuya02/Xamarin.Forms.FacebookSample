using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace FacebookSample {
    public class App : Application {
        public App() {
            //メインページをパラメータとしてNavigationPageを生成する
            MainPage = new NavigationPage(new MyPage());
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }

    }
    //メインのページ
    public class MyPage : ContentPage {

        //Facebook アプリID
        //        private const string AppId = "YOUR_FACEBOOK_APP_ID_HERE";
        private const string AppId = "388450734589960";


        //要求するアクセス許可                                                
        //        private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream";
        private const string ExtendedPermissions = "user_friends,user_posts";


        private bool _isLoggedIn = false;
        private FacebookClient _fb = null;

        private readonly Label _label;
        private Image _img;

        private String _lastMessageId = null;

        public MyPage() {


            if (AppId == "YOUR_FACEBOOK_APP_ID_HERE") {

                Content = new Label() {
                    Text = "https://developers.facebook.com/ \nで、Facebookアプリを作成し、取得したIDを、このクラスのAppIdにセットして下さい。"
                };
                return;
            }


            Title = "Facebook Sample"; //ページのタイトル



            _img = new Image() {
                WidthRequest = 100,
                HeightRequest = 100
            };
            _label = new Label() {
                XAlign = TextAlignment.Center
            };



            SetStatus(false, "", "", "");//ログアウトに初期化する

            //リストビュー(メニュー)の生成
            var listView = new ListView {
                ItemsSource = new List<string>{
                        "Login",
                        "Graph API Sample",
                        "FQL Sample",
                        "Post \"Hi\" to your wall",
                        "Remove \"Hi\" to your wall"
                    }
            };

            //リストビュー(メニュー)選択時の処理
            listView.ItemSelected += async (s, a) => {
                if (a.SelectedItem != null) {
                    var menu = (string)a.SelectedItem;
                    switch (menu[0]) {
                        case 'L': //Login
                            SetStatus(false, "", "", "");//いったんログアウト状態に初期化する
                            //LoginPageへの遷移
                            await Navigation.PushAsync(new LoginPage(this, AppId, ExtendedPermissions, true));
                            break;
                        case 'G': //Graph API Sample
                            FuncGraph();
                            break;
                        case 'F': //FQL Sample
                            FuncFql();
                            break;
                        case 'P': //Post Hi to your wall
                            FuncPost();
                            break;
                        case 'R': //Remove Hi to your wall
                            FuncRemove();
                            break;
                    }
                    listView.SelectedItem = null; //メニュー選択の解除
                }
            };

            //メインページの画面構成
            Content = new StackLayout {
                Children = { _img, _label, listView }
            };

        }


        public void SetStatus(bool isSuccess, string accessToken, string id, string error) {
            _isLoggedIn = isSuccess;
            if (_isLoggedIn) {
                _fb = new FacebookClient(accessToken);

                //アイコン及びステータスの初期化（ログイン）
                _label.Text = "Login";
                _label.BackgroundColor = Color.Aqua;


                _img.Source = string.Format("https://graph.facebook.com/{0}/picture?width=100&height=100", id);


            } else {
                if (!String.IsNullOrEmpty(error)) { //エラーメッセージがセットされている場合
                    DisplayAlert("ERROR", error, "OK");
                }
                _fb = null;
                //アイコン及びステータスの初期化（ログアウト）
                _label.Text = "Not Logged In";
                _label.BackgroundColor = Color.Red;
                _img.Source = ImageSource.FromFile("logout.png");
            }

        }

        private async void FuncGraph() {
            if (_isLoggedIn) {
                var title = "ERROR";
                var msg = "";
                try {
                    var json = await _fb.GetTaskAsync("me");
                    var o = JObject.Parse(json);
                    msg = "Name: " + o["name"] + "\n" +
                        "First Name: " + o["first_name"] + "\n" +
                        "Last Name: " + o["last_name"] + "\n" +
                        "Profile Url: " + o["link"];
                    title = "Your Info";
                } catch (Exception ex) {
                    msg = ex.Message;
                }
                await DisplayAlert(title, msg, "OK");
            } else {
                await DisplayAlert("Not Logged In", "Please Log In First", "OK");
            }
        }

        private async void FuncFql() {



            if (_isLoggedIn) {
                var title = "ERROR";
                var msg = "";
                try {
                    var query = string.Format(
                            "SELECT uid FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})",
                            "me()");
                    var json = await _fb.GetTaskAsync("fql?q=" + query);
                    var o = JObject.Parse(json);
                    title = "Info";
                    msg = string.Format("You have {0} friend(s)", o["data"].ToList().Count);
                } catch (Exception ex) {
                    msg = ex.Message;
                }
                await DisplayAlert(title, msg, "OK");
            } else {
                DisplayAlert("Not Logged In", "Please Log In First", "OK");
            }

        }

        private async void FuncPost() {
            if (_isLoggedIn) {

                if (!string.IsNullOrEmpty(_lastMessageId)) {
                    DisplayAlert("Error", "Please Remove \"Hi\" to your wall", "Ok");
                    return;
                }

                var title = "ERROR";
                var msg = "";
                try {
                    var json = await _fb.PostTaskAsync("me/feed", new { message = "Hi" });
                    var o = JObject.Parse(json);
                    var id = (String)o["id"];
                    if (!String.IsNullOrEmpty(id)) {
                        title = "Success";
                        msg = string.Format("You have posted \"Hi\" to your wall. Id: {0}", id);
                        _lastMessageId = id;
                    } else {
                        msg = (string)o["error"]["message"];
                    }
                } catch (Exception ex) {
                    _lastMessageId = null;
                    msg = ex.Message;
                }
                await DisplayAlert(title, msg, "OK");
            } else {
                DisplayAlert("Not Logged In", "Please Log In First", "OK");
            }
        }

        private async void FuncRemove() {
            if (_isLoggedIn) {
                if (string.IsNullOrEmpty(_lastMessageId)) {
                    DisplayAlert("Error", "Please Post \"Hi\" to your wall first", "Ok");
                    return;
                }

                var title = "ERROR";
                var msg = "";
                try {
                    var result = await _fb.DeleteTaskAsync(_lastMessageId);
                    if (result == "true") {
                        title = "Success";
                        msg = "You have deleted \"Hi\" from you wall.";
                    }
                } catch (Exception ex) {
                    msg = ex.Message;
                }
                await DisplayAlert(title, msg, "OK");
                _lastMessageId = null;
            } else {
                DisplayAlert("Not Logged In", "Please Log In First", "OK");
            }
        }
    }
}
