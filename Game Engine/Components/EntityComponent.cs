using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public abstract class EntityComponent
    {
        public int _EntityId { get; set; }
        public EntityComponent()
        {

        }
        public int EntityID
        {
            get { return _EntityId;  }
            set { _EntityId = value; }
        }
    }
}
