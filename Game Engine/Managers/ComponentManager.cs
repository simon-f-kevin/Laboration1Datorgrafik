using Game_Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Ska ha en (eller två) Dictionary innehållande alla componenter av typ EntityComponent
 * Har en kö med lediga Id'n
 * innehåller en instans av sig självt
 * 
 * har metod för att hämta en lista av alla komponenter av en viss typ eller hämta komponenter med ett viss id
 * har metod för att lägga till och ta bort komponenter
 */

namespace Game_Engine.Managers
{
    public class ComponentManager
    {
        private Dictionary<Type, Dictionary<int, EntityComponent>> _componentsByType;
        private Dictionary<int, List<EntityComponent>> _componentsById;

        private Queue<int> _freeIds;
        private static int _idCount;

        private static ComponentManager instance;

        private ComponentManager()
        {
            _componentsById = new Dictionary<int, List<EntityComponent>>();
            _componentsByType = new Dictionary<Type, Dictionary<int, EntityComponent>>();
            _freeIds = new Queue<int>();
        }

        public Dictionary<int, EntityComponent> getDictionary<T>() where T : EntityComponent
        {
            Dictionary<int, EntityComponent> compDictionary;
            if (_componentsByType.TryGetValue(typeof(T), out compDictionary))
            {
                return compDictionary;
            }
            compDictionary = new Dictionary<int, EntityComponent>();
            _componentsByType.Add(typeof(T), compDictionary);
            return compDictionary;
        }

        //creates an instance of the compmanager if one does not already exist
        public static ComponentManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ComponentManager();
                }
                return instance;
            }
        }

        public T getComponentFromDict<T>(int entityID, Dictionary<int, EntityComponent> dictionary) where T : EntityComponent
        {
            EntityComponent outValue;
            if(dictionary.TryGetValue(entityID, out outValue))
            {
                return (T)outValue;
            }
            return null;
        }

        public int getNewId()
        {
            //if the queue of free IDs is empty add 1000 new free IDs to the queue
            if (_freeIds.Count == 0)
            {
                for (int i = _idCount; i < _idCount + 1000; i++)
                {
                    _freeIds.Enqueue(i);
                }
                _idCount += 1000;
            }
            return _freeIds.Dequeue();
        }

        public void AddComponent(EntityComponent component, Type componentType = null)
        {
            if(componentType == null) { componentType = component.GetType(); }
            Dictionary<int, EntityComponent> tempDict;
            if (!_componentsByType.TryGetValue(componentType, out tempDict))
            {
                tempDict = new Dictionary<int, EntityComponent>();
                _componentsByType[componentType] = tempDict;
            }
            _componentsByType[componentType][component.EntityID] = component;
            List<EntityComponent> list;
            if (!_componentsById.TryGetValue(component.EntityID, out list))
            {
                list = new List<EntityComponent>();
                _componentsById[component.EntityID] = list;
            }
            _componentsById[component.EntityID].Add(component);
        }

        //public void removeComponent<T>(int id) where T : EntityComponent
        //{
        //    Dictionary<int, EntityComponent> tempDict;
        //    if(_componentsByType.TryGetValue(typeof(T), out tempDict))
        //    {
        //        EntityComponent comp;
        //        if(tempDict.TryGetValue(id, out comp))
        //        {
        //            List<EntityComponent> tempList;
        //            if(_componentsById.TryGetValue(id, out tempList))
        //            {
        //                tempList.Remove(comp);
        //                tempDict.Remove(comp.EntityID);
        //            }
        //        }
        //    }
        //}

        public T GetComponentsById<T>(int id) where T : EntityComponent
        {
            Dictionary<int, EntityComponent> tempDict;
            if (_componentsByType.TryGetValue(typeof(T), out tempDict))
            {
                EntityComponent component;
                if (tempDict.TryGetValue(id, out component))
                {
                    return (T)component;
                }
            }
            return null;
        }

        //public List<T> getComponentsByType<T>(int id) where T : EntityComponent
        //{
        //    Dictionary<int, EntityComponent> tempDict;
        //    List<T> tempList = new List<T>();
        //    if (_componentsByType.TryGetValue(typeof(T), out tempDict))
        //    {
        //        foreach(var item in tempDict.Values)
        //        {
        //            tempList.Add((T)item);
        //        }
        //        return tempList;
        //    }
        //    return null;
        //}

    }
}
