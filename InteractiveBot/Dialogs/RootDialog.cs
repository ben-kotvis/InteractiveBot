using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using InteractiveBot.Model;

namespace InteractiveBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private readonly IHandler _qnaHandler;
        public RootDialog(IHandler qnaHandler)
        {
            _qnaHandler = qnaHandler;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var answer = await _qnaHandler.Handle(activity.Text);
            
            await context.PostAsync(answer.Answer);
            
            await context.FlushAsync(System.Threading.CancellationToken.None);

            context.Wait(MessageReceivedAsync);
        }
    }
}