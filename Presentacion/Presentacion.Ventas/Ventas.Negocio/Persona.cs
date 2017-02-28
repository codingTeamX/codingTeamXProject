 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Ventas.Negocio
{
    public class Persona:Ventas.Datos.Contexto
    {
        string tabla = "persona";
        public string ERROR
        {
            get { return traducir(error); }
        }
        
    }
}
