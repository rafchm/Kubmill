using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Kubmill.Messages
{
    public class PageLoadingMessage : ValueChangedMessage<int>
    {
        public PageLoadingMessage(int value = 0) : base(value)
        {
        }
    }
}
