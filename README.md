@@ -108,6 +108,30 @@ sources["last"] = new string[] { "Johnson", "Smith" };
generator.AddValues(sources.ToArray());
var emailAddress = generator.NextValue();
```
## Generator: CompoundValuesGenerator

This type generates a collection of new strinsg by randomly selecting strings from `IEnumerable<string>` objects and combining those strings into a new string based on formatting options that are specified.

This generator does not keep track of the strings it selects. Therefore it is possible for a single value to be returned multiple times.

### Generating a random email address

The following code demonstrates how to use the `CompoundValuesGenerator` to generate a new name by combining a first name (given name) with a last name (surname) with a domain:

```
var context = new CompoundValuesGeneratorContext();
context.Formats.Add("email", values =>
{
    return string.Format("{0}.{1}@{2}", values["first"].ToLower(), values["last"].ToLower(), values["domain"]);
});
var sourcesFirst = new KeyValuePair<string, IEnumerable<string>>("first", new string[] { "Ann" });
var sourcesLast = new KeyValuePair<string, IEnumerable<string>>("last", new string[] { "Johnson" });
var sourcesDomain = new KeyValuePair<string, IEnumerable<string>>("domain", new string[] { "gmail.com" });
var generator = new CompoundValuesGenerator(context);
generator.AddValues(sourcesFirst, sourcesLast, sourcesDomain);
var formattedValues = generator.NextValue();
//formattedValues["email"] == "ann.johnson@gmail.com"
```

## Sanitizer: UniqueValueSanitizer

@@ -126,7 +150,7 @@ var namesOriginal = new string[] { "Los Angeles", "New York", "Los Angeles" };
var namesSanitized = new string[3];  
for (var i = 0; i < namesOriginal.Length; i++)  
{  
    namesSanitized[i] = sanitizer.Sanitize(namesOriginal[i]);  
    namesSanitized[i] = sanitizer.Sanitize(namesOriginal[i], null);  
}  
//namesSanitized[0] will be one of the values in the namesNew array  
//namesSanitized[1] will be one of the values in the namesNew array  
@@ -210,7 +234,7 @@ var locCa = sanitizer.Sanitize(new Location() { Country = "CA", City = "Toronto"
var locAu = sanitizer.Sanitize(new Location() { Country = "AU", City = "Sydney" });  
//locUs.City == "Washington"  
//locCa.City == "Ottawa"  
//locaAu == null  
//locaAu.City == "Sydney"
```

### Getting the state capital for a specified US city and a national capital for other cities