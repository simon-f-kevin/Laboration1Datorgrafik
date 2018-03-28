using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Mediator
{
    public interface IMediatorReciever
    { 
        void recieveMessage(IMediatorMessage message);
    }
}
