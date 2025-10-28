using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ServiceManager
    {
        private GameObject _globalComponentsObject;
        private Dictionary<Type, object> _services;
        private Dictionary<Type, bool> _persistentServiceTypes;

        public ServiceManager(GameObject globalComponentsObject)
        {
            _services = new Dictionary<Type, object>();
            _persistentServiceTypes = new Dictionary<Type, bool>();
            _globalComponentsObject = globalComponentsObject;
        }

        public void SetService(Type serviceType, object service, bool markPersistent, bool overwrite)
        {
            if (_services.TryGetValue(serviceType, out _))
            {
                if (overwrite)
                {
                    Log.Debug($"Overwriting service of type {serviceType}.");
                }
                else
                {
                    Log.Warning($"Trying to overwrite service of type {serviceType} non-explicitly, not currently supported.");
                    return;
                }
            }

            if (service is Component)
            {
                Log.Debug($"Object being added for service type {serviceType} is a Component. Use CreateServiceComponent to reduce liklihood of bugs with component lifecycle.");
            }

            _services[serviceType] = service;
            _persistentServiceTypes[serviceType] = markPersistent;
        }

        public void CreateComponentService(Type serviceType, Type componentType, bool markPersistent)
        {
            if (_services.TryGetValue(serviceType, out _))
            {
                Log.Warning($"Trying to overwrite service of type {serviceType}, not currently supported.");
                return;
            }

            _services[serviceType] = _globalComponentsObject.AddComponent(componentType);
            _persistentServiceTypes[serviceType] = markPersistent;
        }

        public object GetService(Type serviceType)
        {
            return _services.TryGetValue(serviceType, out var service) ? service : null;
        }

        public void RemoveNonPersistentServices()
        {
            Log.Debug("Removing non persistent services");

            foreach(var kvp in _persistentServiceTypes)
            {
                //Log.Debug($"{kvp.Key} is persistent : {kvp.Value}");

                if (kvp.Value) 
                {
                    // Persistent service
                    continue;
                }

                if(_services.TryGetValue(kvp.Key, out var obj))
                {

                    if(obj is Component componentService)
                    {
                        GameObject.DestroyImmediate(componentService);
                    }

                    _services.Remove(kvp.Key);
                    Log.Debug($"Removing service {kvp.Key}");
                }
            }
        }
    }
}