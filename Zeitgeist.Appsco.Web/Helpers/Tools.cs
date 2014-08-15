using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zeitgeist.Appsco.Web.Helpers
{
    public class ResultList
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public static class Tools
    {
        
        
        public static SelectList GetSelectList<T>(List<T> listado, Func<T, ResultList> func)
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            foreach (var a in listado)
            {
                ResultList v = func(a);
                lst.Add(new SelectListItem() { Text = v.Name, Value = v.Value });
            }
            SelectList sl = new SelectList(lst, "Value", "Text");
            return sl;
        }

    }
}