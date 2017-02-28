using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Datos
{
     public class claseTraductor
     {
        public string traducir(string er)
        {
            try
            {
                if (er.Contains("No existe el servidor"))
                    return "Imposible conectar con el servidor.";
                if (er.Contains("No se puede abrir la base de datos"))
                    return "Fuera de servicio.";
                return "Ha ocurrido un error!.";
            }
            catch (Exception e)
            {
                return "ERROR";
            }
        }
    }
}
