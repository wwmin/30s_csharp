ref struct MyStruct
{
    public int Value { get; set; }
}

class RefStructGuid
{
   public static void Test()
    {
        MyStruct x = new MyStruct();
        x.Value = 100;
        Foo(x);

    }

    static void Foo(MyStruct x) { 
        x.Value.Dump();
    }

    static void Bar(object x) { }
}

RefStructGuid.Test();