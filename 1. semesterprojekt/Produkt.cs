using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.semesterprojekt
{
    class Produkt
    {
        public int Id { get; set; }
        public string ProduktNavn { get; set; }
        public string Produkttype { get; set; }
        public string Medie { get; set; }
        public string Folie { get; set; }
        public string Farve { get; set; }
        public string Længde { get; set; }
        public string Bredde { get; set; }
        public string Antal { get; set; }
        public double KvadratM { get; set; }
     
        public string Kommentar { get; set; }

        public Produkt()
        {
            
        }

        public Produkt(string produktNavn, string produkttype, string medie, string folie, string farve, string længde, string bredde, string antal, string kommentar)
        {

            if (OrdreVM.ProduktCollection.Count > 0)
            {
                var GetProdukt = (from produkt in OrdreVM.ProduktCollection select produkt.Id).Max();
                Id = GetProdukt + 1;
            }
            else
            {
                Id = 1;
            }
            ProduktNavn = produktNavn;       
            Produkttype = produkttype;
            Medie = medie;
            Folie = folie;
            Farve = farve;
            Længde = længde;
            Bredde = bredde;
            Antal = antal;
            KvadratM = ((Convert.ToDouble(Længde) / 1000) * (Convert.ToDouble(Bredde) / 1000)) * Convert.ToDouble(Antal);
           
            Kommentar = kommentar;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ProduktNavn)}: {ProduktNavn}, {nameof(Produkttype)}: {Produkttype}, {nameof(Medie)}: {Medie}, {nameof(Folie)}: {Folie}, {nameof(Farve)}: {Farve}, {nameof(Længde)}: {Længde}, {nameof(Bredde)}: {Bredde}, {nameof(Antal)}: {Antal}, {nameof(KvadratM)}: {KvadratM}, {nameof(Kommentar)}: {Kommentar}";
        }
    }
}
