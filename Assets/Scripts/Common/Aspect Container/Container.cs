﻿using System.Collections.Generic;

namespace TheLiquidFire.AspectContainer
{
    public interface IContainer
    {
        T AddAspect<T>(string key = null) where T : IAspect, new();
        T AddAspect<T>(T aspect, string key = null) where T : IAspect;
        T GetAspect<T>(string key = null) where T : IAspect;
        ICollection<IAspect> Aspects();
    }

    public class Container : IContainer
    {
        private readonly Dictionary<string, IAspect> aspects = new();

        public T AddAspect<T>(string key = null) where T : IAspect, new()
        {
            return AddAspect(new T(), key);
        }

        public T AddAspect<T>(T aspect, string key = null) where T : IAspect
        {
            key = key ?? typeof(T).Name;
            aspects.Add(key, aspect);
            aspect.container = this;
            return aspect;
        }

        public T GetAspect<T>(string key = null) where T : IAspect
        {
            key = key ?? typeof(T).Name;
            var aspect = aspects.ContainsKey(key) ? (T)aspects[key] : default;
            return aspect;
        }

        public ICollection<IAspect> Aspects()
        {
            return aspects.Values;
        }
    }
}