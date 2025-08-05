using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Ports;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WFApp_Electronic_Scale
{
    public class MasarakApi
    {
        private static string settingFilePath = "setting.json";
        private static string apiUrl = "https://stage-masarak.frappe.cloud/api/method/get_trip_by_tag_id";// "http://60.253.213.6:8091/op";
        private static string username = "f39a13dc264037d";
        private static string password = "42ec5dcfc6e0d58";

        public MasarakApi()
        {
            GetSettingFromFile();
        }
        public async Task<string> GetPlateNumberAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(apiUrl);
                    // Parse JSON array and extract first plate number
                    var plates = JArray.Parse(response);
                    return plates[0]?["plateNumber"]?.ToString();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Network error: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Invalid API response: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in fetch plate number from API: {ex.Message}");
                }
                return null;
            }
        }

        public async Task<string> SendVehicleDataAsync(string tagId, string plateNumber, double weight)
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(30); // تحديد مهلة 30 ثانية

                    // إنشاء كائن البيانات
                    var data = new VehicleData
                    {
                        TagId = tagId,
                        PlateNumber = plateNumber,
                        VehicleWeight = weight
                    };

                    // استخدام System.Text.Json مع خيارات
                    //string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                    //{
                    //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // تحويل الأسماء إلى camelCase
                    //});

                    string json = JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // إرسال الطلب واستلام الرد
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // التحقق من نجاح العملية
                    response.EnsureSuccessStatusCode();

                    // قراءة المحتوى وإعادته
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"خطأ في الاتصال بالخادم: {ex.Message}");
                }
                catch (TaskCanceledException)
                {
                    MessageBox.Show("انتهت مهلة الاتصال بالخادم");
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"خطأ في معالجة البيانات: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ غير متوقع: {ex.Message}");
                }

                return null;
            }
        }
        public async Task<string> GetPlateNumberAsync(string command, int tag)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // إضافة التوثيق الأساسي (Basic Auth)
                    var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                    // بناء عنوان URL مع معاملات الاستعلام
                    var uriBuilder = new UriBuilder(apiUrl);
                    string query = $"command={Uri.EscapeDataString(command)}&tag_id={Uri.EscapeDataString(tag.ToString())}";

                    // التعامل مع العلامات الموجودة مسبقًا في URL
                    if (uriBuilder.Query != null && uriBuilder.Query.Length > 1)
                    {
                        uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + query;
                    }
                    else
                    {
                        uriBuilder.Query = query;
                    }

                    string fullUrl = uriBuilder.ToString();

                    // إرسال طلب GET
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    // التحقق من نجاح الاستجابة
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"API error: {response.StatusCode}\n{errorContent}");
                        return null;
                    }

                    // قراءة ومعالجة البيانات
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //JArray plates = JArray.Parse(responseBody);
                    //return plates[0]?["plateNumber"]?.ToString();

                    // تحليل الاستجابة ككائن JSON بدلاً من مصفوفة
                    var responseObj = JObject.Parse(responseBody);

                    // الحصول على المصفوفة داخل message["1"]
                    var platesArray = responseObj["message"]?["trip"] as JObject;

                    // التحقق من وجود المصفوفة وأنها تحتوي على عناصر
                    if (platesArray != null && platesArray.Count > 0)
                    {
                        // الحصول على أول عنصر في المصفوفة
                        //var firstPlate = platesArray[0];

                        // استخراج قيمة "name"
                        return platesArray["truck_number"]?.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Network error: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Invalid API response: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching plate number: {ex.Message}");
                }
                return null;
            }
        }
        private static void GetSettingFromFile()
        {
            try
            {
                if (File.Exists(settingFilePath))
                {
                    string json = File.ReadAllText(settingFilePath);
                    var settings = JsonConvert.DeserializeObject<SettingsModel>(json);

                    apiUrl = string.IsNullOrWhiteSpace(settings.ApiUrl) ? apiUrl : settings.ApiUrl;
                    username = string.IsNullOrWhiteSpace(settings.Username) ? username : settings.Username;
                    password = string.IsNullOrWhiteSpace(settings.Password) ? password : settings.Password;

                    MessageBox.Show($"Port Configured: {apiUrl}, {username}, {password}");
                }
                else
                {
                    MessageBox.Show("ملف الاعدادات غير موجود!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
