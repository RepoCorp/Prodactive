using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite.Net.Attributes;

namespace ProdactiveMovil.ModelServicio.SQLite
{
    public class PersonaSql
    {
        public PersonaSql(Persona p)
        {
            Id              = p.Id;
            Type            = p.Type;
            Nombre          = p.Nombre;
            Apellido        = p.Apellido;
            Identificacion  = p.Identificacion;
            FechaNacimiento = p.FechaNacimiento;
            Sexo            = p.Sexo;
            Peso            = p.Peso;
            Estatura        = p.Estatura;
        }

        public PersonaSql()
        {
        }
        [PrimaryKey]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Int64 Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public Double Peso { get; set; }
        public Double Estatura { get; set; }
    }


    public class ReporteSql
    {
        public ReporteSql()
        {
            Reportado = false;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public Int64 Pasos { get; set; }
        public double Calorias { get; set; }
        public DateTime Fecha { get; set; }
        public string IdReto { get; set; }
        public bool Reportado { get; set; }
    }
}