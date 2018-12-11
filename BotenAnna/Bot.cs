using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;

namespace BotenAnna
{
    public class Bot : IBot
    {
        public Bot(QnAMaker qnAMaker)
        {
            QnAMaker = qnAMaker;
        }

        public QnAMaker QnAMaker { get; }
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!turnContext.Responded && turnContext.Activity.Type == ActivityTypes.Message)
            {
                var result = await QnAMaker.GetAnswersAsync(turnContext);

                if (result != null && result.Length > 0)
                {
                    await turnContext.SendActivityAsync(result[0].Answer, cancellationToken: cancellationToken);
                }
                else
                {
                    var msg = @"Sorry, I don't have an answer to that";

                    await turnContext.SendActivityAsync(msg, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
