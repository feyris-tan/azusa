using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.OldStuff
{
    public class JsonPropertyAttribute : Attribute
    {
        public JsonPropertyAttribute(string id)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonConverterAttribute : Attribute
    {
        public JsonConverterAttribute(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
