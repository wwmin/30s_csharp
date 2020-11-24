using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

public class CreateClaptrapInput
{
    [Required] [MinLength(4)] public string? Name { get; set; }
    [Required] public string? Address { get; set; }
    [Required] public int Age{get;set;}
}

var input = new CreateClaptrapInput();
Type t = input.GetType();

var stringProps = t.GetProperties().Where(x => x.PropertyType == typeof(string));

var requiredNameList = new List<string>();
var minLengthNameList = new List<string>();
foreach (var propertyInfo in stringProps)
{
    if (propertyInfo.GetCustomAttribute<RequiredAttribute>() != null)
    {
        requiredNameList.Add(propertyInfo.Name);
    }
    if (propertyInfo.GetCustomAttribute<MinLengthAttribute>() != null)
    {
        minLengthNameList.Add(propertyInfo.Name);
        var minLengthAttribute = propertyInfo.GetCustomAttribute<MinLengthAttribute>();
        minLengthAttribute!.Length.Dump();
    }
}

requiredNameList.Dump();
minLengthNameList.Dump();