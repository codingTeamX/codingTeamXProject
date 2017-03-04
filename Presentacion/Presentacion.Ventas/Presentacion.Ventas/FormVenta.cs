using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ventas.Negocio;
using System.Data.OleDb;
using Ventas.ControlTransaccion;


namespace Presentacion.Ventas
{
    public partial class FormVenta : Form
    {
        public FormVenta()
        {
            InitializeComponent();
        }

        Venta obventa = new Venta();

        DetalleLoteEntrega obdetallelote = new DetalleLoteEntrega();
        DetalleVenta obdetalleventa = new DetalleVenta();

        DataTable dtventa = new DataTable();
        DataTable dt = new DataTable();

        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (insetarDatos())
            {
                txt_buscar.Text = t1.Value.ToShortDateString();
                if (cargarBusqueda())
                    inicio();
                lb1.Items.Clear();
                lb2.Items.Clear();
                lb3.Items.Clear();
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            if (cargarBusqueda())
                inicio();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (actualizarDatos())
            {
                txt_buscar.Text = t1.Text;
                if (cargarBusqueda())
                    inicio();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (eliminarDatos())
            {
                txt_buscar.Text = "";
                if (cargarBusqueda())
                    inicio();
            }
        }

        private void btn_inicio_Click(object sender, EventArgs e)
        {
            if (hayDatos())
                inicio();
        }

        private void btn_atras_Click(object sender, EventArgs e)
        {
            if (hayDatos() && puedeAtras())
                atras();
        }

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            if (hayDatos() && puedeSiguiente())
                siguiente();
        }

        private void btn_fin_Click(object sender, EventArgs e)
        {
            if (hayDatos())
                fin();
        }

        bool cargarBusqueda()
        {
            dtventa = obventa.datosVentaFecha(txt_buscar.Text);
            if (dtventa != null)
                dg1.DataSource = obventa.datosVentaFecha(txt_buscar.Text);
            else
            {
                MessageBox.Show(obventa.ERROR);
                return false;
            }
            return true;
        }

        int pos = -1;
        string llave = "";

