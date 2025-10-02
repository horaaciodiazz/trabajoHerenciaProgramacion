namespace Library
{
    public abstract class Driver : User
    {
        public Driver(
            string nombre, 
            string apellido, 
            int cedula, 
            int calificacion, 
            string vehiculo, 
            string bio) 
            : base(nombre, apellido, cedula) 
        {
        }
    }
}