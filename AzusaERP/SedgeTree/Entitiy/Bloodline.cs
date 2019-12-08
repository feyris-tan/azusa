using System;
using System.Collections.Generic;

namespace moe.yo3explorer.azusa.SedgeTree.Entitiy
{
    [Serializable]
    class Bloodline : List<Person>
    {
        public Bloodline()
        {
        }
        public Guid _guid;
        public DateTime _last_edited;
        public string author;
        public string author_machine_name;

        public bool ConatinsMales
        {
            get
            {
                foreach (Person p in this)
                {
                    if (p.gender == Gender.Male)
                        return true;
                }
                return false;
            }
        }

        public bool ContainsFemales
        {
            get
            {
                foreach (Person p in this)
                {
                    if (p.gender == Gender.Female)
                        return true;
                }
                return false;
            }
        }
    }
}
