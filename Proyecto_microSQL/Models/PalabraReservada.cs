using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace Proyecto_microSQL.Models
{
    public class PalabraReservada
    {
        string select;
        string from;
        string delete;
        string where;
        string create;
        string drop;
        string table;
        string insert;
        string into;
        string values;
        string go;

        public string Select { get => select; set => select = value; }
        public string From { get => from; set => from = value; }
        public string Delete { get => delete; set => delete = value; }
        public string Where { get => where; set => where = value; }
        public string Create { get => create; set => create = value; }
        public string Drop { get => drop; set => drop = value; }
        public string Insert { get => insert; set => insert = value; }
        public string Values { get => values; set => values = value; }
        public string Go { get => go; set => go = value; }
        public string Table { get => table; set => table = value; }
        public string Into { get => into; set => into = value; }


        #region métodos

        //verifica si existe el archivo con las palabras reservadas
        public bool existe() => File.Exists("C:\\microSQL\\microSQL.ini");

        //crear el archivo por defecto
        public void crearDiccionario()
        {
            //palabras reservadas por defecto
            select = "SELECT";
            from = "FROM";
            delete = "DELETE";
            where = "WHERE";
            create = "CREATE";
            drop = "DROP";
            table = "TABLE";
            insert = "INSERT";
            into = "INTO";
            values = "VALUES";
            go = "GO";

            //crear nuevo archivo
            string path = "C:\\microSQL\\microSQL.ini";
            FileStream fs = File.Create(path);

            //escribir en el nuevo archivo
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("SELECT,"+select);
            sw.WriteLine("FROM," + from);
            sw.WriteLine("DELETE," + delete);
            sw.WriteLine("WHERE," + where);
            sw.WriteLine("CREATE," + create);
            sw.WriteLine("DROP," + drop);
            sw.WriteLine("TABLE," + table);
            sw.WriteLine("INSERT," + insert);
            sw.WriteLine("INTO," + into);
            sw.WriteLine("VALUES," + values);
            sw.WriteLine("GO," + go);
            sw.Flush();
            sw.Close();
        }

        //carga el archivo existente al modelo de palabras reservadas
        //de este modo se pueden usar los tributos de la clase sin importar los cambios en las palabras reservadas
        public int cargarDiccionario()
        {
            //leer archivo
            StreamReader sr = new StreamReader("C:\\microSQL\\microSQL.ini");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] separado = line.Split(',');
                switch(separado[0])
                {
                    case "SELECT":
                        select = separado[1];
                        break;
                    case "FROM":
                        from = separado[1];
                        break;
                    case "DELETE":
                        delete = separado[1];
                        break;
                    case "WHERE":
                        where = separado[1];
                        break;
                    case "CREATE":
                        create = separado[1];
                        break;
                    case "DROP":
                        drop = separado[1];
                        break;
                    case "TABLE":
                        table = separado[1];
                        break;                    
                    case "INSERT":
                        insert = separado[1];
                        break;
                    case "INTO":
                        into = separado[1];
                        break;
                    case "VALUES":
                        values = separado[1];
                        break;
                    case "GO":
                        go = separado[1];
                        break;

                        //si se llega a leer en el archivo una palabra no definida por el modelo
                    default:
                        return 2;
                }
            }
            sr.Close();


            //se verifica que el archivo contenía todas las palabras definidas por el modelo
            if (select != null && from != null && delete != null && where != null && create != null && drop != null && insert != null
                && values != null && go != null && table != null && into != null)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        #endregion
    }
}