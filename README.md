# Data generator and sanitizer
Library to facilitate generating random values and working with randomly generated values
* **Generators** are used to "generate" a random value based on certain parameters.
* **Sanitizers** are used to replace values consistently. Consider a requirement where you must replace all names in a database with randomly generated names. In order for the data to continue to make sense names must be replaced consistently. For example, if the name *Adam Conn* is replaced with *Santos Figeroa*, all occurrences of *Adam Conn* must be replaced with *Santos Figeroa*. A sanitizer is used to handle this.
* **Other types** are... well... other types that support generators and sanitizers and may be useful to you.

## Generator: `IpAddressGenerator`
This type generates a `System.Net.IPAddress` object with a random IP address within a specified range.

This generator does not keep track of the addresses it has generated. Therefore it is possible for duplicate addresses to be generated.

By default `IpAddressGenerator` will generate dynamic addresses within the following ranges (inclusive):

* 0.0.0.0 to 9.255.255.255
* 10.255.255.255 to 172.15.255.255
* 172.31.255.255 to 192.167.255.255
* 192.168.255.255 to 255.255.255.255

### Generating a random IP address
`IpAddressGenerator` can generate a random IP address using the default address ranges:

 ```
 var generator = new IpAddressGenerator();  
 var address = generator.NextValue();
 ```
 
### Generating a random IP address within a range

The following code demonstrates how to generate a random IP address within a specified address range inclusive (192.168.10.1 to 192.168.10.255):

```
var generator = new IpAddressGenerator();  
var start = IPAddress.Parse("192.168.10.1");  
var end = IPAddress.Parse("192.168.11.0");  
var range = new IpAddressRange(start, end);  
generator.AddValues(range);  
var address = generator.NextValue();  
```

## Generator: StringGenerator

This type retrieves a randomly selected string from an `IEnumerable<string>` object.

This generator does not keep track of the strings it selects. Therefore it is possible for a single value to be returned multiple times.

### Reading a randomly selected value from an array

The following code demonstrates how to use the `StringGenerator` to read a random value from a string array:

```
var places = new string[] {"Seattle", "Portland", "San Francisco", "Los Angeles", "San Diego"};  
var generator = new StringGenerator();  
var randomPlace = generator.NextValue();  
```

## Generator: CompoundStringGenerator

This type generates new string by randomly selecting strings from `IEnumerable<string>` objects and combining those strings into a new string based on formatting options that are specified.

This generator does not keep track of the strings it selects. Therefore it is possible for a single value to be returned multiple times.

### Generating a random full name

The following code demonstrates how to use the `CompoundStringGenerator` to generate a new name by combining a first name (given name) with a last name (surname):

```
var sources = new Dictionary<string, IEnumerable<string>>();  
sources["first"] = new string[] {"Ann", "Bob", "Catherine", "David"};  
sources["last"] = new string[] {"Johnson", "Smith", "Delaney"};  
var context = new CompoundStringGeneratorContext();  
context.Formats.Add("{first} {last}");  
var generator = new CompoundStringGenerator(context);  
generator.AddValues(sources.ToArray());  
var fullName = generator.NextValue();
```

### Generating a random full name, example 2

The following code demonstrates how to use the `CompoundStringGenerator` to generate a new name by combining a first name (given name) with a last name (surname) where male and female names are allowed different last names:

```
var sources = new Dictionary<string, IEnumerable<string>>();  
sources["first-man"] = new string[] {"Ann", "Catherine"};  
sources["first-woman"] = new string[] {"Bob", "David"};  
sources["last-man"] = new string[] {"Johnson", "Smith"};  
sources["last-woman"] = new string[] { "Roberts", "Delaney" };  
var context = new CompoundStringGeneratorContext();  
context.Formats.Add("{first-man} {last-man}");  
context.Formats.Add("{first-woman} {last-woman}");  
var generator = new CompoundStringGenerator(context);  
generator.AddValues(sources.ToArray());  
var fullName = generator.NextValue();  
```

