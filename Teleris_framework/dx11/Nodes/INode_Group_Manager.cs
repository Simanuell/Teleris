﻿using System;
using Teleris.Core;
using Teleris.Entities;

namespace Teleris.Nodes
{
    /**
     * The interface for classes that are used to manage NodeLists
     * (set as the familyClass property in the Game object)
     */
    public interface INodeGroupManager
    {
        void Setup(IEngine engine, Type nodeType);

        /* Returns the NodeList managed by this class. This should be a reference that remains valid always
        * since it is retained and reused by Systems that use the list. i.e. never recreate the list,
        * always modify it in place.
        */
        NodeList NodeList { get; }

        /**
         * An entity has been added to the game. It may already have components so test the entity
         * for inclusion in this family's NodeList.
         */
        void NewEntity(Entity entity);

        /**
         * An entity has been removed from the game. If it's in this family's NodeList it should be removed.
         */
        void RemoveEntity(Entity entity);

        /**
         * A component has been added to an entity. Test whether the entity's inclusion in this family's
         * NodeList should be modified.
         */
        void ComponentAddedToEntity(Entity entity, Type componentClass);

        /**
         * A component has been removed from an entity. Test whether the entity's inclusion in this family's
         * NodeList should be modified.
         */
        void ComponentRemovedFromEntity(Entity entity, Type componentClass);

        /**
         * The family is about to be discarded. Clean up all properties as necessary. Usually, you will
         * want to empty the NodeList at this time.
         */
        void CleanUp();
    }
}
