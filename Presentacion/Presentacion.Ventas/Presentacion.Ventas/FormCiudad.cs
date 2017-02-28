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
    public partial class FormCiudad : Form
    {
        public FormCiudad()
        {
            InitializeComponent();
        }

        Ciudad obciudad = new Ciudad();
        DataTable dtciudad = new DataTable();
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
            dtciudad = obciudad.datosCiudadNombre(txt_buscar.Text);
            if (dtciudad != null)
                dg1.DataSource = obciudad.datosCiudadNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obciudad.ERROR);
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
                llave = dtciudad.Rows[pos][0].ToString();
                t1.Text = dtciudad.Rows[pos][1].ToString();
                c1.SelectedValue = dtciudad.Rows[pos][2].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtciudad.Rows.Count - 1;
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

        bool insertarCiudad(OleDbTransaction tr)
        {
            if (obciudad.llave(tr) == "")
                obciudad.vdatos[0] = "1";
            else
                obciudad.vdatos[0] = (int.Parse(obciudad.llave(tr)) + 1).ToString();
            obciudad.vdatos[1] = t1.Text;
            obciudad.vdatos[2] = c1.SelectedValue.ToString();
            if (obciudad.insertar(tr) == 0)
            {
                MessageBox.Show(obciudad.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarCiudad(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarCiudad(OleDbTransaction tr)
        {
            obciudad.vdatos[0] = llave;
            obciudad.vdatos[1] = t1.Text;
            obciudad.vdatos[2] = c1.SelectedValue.ToString();
            if (obciudad.actualizar(tr) == 0)
            {
                MessageBox.Show(obciudad.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarCiudad(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarCiudad(OleDbTransaction tr)
        {
            obciudad.vdatos[0] = llave;
            if (obciudad.eliminar(tr) == 0)
            {
                MessageBox.Show(obciudad.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarCiudad(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtciudad.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtciudad.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        DataTable dtpais = new DataTable();
        Pais obpais = new Pais();

        void cargarPais()
        {
            dtpais = obpais.datosPais();
            if (dtpais != null)
            {
                c1.DataSource = obpais.datosPais();
                c1.ValueMember = "id";
                c1.DisplayMember = "nombrePais";
            }
        }

        private void FormCiudad_Load(object sender, EventArgs e)
        {
            cargarPais();
        }

    }
}