### Formatting values to generate an email address

The following code demonstrates how to use the `ComponentStringGenerator` to generate an email address by combining the first letter of the first name (given name) with the last name (surname) and making all characters lower-case:

```
var context = new CompoundStringGeneratorContext();
context.Formats.Add("{first}{last}@email.com");
context.ValueFormatters.Add("first", s => s.Substring(0, 1).ToLower());
context.ValueFormatters.Add("last", s => s.ToLower());
var generator = new CompoundStringGenerator(context);
var sources = new Dictionary<string, IEnumerable<string>>();
sources["first"] = new string[] { "Ann", "Catherine" };
sources["last"] = new string[] { "Johnson", "Smith" }; 
generator.AddValues(sources.ToArray());
var emailAddress = generator.NextValue();
```

## Sanitizer: UniqueValueSanitizer

This type, given an object, retrieves a randomly selected object of the same type.

### Sanitizing a list of names

The following code demonstrates how to use the `UniqueValueSanitizer` to sanitize a list of names:

```
var namesNew = new string[] { "Seattle", "Portland", "San Francisco" };  
var generator = new StringGenerator();  
generator.AddValues(namesNew);  
var sanitizer = new UniqueValueSanitizer<string, string>(generator);  
var namesOriginal = new string[] { "Los Angeles", "New York", "Los Angeles" };  
var namesSanitized = new string[3];  
for (var i = 0; i < namesOriginal.Length; i++)  
{  
    namesSanitized[i] = sanitizer.Sanitize(namesOriginal[i]);  
}  
//namesSanitized[0] will be one of the values in the namesNew array  
//namesSanitized[1] will be one of the values in the namesNew array  
//namesSanitized[0] != namesSanitized[1]  
//namesSanitized[0] == namesSanitized[2]
```

### Sanitizing a list of IP addresses

The following code demonstrates how to use the `UniqueValueSanitizer` to sanitize a collection of IP addresses:

```
var start = IPAddress.Parse("192.168.10.1");  
var end = IPAddress.Parse("192.168.10.100");  
var range = new IpAddressRange(start, end);  
var generator = new IpAddressGenerator();  
generator.AddValues(range);  
var sanitizer = new UniqueValueSanitizer(generator);  
var original = new IPAddress[]  
{  
    IPAddress.Parse("50.50.50.50"),  
    IPAddress.Parse("100.100.100.100"),  
    IPAddress.Parse("200.200.200.200")  
};  
var sanitized = new IPAddress[3];  
for (var i = 0; i < original.Length; i++)  
{  
    sanitized[i] = sanitizer.Sanitize(original[i]);  
}  
//sanitized[0] will be within the range 192.168.10.1 to 192.168.10.99  
//sanitized[1] will be within the range 192.168.10.1 to 192.168.10.99  
//sanitized[2] will be within the range 192.168.10.1 to 192.168.10.99  
//sanitized[0] != namesSanitized[1] != namesSanitized[2]
```

### Limiting which values get sanitized

The following code demonstrates how to use the `UniqueValueSanitizer` to sanitize a list of names, except the name *Sitecore* will not be changed:

```
var namesNew = new string[] { "Company Inc.", "Business Brothers" };  
var generator = new StringGenerator();  
generator.AddValues(namesNew);  
var sanitizer = new UniqueValueSanitizer<string, string>(generator);  
sanitizer.ShouldSanitize = (name => name != "Sitecore");  
var namesOriginal = new string[] { "Internet Ventures", "Sitecore", "Cash Limited" };  
var namesSanitized = new string[3];  
for (var i = 0; i < namesOriginal.Length; i++)  
{  
    namesSanitized[i] = sanitizer.Sanitize(namesOriginal[i]);  
}  
//namesSanitized[0] will be one of the values in the namesNew array  
//namesSanitized[1] will be "Sitecore"  
//namesSanitized[2] will be one of the values in the namesNew array  
//namesSanitized[0] != namesSanitized[1] != namesSanitized[2]  
```

