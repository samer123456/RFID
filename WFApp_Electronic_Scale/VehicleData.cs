using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFApp_Electronic_Scale
{
    public class VehicleData
    {
        public string PlateNumber { get; set; }
        public string TagId { get; set; }
        public double VehicleWeight { get; set; }
    }
    public class UhfModel
    {
        public string Command { get; set; } = string.Empty;    
        public string Tag { get; set; } = string.Empty;    
    }
}
