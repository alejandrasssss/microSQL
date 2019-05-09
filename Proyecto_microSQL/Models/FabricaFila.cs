using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibraryBTree;

namespace Proyecto_microSQL.Models
{
    public class FabricaFila : IFabricaTextoTamañoFijo<Fila>
    {
        public Fila Fabricar(string textoTamañoFijo)
        {
            Fila fila = new Fila();
            var datos = textoTamañoFijo.Split('^');  //cambiar caracter que los separa          
            fila.ID = Convert.ToInt32(datos[0].Trim());
            fila.INT1 = Convert.ToInt32(datos[1].Trim());
            fila.INT2 = Convert.ToInt32(datos[2].Trim());
            fila.INT3 = Convert.ToInt32(datos[3].Trim());
            fila.DT1 = datos[4].Trim();
            fila.DT2 = datos[5].Trim();
            fila.DT3 = datos[6].Trim();
            fila.VAR1 = datos[7].Trim();
            fila.VAR2 = datos[8].Trim();
            fila.VAR3 = datos[9].Trim();

            return fila;
        }

        public Fila FabricarTrim(string textoTamañoFijo)
        {
            Fila fila = new Fila();
            var datos = textoTamañoFijo.Split('^');  //cambiar caracter que los separa          
            fila.ID = Convert.ToInt32(datos[0].Trim());
            fila.INT1 = Convert.ToInt32(datos[1].Trim());
            fila.INT2 = Convert.ToInt32(datos[2].Trim());
            fila.INT3 = Convert.ToInt32(datos[3].Trim());
            fila.DT1 = datos[4].Trim('#');
            fila.DT2 = datos[5].Trim('#');
            fila.DT3 = datos[6].Trim('#');
            fila.VAR1 = datos[7].Trim('#');
            fila.VAR2 = datos[8].Trim('#');
            fila.VAR3 = datos[9].Trim('#');

            return fila;
        }

        public Fila FabricarNulo()
        {
            return new Fila();
        }
    }
}