using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Teleris.Entities
{
    public class Entity : IEntity
    {

        //Component collection
        private readonly Dictionary<Type, object> _components;

        #region Standard properties
        public string Name { get; set; }
        internal Entity Previous { get; set; }
        internal Entity Next { get; set; }
        #endregion

        //Constructor with set Name
        public Entity(string Name)
        {
            _components = new Dictionary<Type, object>();
            this.Name = Name;
        }

        //Constructor
        public Entity()
        {
            _components = new Dictionary<Type, object>();

        }

        //Add component to entity
        public Entity Add(object component)
        {
            //System.Console.WriteLine(component.GetType());
            AddComponentAndDispatchAddEvent(component, component.GetType());
            return this;
        }

        //Add component to entity
        public Entity Add(object component, Type componentType)
        {

            if (!componentType.IsInstanceOfType(component))
            {
                throw new InvalidOperationException("Component is not an instance of " + componentType +
                                                    " or its parent types.");
            }

            AddComponentAndDispatchAddEvent(component, componentType);
            return this;
        }

        private Entity AddComponentAndDispatchAddEvent(object component, Type componentType)
        {


            if (component == null)
            {
                throw new NullReferenceException("Component = null.");
            }

            if (_components.ContainsKey(componentType))
            {
                _components.Remove(componentType);
            }

            _components[componentType] = component;

            if (ComponentAdded != null) { ComponentAdded(this, componentType); }
            return this;
        }

        //Removes a component from the entity.
        public object Remove<T>()
        {
            return Remove(typeof(T));
        }

        public object Remove(Type componentClass)
        {
            if (_components.ContainsKey(componentClass))
            {
                var component = _components[componentClass];
                _components.Remove(componentClass);
                if (ComponentRemoved != null) { ComponentRemoved(this, componentClass); }
                return component;
            }
            return null;
        }

        public object Get(Type componentType)
        {

            return _components.ContainsKey(componentType) ? _components[componentType] : null;
        }

        public List<object> GetAll()
        {
            return _components.Values.ToList();
        }

        public int Count()
        {
            return _components.Count();
        }

        public bool Has(Type componentClass)
        {
            return _components.ContainsKey(componentClass);
        }

        public Entity Clone()
        {
            var copy = new Entity();
            foreach (var component in _components)
            {
                var componentType = component.Key;
                var clonedComponent = Activator.CreateInstance(componentType);
                foreach (var property in componentType.GetProperties().Where(property => property.CanRead && property.CanWrite))
                {
                    property.SetValue(clonedComponent, property.GetValue(component.Value, null), null);
                }
                copy.Add(clonedComponent, component.Key);
            }

            return copy;
        }

        #region events
        //This signal is dispatched when a component is added to the entity. 
        internal event Action<Entity, Type> ComponentAdded;
        //This signal is dispatched when a component is removed from the entity.
        internal event Action<Entity, Type> ComponentRemoved;
        #endregion

    }


}
