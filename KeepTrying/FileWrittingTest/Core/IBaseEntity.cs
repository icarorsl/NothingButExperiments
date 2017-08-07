using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWrittingTest.Core
{
    public interface IBaseEntity<M> where M : IBaseModel
    {
        /// <summary>
        /// Returns a new instance of M
        /// </summary>
        /// <returns></returns>
        M New();

        /// <summary>
        /// Validates and Pushes a new M into the database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Update(M obj);

        /// <summary>
        /// Validates M
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ValidationStatus Validate(M obj);

        /// <summary>
        /// Find an specific object into the database
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        M Find(Func<M, bool> func);

        /// <summary>
        /// Returns a clone of each object thats
        /// </summary>
        /// <returns></returns>
        IEnumerable<M> All();

        /// <summary>
        /// Commit the changes to file
        /// </summary>
        void Commit();
    }
}
