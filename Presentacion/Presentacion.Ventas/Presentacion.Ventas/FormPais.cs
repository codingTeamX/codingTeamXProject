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
    public partial class FormPais : Form
    {
        public FormPais()
        {
            InitializeComponent();
        }

        Pais obpais = new Pais();
        DataTable dtpais = new DataTable();
        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            btnGuardar.Enabled = false;
            if (!string.IsNullOrEmpty(t1.Text))
            {
                if (insetarDatos())
                {
                    txt_buscar.Text = t1.Text;
                    if (cargarBusqueda())
                        inicio();
                }
            }
            btnGuardar.Enabled = true;
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            if (cargarBusqueda())
                inicio();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            btnActualizar.Enabled = false;
            if (!string.IsNullOrEmpty(t1.Text))
            {
                if (actualizarDatos())
                {
                    txt_buscar.Text = t1.Text;
                    if (cargarBusqueda())
                        inicio();
                }
            }
            btnActualizar.Enabled = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            btnEliminar.Enabled = false;
            if (eliminarDatos())
            {
                txt_buscar.Text = "";
                if (cargarBusqueda())
                    inicio();
            }
            btnEliminar.Enabled = true;
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
            dtpais = obpais.datosPaisNombre(txt_buscar.Text);
            if (dtpais != null)
                dg1.DataSource = obpais.datosPaisNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obpais.ERROR);
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
                llave = dtpais.Rows[pos][0].ToString();
                t1.Text = dtpais.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtpais.Rows.Count - 1;
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

        bool insertarPais(OleDbTransaction tr)
        {
            if (obpais.llave(tr) == "")
                obpais.vdatos[0] = "1";
            else
                obpais.vdatos[0] = (int.Parse(obpais.llave(tr)) + 1).ToString();
            obpais.vdatos[1] = t1.Text;
            if (obpais.insertar(tr) == 0)
            {
                MessageBox.Show(obpais.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarPais(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarPais(OleDbTransaction tr)
        {
            obpais.vdatos[0] = llave;
            obpais.vdatos[1] = t1.Text;
            if (obpais.actualizar(tr) == 0)
            {
                MessageBox.Show(obpais.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarPais(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarPais(OleDbTransaction tr)
        {
            obpais.vdatos[0] = llave;
            if (obpais.eliminar(tr) == 0)
            {
                MessageBox.Show(obpais.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarPais(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtpais.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtpais.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }
    }
}
