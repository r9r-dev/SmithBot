using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Cognitive.LUIS;
using Smith.Proto.ProtoSmith;
using Smith.Service.Services;

namespace Smith.Service
{
    public class BotService : Bot.BotBase
    {
        private readonly LuisClient _luis;

        public BotService(LuisClient luis)
        {
            _luis = luis;
        }

        public override async Task<AskResponse> Ask(AskRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Id}:{request.Message}");

            var result = await _luis.Predict(request.Message);



            var intent = result.Intents.First();

            var response = new AskResponse();
            response.Id = request.Id;
            response.Message = request.Message;
            
            Console.WriteLine($"   [Intent] {intent.Name} ({intent.Score:N2})");
            switch (intent.Name)
            {
                case "Bonjour":
                    response.Answers.Add($"Bonjour à vous !");
                    break;
                case "Reservite.Reserver":
                    response.Answers.Add($"Vous souhaitez réserver une salle.");
                    string salle = null;
                    string jour = null;
                    List<string> heures = new List<string>();
                    foreach (var e in result.Entities)
                    {
                        switch (e.Key)
                        {
                            case "salle":
                                salle = e.Value.First().Value;
                                Console.WriteLine($"   [Entity] {e.Key} : {salle}");
                                response.Answers.Add($"   {e.Key} : {salle}");
                                break;
                            case "heure":
                                foreach (var v in e.Value)
                                {
                                    heures.Add(v.Value);
                                    Console.WriteLine($"   [Entity] {e.Key} : {v.Value}");
                                    response.Answers.Add($"   {e.Key} : {v.Value}");
                                }
                                break;
                            case "date":
                                jour = e.Value.First().Value;
                                Console.WriteLine($"   [Entity] {e.Key} : {jour}");
                                response.Answers.Add($"   {e.Key} : {jour}");
                                break;
                        }
                    }
                    var reservationBuilder = ReservationSalle.Parse(salle, jour, heures);
                    foreach (var missing in reservationBuilder.MissingInformations)
                    {
                        response.Answers.Add(missing.Question);
                    }
                    break;
                case "Reservite.InfosSalle":
                    response.Answers.Add($"Vous souhaitez obtenir des informations sur une salle.");
                    break;
                case "Reservite.SallesDisponibles":
                    response.Answers.Add($"Vous souhaitez savoir quelles salles sont disponibles.");
                    break;
                case "Quitter":
                    break;
                case "Annuler":
                    response.Answers.Add($"Vous souhaitez annuler l'opération en cours.");
                    break;
                case "None":
                    response.Answers.Add($"Je n'ai pas compris votre question.");
                    break;
                default:
                    response.Answers.Add($"Veuillez répéter svp.");
                    break;
            }

            return response;
        }

        [LuisIntent("Reservite.Reserver")]
        public async Task ReserverIntent(IDialogContext context, LuisResult result)
        {

        }

        
    }

    public class LuisIntentAttribute : Attribute
    {
        private readonly string _name;

        public LuisIntentAttribute(string name)
        {
            _name = name;
        }
    }
}
