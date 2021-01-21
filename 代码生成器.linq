<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	var p1 = new Person(1, "wwmin");
	var p2 = new Person(1, "wwmin");
	(p1 == p2).Dump();
}

public sealed class Person : IEquatable<Person?>
{
    public Person(uint age, string name) => (this.Age, this.Name) = (age, name);
    public uint Age { get; }
    public string Name { get; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Person);
    }

    public bool Equals(Person? other)
    {
        return other != null &&
               Age == other.Age &&
               Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Age, Name);
    }

    public static bool operator ==(Person? left, Person? right)
    {
        return EqualityComparer<Person>.Default.Equals(left, right);
    }

    public static bool operator !=(Person? left, Person? right)
    {
        return !(left == right);
    }
}

//示例2
