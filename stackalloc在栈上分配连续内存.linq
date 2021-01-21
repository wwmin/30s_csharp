<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	AllocGuid.RefStructAlloc();
	AllocGuid.StructAllo();
}

//高阶用法 , 一般推荐使用Span<T> 构建高性能的连续栈内存对象
ref struct MyStruct{
    public int Value {get;set;}
}

static class AllocGuid{
    public static unsafe void RefStructAlloc(){
    
        MyStruct* x = stackalloc MyStruct[10];

        for (int i = 0; i < 10; i++)
        {
            *(x+1) = new MyStruct {Value = i};
        }
    }
    
    public static void StructAllo(){
        Span<int> x = stackalloc int[10];
        for (int i = 0; i < x.Length; i++)
        {
            x[i] = i;
        }
        
    }
}
