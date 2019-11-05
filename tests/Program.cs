using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rolang;

namespace Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rolang = new Interpreter();
            rolang.ExecuteFile("main.ro");
        }
    }
}
