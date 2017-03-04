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
    public partial class FormContrato : Form
    {
        public FormContrato()
        {
            InitializeComponent();
        }

        Contrato obcontrato = new Contrato();
        DataTable dtcontrato = new DataTable();
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
            dtcontrato = obcontrato.datosNroContrato(txt_buscar.Text);
            if (dtcontrato != null)
                dg1.DataSource = obcontrato.datosNroContrato(txt_buscar.Text);
            else
            {
                MessageBox.Show(obcontrato.ERROR);
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
                llave = dtcontrato.Rows[pos][0].ToString();
                t1.Text = dtcontrato.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtcontrato.Rows.Count - 1;
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

        bool insertarContrato(OleDbTransaction tr)
        {
            if (obcontrato.llave(tr) == "")
                obcontrato.vdatos[0] = "1";
            else
                obcontrato.vdatos[0] = (int.Parse(obcontrato.llave(tr)) + 1).ToString();
            obcontrato.vdatos[1] = t1.Text;
            if (obcontrato.insertar(tr) == 0)
            {
                MessageBox.Show(obcontrato.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarContrato(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarContrato(OleDbTransaction tr)
        {
            obcontrato.vdatos[0] = llave;
            obcontrato.vdatos[1] = t1.Text;
            if (obcontrato.actualizar(tr) == 0)
            {
                MessageBox.Show(obcontrato.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarContrato(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarContrato(OleDbTransaction tr)
        {
            obcontrato.vdatos[0] = llave;
            if (obcontrato.eliminar(tr) == 0)
            {
                MessageBox.Show(obcontrato.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarContrato(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtcontrato.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtcontrato.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

    }
}
