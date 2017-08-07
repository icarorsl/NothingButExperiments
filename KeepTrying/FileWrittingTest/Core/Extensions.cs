using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWrittingTest.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Clone an object
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static M Clone<M>(this object value)
        {
            //cloning the object to make sure the dictionary gets changed only when Update is called
            var inst = value.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            object clone = inst?.Invoke(value, null);
            return (M)clone;
        }

        /// <summary>
        /// Calculate the age based on the birth param
        /// </summary>
        /// <param name="value"></param>
        /// <param name="birth"></param>
        /// <returns></returns>
        public static int Age(this DateTime value, DateTime birth)
        {
            if ((value.Year - birth.Year) > 0 || (((value.Year - birth.Year) == 0) && ((birth.Month < value.Month) || ((birth.Month == value.Month) && (birth.Day <= value.Day)))))
            {
                int DaysInbirthMonth = DateTime.DaysInMonth(birth.Year, birth.Month);
                int DaysRemain = value.Day + (DaysInbirthMonth - birth.Day);

                if (value.Month > birth.Month)
                {
                    return value.Year - birth.Year;
                }
                else if (value.Month == birth.Month)
                {
                    if (value.Day >= birth.Day)
                    {
                        return value.Year - birth.Year;
                    }
                    else
                    {
                        return (value.Year - 1) - birth.Year;
                    }
                }
                else
                {
                    return (value.Year - 1) - birth.Year;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
