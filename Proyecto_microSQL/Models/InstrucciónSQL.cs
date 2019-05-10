using System;
using System.Collections.Generic;
using System.IO;
using ClassLibraryBTree;

namespace Proyecto_microSQL.Models
{
    public class InstrucciónSQL
    {
        public List<string> LeerInstrucciones(string TextoPlano, PalabraReservada diccionario, ref List<selection> selecciones)
        {
            List<string> listaErrores = new List<string>();

            TextoPlano = TextoPlano.Replace("\r","");
            TextoPlano = TextoPlano.Replace("\n"," ");
            TextoPlano = TextoPlano.Replace(diccionario.Go, " " + diccionario.Go + " ");

            //Pueden venir en el texto escrito, varias instrucciones las cuales son separadas por GO
            //GO no se debe colocar en la última instrucción o si viene sólo una
            string[] instrucciones = TextoPlano.Split(new string[] { " " + diccionario.Go + " " }, StringSplitOptions.RemoveEmptyEntries);

            //recorremos cada una de las instrucciones que separamos previamente.
            foreach (var item in instrucciones)
            {
                //como no es necesario que "(" tenga espacios antes o después, se garantiza su separación
                //mismo caso con las comas
                string instruccion = item.Replace("(100)", "#");
                instruccion = instruccion.Replace("(", " ( ");
                instruccion = instruccion.Replace(")", " ) ");
                instruccion = instruccion.Replace(","," , ");
                instruccion = instruccion.Replace("\'"," \' ");
                instruccion = instruccion.Replace("*", " * ");
                instruccion = instruccion.Replace("=", " = ");

                //para facilitar, separamos cada una de las instrucciones por cada una de sus expresiones
                string[] partesDeInstruccion = instruccion.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                //INSTRUCCIONES PRINCIPALES, DENTRO DE CADA IF SE VALIDAN SUS VARIANTES
                #region instrucciones

                #region CREATE
                //CREATE TABLE
                if (partesDeInstruccion[0] == diccionario.Create)
                {
                    //validación
                    //después de create tiene que venir un table
                    if (partesDeInstruccion[1] == diccionario.Table)
                    {
                        int contadorINT = 0;
                        int contadorVARCHAR = 0;
                        int contadorDATETIME = 0;
                        string NombreTabla = partesDeInstruccion[2];
                        Dictionary<string, string> columnas = new Dictionary<string, string>();

                        //validación
                        //si la tabla ya existe y se intenta volver a crear, devolverá un error
                        if (!File.Exists("C://microSQL//tablas//"+NombreTabla+".tabla"))
                        {
                            if (NombreTabla != diccionario.Create && NombreTabla.Length > 1
                                && NombreTabla != diccionario.Delete && NombreTabla != diccionario.Drop
                                && NombreTabla != diccionario.From && NombreTabla != diccionario.Insert
                                && NombreTabla != diccionario.Into && NombreTabla != diccionario.Select
                                && NombreTabla != diccionario.Table && NombreTabla != diccionario.Values
                                && NombreTabla != diccionario.Where && NombreTabla != "INT"
                                && NombreTabla != "DATETIME" //varchar(100) no, porque se elimina con la condición de #
                                && !NombreTabla.Contains(".") && !NombreTabla.Contains(";")
                                && !NombreTabla.Contains("=") && !NombreTabla.Contains(":")
                                && !NombreTabla.Contains("{") && !NombreTabla.Contains("}")
                                && !NombreTabla.Contains("¿") && !NombreTabla.Contains("?")
                                && !NombreTabla.Contains("¡") && !NombreTabla.Contains("!")
                                && !NombreTabla.Contains("\\") && !NombreTabla.Contains("/")
                                && !NombreTabla.Contains("&") && !NombreTabla.Contains("$")
                                && !NombreTabla.Contains("\"") && !NombreTabla.Contains("#")
                                && !NombreTabla.Contains("[") && !NombreTabla.Contains("]")
                                && !NombreTabla.Contains("+") && !NombreTabla.Contains("*")
                                )
                            {
                                //si no existe y es nombre válido continúa la validación de paréntesis
                                //junto con la validación de ID INT PRIMARY KEY,
                                try
                                {
                                    if (partesDeInstruccion[3] == "(" && partesDeInstruccion[4] == "ID" && partesDeInstruccion[5] == "INT"
                                    && partesDeInstruccion[6] == "PRIMARY" && partesDeInstruccion[7] == "KEY")
                                    {
                                        if (partesDeInstruccion[8] == "," && partesDeInstruccion[9] != ")" && partesDeInstruccion[10] != ")")
                                        {
                                            int pos = 6;
                                            bool Correcto = true;

                                            //validación
                                            //saber si se cerró el paréntesis (el cuál debería ser el último elemento de la instrucción)
                                            if (partesDeInstruccion[partesDeInstruccion.Length - 1] == ")")
                                            {
                                                do
                                                {
                                                    pos = pos + 3;
                                                    string nombCol = partesDeInstruccion[pos];
                                                    if (partesDeInstruccion[pos + 2] == ")" || partesDeInstruccion[pos + 2] == ",")
                                                    {
                                                        //validación extrema de nombres de tabla
                                                        //no pueden ser palabras reservadas como tal, ni una sola letra, ni contener caracteres especiales 
                                                        if (partesDeInstruccion[pos] != diccionario.Create && partesDeInstruccion[pos].Length > 1
                                                            && partesDeInstruccion[pos] != diccionario.Delete && partesDeInstruccion[pos] != diccionario.Drop
                                                            && partesDeInstruccion[pos] != diccionario.From && partesDeInstruccion[pos] != diccionario.Insert
                                                            && partesDeInstruccion[pos] != diccionario.Into && partesDeInstruccion[pos] != diccionario.Select
                                                            && partesDeInstruccion[pos] != diccionario.Table && partesDeInstruccion[pos] != diccionario.Values
                                                            && partesDeInstruccion[pos] != diccionario.Where && partesDeInstruccion[pos] != "INT"
                                                            && partesDeInstruccion[pos] != "DATETIME"                                                           //varchar(100) no, porque se elimina con la condición de #
                                                            && !partesDeInstruccion[pos].Contains(".") && !partesDeInstruccion[pos].Contains(";")
                                                            && !partesDeInstruccion[pos].Contains("=") && !partesDeInstruccion[pos].Contains(":")
                                                            && !partesDeInstruccion[pos].Contains("{") && !partesDeInstruccion[pos].Contains("}")
                                                            && !partesDeInstruccion[pos].Contains("¿") && !partesDeInstruccion[pos].Contains("?")
                                                            && !partesDeInstruccion[pos].Contains("¡") && !partesDeInstruccion[pos].Contains("!")
                                                            && !partesDeInstruccion[pos].Contains("\\") && !partesDeInstruccion[pos].Contains("/")
                                                            && !partesDeInstruccion[pos].Contains("&") && !partesDeInstruccion[pos].Contains("$")
                                                            && !partesDeInstruccion[pos].Contains("\"") && !partesDeInstruccion[pos].Contains("#")
                                                            && !partesDeInstruccion[pos].Contains("[") && !partesDeInstruccion[pos].Contains("]")
                                                            && !partesDeInstruccion[pos].Contains("+") && !partesDeInstruccion[pos].Contains("*")
                                                            )
                                                        {
                                                            if (partesDeInstruccion[pos + 1] == "INT")
                                                            {
                                                                columnas.Add(partesDeInstruccion[pos], "INT");
                                                                contadorINT++;
                                                            }
                                                            else if (partesDeInstruccion[pos + 1] == "VARCHAR#")
                                                            {
                                                                columnas.Add(partesDeInstruccion[pos], "VARCHAR(100)");
                                                                contadorVARCHAR++;
                                                            }
                                                            else if (partesDeInstruccion[pos + 1] == "DATETIME")
                                                            {
                                                                columnas.Add(partesDeInstruccion[pos], "DATETIME");
                                                                contadorDATETIME++;
                                                            }
                                                            else { listaErrores.Add("CREATE ERROR: tipo de dato para columna no válido"); Correcto = false; }
                                                        }
                                                        else { listaErrores.Add("CREATE ERROR: las columnas no pueden llamarse como palabras reservadas o contener ciertos caracteres"); Correcto = false; }
                                                    }
                                                    else { listaErrores.Add("CREATE ERROR: falta <,> o se definió mal una columna"); Correcto = false; }
                                                }
                                                while (partesDeInstruccion[pos + 2] != ")");

                                                //se debe comprobar si pasó todas las validaciones dentro del while
                                                if (Correcto)
                                                {
                                                    if (contadorDATETIME <= 3 && contadorINT <= 3 && contadorVARCHAR <= 3)
                                                    {
                                                        //crear archivo .tabla a partir del diccionario
                                                        string path = "C://microSQL//tablas//" + partesDeInstruccion[2] + ".tabla";
                                                        FileStream fs = File.Create(path);
                                                        StreamWriter sw = new StreamWriter(fs);
                                                        sw.WriteLine("ID,INT PRIMARY KEY");
                                                        foreach (var columna in columnas)
                                                        {
                                                            sw.WriteLine(columna.Key + "," + columna.Value);
                                                        }
                                                        sw.Flush();
                                                        sw.Close();

                                                        //crear archivo .arbolb
                                                       ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//"+NombreTabla+".arbolb", new FabricaFila());
                                                       arbol.Cerrar();
                                                       listaErrores.Add("CREATE SUCCESSFUL");
                                                    }
                                                    else { listaErrores.Add("CREATE ERROR: no se pueden crear más de 3 columnas con el mismo tipo de dato"); }
                                                }
                                            }
                                            else { listaErrores.Add("CREATE ERROR: nunca se cerró la instrucción <)> o se empezó otra sin separarla con <" + diccionario.Go + ">"); }
                                        }
                                        else { listaErrores.Add("CREATE ERROR: falta <;> o no se definió otra columna diferente a ID"); }
                                    }
                                    else { listaErrores.Add("CREATE ERROR: es obligatorio inculir la columna ID del tipo INT PRIMARY KEY"); }
                                }
                                catch (Exception) { listaErrores.Add("CREATE ERROR: instrucción incompleta"); }
                            }
                            else { listaErrores.Add("CREATE ERROR: Nombre de tabla no válido"); }
                        }
                        else{ listaErrores.Add("CREATE ERROR: ya existe una tabla con ese nombre"); }
                    }
                    else{ listaErrores.Add("CREATE ERROR: falta <" +diccionario.Table+ ">"); }
                }
                #endregion

                #region INSERT
                //INSERT INTO
                else if (partesDeInstruccion[0] == diccionario.Insert)
                {
                    try
                    {
                        if (partesDeInstruccion[1] == diccionario.Into)
                        {
                            string nombreTabla = partesDeInstruccion[2];
                            if (File.Exists("C://microSQL//tablas//"+nombreTabla+".tabla"))
                            {
                                if (partesDeInstruccion[3] == "(")
                                {
                                    Dictionary<string, string> columnas = new Dictionary<string, string>();
                                    StreamReader sr = new StreamReader("C://microSQL//tablas//" +nombreTabla+ ".tabla");
                                    string line;
                                    //para saber en que propiedad de Fila guardar
                                    int contadorINT = 0;
                                    int contadorDT = 0;
                                    int contadorVAR = 0;
                                    //nuevo objeto
                                    Fila nuevaFila = new Fila();

                                    while ((line = sr.ReadLine()) != null) { columnas.Add(line.Split(',')[0], line.Split(',')[1]); }
                                    sr.Close();
                                    int pos = 4;
                                    string nombresCol = "";
                                    bool correcto = true;
                                    for (int i = 0; i < columnas.Count; i++) {nombresCol += partesDeInstruccion[pos] + partesDeInstruccion[pos + 1]; pos = pos + 2; }
                                    string[] nombres = nombresCol.Split(',');
                                    int j = 0;
                                    foreach (var col in columnas)
                                    {
                                        if (j == nombres.Length - 1)
                                        {
                                            if (col.Key + ")" != nombres[j]){ correcto = false; }
                                        }
                                        else { if (col.Key != nombres[j]){ correcto = false; } }
                                        j++;
                                    }
                                    if (correcto)
                                    {
                                        if (partesDeInstruccion[pos] == diccionario.Values && partesDeInstruccion[pos + 1] == "(" && partesDeInstruccion[partesDeInstruccion.Length-1] == ")")
                                        {
                                            pos = pos+2;
                                            string campos = "";
                                            for (int i = pos; i < partesDeInstruccion.Length - 1; i++)
                                            {
                                                campos += partesDeInstruccion[i] + " ";
                                            }
                                            string[] camposSeparados = campos.Split(',');
                                            int k = 0;
                                            foreach (var column in columnas)
                                            {
                                                string valor = camposSeparados[k].Trim();
                                                switch (column.Value)
                                                {
                                                    case "INT":
                                                        if (int.TryParse(valor, out int resultINT))
                                                        {
                                                            //insertar en el nuevo objeto
                                                            switch (contadorINT)
                                                            {
                                                                case 0:
                                                                    nuevaFila.INT1 = resultINT;
                                                                    break;
                                                                case 1:
                                                                    nuevaFila.INT2 = resultINT;
                                                                    break;
                                                                case 2:
                                                                    nuevaFila.INT3 = resultINT;
                                                                    break;

                                                                default:
                                                                    break;
                                                            }
                                                            contadorINT++;
                                                        }
                                                        else
                                                        {
                                                            correcto = false;
                                                            listaErrores.Add("INSERT ERROR: valor no especificado correctamente para" + column.Key + " de tipo INT");
                                                        }
                                                        break;
                                                    case "DATETIME":
                                                        if (DateTime.TryParse(valor, out DateTime resultDATE))
                                                        {
                                                            //insertar en el nuevo objeto
                                                            switch (contadorINT)
                                                            {
                                                                case 0:
                                                                    nuevaFila.DT1 = valor;
                                                                    break;
                                                                case 1:
                                                                    nuevaFila.DT2 = valor;
                                                                    break;
                                                                case 2:
                                                                    nuevaFila.DT3 = valor;
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            contadorDT++;
                                                        }
                                                        else
                                                        {
                                                            correcto = false;
                                                            listaErrores.Add("INSERT ERROR: valor no especificado correctamente para" + column.Key + " de tipo DATETIME");
                                                        }
                                                        break;
                                                    case "VARCHAR(100)":
                                                        if (valor[0] == '\'' && valor[valor.Length - 1] == '\'')
                                                        {
                                                            string aux = valor.TrimStart('\'');
                                                            aux = aux.TrimEnd('\'');
                                                            aux = aux.Trim();
                                                            //insertar en el nuevo objeto
                                                            switch (contadorVAR)
                                                            {
                                                                case 0:
                                                                    nuevaFila.VAR1 = aux;
                                                                    break;
                                                                case 1:
                                                                    nuevaFila.VAR2 = aux;
                                                                    break;
                                                                case 2:
                                                                    nuevaFila.VAR3 = aux;
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            contadorVAR++;
                                                        }
                                                        else
                                                        {
                                                            correcto = false;
                                                            listaErrores.Add("INSERT ERROR: valor no especificado correctamente para" + column.Key + " de tipo DATETIME");
                                                        }
                                                        break;

                                                    case "INT PRIMARY KEY":
                                                        if (Int32.TryParse(valor, out int RES))
                                                        {
                                                            nuevaFila.ID = RES;
                                                        }
                                                        else
                                                        {
                                                            correcto = false;
                                                            listaErrores.Add("INSERT ERROR: valor no especificado correctamente para" + column.Key + " de tipo INT PRIMARY KEY");
                                                        }
                                                         
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                k++;
                                            }
                                            if (correcto)
                                            {
                                                //INSERTAR NUEVO OBJETO EN EL ARBOL
                                                ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + nombreTabla + ".arbolb", new FabricaFila());
                                                arbol.Agregar(nuevaFila.ID.ToString().Trim('x'), nuevaFila, "");
                                                arbol.Cerrar();
                                                listaErrores.Add("INSERT SUCCESSFUL");
                                            }
                                        }
                                        else { listaErrores.Add("INSERT ERROR: nombres de columnas no definidos o ausencia de¨<)>"); }
                                    }
                                    else { listaErrores.Add("INSERT ERROR: nombres de columnas no definidos o ausencia de¨<)>"); }
                                }
                            }
                            else { listaErrores.Add("INSERT ERROR: No existe la tabla"); }
                        }
                        else { listaErrores.Add("INSERT ERROR: falta <" + diccionario.Into + ">"); }
                    }
                    catch (Exception) { listaErrores.Add("INSERT ERROR: Instrucción incompleta"); }                   
                }
                #endregion

                #region DELETE
                //DELETE
                else if (partesDeInstruccion[0] == diccionario.Delete)
                {
                    try
                    {
                        //borrar todo los datos de la tabla
                        if (partesDeInstruccion.Length == 3)
                        {
                            if (partesDeInstruccion[1] == diccionario.From)
                            {
                                if (File.Exists("C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb"))
                                {
                                    //borrar el archivo árbol y volverlo a crear
                                    File.Delete("C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb");
                                    ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb", new FabricaFila());
                                    arbol.Cerrar();
                                    listaErrores.Add("DELETE SUCCESSFUL");
                                }
                                else { listaErrores.Add("DELETE ERROR: no existe esa tabla"); }
                            }
                            else { listaErrores.Add("DELETE ERROR: falta <" + diccionario.From + ">"); }
                        }
                        else if (partesDeInstruccion.Length == 7)
                        {
                            if (partesDeInstruccion[1] == diccionario.From)
                            {
                                if (File.Exists("C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb"))
                                {
                                    //FILTRADO POR LLAVE
                                    if (partesDeInstruccion[3] == diccionario.Where && partesDeInstruccion[4] == "ID"
                                        && partesDeInstruccion[5] == "=")
                                    {
                                        if (Int32.TryParse(partesDeInstruccion[6], out int res))
                                        {
                                            List<Fila> filas = new List<Fila>();
                                            //OBTENER TODOS LOS DATOS DESDE EL ÁRBOL
                                            ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb", new FabricaFila());
                                            foreach (var itemfila in arbol.RecorrerPreOrden())
                                            {
                                                var fabricada = new FabricaFila();
                                                var fila = fabricada.FabricarTrim(itemfila.ToString());
                                                filas.Add(fila);
                                            }
                                            arbol.Cerrar();
                                            bool Econtro = false;
                                            //insertar dato por dato e ignorar el de la llave
                                            ArbolB<Fila> arbol2 = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + partesDeInstruccion[2] + "AUX.arbolb", new FabricaFila());
                                            foreach (var cadaFila in filas)
                                            {
                                                if (cadaFila.ID != res) { arbol2.Agregar(cadaFila.ID.ToString().Trim('x'), cadaFila, ""); }
                                                else { Econtro = true; }
                                            }
                                            arbol2.Cerrar();
                                            if (Econtro)
                                            {
                                                listaErrores.Add("DELETE SUCCESSFUL");
                                                //Borrar original y Renombrar el archivo auxiliar al original
                                                File.Delete("C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb");
                                                File.Move("C://microSQL//arbolesb//" + partesDeInstruccion[2] + "AUX.arbolb", "C://microSQL//arbolesb//" + partesDeInstruccion[2] + ".arbolb");
                                            }
                                            else
                                            {
                                                listaErrores.Add("DELETE ERROR: no se encontró un registro con ese ID");
                                                File.Delete("C://microSQL//arbolesb//" + partesDeInstruccion[2] + "AUX.arbolb");
                                            }
                                        }
                                        else { listaErrores.Add("DELETE ERROR: ID no válido"); }
                                    }
                                    else { listaErrores.Add("DELETE ERROR: FILTRO DE LLAVE MAL APLICADO"); }
                                }
                                else { listaErrores.Add("DELETE ERROR: no existe esa tabla"); }
                            }
                            else { listaErrores.Add("DELETE ERROR: falta <" + diccionario.From + ">"); }
                        }
                        else { listaErrores.Add("DELETE ERROR: instrucción incompleta o incorrecta"); }
                    }
                    catch(Exception) { listaErrores.Add("DELETE ERROR: se deben separar instrucciones"); }
                }
                #endregion

                #region DROP
                //DROP TABLE
                else if (partesDeInstruccion[0] == diccionario.Drop)
                {
                    try
                    {
                        if (partesDeInstruccion.Length > 3)
                        {
                            listaErrores.Add("ERROR: se deben separar las instrucciones");
                        }
                        else
                        {
                            if (partesDeInstruccion[1] == diccionario.Table)
                            {
                                //VERIFICAR SI EL ARCHIVO EXISTE, BORRAR AMBOS ARCHIVOS 
                                if (File.Exists("C://microSQL//tablas//" + partesDeInstruccion[2] + ".tabla"))
                                {
                                    File.Delete("C://microSQL//tablas//" + partesDeInstruccion[2] + ".tabla");
                                    File.Delete("C://microSQL//tablas//" + partesDeInstruccion[2] + ".arbolb");
                                    listaErrores.Add("DROP SUCCESSFUL");
                                }
                                else { listaErrores.Add("DROP ERROR: Esta tabla no existe"); }
                            }
                            else { listaErrores.Add("DROP ERROR: falta <" + diccionario.Table + ">"); }
                        }
                    }
                    catch (Exception) { listaErrores.Add("DROP ERROR: Instrucción incompleta"); }                                        
                }
                #endregion

                #region SELECT
                //SELECT
                else if (partesDeInstruccion[0] == diccionario.Select)
                {
                    try
                    {
                        if (partesDeInstruccion[1] == "*")
                        {
                            if (partesDeInstruccion[2] == diccionario.From)
                            {
                                if (File.Exists("C://microSQL//arbolesb//" + partesDeInstruccion[3] + ".arbolb"))
                                {
                                    if (partesDeInstruccion.Length > 4 && partesDeInstruccion.Length < 9)
                                    {
                                        //SE APLICA EL FILTRO DE LLAVE
                                        if (partesDeInstruccion[4] == diccionario.Where && partesDeInstruccion[5] == "ID"
                                            && partesDeInstruccion[6] == "=")
                                        {
                                            if (Int32.TryParse(partesDeInstruccion[7], out int res))
                                            {
                                                //OBTENER LA FILA DEL ARBOL (SI EXISTE)
                                                ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + partesDeInstruccion[3] + ".arbolb", new FabricaFila());
                                                var filaobtenida = arbol.Obtener(res.ToString());
                                                arbol.Cerrar();
                                                var Fabricada = new FabricaFila();
                                                var FilaFabricada = Fabricada.FabricarTrim(filaobtenida.ToString());

                                                //devolverlo a lo que me va a servir para mostrarlo a la vista
                                                List<Fila> filas = new List<Fila>();
                                                filas.Add(FilaFabricada);
                                                List<string> col = new List<string>();
                                                col.Add("*");
                                                selecciones.Add(new selection(partesDeInstruccion[3], col, filas));
                                                //devolverlo a lo que me va a servir para mostrarlo a la vista

                                                listaErrores.Add("SELECT SUCCESSFUL");
                                            }
                                            else { listaErrores.Add("SELECT ERROR: ID no válido"); }
                                        }
                                        else { listaErrores.Add("SELECT ERROR: FILTRO DE LLAVE MAL APLICADO"); }
                                    }
                                    else
                                    {
                                        List<Fila> filas = new List<Fila>();
                                        //OBTENER TODOS LOS DATOS DESDE EL ÁRBOL
                                        ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + partesDeInstruccion[3] + ".arbolb", new FabricaFila());
                                        foreach (var itemfila in arbol.RecorrerPreOrden())
                                        {
                                            var fabricada = new FabricaFila();
                                            var fila = fabricada.FabricarTrim(itemfila.ToString());
                                            filas.Add(fila);
                                        }
                                        arbol.Cerrar();

                                        List<string> col = new List<string>();
                                        col.Add("*");
                                        selecciones.Add(new selection(partesDeInstruccion[3], col, filas));
                                        listaErrores.Add("SELECT SUCCESSFUL");
                                        //devolverlo a lo que me va a servir para mostrarlo a la vista
                                    }
                                }
                                else { listaErrores.Add("SELECT ERROR: No existe el nombre de esa tabla"); }
                            }
                            else { listaErrores.Add("SELECT ERROR: Debe colocar <" + diccionario.From + ">"); }
                        }
                        else
                        {
                            //VERIFICAR QUE SI EXISTAN LAS COLUMNAS
                            Dictionary<string, string> columnas = new Dictionary<string, string>();                            
                            int pos = 1;
                            bool contieneFrom = false;
                            bool todasExisten = true;
                            string ColumnasDescritas = "";
                            for (int i = 1; i < partesDeInstruccion.Length; i++) { if (partesDeInstruccion[i] == diccionario.From) { contieneFrom = true; }}
                            if (contieneFrom)
                            {
                                while (partesDeInstruccion[pos] != diccionario.From) { ColumnasDescritas += partesDeInstruccion[pos]; pos++; }
                                string[] nombres = ColumnasDescritas.Split(',');
                                pos++; //después del FROM
                                if (File.Exists("C://microSQL//tablas//" + partesDeInstruccion[pos] + ".tabla"))
                                {
                                    StreamReader sr = new StreamReader("C://microSQL//tablas//" + partesDeInstruccion[pos] + ".tabla");
                                    string line;
                                    while ((line = sr.ReadLine()) != null) { columnas.Add(line.Split(',')[0], line.Split(',')[1]); }
                                    sr.Close();
                                    foreach (var nombre in nombres) { if (columnas[nombre] == null) { todasExisten = false; } }
                                    if (todasExisten)
                                    {
                                        if (partesDeInstruccion.Length-1 == pos)
                                        {
                                            List<Fila> filas = new List<Fila>();
                                            //OBTENER TODOS LOS DATOS DESDE EL ÁRBOL
                                            ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + partesDeInstruccion[pos] + ".arbolb", new FabricaFila());
                                            foreach (var itemfila in arbol.RecorrerPreOrden())
                                            {
                                                var fabricada = new FabricaFila();
                                                var fila = fabricada.FabricarTrim(itemfila.ToString());
                                                filas.Add(fila);
                                            }
                                            arbol.Cerrar();


                                            List<string> col = new List<string>();
                                            foreach (var nombre in nombres)
                                            {
                                                col.Add(nombre);
                                            }
                                            selecciones.Add(new selection(partesDeInstruccion[pos], col, filas));
                                            listaErrores.Add("SELECT SUCCESSFUL");
                                        }
                                        else
                                        {
                                            string nombreTABLA = partesDeInstruccion[pos];
                                            pos++;
                                            //SE APLICA EL FILTRO DE LLAVE
                                            if (partesDeInstruccion[pos] == diccionario.Where && partesDeInstruccion[pos+1] == "ID"
                                                && partesDeInstruccion[pos+2] == "=")
                                            {
                                                if (int.TryParse(partesDeInstruccion[pos+3], out int res))
                                                {
                                                    pos = pos + 3;
                                                    if (partesDeInstruccion.Length > pos)
                                                    {
                                                        //OBTENER LA FILA CON EL ID (SI EXISTE)
                                                        ArbolB<Fila> arbol = new ArbolB<Fila>(5, "C://microSQL//arbolesb//" + nombreTABLA + ".arbolb", new FabricaFila());
                                                        var filaobtenida = arbol.Obtener(res.ToString());
                                                        arbol.Cerrar();
                                                        var Fabricada = new FabricaFila();
                                                        var FilaFabricada = Fabricada.FabricarTrim(filaobtenida.ToString());
                                                        //devolverlo a lo que me va a servir para mostrarlo a la vista
                                                        List<Fila> filas = new List<Fila>();
                                                        filas.Add(FilaFabricada);
                                                        List<string> col = new List<string>();
                                                        foreach (var nombre in nombres)
                                                        {
                                                            col.Add(nombre);
                                                        }
                                                        selecciones.Add(new selection(nombreTABLA, col, filas));
                                                        //devolverlo a lo que me va a servir para mostrarlo a la vista
                                                        listaErrores.Add("SELECT SUCCESSFUL");
                                                    }
                                                }
                                                else { listaErrores.Add("SELECT ERROR: ID no válido"); }
                                            }
                                            else { listaErrores.Add("SELECT ERROR: FILTRO DE LLAVE MAL APLICADO"); }
                                        }
                                    }
                                    else { listaErrores.Add("SELECT ERROR: algunos nombres de columnas no existen"); }
                                }
                                else { listaErrores.Add("SELECT ERROR: la tabla no existe"); }    
                            }
                            else{ listaErrores.Add("SELECT ERROR: instrucción no contiene <" + diccionario.From+ ">");}
                        }
                    }
                    catch (Exception){ listaErrores.Add("SELECT ERROR: instrucción incompleta");}
                }
                #endregion

                #region DEFAULT
                //si no cae en la categoría de instrucción realiza la siguiente acción.
                else
                { listaErrores.Add("ERROR: instrucción no reconocida"); }
                #endregion

                #endregion

            }

            return listaErrores;
        }

        
    }
}