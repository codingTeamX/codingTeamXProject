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
    public partial class FormCarbohidrato : Form
    {
        public FormCarbohidrato()
        {
            InitializeComponent();
        }
        Carbohidrato obcarbohidrato = new Carbohidrato();
        DataTable dtcarbohidrato = new DataTable();
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
            dtcarbohidrato = obcarbohidrato.datosCarbohidratoNombre(txt_buscar.Text);
            if (dtcarbohidrato != null)
                dg1.DataSource = obcarbohidrato.datosCarbohidratoNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obcarbohidrato.ERROR);
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
                llave = dtcarbohidrato.Rows[pos][0].ToString();
                t1.Text = dtcarbohidrato.Rows[pos][1].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtcarbohidrato.Rows.Count - 1;
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
            if (obcarbohidrato.llave(tr) == "")
                obcarbohidrato.vdatos[0] = "1";
            else
                obcarbohidrato.vdatos[0] = (int.Parse(obcarbohidrato.llave(tr)) + 1).ToString();
            obcarbohidrato.vdatos[1] = t1.Text;
            if (obcarbohidrato.insertar(tr) == 0)
            {
                MessageBox.Show(obcarbohidrato.ERROR);
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
            obcarbohidrato.vdatos[0] = llave;
            obcarbohidrato.vdatos[1] = t1.Text;
            if (obcarbohidrato.actualizar(tr) == 0)
            {
                MessageBox.Show(obcarbohidrato.ERROR);
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
            obcarbohidrato.vdatos[0] = llave;
            if (obcarbohidrato.eliminar(tr) == 0)
            {
                MessageBox.Show(obcarbohidrato.ERROR);
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
            return dtcarbohidrato.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtcarbohidrato.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        private void FormCarbohidrato_Load(object sender, EventArgs e)
        {

        }
     }
  }

       
   
