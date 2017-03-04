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
    public partial class FormEmpleado : Form
    {
        public FormEmpleado()
        {
            InitializeComponent();
        }


        Persona obpersona = new Persona();
        Empleado obempleado = new Empleado();

        Contrato obcontrato = new Contrato();
        EmpleadoContrato obdetalle = new EmpleadoContrato();

        DataTable dtempleado = new DataTable();
        DataTable dt = new DataTable();

        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (insetarDatos())
            {
                txt_buscar.Text = t2.Text;
                if (cargarBusqueda())
                    inicio();
                lb1.Items.Clear();
                lb2.Items.Clear();
                lb3.Items.Clear();
                lb4.Items.Clear();
                lb5.Items.Clear();
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
            dtempleado = obempleado.datosEmpleadoNombre(txt_buscar.Text);
            if (dtempleado != null)
                dg1.DataSource = obempleado.datosEmpleadoNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obempleado.ERROR);
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
                llave = dtempleado.Rows[pos][0].ToString();
                t1.Text = dtempleado.Rows[pos][1].ToString();
                t2.Text = dtempleado.Rows[pos][2].ToString();
                t3.Text = dtempleado.Rows[pos][3].ToString();
                t4.Text = dtempleado.Rows[pos][4].ToString();
                t5.Text = dtempleado.Rows[pos][5].ToString();
                t6.Text = dtempleado.Rows[pos][6].ToString();
                c1.SelectedValue = dtempleado.Rows[pos][7].ToString();
                c2.SelectedValue = dtempleado.Rows[pos][8].ToString();

                t7.Text = dtempleado.Rows[pos][10].ToString();
            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtempleado.Rows.Count - 1;
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

        bool insertarEmpleado(OleDbTransaction tr)
        {
            obempleado.vdatos[0] = obpersona.llave(tr);
            obempleado.vdatos[1] = t7.Text;
            if (obempleado.insertar(tr) == 0)
            {
                MessageBox.Show(obempleado.ERROR);
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
                if (insertarEmpleado(tr))
                {
                    if (insertarEmpleadoContrato(tr))
                    {
                        ctr.finTR(tr);
                        return true;
                    }
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

        bool actualizarEmpleado(OleDbTransaction tr)
        {
            obempleado.vdatos[0] = llave;
            obempleado.vdatos[1] = t7.Text;
            if (obempleado.actualizar(tr) == 0)
            {
                MessageBox.Show(obempleado.ERROR);
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
                if (actualizarEmpleado(tr))
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

        bool eliminarEmpleado(OleDbTransaction tr)
        {
            obempleado.vdatos[0] = llave;
            if (obempleado.eliminar(tr) == 0)
            {
                MessageBox.Show(obempleado.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarEmpleado(tr))
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
            return dtempleado.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtempleado.Rows.Count - 1;
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

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            cargarNuevo();
        }

        bool cargarNuevo()
        {
            dt = obcontrato.datosContrato();
            if (dt != null)
            {
                dt.Columns.Add("Fecha Inicio");
                dt.Columns.Add("Fecha Fin");
                dt.Columns.Add("Sueldo");
                dt.Columns.Add("Cargo");
                dg1.DataSource = dt;
            }
            else
            {
                MessageBox.Show(obempleado.ERROR);
                return false;
            }
            return true;
        }

        int cont = 0;

        bool existeDuplicado(int num)
        {
            for (int i = 0; i < cont; i++)
            {
                if (int.Parse(lb1.Items[i].ToString()) == num)
                    return true;
            }
            return false;
        }

        private void dg1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    if (!existeDuplicado(int.Parse(dg1.CurrentRow.Cells["id"].Value.ToString())))
                    {
                        lb1.Items.Add(dg1.CurrentRow.Cells["id"].Value);
                        lb2.Items.Add(dg1.CurrentRow.Cells["Fecha Inicio"].Value);
                        lb3.Items.Add(dg1.CurrentRow.Cells["Fecha Fin"].Value);
                        lb4.Items.Add(dg1.CurrentRow.Cells["Sueldo"].Value);
                        lb5.Items.Add(dg1.CurrentRow.Cells["Cargo"].Value);
                        cont++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool insertarEmpleadoContrato(OleDbTransaction tr)
        {
            string llave = obempleado.llave(tr);
            for (int i = 0; i <= lb1.Items.Count - 1; i++)
            {
                obdetalle.vdatos[0] = llave;
                obdetalle.vdatos[1] = lb1.Items[i].ToString();
                obdetalle.vdatos[2] = lb2.Items[i].ToString();
                obdetalle.vdatos[3] = lb3.Items[i].ToString();
                obdetalle.vdatos[4] = lb4.Items[i].ToString();
                obdetalle.vdatos[5] = lb5.Items[i].ToString();
                if (obdetalle.insertar(tr) == 0)
                {
                    MessageBox.Show(obdetalle.ERROR);
                    ctr.desTR(tr);
                    return false;
                }
            }
            return true;
        }
    }
}
