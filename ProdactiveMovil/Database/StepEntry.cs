using System;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProdactiveMovil.Database
{
    public class StepEntry
    {
        public StepEntry()
        {
        }

        public int ID { get; set; }

        public Int64 Steps { get; set; }

        public DateTime Date { get; set; }
    }
}