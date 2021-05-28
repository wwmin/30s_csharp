<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	string abc = "123";
	int b= 1;
	test (() => abc);//abc 123
	test (() => b);//b 1
}


static void test<T>(Expression<Func<T>> expression) {
	
	var me = expression.Body as MemberExpression;
	var name = me.Member.Name;
	var value = expression.Compile().Invoke();
	name.Dump();
	value.Dump();
	
}
