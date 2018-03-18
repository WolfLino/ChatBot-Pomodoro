using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotPomodoro.Dialogs
{
    [Serializable]
    public class ConversationUpdateDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = (Activity)await result;

            if (activity.MembersAdded != null && activity.MembersAdded.Any() && activity.MembersAdded[activity.MembersAdded.Count - 1].Name != "BotPomodoro")
            {
                string resposta = "Sou um bot em fase de testes, tenha paciência...\n" +
                                      "O meu trabalho é criar ou cancelar pomodoros. Tente me pedir coisas como:\n" +
                                      "* **Criar um pomodoro**\n" +
                                      "* **Iniciar 3 pomodoros de estudo**\n" +
                                      "* **Cancelar o último pomodoro**";

                await context.PostAsync(resposta);
            }

            if (activity.MembersRemoved != null && activity.MembersRemoved.Any())
            {
                string membersRemoved = string.Join(
                    ", ",
                    activity.MembersRemoved.Select(
                        removedMember => (removedMember.Id != activity.Recipient.Id) ? $"{removedMember.Name} (Id: {removedMember.Id})" : string.Empty));

                await context.PostAsync($"O membro {membersRemoved} foi removido ou saiu da conversa :(");

            }

            context.Wait(MessageReceivedAsync);
        }
    }
}