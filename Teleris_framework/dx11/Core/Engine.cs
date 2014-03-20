﻿using System;
using System.Collections.Generic;
using Teleris.Entities;
using Teleris.Systems;
using Teleris.Nodes;
using System.Diagnostics;

namespace Teleris.Core
{
    class Engine<NodeGroupManager> : IEngine where NodeGroupManager : INodeGroupManager, new()
    {
        private EntityList _entities;
        private SystemList _systems;
        private Dictionary<Type, NodeGroupManager> _NodeGroups;
        private bool _updating;
        /**
         * Indicates if the game is currently in its update loop.
         */
        public bool Updating { get { return _updating; } }

        /**
         * Dispatched when the update loop ends. If you want to add and remove systems from the
         * game it is usually best not to do so during the update loop. To avoid this you can
         * listen for this signal and make the change when the signal is dispatched.
         */
        public event Action UpdateComplete;


        public Engine()
        {
            _entities = new EntityList();
            _systems = new SystemList();
            _NodeGroups = new Dictionary<Type, NodeGroupManager>();
        }

        /**
         * Add an entity to the game.
         * 
         * @param entity The entity to add.
         */
        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            //System.Console.WriteLine(_entities.Count());
            //System.Console.WriteLine(entity.Name);
            entity.ComponentAdded += ComponentAdded;
            entity.ComponentRemoved += ComponentRemoved;
            foreach (var group in _NodeGroups.Values)
            {
                group.NewEntity(entity);
                

            }
        }

        /**
         * Remove an entity from the game.
         * 
         * @param entity The entity to remove.
         */
        public void RemoveEntity(Entity entity)
        {
            entity.ComponentAdded -= ComponentAdded;
            entity.ComponentRemoved -= ComponentRemoved;
            foreach (var node in _NodeGroups.Values)
            {
                node.RemoveEntity(entity);
            }
            _entities.Remove(entity);
        }

        /**
         * Remove all entities from the game.
         */
        public void RemoveAllEntities()
        {
            while (_entities.Head != null)
            {
                RemoveEntity(_entities.Head);
            }
        }

        private void ComponentAdded(Entity entity, Type componentClass)
        {
            foreach (var node in _NodeGroups.Values)
            {
                node.ComponentAddedToEntity(entity, componentClass);
            }

        }

        private void ComponentRemoved(Entity entity, Type componentClass)
        {
            foreach (var group in _NodeGroups.Values)
            {
                group.ComponentRemovedFromEntity(entity, componentClass);
            }
        }


        /**
         * Get a collection of nodes from the game, based on the type of the node required.
         * 
         * <p>The game will create the appropriate NodeList if it doesn't already exist and 
         * will keep its contents up to date as entities are added to and removed from the
         * game.</p>
         * 
         * <p>If a NodeList is no longer required, release it with the releaseNodeList method.</p>
         * 
         * @param nodeClass The type of node required.
         * @return A linked list of all nodes of this type from all entities in the game.
         */
        public NodeList GetNodeList<T>()
        {
            return GetNodeList(typeof(T));
        }

        public NodeList GetNodeList(Type type)
        {
            if (_NodeGroups.ContainsKey(type))
            {
                return _NodeGroups[type].NodeList;
            }
            var group = new NodeGroupManager();
            group.Setup(this, type);

            _NodeGroups.Add(type, group);

            for (var entity = _entities.Head; entity != null; entity = entity.Next)
            {
                group.NewEntity(entity);
            }
            return group.NodeList;
        }

        /**
         * If a NodeList is no longer required, this method will stop the game updating
         * the list and will release all references to the list within the framework
         * classes, enabling it to be garbage collected.
         * 
         * <p>It is not essential to release a list, but releasing it will free
         * up memory and processor resources.</p>
         * 
         * @param nodeClass The type of the node class if the list to be released.
         */
        public void ReleaseNodeList<T>()
        {
            ReleaseNodeList(typeof(T));
        }

        public void ReleaseNodeList(Type nodeClass)
        {
            if (_NodeGroups.ContainsKey(nodeClass))
            {
                _NodeGroups[nodeClass].CleanUp();
            }
            _NodeGroups.Remove(nodeClass);
        }

        /**
         * Add a system to the game, and set its priority for the order in which the
         * systems are updated by the game loop.
         * 
         * <p>The priority dictates the order in which the systems are updated by the game 
         * loop. Lower numbers for priority are updated first. i.e. a priority of 1 is 
         * updated before a priority of 2.</p>
         * 
         * @param system The system to add to the game.
         * @param priority The priority for updating the systems during the game loop. A 
         * lower number means the system is updated sooner.
         */
        public void AddSystem(ISystem system, int priority)
        {
            system.Priority = priority;
            system.AddToGame(this);
            _systems.Add(system);
        }

        /**
         * Get the system instance of a particular type from within the game.
         * 
         * @param type The type of system
         * @return The instance of the system type that is in the game, or
         * null if no systems of this type are in the game.
         */
        public ISystem GetSystem(Type type)
        {
            return _systems.Get(type);
        }

        /**
         * Remove a system from the game.
         * 
         * @param system The system to remove from the game.
         */
        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
            system.RemoveFromGame(this);
        }

        /**
         * Remove all systems from the game.
         */
        public void RemoveAllSystems()
        {
            while (_systems.Head != null)
            {
                RemoveSystem(_systems.Head);
            }
        }

        /**
         * Update the game. This causes the game loop to run, calling update on all the
         * systems in the game.
         * 
         * <p>The package net.richardlord.ash.tick contains classes that can be used to provide
         * a steady or variable tick that calls this update method.</p>
         * 
         * @time The duration, in seconds, of this update step.
         */
        public void Update(double time)
        {
            _updating = true;
            for (var system = _systems.Head; system != null; system = system.Next)
            {
                system.Update(time);
            }
            _updating = false;
            if (UpdateComplete != null)
            {
                UpdateComplete();
            }
        }
    }
}
