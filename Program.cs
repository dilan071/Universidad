using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Universidad
{
    public List<Estudiante> estudiantes { get; set; }
    public List<Profesor> profesores { get; set; }
    public List<Curso> cursos { get; set; }

    public Universidad()
    {
        estudiantes = new List<Estudiante>();
        profesores = new List<Profesor>();
        cursos = new List<Curso>();
    }

    public void crearEstudiante(string nombre, string correo)
    {
        estudiantes.Add(new Estudiante(nombre, correo));
    }

    public void crearProfesor(string nombre, string correo)
    {
        profesores.Add(new Profesor(nombre, correo));
    }

    public void crearCurso(string nombre)
    {
        cursos.Add(new Curso(nombre));
    }

    public void editarEstudiante(string nombre, string nuevoNombre, string nuevoCorreo)
    {
        Estudiante estudiante = estudiantes.Find(e => e.nombre == nombre);
        estudiante.nombre = nuevoNombre;
        estudiante.correo = nuevoCorreo;
    }

    public void editarProfesor(string nombre, string nuevoNombre, string nuevoCorreo)
    {
        Profesor profesor = profesores.Find(p => p.nombre == nombre);
        profesor.nombre = nuevoNombre;
        profesor.correo = nuevoCorreo;
    }

    public void editarCurso(string nombre, string nuevoNombre)
    {
        Curso curso = cursos.Find(c => c.nombre == nombre);
        curso.nombre = nuevoNombre;
    }

    public void eliminarEstudiante(string nombre)
    {
        estudiantes.RemoveAll(e => e.nombre == nombre);
    }

    public void eliminarProfesor(string nombre)
    {
        profesores.RemoveAll(p => p.nombre == nombre);
    }

    public void eliminarCurso(string nombre)
    {
        cursos.RemoveAll(c => c.nombre == nombre);
    }
}
class GestorDeCursos : Universidad
{
    public void matricularEstudiante(string nombreEstudiante, string nombreCurso)
    {
        Estudiante estudiante = estudiantes.Find(e => e.nombre == nombreEstudiante);
        Curso curso = cursos.Find(c => c.nombre == nombreCurso);
        if (estudiante == null)
        {
            throw new EstudianteNoEncontradoException("El estudiante no existe");
        }
        if (curso == null)
        {
            throw new CursoNoEncontradoException("El curso no existe");
        }
        if (curso.estudiantes.Count >= curso.capacidadMaxima)
        {
            throw new CursoLlenoException("El curso está lleno");
        }
        if (estudiante.cursos.Contains(curso))
        {
            throw new EstudianteDuplicadoException("El estudiante ya está matriculado en este curso");
        }
        curso.estudiantes.Add(estudiante);
        estudiante.cursos.Add(curso);
    }
    public void asignarCursoAProfesor(string nombreProfesor, string nombreCurso)
    {
        Profesor profesor = profesores.Find(p => p.nombre == nombreProfesor);
        Curso curso = cursos.Find(c => c.nombre == nombreCurso);
        if (profesor == null)
        {
            throw new ProfesorNoEncontradoException("El profesor no existe");
        }
        if (curso == null)
        {
            throw new CursoNoEncontradoException("El curso no existe");
        }
        profesor.cursos.Add(curso);
        curso.profesor = profesor;
    }
}
class Estudiante
{
    public string nombre { get; set; }
    public string correo { get; set; }
    public List<Curso> cursos { get; set; }
    public Estudiante(string nombre, string correo)
    {
        this.nombre = nombre;
        this.correo = correo;
        cursos = new List<Curso>();
    }
}

class Profesor
{
    public string nombre { get; set; }
    public string correo { get; set; }
    public List<Curso> cursos { get; set; }
    public Profesor(string nombre, string correo)
    {
        this.nombre = nombre;
        this.correo = correo;
        cursos = new List<Curso>();
    }

    public void generarListaEstudiantes(string nombreCurso, string orden)
    {
        Curso curso = cursos.Find(c => c.nombre == nombreCurso);
        if (curso == null)
        {
            throw new CursoNoEncontradoException("El curso no existe");
        }
        List<Estudiante> estudiantesEnCurso = curso.estudiantes;
        if (orden == "alfabetico")
        {
            estudiantesEnCurso = estudiantesEnCurso.OrderBy(e => e.nombre).ToList();
        }
        else if (orden == "tamano")
        {
            estudiantesEnCurso = estudiantesEnCurso.OrderBy(e => e.correo.Length).ToList();
        }
        using (StreamWriter sw = new StreamWriter($"{curso.nombre}.txt"))
        {
            foreach (Estudiante estudiante in estudiantesEnCurso)
            {
                sw.WriteLine(estudiante.nombre);
            }
        }
    }
}

class Curso
{
    public string nombre { get; set; }
    public List<Estudiante> estudiantes { get; set; }
    public Profesor profesor { get; set; }
    public int capacidadMaxima { get; set; } = 20; public Curso(string nombre)
    {
        this.nombre = nombre;
        estudiantes = new List<Estudiante>();
    }
}

class EstudianteNoEncontradoException : Exception
{
    public EstudianteNoEncontradoException(string message) : base(message) { }
}

class CursoLlenoException : Exception
{
    public CursoLlenoException(string message) : base(message) { }
}

class EstudianteDuplicadoException : Exception
{
    public EstudianteDuplicadoException(string message) : base(message) { }
}

class CursoNoEncontradoException : Exception
{
    public CursoNoEncontradoException(string message) : base(message) { }
}

class ProfesorNoEncontradoException : Exception
{
    public ProfesorNoEncontradoException(string message) : base(message) { }
}
/*
static void Main(string[] args) 
{
    Universidad uni = new Universidad();
    uni.crearEstudiante("juan", "juna@gmail.c")

}
*/