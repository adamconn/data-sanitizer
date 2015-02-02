using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Readers
{
    public class DelimitedFileReader : StreamReader
    {
        public DelimitedFileReader(string path, params char[] delimiters)
            : base(File.OpenRead(path))
        {
            this.Path = path;
            if (delimiters == null || delimiters.Length == 0)
            {
                throw new ArgumentNullException("delimiters");
            }
            this.Delimiters = delimiters;
            this.Init();
        }
        public string[] ColumnPositions { get; private set; }
        public string Path { get; private set; }
        public char[] Delimiters { get; private set; }

        private void Init()
        {
            var line = this.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                var parts = line.Split(this.Delimiters);
                this.ColumnPositions = new string[parts.Length];
                for (var i = 0; i < parts.Length; i++)
                {
                    this.ColumnPositions[i] = parts[i].Trim();
                }
            }
        }

        public virtual IDictionary<string, string> ReadValues()
        {
            var line = this.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }
            var values = new Dictionary<string, string>();
            var parts = line.Split(this.Delimiters);
            for (var i = 0; i < this.ColumnPositions.Length; i++)
            {
                string value = null;
                if (parts.Length > i)
                {
                    value = parts[i].Trim();
                }
                values.Add(this.ColumnPositions[i], value);
            }
            return values;
        }

        //public virtual IEnumerable<IDictionary<string, string>> ReadAllValues()
        //{
        //    var allValues = new List<IDictionary<string, string>>();
        //    var values = ReadValues();
        //    while (values != null)
        //    {
        //        allValues.Add(values);
        //        values = ReadValues();
        //    }
        //    return allValues;
        //}
    }
}
