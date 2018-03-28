using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Mediator
{
    public class Mediator : IMediator
    {
        public IMediatorReciever reciever { get; set; }

        public Mediator()
        {

        }
        public Mediator(IMediatorReciever reciever)
        {
            this.reciever = reciever;
        }

        public void sendMessage(IMediatorMessage message)
        {
            reciever.recieveMessage(message);
        }
    }
}
