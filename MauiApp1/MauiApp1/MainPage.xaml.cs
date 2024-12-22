using System.ComponentModel;
using System.Windows.Input;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Plugin.Maui.Audio;
//using Microsoft.Maui.Essentials;


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

        string BackendApi = "https://192.168.22.53:5000";
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
        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            // Sample
            //await DisplayAlert("Login Successful", $"Welcome, {Username}!", "OK");
            //await DisplayAlert("Login Failed", "Please enter both Username and Password.", "OK");
            //await Navigation.PushAsync(new HomePage());

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                //var LoginResult = CheckUserInformation(Username, Password);

                //if (Convert.ToBoolean(LoginResult) == true)
                //    await Navigation.PushAsync(new HomePage());
                //else
                //    await DisplayAlert("Login Fail", "Please Check your Acc and Pwd", "OK");
            }
            else
            {
                await DisplayAlert("Login Fail", "Please Check your Input , Can't Null", "OK");
            }
        }

        private async void BtnRing_Clicked(object sender, EventArgs e)
        {
            //var audioManager = App.Current.Handler.MauiContext.Services.GetService<IAudioManager>();

            //if (audioManager != null)
            //{
            //    var player = await audioManager.CreateAsyncPlayer("Resources/Raw/notification.mp3");
            //    player.Play();
            //}
            //else
            //{
            //    Console.WriteLine("無法取得 IAudioManager 服務！");
            //}

        }

        private async void BtnShock_Clicked(object sender, EventArgs e)
        {
            Vibration.Default.Vibrate();

            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));

            Vibration.Default.Cancel();
        }
        #endregion

        #region Function
        void Init()
        {
            //LoadAppConfig();

            CompareApkVersion();
        }

        void LoadAppConfig()
        {
            //    BackendUrl = ConfigurationManager.AppSettings["BackendUrl"];
            //    BackendPort = ConfigurationManager.AppSettings["BackendPort"];

            //string baseUrl = _configuration["ApiSettings:BackendAPI"];
            //string endpoint = _configuration["ApiSettings:Endpoint"];
        }

        async Task CompareApkVersion()
        {
            string dbApkVersion = await CheckDBApkVersion();

            if (dbApkVersion == null)
            {
                return;
            }
            else
            {
                VersionTracking.Track();
                if (dbApkVersion == VersionTracking.CurrentVersion)
                {
                    lbVersion = "Version : " + VersionTracking.CurrentVersion;
                }
                else
                {
                    lbVersion = "Version Wrong!";
                    await DisplayAlert("Error", "Version Wrong!, please check your APK version", "OK");
                }
            }
        }

        async Task<string> CheckDBApkVersion()
        {
            string dbApkVersion = "";

            try
            {

                string apk = "maui";
                string apiUrl = $"{BackendApi}/api/Common/CheckApkVersion?apk={apk}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var json = System.Text.Json.JsonDocument.Parse(responseData);
                    if (json.RootElement.TryGetProperty("version", out var versionElement))
                    {
                        dbApkVersion = versionElement.GetString();
                    }
                    else
                    {
                        dbApkVersion = "Not Found Version";
                    }
                }
                else
                {
                    //Console.WriteLine($"API Call Fail，StatusCode: {response.StatusCode}");
                    return "API Call Fail，StatusCode: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }

            return dbApkVersion;
        }

        //async Task<bool> CheckUserInformation(string username, string password)
        //{
        //    try
        //    {
        //        string apiUrl = $"{BackendApi}/api/Common/CheckUserInfomation";

        //        var loginData = new
        //        {
        //            username = username,
        //            password = password
        //        };

        //        var jsonContent = new StringContent(
        //            System.Text.Json.JsonSerializer.Serialize(loginData),
        //            System.Text.Encoding.UTF8,
        //            "application/json"
        //        );

        //        HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, jsonContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseData = await response.Content.ReadAsStringAsync();

        //            return true;
        //        }
        //        else
        //        {
        //            //Console.WriteLine($"登入失敗，狀態碼: {response.StatusCode}");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine($"發生錯誤: {ex.Message}");
        //        await DisplayAlert("Login Fail", ex.Message.ToString(), "OK");
        //        return false;
        //    }
        //}

        public async Task<(bool success, string message)> CheckUserInformation(string username, string password)
        {
            try
            {
                string apiUrl = $"{BackendApi}/api/Common/CheckUserInfomation";

                // 準備請求資料
                var loginData = new
                {
                    username = username,
                    password = password
                };

                // 手動序列化
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // 使用 PostAsync 而不是 PostAsJsonAsync
                var response = await client.PostAsync(apiUrl, content);

                // 讀取回應內容
                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {responseData}"); // 用於除錯

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // 設定反序列化選項
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        };

                        // 嘗試解析回應
                        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseData, options);

                        if (loginResponse?.status == "0000")
                        {
                            return (true, "Login successful");
                        }
                        else
                        {
                            return (false, loginResponse?.message ?? "Login failed");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON Parse Error: {ex.Message}"); // 用於除錯
                        return (false, $"Response parsing error: {ex.Message}");
                    }
                }
                else
                {
                    var errorMessage = $"Server error: {response.StatusCode} - {responseData}";
                    await DisplayAlert("Login Failed", errorMessage, "OK");
                    return (false, errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                var errorMessage = $"Network error: {ex.Message}";
                await DisplayAlert("Login Failed", errorMessage, "OK");
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unexpected error: {ex.Message}";
                await DisplayAlert("Login Failed", errorMessage, "OK");
                return (false, errorMessage);
            }
        }
        #endregion
    }

    public class LoginResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public string? token { get; set; }
    }
}