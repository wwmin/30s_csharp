using Day = System.DayOfWeek;
//索引器
//示例1
public class BaseballTeam
{
    private readonly string[] players = new string[3];
    private readonly List<string> positionAbbreviations = new List<string>{
        "爸爸","妈妈","我"
    };
    public string this[int position]
    {
        get { return players[position - 1]; }
        set { players[position - 1] = value; }
    }

    public string this[string position]
    {
        get { return players[positionAbbreviations.IndexOf(position)]; }
        set { players[positionAbbreviations.IndexOf(position)] = value; }
    }
}

var team = new BaseballTeam
{
    [1] = "wwmin",
    [2] = "liyue",
    ["我"] = "wjt"
};

team[1].Dump();
team["我"].Dump();

//示例2
interface ISampleCollection<T>
{
    T this[int index]
    {
        get;
        set;
    }
}
class SampleCollection<T> : ISampleCollection<T>
{
    private readonly T[] arr = new T[100];
    int nextIndex = 0;

    //    public T this[int i] => arr[i];
    public T this[int i]
    {
        get => arr[i];
        set => arr[i] = value;
    }
    public void Add(T value)
    {
        if (nextIndex >= arr.Length)
            throw new IndexOutOfRangeException($"The collection can hold only {arr.Length} elements.");
        arr[nextIndex++] = value;
    }
}

var stringCollection = new SampleCollection<string>();
stringCollection.Add("hello,world");
stringCollection[0].Dump();

stringCollection[1] = "wwmin";
stringCollection[1].Dump();


//示例3
class DayOfWeekCollection
{
    readonly Day[] days = {
        Day.Monday,Day.Tuesday,Day.Wednesday,
        Day.Thursday,Day.Friday,Day.Saturday,Day.Sunday
    };

    //Indexer with only a get accessor with the expression-bodied difinition:
    public int this[Day day] => FindDayIndex(day);

    private int FindDayIndex(Day day)
    {
        for (int j = 0; j < days.Length; j++)
        {
            if (days[j] == day)
            {
                return j;
            }
        }
        throw new ArgumentOutOfRangeException(
         nameof(day),
         $"Day {day} is not supported.\nDay input must be a defined System.DayOfWeek value."
            );
    }
}

var week = new DayOfWeekCollection();
week[Day.Monday].Dump();
week[(DayOfWeek)1].Dump();