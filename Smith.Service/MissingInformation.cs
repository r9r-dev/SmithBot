using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;
using Smith.Service.Services;

namespace Smith.Service
{
    public class Missing
    {
        public string Name { get; set; }
        public string Value { get; set; } 
    }

    public interface IMissingInformation<out T> where T : Missing
    {
        string Nom { get; }
        string Question { get; }
        T Build(string answer);
        
    }

    public class MissingSalle : IMissingInformation<Salle>
    {
        public string Nom => nameof(Salle);
        public string Question => "Quelle salle ?";
        
        public Salle Build(string answer)
        {
            return new Salle
            {
                Name = "Salle",
                Value = answer,
                Nom = answer
            };
        }
    }

    public class MissingStartingTime : IMissingInformation<Heure>
    {
        public string Nom => "Heure début";

        public string Question => "A partir de quelle heure ?";

        public Heure Build(string answer) // 1h 13h45 14h00 15h
        {
            var temps = answer.Split('h');
            var heure = temps[0];
            var minutes = temps[1].PadRight(2, '0');

            return new Heure
            {
                Name = "Heure début",
                Value = answer,
                Moment = DateTime.Parse($"{heure}:{minutes}")
            };
        }
    }

    
}
