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
    public partial class FormGrasa : Form
    {
        public FormGrasa()
        {
            InitializeComponent();
        }
        Grasa obgrasa = new Grasa();
        DataTable dtgrasa = new DataTable();
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
            dtgrasa = obgrasa.datosGrasaNombre(txt_buscar.Text);
            if (dtgrasa != null)
                dg1.DataSource = obgrasa.datosGrasaNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obgrasa.ERROR);
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
                llave = dtgrasa.Rows[pos][0].ToString();
                t1.Text = dtgrasa.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtgrasa.Rows.Count - 1;
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

        bool insertarGrasa(OleDbTransaction tr)
        {
            if (obgrasa.llave(tr) == "")
                obgrasa.vdatos[0] = "1";
            else
                obgrasa.vdatos[0] = (int.Parse(obgrasa.llave(tr)) + 1).ToString();
            obgrasa.vdatos[1] = t1.Text;
            if (obgrasa.insertar(tr) == 0)
            {
                MessageBox.Show(obgrasa.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarGrasa(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarGrasa(OleDbTransaction tr)
        {
            obgrasa.vdatos[0] = llave;
            obgrasa.vdatos[1] = t1.Text;
            if (obgrasa.actualizar(tr) == 0)
            {
                MessageBox.Show(obgrasa.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarGrasa(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarGrasa(OleDbTransaction tr)
        {
            obgrasa.vdatos[0] = llave;
            if (obgrasa.eliminar(tr) == 0)
            {
                MessageBox.Show(obgrasa.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarGrasa(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtgrasa.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtgrasa.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }



    }
}
