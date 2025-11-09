using System;
using System.Collections.Generic;

namespace TechSolutions.HRSystem
{
    public abstract class Empleado
    {
        public string Nombre { get; set; }

        private double salarioBase;
        public double SalarioBase
        {
            get => salarioBase;
            set => salarioBase = value < 0 ? 0.0 : value;
        }

        public Empleado(string nombre, double salarioBase)
        {
            Nombre = nombre;
            SalarioBase = salarioBase;
        }

        public abstract double CalcularNomina();

        public override string ToString()
        {
            return $"Empleado: {Nombre}, Salario Base: {SalarioBase:C2}";
        }
    }

    public class EmpleadoFijo : Empleado
    {
        private double bonoAnual;
        public double BonoAnual
        {
            get => bonoAnual;
            set => bonoAnual = value < 0 ? 0.0 : value;
        }

        public EmpleadoFijo(string nombre, double salarioBase, double bonoAnual)
            : base(nombre, salarioBase)
        {
            BonoAnual = bonoAnual;
        }

        public override double CalcularNomina()
        {
            return SalarioBase + (BonoAnual / 12);
        }

        public override string ToString()
        {
            return base.ToString() + $", Bono Anual: {BonoAnual:C2}";
        }
    }

    public class EmpleadoPorHora : Empleado
    {
        private double tarifaHora;
        private double horasTrabajadasMes;

        public double TarifaHora
        {
            get => tarifaHora;
            set => tarifaHora = value < 0 ? 0.0 : value;
        }

        public double HorasTrabajadasMes
        {
            get => horasTrabajadasMes;
            set => horasTrabajadasMes = value < 0 ? 0.0 : value;
        }

        public EmpleadoPorHora(string nombre, double salarioBase, double tarifaHora, double horasTrabajadasMes)
            : base(nombre, salarioBase)
        {
            TarifaHora = tarifaHora;
            HorasTrabajadasMes = horasTrabajadasMes;
        }

        public override double CalcularNomina()
        {
            return SalarioBase + (TarifaHora * HorasTrabajadasMes);
        }

        public override string ToString()
        {
            return base.ToString() + $", Tarifa por Hora: {TarifaHora:C2}, Horas Trabajadas: {HorasTrabajadasMes}";
        }
    }

    class Program
    {
        static void Main()
        {
            List<Empleado> empleados = new List<Empleado>();
            int opcion;

            do
            {
                Console.WriteLine("\n--- SISTEMA DE GESTIÓN DE PERSONAL ---");
                Console.WriteLine("1. Contratar Empleado");
                Console.WriteLine("2. Ver Nóminas Individuales");
                Console.WriteLine("3. Calcular Coste Total de Nóminas");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");
                bool entradaValida = int.TryParse(Console.ReadLine(), out opcion);

                if (!entradaValida)
                {
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        ContratarEmpleado(empleados);
                        break;
                    case 2:
                        VerNominas(empleados);
                        break;
                    case 3:
                        CalcularCosteTotal(empleados);
                        break;
                    case 4:
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }

            } while (opcion != 4);
        }

        static void ContratarEmpleado(List<Empleado> empleados)
        {
            Console.WriteLine("\nTipo de empleado: 1) Fijo  2) Por Hora");
            int tipo;
            if (!int.TryParse(Console.ReadLine(), out tipo))
            {
                Console.WriteLine("Entrada inválida.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Salario Base: ");
            double salario;
            if (!double.TryParse(Console.ReadLine(), out salario))
                salario = 0.0;

            if (tipo == 1)
            {
                Console.Write("Bono Anual: ");
                double bono;
                if (!double.TryParse(Console.ReadLine(), out bono))
                    bono = 0.0;

                empleados.Add(new EmpleadoFijo(nombre, salario, bono));
                Console.WriteLine("Empleado fijo contratado correctamente.");
            }
            else if (tipo == 2)
            {
                Console.Write("Tarifa por Hora: ");
                double tarifa;
                if (!double.TryParse(Console.ReadLine(), out tarifa))
                    tarifa = 0.0;

                Console.Write("Horas Trabajadas: ");
                double horas;
                if (!double.TryParse(Console.ReadLine(), out horas))
                    horas = 0.0;

                empleados.Add(new EmpleadoPorHora(nombre, salario, tarifa, horas));
                Console.WriteLine("Empleado por hora contratado correctamente.");
            }
            else
            {
                Console.WriteLine("Tipo de empleado no válido.");
            }
        }

        static void VerNominas(List<Empleado> empleados)
        {
            Console.WriteLine("\n--- NÓMINAS INDIVIDUALES ---");
            if (empleados.Count == 0)
            {
                Console.WriteLine("No hay empleados registrados.");
                return;
            }

            foreach (var e in empleados)
            {
                Console.WriteLine($"{e}\nNómina Mensual: {e.CalcularNomina():C2}\n");
            }
        }

        static void CalcularCosteTotal(List<Empleado> empleados)
        {
            double total = 0;
            foreach (var e in empleados)
                total += e.CalcularNomina();

            Console.WriteLine($"\nCoste total mensual de nóminas: {total:C2}");
        }
    }
}
