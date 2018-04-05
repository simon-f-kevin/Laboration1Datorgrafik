using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public abstract class EntityComponent
    {
        private int _EntityId { get; set; }
        protected EntityComponent(int entityID)
        {
            _EntityId = entityID;
        }
        public int EntityID
        {
            get { return _EntityId;  }
            set { _EntityId = value; }
        }
    }
}
