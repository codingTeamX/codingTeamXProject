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
    public partial class FormCargo : Form
    {
        public FormCargo()
        {
            InitializeComponent();
        }

        Cargo obcargo = new Cargo();
        DataTable dtcargo = new DataTable();
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
            dtcargo = obcargo.datosCargoNombre(txt_buscar.Text);
            if (dtcargo != null)
                dg1.DataSource = obcargo.datosCargoNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obcargo.ERROR);
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
                llave = dtcargo.Rows[pos][0].ToString();
                t1.Text = dtcargo.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtcargo.Rows.Count - 1;
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

        bool insertarCargo(OleDbTransaction tr)
        {
            if (obcargo.llave(tr) == "")
                obcargo.vdatos[0] = "1";
            else
                obcargo.vdatos[0] = (int.Parse(obcargo.llave(tr)) + 1).ToString();
            obcargo.vdatos[1] = t1.Text;
            if (obcargo.insertar(tr) == 0)
            {
                MessageBox.Show(obcargo.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarCargo(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarCargo(OleDbTransaction tr)
        {
            obcargo.vdatos[0] = llave;
            obcargo.vdatos[1] = t1.Text;
            if (obcargo.actualizar(tr) == 0)
            {
                MessageBox.Show(obcargo.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarCargo(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarCargo(OleDbTransaction tr)
        {
            obcargo.vdatos[0] = llave;
            if (obcargo.eliminar(tr) == 0)
            {
                MessageBox.Show(obcargo.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarCargo(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtcargo.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtcargo.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }
    }
}
