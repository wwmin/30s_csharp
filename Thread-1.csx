Thread.CurrentThread.Name = "Main Thread ....";
Thread t = new Thread(WriteY);
t.Name = "Y Thread ...";
t.Start();//运行WriteY();
Thread.CurrentThread.Name.Dump();
for (int i = 0; i < 1000; i++)
{
    "x".Dump("x");
}
void WriteY()
{
    Thread.CurrentThread.Name.Dump();
    for (int i = 0; i < 1000; i++)
    {
        "y".Dump("y");
    }
}