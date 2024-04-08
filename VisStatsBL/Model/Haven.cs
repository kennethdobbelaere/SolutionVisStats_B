using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exeptions;

namespace VisStatsBL.Model
{
    public class Haven
    {
        public int? id;
        private string naam;
        

        public Haven(string naam)
        {
            Naam = naam;
        }

        public Haven(int Id, string naam)
        {
            Naam = naam;
            this.id = Id;

        }

        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomeinException("Havens_naam");
                naam = value;
            }
        }

        public override string ToString()
        {
            return Naam;
        }
    }
}
