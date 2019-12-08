using System;
using System.Collections.Generic;

namespace moe.yo3explorer.azusa.SedgeTree.Entitiy
{
    [Serializable]
    public class Family : List<Person>
    {
        public Family()
        {
        }
        public override string ToString()
        {
            string result = "";
            foreach (Person member in this)
            {
                result += member.FullName + "\r\n";
            }
            return result;
        }
    }
}
