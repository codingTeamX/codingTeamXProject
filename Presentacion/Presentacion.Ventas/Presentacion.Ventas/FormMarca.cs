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
    public partial class FormMarca : Form
    {
        public FormMarca()
        {
            InitializeComponent();
        }

        Marca obmarca = new Marca();
        DataTable dtmarca = new DataTable();
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
            dtmarca = obmarca.datosMarcaNombre(txt_buscar.Text);
            if (obmarca != null)
                dg1.DataSource = obmarca.datosMarcaNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obmarca.ERROR);
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
                llave = dtmarca.Rows[pos][0].ToString();
                t1.Text = dtmarca.Rows[pos][1].ToString();
                c1.SelectedValue = dtmarca.Rows[pos][2].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtmarca.Rows.Count - 1;
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

        bool insertarMarca(OleDbTransaction tr)
        {
            if (obmarca.llave(tr) == "")
                obmarca.vdatos[0] = "1";
            else
                obmarca.vdatos[0] = (int.Parse(obmarca.llave(tr)) + 1).ToString();
            obmarca.vdatos[1] = t1.Text;
            obmarca.vdatos[2] = c1.SelectedValue.ToString();
            if (obmarca.insertar(tr) == 0)
            {
                MessageBox.Show(obmarca.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarMarca(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarMarca(OleDbTransaction tr)
        {
            obmarca.vdatos[0] = llave;
            obmarca.vdatos[1] = t1.Text;
            obmarca.vdatos[2] = c1.SelectedValue.ToString();
            if (obmarca.actualizar(tr) == 0)
            {
                MessageBox.Show(obmarca.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarMarca(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarMarca(OleDbTransaction tr)
        {
            obmarca.vdatos[0] = llave;
            if (obmarca.eliminar(tr) == 0)
            {
                MessageBox.Show(obmarca.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarMarca(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtmarca.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtmarca.Rows.Count - 1;
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

        private void FormMarca_Load(object sender, EventArgs e)
        {
            cargarCiudad();
        }

    }
}
