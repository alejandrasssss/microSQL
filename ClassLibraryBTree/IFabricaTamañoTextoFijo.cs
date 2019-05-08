﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryBTree
{
    public interface IFabricaTextoTamañoFijo<T> where T : ITextoTamañoFijo
    {
        T Fabricar(string textoTamañoFijo);
        T FabricarNulo();
    }
}
