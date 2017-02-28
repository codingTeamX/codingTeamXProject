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
    public partial class FormRubro : Form
    {
        public FormRubro()
        {
            InitializeComponent();
        }

        Rubro obrubro = new Rubro();
        DataTable dtrubro = new DataTable();
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
            dtrubro = obrubro.datosRubroNombre(txt_buscar.Text);
            if (dtrubro != null)
                dg1.DataSource = obrubro.datosRubroNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obrubro.ERROR);
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
                llave = dtrubro.Rows[pos][0].ToString();
                t1.Text = dtrubro.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtrubro.Rows.Count - 1;
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

        bool insertarRubro(OleDbTransaction tr)
        {
            if (obrubro.llave(tr) == "")
                obrubro.vdatos[0] = "1";
            else
                obrubro.vdatos[0] = (int.Parse(obrubro.llave(tr)) + 1).ToString();
            obrubro.vdatos[1] = t1.Text;
            if (obrubro.insertar(tr) == 0)
            {
                MessageBox.Show(obrubro.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarRubro(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool actualizarRubro(OleDbTransaction tr)
        {
            obrubro.vdatos[0] = llave;
            obrubro.vdatos[1] = t1.Text;
            if (obrubro.actualizar(tr) == 0)
            {
                MessageBox.Show(obrubro.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarRubro(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarRubro(OleDbTransaction tr)
        {
            obrubro.vdatos[0] = llave;
            if (obrubro.eliminar(tr) == 0)
            {
                MessageBox.Show(obrubro.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarRubro(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtrubro.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtrubro.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

    }
}
