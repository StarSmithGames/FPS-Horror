using System;
using System.Collections.Generic;

namespace Game
{
    public sealed class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary< Type, object > _services = new();

        public void Clear()
        {
            _services.Clear();
        }
        
        public void Register< T >( T service )
        {
            _services[ typeof(T) ] = service;
        }

        public T GetAs< T >()
        {
            return (T)_services[ typeof(T) ];
        }
    }
}