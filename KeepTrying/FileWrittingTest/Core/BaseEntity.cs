using System;
using System.Collections.Generic;
using System.Linq;

namespace FileWrittingTest.Core
{

    /// <summary>
    /// Base Entity class. This is responsible for the basic entity operations
    /// It also look like a repository
    /// Each entity will implement this class
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public abstract class BaseEntity<M> : IBaseEntity<M> where M : IBaseModel, new()
    {
        #region
        /// <summary>
        /// Contains the values from files
        /// </summary>
        private readonly Dictionary<int, M> _objects = new Dictionary<int, M>();

        /// <summary>
        /// Updates the file with the data
        /// </summary>
        private FileWriter _fileWriter;

        #endregion

        #region ctor
        public BaseEntity(FileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }
        #endregion

        #region private
        private void LoadMissingRecords()
        {
            //collecting the missing records....
            while (_fileWriter.Peek())
            {
                string line = _fileWriter.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                if (string.IsNullOrWhiteSpace(line)) continue;

                M newObj = New();
                string[] values = line.Split('|');
                newObj.ID = int.Parse(values[0]);

                _objects.Add(newObj.ID, newObj);
                FromFile(newObj, line);
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Returns a valid instance of M.
        /// </summary>
        /// <returns></returns>
        public M New()
        {
            return new M();
        }

        /// <summary>
        /// Validation method that must be implemented on each entity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract ValidationStatus Validate(M obj);

        /// <summary>
        /// generates a string in the correct format to save into the file
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract string ToFile(M obj);

        /// <summary>
        /// used to create a model based on the info from file
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract void FromFile(M obj, string value);
        
        /// <summary>
        /// Validates and pushed the record into the "database"
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(M obj)
        {
            if (Validate(obj).Validated)
            {
                LoadMissingRecords();
                if (obj.ID == 0)
                {
                    obj.ID = _objects.Keys.Count() + 1;
                    _objects.Add(obj.ID, obj);
                }
                else
                {
                    //replacing the old object with the clone
                    _objects[obj.ID] = obj;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Find an object into the dictionary
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public M Find(Func<M, bool> func)
        {
            LoadMissingRecords();

            M obj = _objects.Values.FirstOrDefault(func);

            if (obj != null)
            {
                //cloning the object to make sure the dictionary gets changed only when Update is called
                return obj.Clone<M>();
            }
            return default(M);
        }

        /// <summary>
        /// returns a ienumerable of M
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IEnumerable<M> All()
        {
            LoadMissingRecords();
            return _objects.Values.Select(s => s.Clone<M>());
        }

        /// <summary>
        /// Commit the updates into the file
        /// </summary>
        public void Commit()
        {
            LoadMissingRecords();

            int size = 0;

            string[] records = new string[10000];
            int i = 0;
            int j = 0;
            int count = _objects.Keys.Count;

            _fileWriter.BeginUpdate();

            foreach (var item in _objects.OrderBy(o => o.Key))
            {
                string record = ToFile(item.Value);
                size += record.Length + Environment.NewLine.Length;
                records[i] = record;
                i++;
                j++;

                if (i == 10000 || j == count)
                {
                    _fileWriter.SetLenght(size);
                    for (var l = 0; l < i; l++)
                    {
                        _fileWriter.Update(records[l]);
                    }
                    Array.Clear(records, 0, 10000);
                    i = 0;
                }
            }
            _fileWriter.EndUpdate();
        }
        #endregion
    }
}
