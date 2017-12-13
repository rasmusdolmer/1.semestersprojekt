using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.semesterprojekt
{
    class Kunde
    {
        public int Id { get; set; }
        public string KundeCVRnummer { get; set; }
        public string Navn { get; set; }
        public string Firma { get; set; }
        public string Adresse { get; set; }
        public string By { get; set; }
        public string Postnr { get; set; }
        public string Mail { get; set; }
        public string Telefonnr { get; set; }

        public Kunde()
        {

        }

        public Kunde(string kundeCVRnummer, string navn, string firma, string adresse, string @by, string postnr, string mail, string telefonnr)
        {
            if (OrdreVM.KundeCollection.Count > 0)
            {
                var GetMaxKundeId = (from kunde in OrdreVM.KundeCollection select kunde.Id).Max();
                Id = GetMaxKundeId + 1;
            }
            else
            {
                Id = 1;
            }
            KundeCVRnummer = kundeCVRnummer;
            Navn = navn;
            Firma = firma;
            Adresse = adresse;
            By = @by;
            Postnr = postnr;
            Mail = mail;
            Telefonnr = telefonnr;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(KundeCVRnummer)}: {KundeCVRnummer}, {nameof(Navn)}: {Navn}, {nameof(Firma)}: {Firma}, {nameof(Adresse)}: {Adresse}, {nameof(By)}: {By}, {nameof(Postnr)}: {Postnr}, {nameof(Mail)}: {Mail}, {nameof(Telefonnr)}: {Telefonnr}";
        }
    }
}