        void cargarText()
        {
            if (hayDatos())
            {
                llave = dtventa.Rows[pos][0].ToString();
                t1.Value = Convert.ToDateTime(dtventa.Rows[pos][1].ToString());
                t2.Text = dtventa.Rows[pos][2].ToString();
                c1.SelectedValue = dtventa.Rows[pos][3].ToString();
                c2.SelectedValue = dtventa.Rows[pos][4].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtventa.Rows.Count - 1;
            cargarText();
        }

        void atras()
        {
            pos--;
            cargarText();
        }

        void siguiente()
        {
            pos++;
            cargarText();
        }

        bool insertarVenta(OleDbTransaction tr)
        {
            if (obventa.llave(tr) == "")
                obventa.vdatos[0] = "1";
            else
                obventa.vdatos[0] = (int.Parse(obventa.llave(tr)) + 1).ToString();
            obventa.vdatos[1] = t1.Value.ToShortDateString();
            obventa.vdatos[2] = t2.Text;
            obventa.vdatos[3] = c1.SelectedValue.ToString();
            obventa.vdatos[4] = c2.SelectedValue.ToString();
            if (obventa.insertar(tr) == 0)
            {
                MessageBox.Show(obventa.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }


        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarVenta(tr))
            {
                if (insertarDetalleVenta(tr))
                {
                    ctr.finTR(tr);
                    return true;
                }
            }
            return false;
        }

        bool insertarDetalleVenta(OleDbTransaction tr)
        {
            string llave = obventa.llave(tr);
            for (int i = 0; i <= lb1.Items.Count - 1; i++)
            {
                obdetalleventa.vdatos[1] = llave;
                obdetalleventa.vdatos[2] = lb1.Items[i].ToString();
                obdetalleventa.vdatos[3] = lb2.Items[i].ToString();
                obdetalleventa.vdatos[4] = lb3.Items[i].ToString();
                if (obdetalleventa.insertar(tr) == 0)
                {
                    MessageBox.Show(obdetalleventa.ERROR);
                    ctr.desTR(tr);
                    return false;
                }
            }
            return true;
        }

        bool actualizarVenta(OleDbTransaction tr)
        {
            obventa.vdatos[0] = llave;
            obventa.vdatos[1] = t1.Text;
            obventa.vdatos[2] = t2.Text;
            obventa.vdatos[3] = c1.SelectedValue.ToString();
            obventa.vdatos[4] = c2.SelectedValue.ToString();
            if (obventa.actualizar(tr) == 0)
            {
                MessageBox.Show(obventa.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarVenta(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarVenta(OleDbTransaction tr)
        {
            obventa.vdatos[0] = llave;
            if (obventa.eliminar(tr) == 0)
            {
                MessageBox.Show(obventa.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarVenta(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtventa.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtventa.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        DataTable dtpersona = new DataTable();
        Persona obpersona = new Persona();

        DataTable dtempleado = new DataTable();

        void cargarPersona()
        {
            dtpersona = obpersona.datosPersona();
            if (dtpersona != null)
            {
                c1.DataSource = obpersona.datosPersona();
                c1.ValueMember = "id";
                c1.DisplayMember = "nombre";
            }
        }

        void cargarEmpleado()
        {
            dtempleado = obpersona.datosPersonaEmpleado();
            if (dtempleado != null)
            {
                c2.DataSource = obpersona.datosPersonaEmpleado();
                c2.ValueMember = "id";
                c2.DisplayMember = "nombre";
            }
        }

        bool cargarProductos()
        {
            dt = obdetallelote.datosDetalleLoteEntrega();
            if (dt != null)
            {
                dt.Columns.Add("Cantidad");
                dg1.DataSource = dt;
            }
            else
            {
                MessageBox.Show(obdetallelote.ERROR);
                return false;
            }
            return true;
        }

        private void FormCliente_Load(object sender, EventArgs e)
        {
            cargarPersona();
            cargarEmpleado();
        }

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            if (cargarProductos()) { }
        }

        //bool cargarNuevo()
        //{
        //    dt = obcontrato.datosContrato();
        //    if (dt != null)
        //    {
        //        dt.Columns.Add("Fecha Inicio");
        //        dt.Columns.Add("Fecha Fin");
        //        dt.Columns.Add("Sueldo");
        //        dt.Columns.Add("Cargo");
        //        dg1.DataSource = dt;
        //    }
        //    else
        //    {
        //        MessageBox.Show(obempleado.ERROR);
        //        return false;
        //    }
        //    return true;
        //}

        int cont = 0;

        bool existeProductoLoteDuplicado(int producto, int lote)
        {
            for (int i = 0; i < cont; i++)
            {
                if (int.Parse(lb1.Items[i].ToString()) == producto && int.Parse(lb2.Items[i].ToString()) == lote)
                    return true;
            }
            return false;
        }

        private void dg1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    if (!existeProductoLoteDuplicado(int.Parse(dg1.CurrentRow.Cells["idProducto"].Value.ToString()), int.Parse(dg1.CurrentRow.Cells["idLoteEntrega"].Value.ToString())))
                    {
                        lb1.Items.Add(dg1.CurrentRow.Cells["idProducto"].Value);
                        lb2.Items.Add(dg1.CurrentRow.Cells["idLoteEntrega"].Value);
                        lb3.Items.Add(dg1.CurrentRow.Cells["Cantidad"].Value);
                        cont++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //bool insertarEmpleadoContrato(OleDbTransaction tr)
        //{
        //    string llave = obempleado.llave(tr);
        //    for (int i = 0; i <= lb1.Items.Count - 1; i++)
        //    {
        //        obdetalle.vdatos[0] = llave;
        //        obdetalle.vdatos[1] = lb1.Items[i].ToString();
        //        obdetalle.vdatos[2] = lb2.Items[i].ToString();
        //        obdetalle.vdatos[3] = lb3.Items[i].ToString();
        //        obdetalle.vdatos[4] = lb4.Items[i].ToString();
        //        obdetalle.vdatos[5] = lb5.Items[i].ToString();
        //        if (obdetalle.insertar(tr) == 0)
        //        {
        //            MessageBox.Show(obdetalle.ERROR);
        //            ctr.desTR(tr);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

    }
}
