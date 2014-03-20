﻿using System;
using Teleris.Core;

namespace Teleris.Systems
{
    /**
     * The base class for a system.
     * 
     * <p>A system is part of the core functionality of the game. After a system is added to the game, its
     * update method will be called on every frame of the game. When the system is removed from the game, 
     * the update method is no longer called.</p>
     * 
     * <p>The aggregate of all systems in the game is the functionality of the game, with the update
     * methods of those systems collectively constituting the game loop. Systems generally operate on
     * node lists - collections of nodes. Each node contains the components from an entity in the game
     * that match the node.</p>
     */
    public abstract class ISystem
    {
        /**
         * Used internally to manage the list of systems within the game. The previous system in the list.
         */
        internal ISystem Previous { get; set; }

        /**
         * Used internally to manage the list of systems within the game. The next system in the list.
         */
        internal ISystem Next { get; set; }

        /**
         * Used internally to hold the priority of this system within the system list. This is 
         * used to order the systems so they are updated in the correct order.
         */
        internal int Priority { get; set; }

        /**
         * Called just after the system is added to the game, before any calls to the update method.
         * Override this method to add your own functionality.
         * 
         * @param game The game the system was added to.
         */
        public abstract void AddToGame(IEngine game);

        /**
         * Called just after the system is removed from the game, after all calls to the update method.
         * Override this method to add your own functionality.
         * 
         * @param game The game the system was removed from.
         */
        public abstract void RemoveFromGame(IEngine game);

        /**
         * After the system is added to the game, this method is called every frame until the system
         * is removed from the game. Override this method to add your own functionality.
         * 
         * <p>If you need to perform an action outside of the update loop (e.g. you need to change the
         * systems in the game and you don't want to do it while they're updating) add a listener to
         * the game's updateComplete signal to be notified when the update loop completes.</p>
         * 
         * @param time The duration, in seconds, of the frame.
         */
        public abstract void Update(double time);
    }
}
