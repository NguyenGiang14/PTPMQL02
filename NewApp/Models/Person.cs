using System.Net.Sockets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewApp.Models
{
    [Table ("person")]
    public class  Person
    {
        public string Fullname {get; set;}
        public string Adress {get;set;}
        public int Age {get; set;}
        public void EnterData()
            {
                System.Console.Write("Full");
                Fullname = Console.ReadLine();
                System.Console.Write ("Age =");
                Age =Convert.ToInt16(Console.ReadLine());

            }
            public void Display()
            {System.Console.WriteLine("{0}- {1} - {2} ", Fullname, Age);
            }
    }
}