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

        private const string FormatoConst = "0000000000¡0000000000¡0000000000¡0000000000¡#########################¡#########################¡#########################¡####################################################################################################¡####################################################################################################¡####################################################################################################";

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
                return 424;
            }
        }

        public override string ToString()
        {
            return ToFixedSizeString();
        }

        public string ToFixedSizeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID.ToString().PadLeft(10, '0'));
            sb.Append('^');
            sb.Append(INT1.ToString().PadLeft(10, '0'));
            sb.Append('^');
            sb.Append(INT2.ToString().PadLeft(10, '0'));
            sb.Append('^');
            sb.Append(INT3.ToString().PadLeft(10, '0'));
            sb.Append('^');
            sb.Append(DT1.PadLeft(25, '#'));
            sb.Append('^');
            sb.Append(DT2.PadLeft(25, '#'));
            sb.Append('^');
            sb.Append(DT3.PadLeft(25, '#'));
            sb.Append('^');
            sb.Append(VAR1.PadLeft(100, '#'));
            sb.Append('^');
            sb.Append(VAR2.PadLeft(100, '#'));
            sb.Append('^');
            sb.Append(VAR3.PadLeft(100, '#'));

            return sb.ToString();
        }
    }
}