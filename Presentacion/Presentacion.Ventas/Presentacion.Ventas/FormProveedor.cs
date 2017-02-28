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
    public partial class FormProveedor : Form
    {
        public FormProveedor()
        {
            InitializeComponent();
        }

        Proveedor obproveedor = new Proveedor();
        DataTable dtproveedor = new DataTable();
        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (insetarDatos())
            {
                txt_buscar.Text = t1.Text;
                if (cargarBusqueda())
                    inicio();
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
            dtproveedor = obproveedor.datosProveedorNombre(txt_buscar.Text);
            if (dtproveedor != null)
                dg1.DataSource = obproveedor.datosProveedorNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obproveedor.ERROR);
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
                llave = dtproveedor.Rows[pos][0].ToString();
                t1.Text = dtproveedor.Rows[pos][1].ToString();
                t2.Text = dtproveedor.Rows[pos][2].ToString();
                t3.Text = dtproveedor.Rows[pos][3].ToString();
                c1.SelectedValue = dtproveedor.Rows[pos][4].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtproveedor.Rows.Count - 1;
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

        bool insertarProveedor(OleDbTransaction tr)
        {
            if (obproveedor.llave(tr) == "")
                obproveedor.vdatos[0] = "1";
            else
                obproveedor.vdatos[0] = (int.Parse(obproveedor.llave(tr)) + 1).ToString();
            obproveedor.vdatos[1] = t1.Text;
            obproveedor.vdatos[2] = t2.Text;
            obproveedor.vdatos[3] = t3.Text;
            obproveedor.vdatos[4] = c1.SelectedValue.ToString();
            if (obproveedor.insertar(tr) == 0)
            {
                MessageBox.Show(obproveedor.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarProveedor(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarProveedor(OleDbTransaction tr)
        {
            obproveedor.vdatos[0] = llave;
            obproveedor.vdatos[1] = t1.Text;
            obproveedor.vdatos[2] = t2.Text;
            obproveedor.vdatos[3] = t3.Text;
            obproveedor.vdatos[4] = c1.SelectedValue.ToString();
            if (obproveedor.actualizar(tr) == 0)
            {
                MessageBox.Show(obproveedor.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarProveedor(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarProveedor(OleDbTransaction tr)
        {
            obproveedor.vdatos[0] = llave;
            if (obproveedor.eliminar(tr) == 0)
            {
                MessageBox.Show(obproveedor.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarProveedor(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtproveedor.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtproveedor.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        DataTable dtciudad = new DataTable();
        Ciudad obciudad = new Ciudad();

        void cargarCiudad()
        {
            dtciudad = obciudad.datosCiudad();
            if (dtciudad != null)
            {
                c1.DataSource = obciudad.datosCiudad();
                c1.ValueMember = "id";
                c1.DisplayMember = "nombreCiudad";
            }
        }

        private void FormProveedor_Load(object sender, EventArgs e)
        {
            cargarCiudad();
        }

    }
}
