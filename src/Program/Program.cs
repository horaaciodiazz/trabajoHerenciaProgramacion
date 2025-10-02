using Library;
namespace Ucu.Poo.RideShare
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            En éste método deberas mostrar un ejemplo de funcionamiento de tu programa. A continuación te
            planteamos un ejemplo de como hacerlo. Esto no significa que te limites a hacer solamente esto, debes pensar
            en grande!

            Usuario pasajero1 = nuevo Pasajero()
            Usuario pasajero2 = nuevo Pasajero()
            Usuario pasajero3 = nuevo Pasajero()
            Usuario conductor1 = nuevo Conductor()
            Usuario conductorPool1 = nuevo ConductorPool(maxPasajeros = 3)
            UcuRideShare rideShare = nuevo UcuRideShare()

            rideShare.Add(conductor1)
            Se publica en Twitter un nuevo conductor!

            rideShare.Add(conductorPool1)
            Se publica en Twitter un nuevo conductor!

            rideShare.Add(pasajero1)
            Se publica en Twitter nuevo registro de pasajero!

            rideShare.Add(pasajero2)
            Se publica en Twitter nuevo registro de pasajero!

            rideShare.Add(pasajero3)
            Se publica en Twitter nuevo registro de pasajero!

            */
            User pasajero1 = new Passanger("Juan", "Lopez", 1234567, 3, "No bio");
            User pasajero2 = new Passanger("Maria", "Jose", 2134567, 4, "No");
            User conductor1 = new Driver("Marco", "Aurelio", 3214567, 5, "Suzuki Alto", "No");
            User conductorPool1 = new PoolDriver("Juan", "Díaz", 9876543, 4, "Volkswagen Saveiro", "No", 3);
            UcuRideShare rideShare = new UcuRideShare();

            rideShare.Add(conductor1);
            rideShare.Add(conductorPool1);
            rideShare.Add(pasajero1);
            rideShare.Add(pasajero2);
        }
    }
}
