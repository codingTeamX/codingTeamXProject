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
    public partial class FormLote : Form
    {
        public FormLote()
        {
            InitializeComponent();
        }
        LoteEntrega oblote = new LoteEntrega();
        Producto obproducto = new Producto();

        DetalleLoteEntrega obdetalle = new DetalleLoteEntrega();

        DataTable dtlote = new DataTable();
        DataTable dt = new DataTable();

        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (insetarDatos())
            {
                txt_buscar.Text = t1.Text;
                if (cargarBusqueda())
                    inicio();
                lb0.Items.Clear();
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
            dtlote = oblote.datosFechaLoteEntrega(txt_buscar.Text);
            if (dtlote != null)
                dg1.DataSource = oblote.datosFechaLoteEntrega(txt_buscar.Text);
            else
            {
                MessageBox.Show(oblote.ERROR);
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
                llave = dtlote.Rows[pos][0].ToString();
                t1.Text = dtlote.Rows[pos][1].ToString();
                t2.Text = dtlote.Rows[pos][2].ToString();
                c1.SelectedValue = dtlote.Rows[pos][3].ToString();
                c2.SelectedValue = dtlote.Rows[pos][4].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtlote.Rows.Count - 1;
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

        bool insertarLote(OleDbTransaction tr)
        {
            if (oblote.llave(tr) == "")
                oblote.vdatos[0] = "1";
            else
                oblote.vdatos[0] = (int.Parse(oblote.llave(tr)) + 1).ToString();
            oblote.vdatos[1] = t1.Text;
            oblote.vdatos[2] = t2.Text;
            oblote.vdatos[3] = c1.SelectedValue.ToString();
            oblote.vdatos[4] = c2.SelectedValue.ToString();
            if (oblote.insertar(tr) == 0)
            {
                MessageBox.Show(oblote.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarLote(tr))
            {
                if (insertarDetalleLote(tr))
                {
                    ctr.finTR(tr);
                    return true;
                }
            }
            return false;
        }

        bool actualizarLote(OleDbTransaction tr)
        {
            oblote.vdatos[0] = llave;
            oblote.vdatos[1] = t1.Text;
            oblote.vdatos[2] = t2.Text;
            oblote.vdatos[3] = c1.SelectedValue.ToString();
            oblote.vdatos[4] = c2.SelectedValue.ToString();
            if (oblote.actualizar(tr) == 0)
            {
                MessageBox.Show(oblote.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarLote(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarLote(OleDbTransaction tr)
        {
            oblote.vdatos[0] = llave;
            if (oblote.eliminar(tr) == 0)
            {
                MessageBox.Show(oblote.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarLote(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtlote.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtlote.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        DataTable dtempleado = new DataTable();
        Persona obempleado = new Persona();

        DataTable dtproveedor = new DataTable();
        Proveedor obproveedor = new Proveedor();


        void cargarEmpleado()
        {
            dtempleado = obempleado.datosPersonaEmpleado();
            if (dtempleado != null)
            {
                c1.DataSource = obempleado.datosPersonaEmpleado();
                c1.ValueMember = "id";
                c1.DisplayMember = "nombre";
            }
        }

        void cargarProveedor()
        {
            dtproveedor = obproveedor.datosProveedor();
            if (dtproveedor != null)
            {
                c2.DataSource = obproveedor.datosProveedor();
                c2.ValueMember = "id";
                c2.DisplayMember = "nombreProveedor";
            }
        }


        bool cargarProducto()
        {
            dt = obproducto.datosProductoX();
            if (dt != null)
            {
                dt.Columns.Add("Precio Entrega");
                dt.Columns.Add("Precio Venta");
                dt.Columns.Add("Cantidad Entrega");
                dg1.DataSource = dt;
            }
            else
            {
                MessageBox.Show(obproducto.ERROR);
                return false;
            }
            return true;
        }

        bool existeDuplicadoProducto(int num)
        {
            for (int i = 0; i <= lb1.Items.Count - 1; i++)
            {
                if (int.Parse(lb0.Items[i].ToString()) == num)
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
                    if (!existeDuplicadoProducto(int.Parse(dg1.CurrentRow.Cells["id"].Value.ToString())))
                    {
                        lb0.Items.Add(dg1.CurrentRow.Cells["id"].Value);
                        lb1.Items.Add(dg1.CurrentRow.Cells["Precio Entrega"].Value);
                        lb2.Items.Add(dg1.CurrentRow.Cells["Precio Venta"].Value);
                        lb3.Items.Add(dg1.CurrentRow.Cells["Cantidad Entrega"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool insertarDetalleLote(OleDbTransaction tr)
        {
            string llave = oblote.llave(tr);
            for (int i = 0; i <= lb1.Items.Count - 1; i++)
            {
                obdetalle.vdatos[0] = llave;
                obdetalle.vdatos[1] = lb0.Items[i].ToString();
                obdetalle.vdatos[2] = lb1.Items[i].ToString();
                obdetalle.vdatos[3] = lb2.Items[i].ToString();
                obdetalle.vdatos[4] = lb3.Items[i].ToString();
                if (obdetalle.insertar(tr) == 0)
                {
                    MessageBox.Show(obdetalle.ERROR);
                    ctr.desTR(tr);
                    return false;
                }
            }
            return true;
        }

        private void FormProducto_Load(object sender, EventArgs e)
        {
            cargarEmpleado();
            cargarProveedor();
        }

        private void btn_vitaminas_Click(object sender, EventArgs e)
        {
            cargarProducto();
        }


    }
}
