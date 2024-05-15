using System.Security.Cryptography;
using NewApp.Models;
public class  Demo
{
    private static void Main (string[] agrs)
    {
    Person ps1 = new Person();
    Person ps2 = new Person();
    ps1.Fullname ="Nguyen Van A";
    ps1.Age =18;
    ps1.Display();
    ps2.Display();
}
}