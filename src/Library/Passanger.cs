namespace Library
    
{
    public class Passanger : User
        {
            public Passanger(
                string nombre,
                string apellido,
                int cedula,
                int calificacion,
                string bio)
                : base(nombre, apellido, cedula)
            {
            }
        }
}
    
