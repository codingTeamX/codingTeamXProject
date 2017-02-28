 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Ventas.Negocio
{
    public class Proteina:Ventas.Datos.Contexto
    {
        string tabla = "proteina";
        public string ERROR
        {
            get { return traducir(error); }
        }

        public DataTable datosProteina()
        {
            try
            {
                string sql = "select * from #tabla#";
                sql = sql.Replace("#tabla#", tabla);
                return traerDatos(sql);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }
        public string llave(OleDbTransaction tr)
        {
            try
            {
                string sql = "select max(id) from proteina";
                return traerDatos(sql, tr).Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }

        }
        public DataTable datosProteinaNombre(string nombre)
        {
            try
            {
                string sql = "select * from #tabla# c where nombreProteina like '" + nombre + "%'";
                sql = sql.Replace("#tabla#", tabla);
                return traerDatos(sql);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }

        public string[] vdatos = new string[2];

        public int insertar(OleDbTransaction tr)
        {
            try
            {
                string sql = "insert into #tabla# values('#v1#','#v2#')";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v1#", vdatos[0]);
                sql = sql.Replace("#v2#", vdatos[1]);
                return modificar(sql, tr);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return 0;
            }
        }
        public int actualizar(OleDbTransaction tr)
        {
            try
            {
                string sql = "update #tabla# set nombreProteina='#v1#' where id='#v0#'";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v0#", vdatos[0]);
                sql = sql.Replace("#v1#", vdatos[1]);
                return modificar(sql, tr);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return 0;
            }
        }
        public int eliminar(OleDbTransaction tr)
        {
            try
            {
                string sql = "delete from #tabla# where id='#v0#'";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v0#", vdatos[0]);
                return modificar(sql, tr);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return 0;
            }
        }

    }
}
