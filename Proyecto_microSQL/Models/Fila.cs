using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ClassLibraryBTree;

namespace Proyecto_microSQL.Models
{
    public class Fila : ITextoTamañoFijo
    {
        public int ID { get; set; }

        public int INT1 { get; set; }
        public int INT2 { get; set; }
        public int INT3 { get; set; }

        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string DT3 { get; set; }

        public string VAR1 { get; set; }
        public string VAR2 { get; set; }
        public string VAR3 { get; set; }

        private const string FormatoConst = "xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx-00-xxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";

        public Fila()
        {
            ID = 0;
            INT1 = 0;
            INT2 = 0;
            INT3 = 0;
            DT1 = "";
            DT2 = "";
            DT3 = "";
            VAR1 = "";
            VAR2 = "";
            VAR3 = "";
        }

        public Fila(int id, int int1, int int2, int int3, string dt1, string dt2, string dt3, string var1, string var2, string var3)
        {
            ID = id;
            INT1 = int1;
            INT2 = int2;
            INT3 = int3;
            DT1 = dt1;
            DT2 = dt2;
            DT3 = dt3;
            VAR1 = var1;
            VAR2 = var2;
            VAR3 = var3;
        }

        public int FixedSizeText
        {
            get
            {
                return 81;
            }
        }

        public override string ToString()
        {
            return ToFixedSizeString();
        }

        public string ToFixedSizeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID.ToString().PadLeft(20, '0'));
            //sb.Append('-');
            //sb.Append(Apellido.PadLeft(20, 'x'));
            //sb.Append('-');
            //sb.Append(Edad.ToString().PadLeft(2, '0'));
            //sb.Append('-');
            //sb.Append(Username.PadLeft(15, 'x'));
            //sb.Append('-');
            //sb.Append(Password.PadLeft(20, 'x'));

            return sb.ToString();
        }
    }
}