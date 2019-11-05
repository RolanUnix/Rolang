using System.Collections;
using System.Collections.Generic;

namespace Rolang.Data
{
    public class Space
    {
        public readonly List<Variable> Variables;

        public Space()
        {
            Variables = new List<Variable>();
        }
    }
}
