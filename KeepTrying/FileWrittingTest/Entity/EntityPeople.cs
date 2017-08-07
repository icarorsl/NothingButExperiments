using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileWrittingTest.Domain;
using FileWrittingTest.Core;

namespace FileWrittingTest.Entity
{

    /// <summary>
    /// Implementation of IEntityPerson
    /// </summary>
    public sealed class EntityPeople : BaseEntity<People>, IEntityPeople
    {
        #region ctor
        public EntityPeople(FileWriter fileWriter) : base(fileWriter)
        {
        }
        #endregion

        #region methods
        /// <summary>
        /// validate a person
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override ValidationStatus Validate(People obj)
        {
            ValidationStatus validationStatus = new ValidationStatus();

            if (string.IsNullOrEmpty(obj.FirstName))
                validationStatus.Messages.Add("FirstName is mandatory.");

            if (!obj.DateOfBirth.HasValue)
                validationStatus.Messages.Add("Date of Birth is mandatory.");
            else
                if (obj.DateOfBirth.Value > DateTime.Today)
                    validationStatus.Messages.Add("Date of Birth cannot be bigger then today.");
            else
                if (obj.IsUnder16)
                    validationStatus.Messages.Add("Persons under 16 cannot be registrated.");

            else
                if (obj.IsUnder18 && (!obj.AllowRegistration.HasValue || !obj.AllowRegistration.Value))
                    validationStatus.Messages.Add("Persons under 18 must be allowed registration.");

            if (obj.MaritalStatus == MaritalStatus.Married && obj.Partner == null)
                validationStatus.Messages.Add("Married persons must have a partner related.");

            if (obj.MaritalStatus == MaritalStatus.Single && obj.Partner != null)
                validationStatus.Messages.Add("Single persons cannot have a partner related.");

            return validationStatus;
        }
        
        
        /// <summary>
        /// COnvert the object to the format that will be saved in the file
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override string ToFile(People obj)
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", obj.ID, obj.FirstName, obj.Surname, obj.DateOfBirth.Value.ToString("dd/MM/yyyy"), obj.MaritalStatus.ToString(), obj.AllowRegistration, obj.Partner != null ? obj.Partner.ID.ToString() : null);
        }

        /// <summary>
        /// Generates a new object based on the vlaue that comes from the file
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        protected override void FromFile(People obj, string value)
        {
            string[] values = value.Split('|');
            obj.FirstName = values[1];
            obj.Surname = values[2];
            obj.DateOfBirth = DateTime.Parse(values[3]);
            obj.MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), values[4]);
            obj.AllowRegistration = values[5] == "True";
            int partnerID = 0;
            if (int.TryParse(values[6], out partnerID))
                obj.Partner = Find(f => f.ID == partnerID);
        }
        #endregion
    }
}
