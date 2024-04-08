using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exeptions;

namespace VisStatsBL.Model {
    public class Vissoort {

        public int? id;
        private string naam;


        public Vissoort(string naam) {

            Naam = naam;
        }

        public Vissoort(int id, string naam) {
        
            this.id = id;
            Naam = naam;

        }
        public string Naam { 
            get { return naam; } 
            set { if (string.IsNullOrWhiteSpace(value))
                    throw new DomeinException("Vissoort_naam");
                naam = value; } 
        }
        public override string ToString()
        {
            return Naam;
        }
    }   

}
