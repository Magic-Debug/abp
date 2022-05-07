using System;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;

namespace Volo.Blogging.Comments;
public class CommentProcessActor : Actor, ICommentProcessActor, IRemindable
{
    public CommentProcessActor(ActorHost host) : base(host)
    {
    }

    public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        throw new NotImplementedException();
    }
}
