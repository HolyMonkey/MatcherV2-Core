using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
    class DI : MonoBehaviour
    {
        public void Awake()
        {
            MonoBehaviour[] components = FindObjectsOfType<MonoBehaviour>();
            foreach(var component in components)
            {
                Type type = component.GetType();
               
                MethodInfo injectionMethodInfo = type.GetMethods()
                                                    .FirstOrDefault((x => x.CustomAttributes
                                                    .FirstOrDefault((a) => a.AttributeType == typeof(InjectionAttribute)) != null));
                if(injectionMethodInfo != null) {

                    List<object> args = new List<object>();

                    ParameterInfo[] parameters = injectionMethodInfo.GetParameters();
                    foreach(ParameterInfo p in parameters)
                    {
                        if (p.ParameterType == typeof(Session))
                        {
                            args.Add(SessionActivator.Current);
                        }
                        else if (p.ParameterType == typeof(IExpressionGroupController))
                        {
                            throw new NotImplementedException();
                        }
                    }

                    if (args.Count != parameters.Length)
                        throw new InvalidOperationException("DI don't resolve dependency");

                    injectionMethodInfo.Invoke(component, args.ToArray());
                }
            }
        }
    }
}
