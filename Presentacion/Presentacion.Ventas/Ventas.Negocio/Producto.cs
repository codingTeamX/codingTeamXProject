using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Ventas.Negocio
{
    public class Producto:Ventas.Datos.Contexto
    {
        string tabla = "producto";

        public string ERROR
        {
            get { return traducir(error); }
        }
          public DataTable datosProducto()
        {
            try
            {
                string sql = "select * from #tabla# p inner join detalleLoteEntrega dt on p.id=dt.idProducto";
                sql = sql.Replace("#tabla#", tabla);
                return traerDatos(sql);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }

        public DataTable datosProductoX()
        {
            try
            {
                string sql = "select producto.id,producto.nombreProducto from #tabla#";
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
                string sql = "select max(id) from producto";
                return traerDatos(sql, tr).Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }
        public DataTable datosProductoNombre(string nombre)
        {
            try
            {
                string sql = "select * from #tabla# where nombreProducto like '" + nombre + "%'";
                sql = sql.Replace("#tabla#", tabla);
                return traerDatos(sql);
            }
            catch (Exception e)
            {
                traducir(e.Message);
                return null;
            }
        }
        public string[] vdatos = new string[9];

        public int insertar(OleDbTransaction tr)
        {
            try
            {
                string sql = "insert into #tabla# values('#v1#','#v2#','#v3#','#v4#','#v5#','#v6#','#v7#','#v8#','#v9#')";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v1#", vdatos[0]);
                sql = sql.Replace("#v2#", vdatos[1]);
                sql = sql.Replace("#v3#", vdatos[2]);
                sql = sql.Replace("#v4#", vdatos[3]);
                sql = sql.Replace("#v5#", vdatos[4]);
                sql = sql.Replace("#v6#", vdatos[5]);
                sql = sql.Replace("#v7#", vdatos[6]);
                sql = sql.Replace("#v8#", vdatos[7]);
                sql = sql.Replace("#v9#", vdatos[8]);
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
                string sql = "update #tabla# set nombreProducto='#v1#',cantidad='#v2#',calorias='#v3#',colesterol='#v4#',sodio='#v5#',potasio='#v6#',fechaCaducidad='#v7#',idmarca='#v8#' where id='#v0#'";
                sql = sql.Replace("#tabla#", tabla);
                sql = sql.Replace("#v0#", vdatos[0]);
                sql = sql.Replace("#v1#", vdatos[1]);
                sql = sql.Replace("#v2#", vdatos[2]);
                sql = sql.Replace("#v3#", vdatos[3]);
                sql = sql.Replace("#v4#", vdatos[4]);
                sql = sql.Replace("#v5#", vdatos[5]);
                sql = sql.Replace("#v6#", vdatos[6]);
                sql = sql.Replace("#v7#", vdatos[7]);
                sql = sql.Replace("#v8#", vdatos[8]);
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
