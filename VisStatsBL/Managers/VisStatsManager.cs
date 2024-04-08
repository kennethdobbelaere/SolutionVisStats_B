using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exeptions;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsBL.Managers {
    public class VisStatsManager {

        private IFileProcessor fileProcessor;
        private IVisStatsRepository visStatsRepository;

        public VisStatsManager(IFileProcessor fileProcessor, IVisStatsRepository visStatsRepository) {
            this.fileProcessor = fileProcessor;
            this.visStatsRepository = visStatsRepository;
        }

        public void UploadVissoorten(string fileName) {
            List<string> soorten = fileProcessor.LeesSoorten(fileName);
            List<Vissoort> vissoorten = MaakVissoorten(soorten);
            foreach (Vissoort vissoort in vissoorten) {

                if (!visStatsRepository.HeeftVissoort(vissoort))
                    visStatsRepository.SchrijfVissoort(vissoort);
            }
        }
        private List<Vissoort> MaakVissoorten(List<string> soorten) {

            Dictionary<string, Vissoort> VisSoorten = new();
            foreach (string soort in soorten) {
                if (!VisSoorten.ContainsKey(soort)) {
                    try {
                        VisSoorten.Add(soort, new Vissoort(soort));
                    }
                    catch (DomeinException) { }
                }
            }

            return VisSoorten.Values.ToList();
        }

        private List<Haven> MaakHavens(List<string> havens)
        {

            Dictionary<string, Haven> Haven = new();
            foreach (string haven in havens)
            {
                if (!Haven.ContainsKey(haven))
                {
                    try
                    {
                        Haven.Add(haven, new Haven(haven));
                    }
                    catch (DomeinException) { }
                }
            }
            return Haven.Values.ToList();
        }

        public void UploadHavens(string fileName)
        {
            List<string> havens = fileProcessor.LeesHavens(fileName);
            List<Haven> Haven = MaakHavens(havens);
            foreach (Haven haven in Haven)
            {

                if (!visStatsRepository.HeeftHaven(haven))
                    visStatsRepository.SchrijfHaven(haven);
            }
        }

        public void UploadStatistieken(string fileName)
        {
            try
            {
                if (!visStatsRepository.IsOpgeladen(fileName))
                {
                    List<Haven> havens = visStatsRepository.LeesHavens();
                    List<Vissoort> soorten = visStatsRepository.LeesVissoorten();
                    List<VisStatsDataRecord> data = fileProcessor.LeesStatistieken(fileName, soorten, havens);
                    visStatsRepository.SchrijfStatistieken(data, fileName);
                }
            }
            catch (Exception ex) { throw new DomeinException("UploadStatistieken", ex); }
        }
        public List<Haven> GeefHavens()
        {
            try
            {
                return visStatsRepository.LeesHavens();
            }
            catch (Exception ex)
            { 
                throw new DomeinException("Geef Havens", ex);
            }
        }

        public List<Vissoort> GeefVissoorten()
        {
            try
            {
                return visStatsRepository.LeesVissoorten();
            }
            catch (Exception ex)
            {
                throw new DomeinException("Geef Vissoorten", ex);
            }
        }
        public List<int> GeefJaartallen()
        {
            try
            {
                return visStatsRepository.LeesJaartallen();
            }
            catch (Exception ex)
            {
                throw new DomeinException("Geef Jaartallen", ex);
            }
        }
        public List<JaarVangst> GeefVangst(int jaar, Haven haven,List<Vissoort> vissoorten, Eenheid eenheid)
        {
            try
            {
                return visStatsRepository.LeesStatistieken(jaar, haven, vissoorten, eenheid);
            }
            catch (Exception e)
            {
                throw new DomeinException("GeefVangst");
            }
        }
    }
}
