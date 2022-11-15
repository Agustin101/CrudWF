using Entidades;
using Entidades.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class AgregarEmpleado : Form
    {
        private int? _id;
        private EmpleadoADO _empleadoADO;

        public AgregarEmpleado(int? id = null)
        {
            InitializeComponent();
            _empleadoADO = new EmpleadoADO();

            _id = id;
            if (_id is not null)
                CargarDatos();
        }

        private void CargarDatos()
        {
            Empleado empleado = _empleadoADO.ObtenerPorId(_id);
            txtApellido.Text = empleado.Apellido;
            txtEmail.Text = empleado.Email;
            txtNombre.Text = empleado.Nombre;
            cmbPuesto.SelectedItem = empleado.Puesto;
            dtpFechaNacimiento.Value = empleado.FecNac;
            btnAlta.Text = "Modificar";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();
                Empleado emp = new Empleado();
                emp.Email = txtEmail.Text;
                emp.Nombre = txtNombre.Text;
                emp.Apellido = txtApellido.Text;
                emp.Puesto = (EPuesto)cmbPuesto.SelectedItem;
                emp.FecNac = dtpFechaNacimiento.Value;
                if(_id is not null)
                {
                    emp.Id = (int)_id;
                    _empleadoADO.ModificarEmpleado(emp);
                }
                else
                {
                    _empleadoADO.AgregarEmpleado(emp);
                }
                
                DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
            {
                lblError.Text=ex.Message;
            }
        }

        private void ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                throw new Exception("Verificar el campo apellido");
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                throw new Exception("Verificar el campo nombre");
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                throw new Exception("Verificar el campo email");
            }

            lblError.Text = "";
        }

        private void AgregarEmpleado_Load(object sender, EventArgs e)
        {
            cmbPuesto.DataSource = Enum.GetValues(typeof(EPuesto));
            cmbPuesto.SelectedIndex = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
