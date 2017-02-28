using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Ventas.Datos
{
    public class Contexto:claseTraductor
    {
        private string servidor = "(local)";
        private string bd = "prueba";

        protected string error = "";
        public string[] vdatosx;
        public string[] vcx;
        public string opx;

        private OleDbConnection conectar()
        {
            try
            {
                string conectar = "Provider=SQLOLEDB.1;Data Source=" + servidor + ";Initial Catalog=" + bd + ";Integrated Security=SSPI";
                OleDbConnection conex = new OleDbConnection(conectar);
                conex.Open();
                return conex;
            }
            catch (Exception ex)
            {
                traducir(ex.Message);
                return null;
            }
        }

        protected OleDbTransaction iniciarTR()
        {
            try
            {
                OleDbConnection conex = conectar();
                return conex.BeginTransaction();
            }
            catch (Exception ex)
            {
                traducir(ex.Message);
                return null;
            }
        }
        protected DataTable traerDatos(string csql)
        {
            try
            {
                OleDbTransaction tr = iniciarTR();
                return traerDatos(csql, tr);
                tr.Commit();
            }
            catch (Exception ex)
            {
                traducir(ex.Message);
                return null;
            }
        }
        protected DataTable traerDatos(string csql, OleDbTransaction tr)
        {
            try
            {
                OleDbDataAdapter ada = new OleDbDataAdapter(csql, tr.Connection);
                if (vdatosx != null)
                    ada_pa(ada);
                ada.SelectCommand.Transaction = tr;
                DataTable dt = new DataTable();
                ada.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                traducir(ex.Message);
                return null;
            }
        }
        OleDbDataAdapter ada_pa(OleDbDataAdapter ada)
        {
            try
            {
                ada.SelectCommand.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i <= vdatosx.Length - 1; i++)
                    if (vdatosx[i] != null)
                    {
                        OleDbParameter para = new OleDbParameter("@" + vcx[i], vdatosx[i]);
                        ada.SelectCommand.Parameters.Add(para);
                    }
                return ada;
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }


        OleDbCommand comando_pa(OleDbCommand ada)
        {
            try
            {
                OleDbParameter para;
                ada.CommandType = CommandType.StoredProcedure;
                if (opx != null)
                {
                    para = new OleDbParameter("@op", opx);
                    ada.Parameters.Add(para);
                }
                for (int i = 0; i <= vdatosx.Length - 1; i++)
                {
                    if (vdatosx[i] != null)
                    {
                        para = new OleDbParameter("@" + vcx[i], vdatosx[i]);
                        ada.Parameters.Add(para);
                    }
                }
                return ada;
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }

        protected int modificar(string sql)
        {
            try
            {
                OleDbTransaction tr = iniciarTR();
                int i = modificar(sql, tr);
                tr.Commit();
                return i;
            }
            catch (Exception ex)
            {
                traducir(ex.Message);
                return 0;
            }
        }

        protected int modificar(string sql, OleDbTransaction tr)
        {
            try
            {
                OleDbCommand comando = new OleDbCommand(sql, tr.Connection);
                if (vdatosx != null)
                    comando_pa(comando);
                comando.Transaction = tr;
                vdatosx = null;
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                traducir(ex.Message);
                return 0;
            }
        }

    }
}