## Sanitizer: LocationSanitizer

This type sanitizes `Location` objects using `IMatcher` objects.

### Getting the national capital for a specified city

The following code demonstrates how a sanitizer, given any location in a country, will return the location of the capital:

```
var capitals = new Location[]  
{  
    new Location() {Country = "US", City = "Washington"},  
    new Location() {Country = "CA", City = "Ottawa"}  
};  
var matcher = new SanitizerMatcher<Location>  
    (  
        loc => true,  
        (loc, locs) => locs.FirstOrDefault(loc2 => loc.Country == loc2.Country)  
    );  
var sanitizer = new LocationSanitizer();  
sanitizer.DataSources.Add(matcher, capitals);  
var locUs = sanitizer.Sanitize(new Location() { Country = "US", City = "New York" });  
var locCa = sanitizer.Sanitize(new Location() { Country = "CA", City = "Toronto" });  
var locAu = sanitizer.Sanitize(new Location() { Country = "AU", City = "Sydney" });  
//locUs.City == "Washington"  
//locCa.City == "Ottawa"  
//locaAu == null  
```

### Getting the state capital for a specified US city and a national capital for other cities

The following code demonstrates how a sanitizer, given a location in the US with a state specified, will return the location of the state capital, otherwise it will return the location of the national capital:

```
var stateCapitals = new Location[]  
{  
    new Location() {Country = "US", Region = "OR", City = "Salem"},  
    new Location() {Country = "US", Region = "WA", City = "Olympia"}  
};  
var capitals = new Location[]  
{  
    new Location() {Country = "US", City = "Washington"},  
    new Location() {Country = "CA", City = "Ottawa"}  
};  
var stateMatcher = new SanitizerMatcher<Location>  
    (loc => loc.Country == "US" && !string.IsNullOrEmpty(loc.Region), //only match if US and a state is specified  
    (loc, locs) => locs.FirstOrDefault(loc2 => loc.Country == loc2.Country && loc.Region == loc2.Region)); //return the first city with the same country and state  
var nationalMatcher = new SanitizerMatcher<Location>  
    (loc => true, //match any location  
    (loc, locs) => locs.FirstOrDefault(loc2 => loc.Country == loc2.Country)); //return the first city with the same country  
var sanitizer = new LocationSanitizer();  
sanitizer.DataSources.Add(stateMatcher, stateCapitals);  
sanitizer.DataSources.Add(nationalMatcher, capitals);  
var locUs1 = sanitizer.Sanitize(new Location() {Country = "US", Region = "OR", City = "Portland"});  
var locUs2 = sanitizer.Sanitize(new Location() {Country = "US", City = "New York"});  
var locCa = sanitizer.Sanitize(new Location() {Country = "CA", City = "Toronto"});  
//locUs1.City == "Salem"  
//locUs2.City == "Washington"  
//locCa.City == "Ottawa"  
```
 
## Other types: DelimitedFileReader

This type makes it easier to read text data from character-delimited files such as comma-separated and tab-separated files.

### Parsing a comma-delimited file

Consider the following text file:

```
capital,state  
Olympia,Washington  
Salem,Oregon  
```

The following code demonstrates how to parse the file:

```
var filePath = @"C:\path\to\file.csv";  
var delimiter = ',';  
using (var reader = new DelimitedFileReader(filePath, delimiter))  
{  
    var columns = reader.ColumnPositions;  //populated from the first line in the file
    //columns[0] == "capital"  
    //columns[1] == "state"  
    var values = reader.ReadValues();  
    //values["capital"] == "Olympia"  
    //values["state"] == "Washington"  
    values = reader.ReadValues();  
    //values["capital"] == "Salem"  
    //values["state"] == "Oregon"  
    values = reader.ReadValue();  
    //values == null  
}
```
