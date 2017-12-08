using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.semesterprojekt
{
    class Ordre
    {
        public string OrdreNummer { get; set; }
        public DateTime DateTime { get; set; }

        private DateTimeOffset _date;

        public DateTimeOffset Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string KundeCVRnummer { get; set; }
        public ObservableCollection<Produkt> ProduktCollection { get; set; }
        public bool Laminering { get; set; }
        public bool Fragt { get; set; }
        public bool OpTil10 { get; set; }
        public bool Montering { get; set; }
        public bool Afhentes { get; set; }
        public bool Accepteret { get; set; }
        public bool Aktiveret { get; set; }

        public Ordre()
        {

        }

        public Ordre(DateTimeOffset deadline, bool laminering, bool fragt, bool opTil10, bool montering, bool afhentes, string kundeCVRnummer, ObservableCollection<Produkt> produktCollection)
        {
            var GetOrdreNr = (from ordre in OrdreVM.OrdrerCollection select ordre.OrdreNummer).Max();
            var GetOrdreNr2 = (from ordre in OrdreVM.DeaktiveredeOrdrerCollection select ordre.OrdreNummer).Max();
            int i = Convert.ToInt32(GetOrdreNr) + 1;
            int i2 = Convert.ToInt32(GetOrdreNr2) + 1;
            if (i <= i2)
            {
                OrdreNummer = i2.ToString();
            }
            else
            {
                OrdreNummer = i.ToString();
            }           
            Laminering = laminering;
            Fragt = fragt;
            OpTil10 = opTil10;
            Montering = montering;
            Afhentes = afhentes;
            DateTime = Convert.ToDateTime(DateTime.Now.ToString("f"));
            Date = deadline;
            KundeCVRnummer = kundeCVRnummer;
            ProduktCollection = produktCollection;
            Accepteret = false;
            Aktiveret = true;
        }

        //public override string ToString()
        //{
        //    string Produkter = "";
        //    foreach (var p in ProduktCollection)
        //    {
        //        Produkter += "Produktid: " + p.Id + ", Navn: " + p.ProduktNavn + ", Type: " + p.Produkttype + ", Medie: " + p.Medie + ", Folie: " + p.Folie + ", Farve: " + p.Farve + ", Længde: " + p.Længde + ", Bredde: " + p.Bredde + ", Antal: " + p.Antal + ", Kvadratmeter: " + p.KvadratM;
        //        if (p.Laminering)
        //        {
        //            Produkter += ", " + p.Laminering.ToString().Replace(Convert.ToString(p.Laminering), "Laminering");
        //        }
        //        if (p.Fragt)
        //        {
        //            Produkter += ", " + p.Fragt.ToString().Replace(Convert.ToString(p.Fragt), "Fragt");
        //        }
        //        if (p.OpTil10)
        //        {
        //            Produkter += ", " + p.OpTil10.ToString().Replace(Convert.ToString(p.OpTil10), "Op til 10%");
        //        }
        //        if (p.Montering)
        //        {
        //            Produkter += ", " + p.Montering.ToString().Replace(Convert.ToString(p.Montering), "Montering");
        //        }
        //        if (p.Afhentes)
        //        {
        //            Produkter += ", " + p.Afhentes.ToString().Replace(Convert.ToString(p.Afhentes), "Afhentes");
        //        }
        //    }
        //    return $"{nameof(OrdreNummer)}: {OrdreNummer}, {nameof(DateTime)}: {DateTime}, {nameof(KundeCVRnummer)}: {KundeCVRnummer}, {"Produkter"}: {Produkter}";
        //}
    }
}
