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
    public partial class FormProteina : Form
    {
        public FormProteina()
        {
            InitializeComponent();
        }

      Proteina obproteina = new Proteina();
        DataTable dtproteina = new DataTable();
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
            dtproteina = obproteina.datosProteinaNombre(txt_buscar.Text);
            if (dtproteina != null)
                dg1.DataSource = obproteina.datosProteinaNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obproteina.ERROR);
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
                llave = dtproteina.Rows[pos][0].ToString();
                t1.Text = dtproteina.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtproteina.Rows.Count - 1;
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

        bool insertarVitamina(OleDbTransaction tr)
        {
            if (obproteina.llave(tr) == "")
                obproteina.vdatos[0] = "1";
            else
                obproteina.vdatos[0] = (int.Parse(obproteina.llave(tr)) + 1).ToString();
            obproteina.vdatos[1] = t1.Text;
            if (obproteina.insertar(tr) == 0)
            {
                MessageBox.Show(obproteina.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarVitamina(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarVitamina(OleDbTransaction tr)
        {
            obproteina.vdatos[0] = llave;
            obproteina.vdatos[1] = t1.Text;
            if (obproteina.actualizar(tr) == 0)
            {
                MessageBox.Show(obproteina.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarVitamina(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarVitamina(OleDbTransaction tr)
        {
            obproteina.vdatos[0] = llave;
            if (obproteina.eliminar(tr) == 0)
            {
                MessageBox.Show(obproteina.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarVitamina(tr))
            {
                    ctr.finTR(tr);
                    return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtproteina.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtproteina.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

    }
}
