using System;
using System.Collections.Generic;

namespace LogiTrack
{
    public abstract class Envio
    {
        public string Descripcion { get; set; }

        private double peso;
        public double Peso
        {
            get => peso;
            set => peso = value < 0 ? 0.0 : value;
        }

        public double CostoBase => 2.0 * Peso;

        public Envio(string descripcion, double peso)
        {
            Descripcion = descripcion;
            Peso = peso;
        }

        public abstract double CalcularCostoTotal();

        public override string ToString()
        {
            return $"Descripción: {Descripcion}, Peso: {Peso} kg, Costo Base: {CostoBase:C2}";
        }
    }

    public class PaqueteEstandar : Envio
    {
        private double tarifaPlana;
        public double TarifaPlana
        {
            get => tarifaPlana;
            set => tarifaPlana = value < 0 ? 0.0 : value;
        }

        public PaqueteEstandar(string descripcion, double peso, double tarifaPlana)
            : base(descripcion, peso)
        {
            TarifaPlana = tarifaPlana;
        }

        public override double CalcularCostoTotal()
        {
            return CostoBase + TarifaPlana;
        }

        public override string ToString()
        {
            return base.ToString() + $", Tarifa Plana: {TarifaPlana:C2}";
        }
    }

    public class PaqueteExpress : Envio
    {
        private double recargoUrgencia;
        public double RecargoUrgencia
        {
            get => recargoUrgencia;
            set => recargoUrgencia = value < 0 ? 0.0 : value;
        }

        public PaqueteExpress(string descripcion, double peso, double recargoUrgencia)
            : base(descripcion, peso)
        {
            RecargoUrgencia = recargoUrgencia;
        }

        public override double CalcularCostoTotal()
        {
            return CostoBase + (RecargoUrgencia * Peso);
        }

        public override string ToString()
        {
            return base.ToString() + $", Recargo Urgencia: {RecargoUrgencia:C2}/kg";
        }
    }

    class Program
    {
        static void Main()
        {
            List<Envio> envios = new List<Envio>();
            int opcion;

            do
            {
                Console.WriteLine("\n--- SISTEMA DE GESTIÓN DE ENVÍOS - LogiTrack S.A ---");
                Console.WriteLine("1. Crear Envío");
                Console.WriteLine("2. Ver Costos Individuales");
                Console.WriteLine("3. Calcular Ingreso Total");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");
                bool valido = int.TryParse(Console.ReadLine(), out opcion);

                if (!valido)
                {
                    Console.WriteLine("Opción inválida, intente de nuevo.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        CrearEnvio(envios);
                        break;
                    case 2:
                        VerCostos(envios);
                        break;
                    case 3:
                        CalcularIngresos(envios);
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

        static void CrearEnvio(List<Envio> envios)
        {
            Console.WriteLine("\nSeleccione el tipo de envío:");
            Console.WriteLine("1) Paquete Estándar");
            Console.WriteLine("2) Paquete Express");
            Console.Write("Opción: ");
            int tipo;
            if (!int.TryParse(Console.ReadLine(), out tipo))
            {
                Console.WriteLine("Entrada inválida.");
                return;
            }

            Console.Write("Descripción del paquete: ");
            string descripcion = Console.ReadLine();

            Console.Write("Peso (kg): ");
            double peso;
            if (!double.TryParse(Console.ReadLine(), out peso))
                peso = 0.0;

            if (tipo == 1)
            {
                Console.Write("Tarifa Plana (€): ");
                double tarifa;
                if (!double.TryParse(Console.ReadLine(), out tarifa))
                    tarifa = 0.0;

                envios.Add(new PaqueteEstandar(descripcion, peso, tarifa));
                Console.WriteLine("Paquete Estándar creado correctamente.");
            }
            else if (tipo == 2)
            {
                Console.Write("Recargo por Urgencia (€/kg): ");
                double recargo;
                if (!double.TryParse(Console.ReadLine(), out recargo))
                    recargo = 0.0;

                envios.Add(new PaqueteExpress(descripcion, peso, recargo));
                Console.WriteLine("Paquete Express creado correctamente.");
            }
            else
            {
                Console.WriteLine("Tipo no válido.");
            }
        }

        static void VerCostos(List<Envio> envios)
        {
            Console.WriteLine("\n--- COSTOS INDIVIDUALES DE ENVÍO ---");
            if (envios.Count == 0)
            {
                Console.WriteLine("No hay envíos registrados.");
                return;
            }

            foreach (var envio in envios)
            {
                Console.WriteLine($"{envio}\nCosto Total: {envio.CalcularCostoTotal():C2}\n");
            }
        }

        static void CalcularIngresos(List<Envio> envios)
        {
            double total = 0;
            foreach (var envio in envios)
                total += envio.CalcularCostoTotal();

            Console.WriteLine($"\nIngreso total por envíos: {total:C2}");
        }
    }
}
