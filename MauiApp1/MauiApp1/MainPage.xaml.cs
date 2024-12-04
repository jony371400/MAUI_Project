using System.ComponentModel;
using System.Windows.Input;
using System.Net.Http;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region Default Sample
        //int count = 0;
        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
        #endregion

        #region Property
        private string? _username;
        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        private string? _lbversion;
        public string? lbVersion
        {
            get => _lbversion;
            set
            {
                _lbversion = value;
                NotifyPropertyChanged(nameof(lbVersion));
            }
        }

        public ICommand LoginCommand { get; }
        #endregion

        //HttpClient client = new HttpClient();

        private static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // 忽略自簽名證書驗證
        });

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            Init();
        }

        #region Update
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Event
        private async void Button_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Login Successful", $"Welcome, {Username}!", "OK");
            //await DisplayAlert("Login Failed", "Please enter both Username and Password.", "OK");

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                if (Username == "11107647" && Password == "000000")
                    await Navigation.PushAsync(new HomePage());
                else
                    await DisplayAlert("Login Fail", "Please Check your Acc and Pwd", "OK");
            }
            else
                await DisplayAlert("Login Fail", "Please Check your Input , Can't Null", "OK");

        }
        #endregion

        #region Function
        void Init()
        {
            CheckApkVersion();

            //// 開啟版本追蹤
            //VersionTracking.Track();

            //// 獲取當前版本號
            ////lbVersion = "Version : " + VersionTracking.CurrentVersion;
            //lbVersion = "Version : " + VersionTracking.CurrentBuild;
        }

        async Task CheckApkVersion()
        {
            try
            {
                // 定義 API URL 和參數
                string apk = "maui";
                //string apiUrl = $"https://127.0.0.1:5000/api/Common/CheckApkVersion?apk={apk}";
                string apiUrl = $"https://10.0.2.2:5000/api/Common/CheckApkVersion?apk={apk}";

                // 發送 GET 請求
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // 確認是否成功
                if (response.IsSuccessStatusCode)
                {
                    // 讀取回應內容
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API 回應: {responseData}");
                }
                else
                {
                    Console.WriteLine($"API 呼叫失敗，狀態碼: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤: {ex.Message}");
            }
        }

        #endregion
    }
}