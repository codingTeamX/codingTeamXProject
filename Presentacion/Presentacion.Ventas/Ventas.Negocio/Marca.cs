 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Ventas.Negocio
{
    public class Marca:Ventas.Datos.Contexto
    {
        string tabla = "marca";

        public string ERROR
        {
            get { return traducir(error); }
        }

        public DataTable datosMarca()
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
                string sql = "select max(id) from marca";
                return traerDatos(sql, tr).Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }

        public DataTable datosMarcaNombre(string nombre)
        {
            try
            {
                string sql = "select * from #tabla# where nombreMarca like '" + nombre + "%'";
                sql = sql.Replace("#tabla#", tabla);
                return traerDatos(sql);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }

        public string[] vdatos = new string[3];

        public int insertar(OleDbTransaction tr)
        {
            try
            {
                string sql = "insert into #tabla# values('#v1#','#v2#','#v3#')";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v1#", vdatos[0]);
                sql = sql.Replace("#v2#", vdatos[1]);
                sql = sql.Replace("#v3#", vdatos[2]);
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
                string sql = "update #tabla# set nombreMarca='#v1#',idCiudad='#v2#' where id='#v0#'";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v0#", vdatos[0]);
                sql = sql.Replace("#v1#", vdatos[1]);
                sql = sql.Replace("#v2#", vdatos[2]);
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
