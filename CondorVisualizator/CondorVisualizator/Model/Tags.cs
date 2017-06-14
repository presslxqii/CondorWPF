using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CondorVisualizator.Model
{
    public class Tags
    {
        public struct Str
        {
            public string Name;
            public string Value;
        }

        public List<Str> ListTags;
        public Tags()
        {
          
                        ListTags = new List<Str>();
            for (var i = 0;
                i < File.ReadAllLines(
                    @"C:\Users\Администратор\Documents\Visual Studio 2013\Projects\CondorVisualizator\Tag.ini").Length;
                i++)
            {
                var s = File.ReadAllLines(
                    @"C:\Users\Администратор\Documents\Visual Studio 2013\Projects\CondorVisualizator\Tag.ini")[i].Replace(" ","");
                if (!s.TrimStart(' ').StartsWith("<"))
                    ListTags.Add(new Str {Name = s.Split('=')[0], Value = s.Split('=')[1]});
            }
        

        }
        public List<Str> GetMassurm()
        {
            return ListTags;
        }
    }

}
