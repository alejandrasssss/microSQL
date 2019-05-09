using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_microSQL.Models
{
    public class selection
    {
        string nombreTabla;
        List<string> columnas;
        List<Fila> filas;

        public selection(string nombreTabla, List<string> columnas, List<Fila> filas)
        {
            this.nombreTabla = nombreTabla;
            this.columnas = columnas;
            this.filas = filas;
        }

        public string NombreTabla { get => nombreTabla; set => nombreTabla = value; }
        public List<string> Columnas { get => columnas; set => columnas = value; }
        public List<Fila> Filas { get => filas; set => filas = value; }
    }
}