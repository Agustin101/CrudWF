using Entidades.Enums;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Entidades
{
    public class EmpleadoADO
    {
        public delegate void DelegadoEmpleados(List<Empleado> empleados);
        public event DelegadoEmpleados EventoObtenerDelegados;

        private readonly string _connection = "server=localhost;Database=EmpleadosWF_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        public List<Empleado> ObtenerTodos()
        {
            List<Empleado> lst = new List<Empleado>();

            using(var sqlConnection = new SqlConnection(_connection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "select e.id, e.nombre,e.apellido,e.email,e.fechaNacimiento, p.idPuesto from Empleados e join Puestos p on e.puesto = p.idPuesto ";
                sqlCommand.ExecuteNonQuery();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    lst.Add(new Empleado()
                    {
                        Id = int.Parse(sqlDataReader["id"].ToString()),
                        Nombre = sqlDataReader["nombre"].ToString(),
                        Apellido = sqlDataReader["apellido"].ToString(),
                        Email = sqlDataReader["email"].ToString(),
                        FecNac = DateTime.Parse(sqlDataReader["fechaNacimiento"].ToString()),
                        Puesto = (EPuesto)int.Parse(sqlDataReader["idPuesto"].ToString())
                    });
                }
                EventoObtenerDelegados?.Invoke(lst);
            }
            return lst;
        }

        public void AgregarEmpleado(Empleado empleado)
        {
            using (var sqlConnection = new SqlConnection(_connection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "insert into Empleados(nombre,apellido,email,fechaNacimiento, puesto) values (@nombre, @apellido,@email,@fecNac,@idPuesto)";
                sqlCommand.Parameters.AddWithValue("@nombre", empleado.Nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", empleado.Apellido);
                sqlCommand.Parameters.AddWithValue("@email", empleado.Email);
                sqlCommand.Parameters.AddWithValue("@fecNac", empleado.FecNac);
                sqlCommand.Parameters.AddWithValue("@idPuesto", (int)empleado.Puesto);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public Empleado ObtenerPorId(int? id)
        {
            Empleado emp = new Empleado();

            using (var sqlConnection = new SqlConnection(_connection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "select * from Empleados where id = @id";
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.ExecuteNonQuery();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    emp.Id = int.Parse(sqlDataReader["id"].ToString());
                    emp.Nombre = sqlDataReader["nombre"].ToString();
                    emp.Apellido = sqlDataReader["apellido"].ToString();
                    emp.Email = sqlDataReader["email"].ToString();
                    emp.FecNac = DateTime.Parse(sqlDataReader["fechaNacimiento"].ToString());
                    emp.Puesto = (EPuesto)int.Parse(sqlDataReader["puesto"].ToString());
                }   
            }

            return emp;
        }

        public void ModificarEmpleado(Empleado emp)
        {
            using (var sqlConnection = new SqlConnection(_connection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "update Empleados set nombre=@nombre,apellido=@apellido,email=@email,fechaNacimiento=@fecNac,puesto=@puesto where id =@id";
                sqlCommand.Parameters.AddWithValue("@nombre", emp.Nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", emp.Apellido);
                sqlCommand.Parameters.AddWithValue("@email", emp.Email);
                sqlCommand.Parameters.AddWithValue("@fecNac", emp.FecNac);
                sqlCommand.Parameters.AddWithValue("@puesto", (int)emp.Puesto);
                sqlCommand.Parameters.AddWithValue("@id", emp.Id);
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void EliminarEmpleado(int id)
        {
            using (var sqlConnection = new SqlConnection(_connection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "delete from Empleados where id = @id";
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}