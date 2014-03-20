﻿using Teleris.Entities;

namespace Teleris.Entities
{
    /**
     * An internal class for a linked list of entities. Used inside the framework for
     * managing the entities.
     */
    internal class EntityList
    {
        private Entity _head;
        private Entity _tail;
        private int _count = 0;

        internal Entity Head { get { return _head; } }
        internal Entity Tail { get { return _tail; } }

        internal void Add(Entity entity)
        {
            if (Head == null)
            {
                _head = _tail = entity;
                entity.Next = entity.Previous = null;
            }
            else
            {
                _tail.Next = entity;
                entity.Previous = _tail;
                entity.Next = null;
                _tail = entity;
            }
            _count++;
        }

        internal void Remove(Entity entity)
        {
            if (_head == entity)
            {
                _head = _head.Next;
            }
            if (_tail == entity)
            {
                _tail = _tail.Previous;
            }

            if (entity.Previous != null)
            {
                entity.Previous.Next = entity.Next;
            }

            if (entity.Next != null)
            {
                entity.Next.Previous = entity.Previous;
            }
            // N.B. Don't set node.next and node.previous to null because that will break the list iteration if node is the current node in the iteration.
        }

        internal void RemoveAll()
        {
            while (Head != null)
            {
                var entity = Head;
                _head = Head.Next;
                entity.Previous = null;
                entity.Next = null;
            }
            _tail = null;
        }

        public int Count() { return _count; }

    }
}
