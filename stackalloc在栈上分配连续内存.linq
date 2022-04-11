<Query Kind="Program">
  <Namespace>System.Runtime.InteropServices</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	AllocGuid.RefStructAlloc();
	AllocGuid.StructAllo();
	
	TestNativeMemoryApi();
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
//Native Memory API
//.NET 6 引入了一个新的 API 来分配本机内存, NativeMemory 有分配和释放内存的方法。
private void TestNativeMemoryApi(){
	unsafe
	{
		byte* buffer= (byte*)NativeMemory.Alloc(100);
		NativeMemory.Free(buffer);
	}

	/* This class contains methods that are mainly used to manage native memory.
 public static class NativeMemory
 {
	 public unsafe static void* AlignedAlloc(nuint byteCount, nuint alignment);
	 public unsafe static void AlignedFree(void* ptr);
	 public unsafe static void* AlignedRealloc(void* ptr, nuint byteCount, nuint alignment);
	 public unsafe static void* Alloc(nuint byteCount);
	 public unsafe static void* Alloc(nuint elementCount, nuint elementSize);
	 public unsafe static void* AllocZeroed(nuint byteCount);
	 public unsafe static void* AllocZeroed(nuint elementCount, nuint elementSize);
	 public unsafe static void Free(void* ptr);
	 public unsafe static void* Realloc(void* ptr, nuint byteCount);
 }*/
}
