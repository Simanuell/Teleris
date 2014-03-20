using System;
using Teleris.Entities;
using Teleris.Systems;
using Teleris.Nodes;

namespace Teleris.Core
{
    public interface IEngine
    {
        /// <summary>
        /// Indicates if the game is currently in its update loop.
        /// </summary>
        bool Updating { get; }

        /// <summary>
        /// Dispatched when the update loop ends. If you want to add and remove systems from the
        /// game it is usually best not to do so during the update loop. To avoid this you can
        /// listen for this signal and make the change when the signal is dispatched.
        /// </summary>
        event Action UpdateComplete;

        NodeList GetNodeList<T>();

        NodeList GetNodeList(Type type);

        void ReleaseNodeList<T>();

        void ReleaseNodeList(Type nodeClass);

        void AddEntity(Entity entity);

        void AddSystem(ISystem system, int priority);

        void RemoveEntity(Entity entity);

        void RemoveAllEntities();

        ISystem GetSystem(Type type);

        void RemoveSystem(ISystem system);

        void RemoveAllSystems();

        void Update(double time);
    }
}
