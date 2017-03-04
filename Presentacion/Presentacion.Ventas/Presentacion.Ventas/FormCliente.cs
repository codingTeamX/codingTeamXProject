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
    public partial class FormCliente : Form
    {
        public FormCliente()
        {
            InitializeComponent();
        }


        Persona obpersona = new Persona();
        Cliente obcliente = new Cliente();

        DataTable dtcliente = new DataTable();
        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (insetarDatos())
            {
                txt_buscar.Text = t2.Text;
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
                txt_buscar.Text = t2.Text;
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
            dtcliente = obcliente.datosClienteNombre(txt_buscar.Text);
            if (dtcliente != null)
                dg1.DataSource = obcliente.datosClienteNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obcliente.ERROR);
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
                llave = dtcliente.Rows[pos][0].ToString();
                t1.Text = dtcliente.Rows[pos][1].ToString();
                t2.Text = dtcliente.Rows[pos][2].ToString();
                t3.Text = dtcliente.Rows[pos][3].ToString();
                t4.Text = dtcliente.Rows[pos][4].ToString();
                t5.Text = dtcliente.Rows[pos][5].ToString();
                t6.Text = dtcliente.Rows[pos][6].ToString();
                c1.SelectedValue = dtcliente.Rows[pos][7].ToString();
                c2.SelectedValue = dtcliente.Rows[pos][8].ToString();

                t7.Text = dtcliente.Rows[pos][10].ToString();
                t8.Text = dtcliente.Rows[pos][11].ToString();
                t9.Text = dtcliente.Rows[pos][12].ToString();
                t10.Text = dtcliente.Rows[pos][13].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtcliente.Rows.Count - 1;
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

        bool insertarPersona(OleDbTransaction tr)
        {
            if (obpersona.llave(tr) == "")
                obpersona.vdatos[0] = "1";
            else
                obpersona.vdatos[0] = (int.Parse(obpersona.llave(tr)) + 1).ToString();
            obpersona.vdatos[1] = t1.Text;
            obpersona.vdatos[2] = t2.Text;
            obpersona.vdatos[3] = t3.Text;
            obpersona.vdatos[4] = t4.Text;
            obpersona.vdatos[5] = t5.Text;
            obpersona.vdatos[6] = t6.Text;
            obpersona.vdatos[7] = c1.SelectedValue.ToString();
            obpersona.vdatos[8] = c2.SelectedValue.ToString();
            if (obpersona.insertar(tr) == 0)
            {
                MessageBox.Show(obpersona.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insertarCliente(OleDbTransaction tr)
        {
            obcliente.vdatos[0] = obpersona.llave(tr);
            obcliente.vdatos[1] = t7.Text;
            obcliente.vdatos[2] = t8.Text;
            obcliente.vdatos[3] = t9.Text;
            obcliente.vdatos[4] = t10.Text;
            if (obcliente.insertar(tr) == 0)
            {
                MessageBox.Show(obcliente.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarPersona(tr))
            {
                if (insertarCliente(tr))
                {
                    ctr.finTR(tr);
                    return true;
                }
            }
            return false;
        }

        bool actualizarPersona(OleDbTransaction tr)
        {
            obpersona.vdatos[0] = llave;
            obpersona.vdatos[1] = t1.Text;
            obpersona.vdatos[2] = t2.Text;
            obpersona.vdatos[3] = t3.Text;
            obpersona.vdatos[4] = t4.Text;
            obpersona.vdatos[5] = t5.Text;
            obpersona.vdatos[6] = t6.Text;
            obpersona.vdatos[7] = c1.SelectedValue.ToString();
            obpersona.vdatos[8] = c2.SelectedValue.ToString();
            if (obpersona.actualizar(tr) == 0)
            {
                MessageBox.Show(obpersona.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarCliente(OleDbTransaction tr)
        {
            obcliente.vdatos[0] = llave;
            obcliente.vdatos[1] = t7.Text;
            obcliente.vdatos[2] = t8.Text;
            obcliente.vdatos[3] = t9.Text;
            obcliente.vdatos[4] = t10.Text;
            if (obcliente.actualizar(tr) == 0)
            {
                MessageBox.Show(obcliente.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarPersona(tr))
            {
                if (actualizarCliente(tr))
                {
                    ctr.finTR(tr);
                    return true;
                }
            }
            return false;
        }

        bool eliminarPersona(OleDbTransaction tr)
        {
            obpersona.vdatos[0] = llave;
            if (obpersona.eliminar(tr) == 0)
            {
                MessageBox.Show(obpersona.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarCliente(OleDbTransaction tr)
        {
            obcliente.vdatos[0] = llave;
            if (obcliente.eliminar(tr) == 0)
            {
                MessageBox.Show(obcliente.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarCliente(tr))
            {
                if (eliminarPersona(tr))
                {
                    ctr.finTR(tr);
                    return true;
                }
            }
            return false;
        }

        bool hayDatos()
        {
            return dtcliente.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtcliente.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        DataTable dtciudad = new DataTable();
        Ciudad obciudad = new Ciudad();

        DataTable dtrubro = new DataTable();
        Rubro obrubro = new Rubro();

        void cargarCiudad()
        {
            dtciudad = obciudad.datosCiudad();
            if (dtciudad != null)
            {
                c2.DataSource = obciudad.datosCiudad();
                c2.ValueMember = "id";
                c2.DisplayMember = "nombreCiudad";
            }
        }

        void cargarRubro()
        {
            dtrubro = obrubro.datosRubro();
            if (dtrubro != null)
            {
                c1.DataSource = obrubro.datosRubro();
                c1.ValueMember = "id";
                c1.DisplayMember = "nombreRubro";
            }
        }

        private void FormCliente_Load(object sender, EventArgs e)
        {
            cargarCiudad();
            cargarRubro();
        }

    }
}
