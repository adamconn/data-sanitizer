using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Text
{
    public class CompoundValuesGenerator : BaseDictionaryGenerator<KeyValuePair<string, IEnumerable<string>>, string>
    {
        public CompoundValuesGenerator(CompoundValuesGeneratorContext context)
        {
            this.Context = context;
            this.Generators = new Dictionary<string, StringGenerator>();
        }
        protected CompoundValuesGeneratorContext Context { get; private set; }
        protected IDictionary<string, StringGenerator> Generators { get; private set; }
        public virtual void AddValues(params KeyValuePair<string, IEnumerable<string>>[] values)
        {
            if (values == null || !values.Any())
            {
                return;
            }
            foreach (var value in values)
            {
                if (this.Generators.ContainsKey(value.Key))
                {
                    var generator = this.Generators[value.Key];
                    generator.AddValues(value.Value.ToArray());
                }
                else
                {
                    var generator = new StringGenerator();
                    generator.AddValues(value.Value.ToArray());
                    this.Generators.Add(value.Key, generator);
                }
            }
        }

        public override void RemoveValues(params KeyValuePair<string, IEnumerable<string>>[] values)
        {
            if (values == null || !values.Any())
            {
                return;
            }
            foreach (var value in values)
            {
                if (! this.Generators.ContainsKey(value.Key))
                {
                    continue;
                }
                var generator = this.Generators[value.Key];
                generator.RemoveValues(value.Value.ToArray());
            }
        }

        public override IDictionary<string, string> NextValue()
        {
            if (this.Context == null || this.Context.Formats == null || !this.Context.Formats.Any())
            {
                throw new Exception("No formatting has been specified in the context.");
            }
            var values = new Dictionary<string, string>();
            foreach (var pair in this.Generators)
            {
                values.Add(pair.Key, pair.Value.NextValue());
            }
            var formattedValues = new Dictionary<string, string>();
            foreach (var pair in this.Context.Formats)
            {
                formattedValues.Add(pair.Key, pair.Value(values));
            }
            return formattedValues;
        }
    }
    //public class CompoundValuesGenerator : BaseGenerator<KeyValuePair<string, IEnumerable<string>>, IDictionary<string, string>>
    //{
    //    public CompoundValuesGenerator(CompoundValuesGeneratorContext context)
    //    {
    //        this.Context = context;
    //    }
    //    protected CompoundValuesGeneratorContext Context { get; private set; }
    //    protected virtual string GetRandomValue(IEnumerable<string> values)
    //    {
    //        if (values == null || !values.Any())
    //        {
    //            return null;
    //        }
    //        var position = this.Random.Next(0, values.Count());
    //        return values.Skip(position).FirstOrDefault();
    //    }

    //    protected virtual IDictionary<string, string> GetRandomValues(IEnumerable<KeyValuePair<string, IEnumerable<string>>> values)
    //    {
    //        var randomValues = new Dictionary<string, string>();
    //        foreach (var pair in values)
    //        {
    //            var randomValue = GetRandomValue(pair.Value);
    //            if (randomValue == null)
    //            {
    //                randomValue = string.Empty;
    //            }
    //            if (randomValues.ContainsKey(pair.Key))
    //            {
    //                randomValues.Remove(pair.Key);
    //            }
    //            randomValues.Add(pair.Key, randomValue);
    //        }
    //        return randomValues;
    //    }
    //    public override IDictionary<string, string> NextValue()
    //    {
    //        if (this.Context == null || this.Context.Formats == null || !this.Context.Formats.Any())
    //        {
    //            throw new Exception("No formatting has been specified in the context.");
    //        }
    //        //
    //        var formattedValues = new Dictionary<string, string>();
    //        var randomValues = GetRandomValues(this.Values);
    //        foreach (var pair in this.Context.Formats)
    //        {
    //            var s = pair.Value(randomValues);
    //            if (s == null)
    //            {
    //                s = string.Empty;
    //            }
    //            if (formattedValues.ContainsKey(pair.Key))
    //            {
    //                formattedValues.Remove(pair.Key);
    //            }
    //            formattedValues.Add(pair.Key, s);
    //        }
    //        return formattedValues;
    //    }
    //}
}
