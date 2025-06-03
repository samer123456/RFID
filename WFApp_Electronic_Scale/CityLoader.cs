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
    public class CityLoader
    {
        private ComboBox cmbCities;
        private string apiUrl = "https://api.example.com/cities"; 

        public CityLoader(ComboBox comboBox)
        {
            cmbCities = comboBox;
        }

        public async Task LoadCitiesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(apiUrl);
                    JArray cities = JArray.Parse(response); 

                    cmbCities.Items.Clear();
                    foreach (var city in cities)
                    {
                        cmbCities.Items.Add(city.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in fetch data from API: {ex.Message}");
                }
            }
        }
    }
}
