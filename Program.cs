using System;
using System.Collections.Generic;

// 1. EL PROTOTIPO ABSTRACTO
// Implementamos ICloneable para estandarizar la clonación en .NET
public abstract class Examen : ICloneable
{
    // Datos protegidos
     protected string claveMateria;
    protected string nombreAsignatura;
   
     protected string docente;
     protected string salon;
    protected string grupo;

    public Examen(string claveMateria, string nombreAsignatura, string docente, string salon)
    {
        this.claveMateria = claveMateria;
        this.nombreAsignatura = nombreAsignatura;
        this.docente = docente;
        this.salon = salon;
    }

    // Setters para los datos que varían al clonar
    public void SetGrupo(string grupo) { this.grupo = grupo; }
     public void SetDocente(string docente) { this.docente = docente; }
    public void SetSalon(string salon) { this.salon = salon; }

    // Método de clonación propio de C# (Shallow Copy)
    public object Clone()
    {
        return this.MemberwiseClone(); 
    }

    public abstract void MostrarExamen();
}

// 2. PROTOTIPOS CONCRETOS
public class ExamenTeorico : Examen
{
    protected int cantidadPreguntas;

    // Usamos 'base' en lugar de 'super' como se hace en Java
    public ExamenTeorico(string clave, string nombre, string docente, string salon, int preguntas)
           : base(clave, nombre, docente, salon)
    {
        this.cantidadPreguntas = preguntas;
    }

    public override void MostrarExamen()
    {
        // Uso de interpolación de strings para mayor legibilidad
        Console.WriteLine($"[TEÓRICO] {claveMateria} - {nombreAsignatura} | Docente: {docente} | Grupo: {grupo} | Salón: {salon} | Preguntas: {cantidadPreguntas}");
    }
}

public class ExamenPractico : Examen
{
     
       protected string softwareRequerido;
  
    public ExamenPractico(string clave, string nombre, string docente, string salon, string software)
        : base(clave, nombre, docente, salon)
    {
        this.softwareRequerido = software;
    }

    public override void MostrarExamen()
    {
        Console.WriteLine($"[PRÁCTICO] {claveMateria} - {nombreAsignatura} | Docente: {docente} | Grupo: {grupo} | Salón: {salon} | Software: {softwareRequerido}");
    }
}

// 3. REGISTRO DE PROTOTIPOS (Gestor)
public class GestorExamenes
{
    // En C# usamos Dictionary en lugar de HashMap
    private Dictionary<string, Examen> catalogoExamenes = new Dictionary<string, Examen>();

    public GestorExamenes()
    {
        CargarPrototiposBase();
    }

    private void CargarPrototiposBase()
    {
         catalogoExamenes.Add("Patrones", new ExamenPractico("SCD-1015", "Patrones de Diseño", "Ing. Pérez", "Lab 1", "Visual Studio / C#"));
        catalogoExamenes.Add("BasesDatos", new ExamenPractico("AEB-1011", "Bases de Datos", "Dra. Gómez", "Lab 2", "SQL Server"));
        catalogoExamenes.Add("Redes", new ExamenPractico("SCD-1021", "Redes de Computadoras", "Ing. López", "Lab Cisco", "Packet Tracer"));
        catalogoExamenes.Add("SistemasOperativos", new ExamenTeorico("AEC-1061", "Sistemas Operativos", "Mtro. Ramírez", "Aula A1", 50));
        catalogoExamenes.Add("IA", new ExamenPractico("SCC-1013", "Inteligencia Artificial", "Dra. Gómez", "Lab 3", "Python / ML.NET"));
        catalogoExamenes.Add("Calculo", new ExamenTeorico("ACF-0904", "Cálculo Vectorial", "Ing. Martínez", "Aula B2", 15));
        catalogoExamenes.Add("Fisica", new ExamenTeorico("ACF-0902", "Física General", "Mtro. Ramírez", "Aula C3", 20));
         catalogoExamenes.Add("Estructuras", new ExamenPractico("AED-1026", "Estructura de Datos", "Ing. Pérez", "Lab 1", "C++ Compiler"));
    }

    public Examen ObtenerExamen(string claveBase)
    {
         if (catalogoExamenes.TryGetValue(claveBase, out Examen prototipo))
        {
            // Casteamos el object devuelto por Clone() a Examen
            return (Examen)prototipo.Clone();
        }
        return null;
    }
}

// 4. CLIENTE (Punto de entrada)
class Program
{
    static void Main(string[] args)
    {
        GestorExamenes gestor = new GestorExamenes();
        Console.WriteLine("--- GENERACIÓN DE EXÁMENES MEDIANTE CLONACIÓN (C#) ---\n");

        // 1. Mismo docente, misma materia, diferente grupo
        Examen examenPD_GrupoA = gestor.ObtenerExamen("Patrones");
        examenPD_GrupoA.SetGrupo("7A");
     
        examenPD_GrupoA.MostrarExamen();

        Examen examenPD_GrupoB = gestor.ObtenerExamen("Patrones");
        examenPD_GrupoB.SetGrupo("7B");
        examenPD_GrupoB.SetSalon("Lab 2"); // Cambio de salón para el grupo B
        examenPD_GrupoB.MostrarExamen();

        Console.WriteLine();

        // 2. Misma materia, DIFERENTE docente
        Examen examenRedes_Prof1 = gestor.ObtenerExamen("Redes");
        examenRedes_Prof1.SetGrupo("6A");
        examenRedes_Prof1.MostrarExamen();

        Examen examenRedes_Prof2 = gestor.ObtenerExamen("Redes");
      
        examenRedes_Prof2.SetGrupo("6B");
        examenRedes_Prof2.SetDocente("Ing. Ruiz"); // El Ing. Ruiz usa el examen base pero le pone su nombre
        examenRedes_Prof2.SetSalon("Aula E4");
         examenRedes_Prof2.MostrarExamen();

        Console.WriteLine();

        // 3. Resto de materias requeridas
        Examen exSO = gestor.ObtenerExamen("SistemasOperativos"); exSO.SetGrupo("5A"); exSO.MostrarExamen();
         Examen exIA = gestor.ObtenerExamen("IA"); exIA.SetGrupo("8A"); exIA.MostrarExamen();
        Examen exCalculo = gestor.ObtenerExamen("Calculo"); exCalculo.SetGrupo("2B"); exCalculo.MostrarExamen();
         Examen exFisica = gestor.ObtenerExamen("Fisica"); exFisica.SetGrupo("3C"); exFisica.MostrarExamen();
        Examen exED = gestor.ObtenerExamen("Estructuras"); exED.SetGrupo("4A"); exED.MostrarExamen();
        Examen exBD = gestor.ObtenerExamen("BasesDatos"); exBD.SetGrupo("5C"); exBD.MostrarExamen();
        
        Console.ReadLine();

        
    }
}
