<Query Kind="Statements">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

int[] array1 = new int[5];
array1.GetLength(0).Dump("GetLength(0)");
int[] array2 = new int[] { 1, 3, 4, 5, 6 };
int[] array3 = { 1, 2, 3, 4, 5, 6 };
int[,] multiDimensionalArray1 = new int[2, 3];
int[,] multiDimensionalArray2 = { { 1, 2, 3 }, { 4, 5, 6 } };

foreach (var element in multiDimensionalArray2)
{
    element.Dump();
}


int[][] jaggedArray = new int[6][];
jaggedArray[0] = new int[4] { 1, 2, 3, 4 };

//foreach (var jagged in jaggedArray)
//{
//    jagged.Dump();
//}

int[,] ps = new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
for (int i = 0; i < ps.GetLength(0); i++)
{
    for (int j = 0; j < ps.GetLength(1); j++)
    {
        $"Element({i},{j})={ps[i, j]}".Dump();
    }
}

//比较两个Array
int[] arr1 = Enumerable.Range(1, 9).ToArray<int>();
int[] arr2 = new[] { 9, 1, 4, 5, 2, 3, 6, 7, 8 };

//方法1
var q = from a in arr1
        join b in arr2 on a equals b
        select a;
bool equals1 = arr1.Length == arr2.Length && q.Count() == arr2.Length;

//方法2
bool equals2 = arr1.OrderBy(a => a).SequenceEqual(arr2.OrderBy(a => a));


//方法3
var set = new HashSet<int>(arr1);
bool equals3 = arr2.All(set.Contains);

//方法4
var shared = arr1.Intersect(arr2);
bool equals4 = arr1.Length == arr2.Length && shared.Count() == arr1.Length;

//int[] a1 = { 1, 2, 3 };
//int[] a2 = { 1, 2, 3 };
//方法5
IStructuralEquatable structuralEquator = arr1.OrderBy(a=>a).ToArray();
structuralEquator.Equals(arr2.OrderBy(a=>a).ToArray(), EqualityComparer<int>.Default).Dump();
