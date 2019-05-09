using Proyecto_microSQL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proyecto_microSQL.Controllers
{
    public class SQLController : Controller
    {
        #region Objetosauxiliares
        //instancia que nos permite utilizar las palabras resevadas dentro del código
        static PalabraReservada diccionario = new PalabraReservada();

        //Listas de apoyo, únicamente para las vistas parciales
        List<EstructuraTabla> ListatreeView = new List<EstructuraTabla>();
        static List<selection> selecciones = new List<selection>();

        //Lista de errores devueltos por el analizador de instrucciones
        //únicamente de apoyo para las vistas
        static List<string> errores = new List<string>();

        //objeto donde se realiza la lectura de las instrucciones agregadas por el usuario
        InstrucciónSQL Analizador = new InstrucciónSQL();

        static string textoPlano;
        #endregion

        // GET: SQL
        //Acción que maneja la carga de las palabras reservadas desde el archivo C://microSQL//microSQL.ini
        public ActionResult Diccionario()
        {
            if (diccionario.existe())
            {
                ViewBag.Mensaje = diccionario.cargarDiccionario();
            }
            else
            {
                diccionario.crearDiccionario();
                ViewBag.Mensaje = 3;
            }
            return View();
        }

        //acción que nos dirige hacia la pantalla principal del programa
        [HttpGet]
        public ActionResult EditorTexto()
        {

            CargarEstructurasDeTabla();
            ViewBag.tablas = ListatreeView;
            ViewBag.texto = textoPlano;
            ViewBag.error = errores;
            ViewBag.grids = selecciones;
            return View();
        }

        [HttpPost]
        public ActionResult EditorTexto(FormCollection collection)
        {
            textoPlano = collection["textArea1"];
            if (textoPlano != "")
            {
                selecciones.Clear();
                errores = Analizador.LeerInstrucciones(textoPlano, diccionario, ref selecciones);                
            }
            return RedirectToAction("EditorTexto");
        }



        #region métodosVarios

        //método para obtener la estructura de las tablas al iniciar el programa
        //se obtienen todos los nombres de los archivos de los directorios, se lee uno por uno
        //se carga la estructura de cada tabla para poderlo colocar en una lista a modo de TreeView

        public void CargarEstructurasDeTabla()
        {
            //obtener los nombres de todos los arcivos dentro del directorio
            List<string> nombres = new List<string>();
            string[] ubicacion = Directory.GetFiles("C://microSQL//tablas");

            for (int i = 0; i < ubicacion.Length; i++)
            {
                nombres.Add(Path.GetFileName(ubicacion[i]));
            }

            //leer archivo por archivo
            //guardar el nombre de cada archivo y su contenido en un modelo estructura tabla
            //almacenarlo en una lista que servirá para mostrarlo en la vista parcial _Tablas
            foreach (var nombre in nombres)
            {
                EstructuraTabla nueva = new EstructuraTabla();
                nueva.Nombre = nombre.Split('.')[0];
                StreamReader sr = new StreamReader("C://microSQL//tablas//" + nombre);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    nueva.Columnas.Add(line.Split(',')[0], line.Split(',')[1]);
                }
                ListatreeView.Add(nueva);
                sr.Close();
            }
            #endregion
        }
    }
}