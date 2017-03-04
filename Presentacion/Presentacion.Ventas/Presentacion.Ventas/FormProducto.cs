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
    public partial class FormProducto : Form
    {
        public FormProducto()
        {
            InitializeComponent();
        }
        Producto obproducto = new Producto();

        Carbohidrato obcarbohidrato = new Carbohidrato();
        Proteina obproteina = new Proteina();
        Grasa obgrasa = new Grasa();

        DetalleProteina obdetalleproteina = new DetalleProteina();
        DetalleGrasa obdetallegrasa = new DetalleGrasa();
        DetalleCarbohidrato obdetallecarbohidrato = new DetalleCarbohidrato();

        DataTable dtproducto = new DataTable();
        DataTable dt = new DataTable();

        CtrlTransaccion ctr = new CtrlTransaccion();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (insetarDatos())
            {
                txt_buscar.Text = t1.Text;
                if (cargarBusqueda())
                    inicio();
                lb1.Items.Clear();
                lb2.Items.Clear();
                lb3.Items.Clear();
                lb4.Items.Clear();
                lb5.Items.Clear();
                lb6.Items.Clear();
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
                txt_buscar.Text = t3.Text;
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
            dtproducto = obproducto.datosProductoNombre(txt_buscar.Text);
            if (dtproducto != null)
                dg1.DataSource = obproducto.datosProductoNombre(txt_buscar.Text);
            else
            {
                MessageBox.Show(obproducto.ERROR);
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
                llave = dtproducto.Rows[pos][0].ToString();
                t1.Text = dtproducto.Rows[pos][1].ToString();
                t2.Text = dtproducto.Rows[pos][2].ToString();
                t3.Text = dtproducto.Rows[pos][3].ToString();
                t4.Text = dtproducto.Rows[pos][4].ToString();
                t5.Text = dtproducto.Rows[pos][5].ToString();
                t6.Text = dtproducto.Rows[pos][6].ToString();
                dtp.Value = Convert.ToDateTime(dtproducto.Rows[pos][7].ToString());
                c1.SelectedValue = dtproducto.Rows[pos][8].ToString();

            }
        }

        void inicio()
        {
            pos = 0;
            cargarText();
        }

        void fin()
        {
            pos = dtproducto.Rows.Count - 1;
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

        bool insertarProducto(OleDbTransaction tr)
        {
            if (obproducto.llave(tr) == "")
                obproducto.vdatos[0] = "1";
            else
                obproducto.vdatos[0] = (int.Parse(obproducto.llave(tr)) + 1).ToString();
            obproducto.vdatos[1] = t1.Text;
            obproducto.vdatos[2] = t2.Text;
            obproducto.vdatos[3] = t3.Text;
            obproducto.vdatos[4] = t4.Text;
            obproducto.vdatos[5] = t5.Text;
            obproducto.vdatos[6] = t6.Text;
            obproducto.vdatos[7] = dtp.Value.ToShortDateString();
            obproducto.vdatos[8] = c1.SelectedValue.ToString();
            if (obproducto.insertar(tr) == 0)
            {
                MessageBox.Show(obproducto.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool insetarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (insertarProducto(tr))
            {
                if (insertarDetalleGrasa(tr))
                {
                    if (insertarDetalleProteina(tr))
                    {
                        if (insertarDetalleCarbohidrato(tr))
                        {
                            ctr.finTR(tr);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool actualizarProducto(OleDbTransaction tr)
        {
            obproducto.vdatos[0] = llave;
            obproducto.vdatos[1] = t1.Text;
            obproducto.vdatos[2] = t2.Text;
            obproducto.vdatos[3] = t3.Text;
            obproducto.vdatos[4] = t4.Text;
            obproducto.vdatos[5] = t5.Text;
            obproducto.vdatos[6] = t6.Text;
            obproducto.vdatos[7] = dtp.Text;
            obproducto.vdatos[8] = c1.SelectedValue.ToString();
            if (obproducto.actualizar(tr) == 0)
            {
                MessageBox.Show(obproducto.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool actualizarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (actualizarProducto(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool eliminarProducto(OleDbTransaction tr)
        {
            obproducto.vdatos[0] = llave;
            if (obproducto.eliminar(tr) == 0)
            {
                MessageBox.Show(obproducto.ERROR);
                ctr.desTR(tr);
                return false;
            }
            return true;
        }

        bool eliminarDatos()
        {
            OleDbTransaction tr = ctr.iniciarTransaccion();
            if (eliminarProducto(tr))
            {
                ctr.finTR(tr);
                return true;
            }
            return false;
        }

        bool hayDatos()
        {
            return dtproducto.Rows.Count != 0;
        }

        bool puedeSiguiente()
        {
            return pos < dtproducto.Rows.Count - 1;
        }

        bool puedeAtras()
        {
            return pos > 0;
        }

        DataTable dtmarca = new DataTable();
        Marca obmarca = new Marca();


        void cargarMarca()
        {
            dtmarca = obmarca.datosMarca();
            if (dtmarca != null)
            {
                c1.DataSource = obmarca.datosMarca();
                c1.ValueMember = "id";
                c1.DisplayMember = "nombreMarca";
            }
        }


        bool cargarProteina()
        {
            dt = obproteina.datosProteina();
            if (dt != null)
            {
                dt.Columns.Add("Cantidad");
                dg1.DataSource = dt;
            }
            else
            {
                MessageBox.Show(obproteina.ERROR);
                return false;
            }
            return true;
        }

        bool cargarGrasa()
        {
            dt = obgrasa.datosGrasa();
            if (dt != null)
            {
                dt.Columns.Add("Cantidad");
                dg1.DataSource = dt;
            }
            else
            {
                MessageBox.Show(obgrasa.ERROR);
                return false;
            }
            return true;
        }

        bool cargarCarbohidrato()
        {
            dt = obcarbohidrato.datosCarbohidrato();
            if (dt != null)
            {
                dt.Columns.Add("Cantidad");
                dg1.DataSource = dt;
            }
            else
            {
                MessageBox.Show(obgrasa.ERROR);
                return false;
            }
            return true;
        }

        bool existeDuplicadoProteina(int num)
        {
            for (int i = 0; i <= lb1.Items.Count - 1; i++)
            {
                if (int.Parse(lb1.Items[i].ToString()) == num)
                    return true;
            }
            return false;
        }

        bool existeDuplicadoGrasa(int num)
        {
            for (int i = 0; i <= lb3.Items.Count - 1; i++)
            {
                if (int.Parse(lb3.Items[i].ToString()) == num)
                    return true;
            }
            return false;
        }

        bool existeDuplicadoCarbohidrato(int num)
        {
            for (int i = 0; i <= lb5.Items.Count - 1; i++)
            {
                if (int.Parse(lb5.Items[i].ToString()) == num)
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
                    if (s == 1 && !existeDuplicadoProteina(int.Parse(dg1.CurrentRow.Cells["id"].Value.ToString())))
                    {
                        lb1.Items.Add(dg1.CurrentRow.Cells["id"].Value);
                        lb2.Items.Add(dg1.CurrentRow.Cells["cantidad"].Value);
                    }
                    else if (s == 2 && !existeDuplicadoGrasa(int.Parse(dg1.CurrentRow.Cells["id"].Value.ToString())))
                    {
                        lb3.Items.Add(dg1.CurrentRow.Cells["id"].Value);
                        lb4.Items.Add(dg1.CurrentRow.Cells["cantidad"].Value);
                    }
                    else if (s == 3 && !existeDuplicadoCarbohidrato(int.Parse(dg1.CurrentRow.Cells["id"].Value.ToString())))
                    {
                        lb5.Items.Add(dg1.CurrentRow.Cells["id"].Value);
                        lb6.Items.Add(dg1.CurrentRow.Cells["cantidad"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool insertarDetalleProteina(OleDbTransaction tr)
        {
            string llave = obproducto.llave(tr);
            for (int i = 0; i <= lb1.Items.Count - 1; i++)
            {
                obdetalleproteina.vdatos[0] = llave;
                obdetalleproteina.vdatos[1] = lb1.Items[i].ToString();
                obdetalleproteina.vdatos[2] = lb2.Items[i].ToString();
                if (obdetalleproteina.insertar(tr) == 0)
                {
                    MessageBox.Show(obdetalleproteina.ERROR);
                    ctr.desTR(tr);
                    return false;
                }
            }
            return true;
        }

        bool insertarDetalleGrasa(OleDbTransaction tr)
        {
            string llave = obproducto.llave(tr);
            for (int i = 0; i <= lb3.Items.Count - 1; i++)
            {
                obdetallegrasa.vdatos[0] = lb3.Items[i].ToString(); // ID GRASA
                obdetallegrasa.vdatos[1] = llave; // ID PRODUCTO
                obdetallegrasa.vdatos[2] = lb4.Items[i].ToString();
                if (obdetallegrasa.insertar(tr) == 0)
                {
                    MessageBox.Show(obdetallegrasa.ERROR);
                    ctr.desTR(tr);
                    return false;
                }
            }
            return true;
        }

        bool insertarDetalleCarbohidrato(OleDbTransaction tr)
        {
            string llave = obproducto.llave(tr);
            for (int i = 0; i <= lb5.Items.Count - 1; i++)
            {
                obdetallecarbohidrato.vdatos[0] = llave;
                obdetallecarbohidrato.vdatos[1] = lb5.Items[i].ToString();
                obdetallecarbohidrato.vdatos[2] = lb6.Items[i].ToString();
                if (obdetallecarbohidrato.insertar(tr) == 0)
                {
                    MessageBox.Show(obdetallecarbohidrato.ERROR);
                    ctr.desTR(tr);
                    return false;
                }
            }
            return true;
        }


        private void FormProducto_Load(object sender, EventArgs e)
        {
            dtp.CustomFormat = "dd/MM/yyyy";
            cargarMarca();
            s = -1;
        }

        int s = -1;

        private void btn_vitaminas_Click(object sender, EventArgs e)
        {
            cargarProteina();
            s = 1;
        }

        private void btn_grasa_Click(object sender, EventArgs e)
        {
            s = 2;
            cargarGrasa();
        }

        private void btn_carbohidrato_Click(object sender, EventArgs e)
        {
            s = 3;
            cargarCarbohidrato();
        }

    }
}
