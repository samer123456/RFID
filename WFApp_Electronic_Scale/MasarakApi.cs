using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    public class MasarakApi
    {
        private string apiUrl = "http://60.253.213.6:8091/op";
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
    }
}
