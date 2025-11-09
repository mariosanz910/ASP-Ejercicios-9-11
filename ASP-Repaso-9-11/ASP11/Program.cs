using System;
using System.Collections.Generic;

namespace FleetManagerSA
{
    abstract class Vehiculo
    {
        public string Matricula { get; set; }

        private double consumo;
        public double Consumo
        {
            get { return consumo; }
            set { consumo = value < 0 ? 0.0 : value; }
        }

        public const double CostoOperacionalBase = 0.15;

        public Vehiculo(string matricula, double consumo)
        {
            Matricula = matricula;
            Consumo = consumo;
        }

        public virtual double CalcularCostoPorKm()
        {
            return Consumo * CostoOperacionalBase;
        }

        public override string ToString()
        {
            return $"Matrícula: {Matricula}, Consumo: {Consumo} L/100km";
        }
    }

    class Autobus : Vehiculo
    {
        private int capacidadMaxima;
        public int CapacidadMaxima
        {
            get { return capacidadMaxima; }
            set { capacidadMaxima = value < 0 ? 0 : value; }
        }

        public const double FactorDesgaste = 1.2;

        public Autobus(string matricula, double consumo, int capacidad)
            : base(matricula, consumo)
        {
            CapacidadMaxima = capacidad;
        }

        public override double CalcularCostoPorKm()
        {
            return base.CalcularCostoPorKm() * FactorDesgaste;
        }

        public override string ToString()
        {
            return $"[Autobús] {base.ToString()}, Capacidad: {CapacidadMaxima} pasajeros";
        }
    }

    class Camion : Vehiculo
    {
        private double peajeAnual;
        public double PeajeAnual
        {
            get { return peajeAnual; }
            set { peajeAnual = value < 0 ? 0.0 : value; }
        }

        public Camion(string matricula, double consumo, double peaje)
            : base(matricula, consumo)
        {
            PeajeAnual = peaje;
        }

        public override double CalcularCostoPorKm()
        {
            return base.CalcularCostoPorKm() + (PeajeAnual / 100000.0);
        }

        public override string ToString()
        {
            return $"[Camión] {base.ToString()}, Peaje anual: {PeajeAnual} €";
        }
    }

    class Program
    {
        static List<Vehiculo> flota = new List<Vehiculo>();

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.WriteLine("\n=== FleetManager S.A ===");
                Console.WriteLine("1. Registrar Vehículo");
                Console.WriteLine("2. Ver Costos Operacionales");
                Console.WriteLine("3. Calcular Costo Total de Flota (100,000 km)");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        RegistrarVehiculo();
                        break;
                    case 2:
                        VerCostos();
                        break;
                    case 3:
                        CalcularCostoTotal();
                        break;
                    case 4:
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

            } while (opcion != 4);
        }

        static void RegistrarVehiculo()
        {
            Console.WriteLine("\nTipo de vehículo:");
            Console.WriteLine("1. Autobús");
            Console.WriteLine("2. Camión");
            Console.Write("Seleccione una opción: ");
            string tipo = Console.ReadLine();

            Console.Write("Matrícula: ");
            string matricula = Console.ReadLine();

            Console.Write("Consumo (L/100km): ");
            double consumo = Convert.ToDouble(Console.ReadLine());

            if (tipo == "1")
            {
                Console.Write("Capacidad máxima: ");
                int capacidad = Convert.ToInt32(Console.ReadLine());
                flota.Add(new Autobus(matricula, consumo, capacidad));
                Console.WriteLine("Autobús registrado correctamente.");
            }
            else if (tipo == "2")
            {
                Console.Write("Peaje anual (€): ");
                double peaje = Convert.ToDouble(Console.ReadLine());
                flota.Add(new Camion(matricula, consumo, peaje));
                Console.WriteLine("Camión registrado correctamente.");
            }
            else
            {
                Console.WriteLine("Tipo de vehículo no válido.");
            }
        }

        static void VerCostos()
        {
            Console.WriteLine("\n=== Lista de Vehículos ===");
            if (flota.Count == 0)
            {
                Console.WriteLine("No hay vehículos registrados.");
                return;
            }

            foreach (var v in flota)
            {
                Console.WriteLine($"{v.ToString()} | Costo por Km: {v.CalcularCostoPorKm():0.00} €");
            }
        }

        static void CalcularCostoTotal()
        {
            if (flota.Count == 0)
            {
                Console.WriteLine("No hay vehículos registrados.");
                return;
            }

            double total = 0.0;
            foreach (var v in flota)
            {
                total += v.CalcularCostoPorKm() * 100000.0;
            }

            Console.WriteLine($"\nCosto total de la flota (100,000 km por vehículo): {total:0.00} €");
        }
    }
}
