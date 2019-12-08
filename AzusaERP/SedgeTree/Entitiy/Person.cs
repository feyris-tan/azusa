using System;

namespace moe.yo3explorer.azusa.SedgeTree.Entitiy
{
    [Serializable]
    public class Person
    {
        #region Klasse
        public Person()
        {
            _guid = Guid.NewGuid();
        }
        public Guid _guid;
        public override string ToString()
        {
            return FullName + " (" + _guid.ToString() + ")";
        }
        #endregion
        public Person father;
        public Person mother;
        public Family siblings;
        public Person marriage;
        public Family children;
        public DateTime born;
        public DateTime died;
        public string forename;
        public string surname;
        public string remarks;
        public Gender gender;
        public DateTime last_edited;
        public string maiden_name;
        public string birthplace;
        public bool illegitimate_marriages;
        public bool consistent;
        public int Generations
        {
            get
            {
                return Math.Max(GetMaternalGenerations(), GetPaternalGenerations());
            }
        }
        private int GetPaternalGenerations()
        {
            if (this.father == null)
            {
                return 1;
            }
            else
            {
                return 1 + father.GetPaternalGenerations();
            }
        }
        private int GetMaternalGenerations()
        {
            if (this.mother == null)
            {
                return 0;
            }
            else
            {
                return 1 + mother.GetMaternalGenerations();
            }
        }
        public string FullName
        {
            get
            {
                return forename + " " + surname;
            }
        }
        public string CallName
        {
            get
            {
                return forename.Split(' ')[0] + " " + surname;
            }
        }
    }
}
