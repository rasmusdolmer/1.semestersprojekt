using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace _1.semesterprojekt
{
    class Bruger
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Kodeord { get; set; }

        public Bruger()
        {
            
        }

        public Bruger(string email, string kodeord)
        {
            if (BrugerVM.BrugerCollection.Count > 0)
            {
                var GetBruger = (from bruger in BrugerVM.BrugerCollection select bruger.Id).Max();
                Id = GetBruger + 1;
            }
            else
            {
                Id = 1;
            }       
            Email = email;
            Kodeord = kodeord;
        }

        public override string ToString()
        {
            return $"{nameof(Email)}: {Email}, {nameof(Kodeord)}: {Kodeord}";
        }
    }
}
