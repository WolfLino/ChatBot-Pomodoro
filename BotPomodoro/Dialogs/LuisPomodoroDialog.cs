using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotPomodoro.Dialogs
{
    [Serializable]
    public class LuisPomodoroDialog : LuisDialog<object>
    {
        public LuisPomodoroDialog(ILuisService service) : base(service) { }

        /// <summary>
        /// Caso a intenção não seja reconhecida.
        /// </summary>
        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            var replyToConversation = (Activity)context.MakeMessage();
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();

            HeroCard heroCard = new HeroCard();
            heroCard.Images = new List<CardImage>
            {
                new CardImage("https://www.qi300.com/download/file.php?id=7899", "Não entendi o que você falou.")
            };

            replyToConversation.Attachments.Add(heroCard.ToAttachment());

            await context.PostAsync(replyToConversation);
            context.Done<string>(null);
        }

        /// <summary>
        /// Quando não houve intenção reconhecida.
        /// </summary>
        [LuisIntent("")]
        public async Task IntencaoNaoReconhecida(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("**( ͡° ͜ʖ ͡°)** - Desculpe, mas não entendi o que você quis dizer.\n" +
                                    "Lembre-se que sou um bot e meu conhecimento é limitado.");
        }

        /// <summary>
        /// Caso a intenção seja de criação.
        /// </summary>
        [LuisIntent("Criacao")]
        public async Task CriacaoAsync(IDialogContext context, LuisResult result)
        {
            int quantidade = 1;
            string tipo = "";

            if (result.Entities.Count > 0)
            {
                foreach (var entidade in result.Entities)
                {
                    if (entidade.Type == "quantidade")
                    {
                        switch (entidade.Entity)
                        {
                            case "um":
                                quantidade = 1;
                                break;

                            case "dois":
                                quantidade = 2;
                                break;

                            case "três":
                                quantidade = 3;
                                break;

                            case "quatro":
                                quantidade = 4;
                                break;

                            case "cinco":
                                quantidade = 5;
                                break;

                            case "seis":
                                quantidade = 6;
                                break;

                            case "sete":
                                quantidade = 7;
                                break;

                            default:
                                quantidade = Convert.ToInt32(entidade.Entity);
                                break;
                        }
                    }
                    else if (entidade.Type == "tipo")
                    {
                        tipo = entidade.Entity;
                    }
                }
            }

            var resposta = $"OK, irei criar {quantidade} pomodoro";

            if (quantidade > 1) resposta += "s";

            if (tipo != "") resposta += $" de {tipo}.";
            else resposta += ".";

            await context.PostAsync(resposta);
            context.Done<string>(null);

            // Método para tratar a criação
            // ...
        }

        /// <summary>
        /// Quando a intenção for para cancelar.
        /// </summary>
        [LuisIntent("Cancelamento")]
        public async Task CancelarAsync(IDialogContext context, LuisResult result)
        {
            var response = "Irei cancelar o pomodoro.";
            await context.PostAsync(response);
            context.Done<string>(null);

            // Método para tratar o cancelamento
            // ...
        }
    }
}