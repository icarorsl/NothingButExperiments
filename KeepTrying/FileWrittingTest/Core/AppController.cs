using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWrittingTest.Core
{
    /// <summary>
    /// Core app controller.
    /// </summary>
    public static class AppController
    {
        #region vars
        /// <summary>
        /// directory where the files will be saved
        /// </summary>
        private static string _fileDirectory;

        /// <summary>
        /// Dictionaty with the injections and its dependencies
        /// </summary>
        private static Dictionary<Type, Type> _entities = new Dictionary<Type, Type>();

        /// <summary>
        /// Instances of the dependencies. These instances are per application.
        /// </summary>
        private static Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        #endregion

        #region methods
        /// <summary>
        /// Injects a depency. Application Scope
        /// </summary>
        /// <typeparam name="I">Injection</typeparam>
        /// <typeparam name="E">Dependency</typeparam>
        public static void Register<I, E>() //where I : IBaseEntity<IBaseModel> //where E : IBaseEntity<IBaseModel> 
        {
            Type obj;
            if (!_entities.TryGetValue(typeof(I), out obj))
                _entities.Add(typeof(I), typeof(E));
        }

        /// <summary>
        /// Returns an instance of I
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <returns></returns>
        public static I Get<I>()
        {
            Type type;
            if (_entities.TryGetValue(typeof(I), out type))
            {
                object newInstance = null;
                if (!_instances.TryGetValue(typeof(I), out newInstance))
                {
                    FileWriter _fileWriter = new FileWriter(_fileDirectory, typeof(I).Name);
                    newInstance = (I)Activator.CreateInstance(type, _fileWriter);
                    _instances.Add(typeof(I), newInstance);
                }
                return (I)newInstance;
            }

            return default(I);
        }

        /// <summary>
        /// Initializes the application
        /// </summary>
        /// <param name="fileDirectoy"></param>
        public static void Init(string fileDirectoy)
        {
            _fileDirectory = fileDirectoy;
        }
        #endregion
    }
}
