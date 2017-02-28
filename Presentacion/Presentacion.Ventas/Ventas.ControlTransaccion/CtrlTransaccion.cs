using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using Ventas.Datos;

namespace Ventas.ControlTransaccion
{
     public class CtrlTransaccion:Ventas.Datos.Contexto
     {
        public OleDbTransaction iniciarTransaccion()
        {
            try
            {
                return iniciarTR();
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }

        public void finTR(OleDbTransaction tr)
        {
            try
            {
                tr.Commit();
            }
            catch (Exception e)
            {
                traducir(e.Message);
            }
        }

        public void desTR(OleDbTransaction tr)
        {
            try
            {
                tr.Rollback();
            }
            catch (Exception e)
            {
                traducir(e.Message);
            }
        }
    }
}
