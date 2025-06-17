using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace practica
{
    internal class UserData
    {
        public static string login { get; set; }
        public static int HowVhod { get; set; }
        public static string isDell { get; set; }
        public static string Accept { get; set; }
        public static string isUpdate { get; set; }
        public static string AddOrUpdt { get; set; }

        public static string idItems { get; set; }
        public static string nameItems { get; set; }
        public static string categorItems { get; set; }
        public static string proizvodItems { get; set; }
        public static string ostatokItems { get; set; }
        public static string izmerItems { get; set; }
        public static string priceItems { get; set; }
        public static string opisanieItems { get; set; }
        public static string skidkaItems { get; set; }
    }
}
