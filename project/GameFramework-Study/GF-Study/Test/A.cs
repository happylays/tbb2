using System;
public class TestInerface
{
    abstract class  AInterface
    {
        public virtual void Display() { Console.WriteLine("AInterface"); }
    }
    public class A
    {
        public override void Display() { Console.WriteLine("A"); }
    }
    public class AA : A
    {
        public override void Display() { Console.WriteLine("AA"); }
    }
    static void Main(string[] args)
    {
        AA a = new AA();
        a.Display();
        Console.ReadKey();
    }
}