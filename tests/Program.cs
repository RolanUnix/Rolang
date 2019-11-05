using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rolang
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
