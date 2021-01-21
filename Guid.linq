<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//Guid
	Guid guid = Guid.NewGuid();
	$"Guid: {guid.ToString()}".Dump();

	//空Guid
	Guid empty_guid = new Guid();
	if (empty_guid == Guid.Empty) "Guid is empty".Dump(empty_guid.ToString());
}

public static class Utils
{
	//扩展Guid
	public static bool IsNullOrEmpty(this Guid? guid)
	{
		if (guid.HasValue)
			if (guid == default(Guid))//注意:default(Guid) == Guid.Empty
				return true;
		return false;
	}
}


//empty_guid.IsNullOrEmpty().Dump();
