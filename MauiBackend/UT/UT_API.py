import requests

# 設定 API 的 URL
# url = "http://10.244.240.5:4564/api/AutoScrew"
url = "http://10.249.1.21/SFCSINFOAPI/WeatherForecast"

# 使用 requests 傳送 GET 請求
response = requests.get(url)

# 印出回應內容
print(response.text)
