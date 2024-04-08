using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exeptions;

namespace VisStatsBL.Model
{
    public class VisStatsDataRecord
    {
        private int _jaar;
        public int Jaar
        {
            get { return _jaar; }
            set
            {
                if ((value < 2000) || (value > 2100)) throw new DomeinException("jaar is niet correct");
                _jaar = value;
            }
        }
        private int _maand;
        public int Maand
        {
            get { return _maand; }
            set
            {
                if ((value < 1) || (value > 12)) throw new DomeinException("maand is niet correct");
                _maand = value;
            }
        }
        private double _gewicht;
        public double Gewicht
        {
            get { return _gewicht; }
            set
            {
                if (value < 0) throw new DomeinException("gewicht is niet correct");
                _gewicht = value;
            }
        }
        private double _waarde;
        public double Waarde
        {
            get { return _waarde; }
            set
            {
                if (value < 0) throw new DomeinException("waarde is niet correct");
                _waarde = value;
            }
        }
        public Haven Haven
        {
            get { return _haven; } set { if (value==null) throw new DomeinException("haven is null"); _haven = value; }
        }
        private Haven _haven;
        private Vissoort _vissoort;
        public Vissoort Vissoort
        {
            get { return _vissoort; }
            set { if (value == null) throw new DomeinException("vissoort is null"); _vissoort = value; }
        }

        public VisStatsDataRecord(int jaar, int maand, double gewicht, double waarde, Haven haven, Vissoort vissoort)
        {
            Jaar = jaar;
            Maand = maand;
            Gewicht = gewicht;
            Waarde = waarde;
            Haven = haven;
            Vissoort = vissoort;
        }
    }
}
