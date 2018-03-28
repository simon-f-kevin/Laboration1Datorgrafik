using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Mediator
{
    public struct CollisionMediatorMessage : IMediatorMessage
    {
        private int _entityId1;
        private int _entityId2;

        public CollisionMediatorMessage(int id1, int id2)
        {
            _entityId1 = id1;
            _entityId2 = id2;
        }

        public int Message1 { get { return _entityId1;  } set { _entityId1 = value; } }
        public int Message2 { get { return _entityId2; } set { _entityId2 = value; } }
    }
}
