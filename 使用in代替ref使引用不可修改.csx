void Foo(in string s){
 //s = "22min"; //s 不允许修改, in修饰符的作用
 s.Dump();
}

string s = "wwmin";
Foo(in s);
s.Dump();