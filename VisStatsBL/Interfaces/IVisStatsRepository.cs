using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Model;

namespace VisStatsBL.Interfaces
{
    public interface IVisStatsRepository
    {
        bool HeeftVissoort(Vissoort vissoort);
        void SchrijfVissoort(Vissoort vissoort);

        void SchrijfHaven(Haven haven);
        bool HeeftHaven(Haven haven);
        bool IsOpgeladen(string fileName);
        List<Haven> LeesHavens();
        List<Vissoort> LeesVissoorten();
        void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName);
        List<int> LeesJaartallen();
        List<JaarVangst> LeesStatistieken(int jaar, Haven haven, List<Vissoort> vissoorten, Eenheid eenheid);
    }
}
