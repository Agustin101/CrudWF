using Entidades;
using Vista;

namespace WindowsFormCrud
{
    public partial class FrmPrincipal : Form
    {
        private readonly EmpleadoADO _empleadoAdo;

        public FrmPrincipal()
        {
            InitializeComponent();
            _empleadoAdo = new EmpleadoADO();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            _empleadoAdo.EventoObtenerDelegados += ActualizarDgv;
            ActualizarEmpleados();
        }

        private void ActualizarEmpleados()
        {
            _empleadoAdo.ObtenerTodos();
        }

        private void ActualizarDgv(List<Empleado> empleados)
        {
            dgvEmpleados.DataSource = null;
            dgvEmpleados.DataSource = empleados;
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            var frmAlta = new AgregarEmpleado();
            frmAlta.ShowDialog();

            if(frmAlta.DialogResult == DialogResult.OK)
            {
                ActualizarEmpleados();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? idEmpleado = ObtenerId();
            var frmModificar = new AgregarEmpleado(idEmpleado);
            frmModificar.ShowDialog();

            if (frmModificar.DialogResult == DialogResult.OK)
            {
                ActualizarEmpleados();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? idEmpleado = ObtenerId();

            if (idEmpleado is not null)
            {
                try
                {
                    _empleadoAdo.EliminarEmpleado((int)idEmpleado);
                    ActualizarEmpleados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private int? ObtenerId()
        {
            try
            {
                return int.Parse(dgvEmpleados.Rows[dgvEmpleados.CurrentRow.Index].Cells[0].Value.ToString());
            }
            catch
            {
                return null;
            }
        }
    }
}