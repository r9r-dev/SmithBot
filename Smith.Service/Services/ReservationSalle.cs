using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;

namespace Smith.Service.Services
{
    public class ReservationSalle
    {
        public Salle Salle;
        public DateTime Debut;
        public DateTime Fin;

        public ReservationSalle()
        {
            
        }

        public ReservationSalle(Salle salle, DateTime debut, DateTime fin)
        {
            Salle = salle;
            Debut = debut;
            Fin = fin;
        }

        public static ReservationSalleBuilder Parse(string salle, string jour, List<string> heures)
        {
            var builder = new ReservationSalleBuilder();
            if (!string.IsNullOrWhiteSpace(salle)) builder.In(salle);
            if (heures.Any())
            {
                builder.From(heures.First());
            }
            return builder;
        }
    }

    public class ReservationSalleBuilder
    {
        private readonly ReservationSalle _reservation = new ReservationSalle();
        public List<IMissingInformation<Missing>> MissingInformations = new List<IMissingInformation<Missing>>();

        public ReservationSalleBuilder()
        {
            MissingInformations.Add(new MissingSalle());
            MissingInformations.Add(new MissingStartingTime());
        }

        public ReservationSalle Build()
        {
            return _reservation;
        }

        public ReservationSalleBuilder In(string salle)
        {
            var missing = MissingInformations.Single(x => x.Nom == nameof(Salle));
            MissingInformations.Remove(missing);
            _reservation.Salle = (Salle) missing.Build(salle);
            return this;
        }

        public ReservationSalleBuilder From(string time)
        {
            var missing = MissingInformations.Single(x => x.Nom == "Heure début");
            MissingInformations.Remove(missing);
            _reservation.Debut = ((Heure)missing.Build(time)).Moment;
            return this;
        }
    }

    public class Salle : Missing
    {
        public string Nom;
    }

    public class Heure : Missing
    {
        public DateTime Moment;
    }

}
