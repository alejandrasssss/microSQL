using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_microSQL.Models
{
    public class EstructuraTabla
    {
        string nombre;
        Dictionary<string, string> columnas = new Dictionary<string, string>();

        public string Nombre { get => nombre; set => nombre = value; }
        public Dictionary<string, string> Columnas { get => columnas; set => columnas = value; }
    }
}