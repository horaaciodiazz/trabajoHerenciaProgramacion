namespace Library
{
    public class CommonDriver : Driver 
    {
        public CommonDriver(
            string nombre, 
            string apellido, 
            int cedula, 
            int calificacion, 
            string vehiculo, 
            string bio) 
            : base(nombre, apellido, cedula, calificacion, vehiculo, bio) 
        {
        }
    }
}