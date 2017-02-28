using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Ventas
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btn_rubro_Click(object sender, EventArgs e)
        {
            FormRubro obj = new FormRubro();
            obj.Show();
        }

        private void btn_pais_Click(object sender, EventArgs e)
        {
            FormPais obj = new FormPais();
            obj.Show();
        }

        private void btn_ciudad_Click(object sender, EventArgs e)
        {
            FormCiudad obj = new FormCiudad();
            obj.Show();
        }

        private void btn_proveedor_Click(object sender, EventArgs e)
        {
            FormProveedor obj = new FormProveedor();
            obj.Show();
        }

        private void btn_marca_Click(object sender, EventArgs e)
        {
            FormMarca obj = new FormMarca();
            obj.Show();
        }

        private void btn_contrato_Click(object sender, EventArgs e)
        {
            FormContrato obj = new FormContrato();
            obj.Show();
        }

        private void btn_cargo_Click(object sender, EventArgs e)
        {
            FormCargo obj = new FormCargo();
            obj.Show();
        }

        private void btn_vitamina_Click(object sender, EventArgs e)
        {
            FormProteina obj = new FormProteina();
            obj.Show();
        }

        private void btn_grasa_Click(object sender, EventArgs e)
        {
            FormGrasa obj = new FormGrasa();
            obj.Show();
        }

        private void btn_cliente_Click(object sender, EventArgs e)
        {
            FormCliente obj = new FormCliente();
            obj.Show();
        }

        private void btn_empleado_Click(object sender, EventArgs e)
        {
            FormEmpleado obj = new FormEmpleado();
            obj.Show();
        }

        private void btn_producto_Click(object sender, EventArgs e)
        {
            FormProducto obj = new FormProducto();
            obj.Show();
        }

        private void btn_lote_Click(object sender, EventArgs e)
        {
            FormLote obj = new FormLote();
            obj.Show();
        }

        private void btn_venta_Click(object sender, EventArgs e)
        {
            FormVenta obj = new FormVenta();
            obj.Show();
        }

        private void btn_carbohidrato_Click(object sender, EventArgs e)
        {
            FormCarbohidrato obj = new FormCarbohidrato();
            obj.Show();
        }

    }
}
