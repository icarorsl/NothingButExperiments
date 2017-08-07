using FileWrittingTest.Core;
using FileWrittingTest.Domain;
using FileWrittingTest.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileWrittingTest
{
    class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("File Writing Test. Initializing...");
            //here is whre the file will be saved
            string path = @"C:\temp\";
            AppController.Init(path);

            Console.WriteLine("File Writing Test. Registering Entities...");
            AppController.Register<IEntityPeople, EntityPeople>();

            Console.WriteLine(string.Format("Path: {0}", path));

            var entityPerson = AppController.Get<IEntityPeople>();

            while (true)
            {
                Console.Write("What do you want to do? (E=Exit, S=Show records, A=Add records, V=View a record, U=Update a record)?: ");
                string value = Console.ReadLine();

                if (value.Equals("U", StringComparison.OrdinalIgnoreCase) || value.Equals("V", StringComparison.OrdinalIgnoreCase))
                {
                    while (true)
                    {
                        int peopleID = 0;
                        while (true)
                        {
                            Console.Write("Type the ID: ");
                            string valueID = Console.ReadLine();
                            if (int.TryParse(valueID, out peopleID))
                                break;
                        }

                        People people = entityPerson.Find(p => p.ID == peopleID);

                        if (people != null)
                        {
                            if (value.Equals("V", StringComparison.OrdinalIgnoreCase))
                                DisplayRecord(people);
                            else
                                CollectPersonDetails(people, null);
                            break;
                        }
                    }
                }

                if (value.Equals("I", StringComparison.OrdinalIgnoreCase))
                {
                    InjectPeople();
                }

                if (value.Equals("E", StringComparison.OrdinalIgnoreCase))
                    break;

                if (value.Equals("A", StringComparison.OrdinalIgnoreCase))
                    CollectPersonDetails(null, null);

                if (value.Equals("S", StringComparison.OrdinalIgnoreCase))
                {
                    int i = 0;
                    Console.Clear();
                    foreach (var item in entityPerson.All())
                    {
                        DisplayRecord(item);
                        i++;

                        if ((i % 10) == 0)
                        {
                            if (!CollectYesNo("Bring another 10 records? (Y=Yes, N=No): "))
                            {
                                break;
                            }

                            Console.Clear();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Collect the details of the person
        /// </summary>
        /// <param name="people"></param>
        /// <param name="partner"></param>
        /// <returns></returns>
        static People CollectPersonDetails(People people, People partner)
        {
            var entityPerson = AppController.Get<IEntityPeople>();

            string value = string.Empty;


            if (people != null)
            {
                DisplayRecord(people);
                Console.WriteLine(string.Format("Changing {0}...", people.FullName));
            }
            else
            {
                people = entityPerson.New();
            }

            Console.Write("First name: ");
            value = Console.ReadLine();
            people.FirstName = value;

            Console.Write("Surname: ");
            people.Surname = Console.ReadLine();

            if (people.ID == 0 || partner != null)
            {
                People another = entityPerson.Find(p => p.ID != people.ID && p.FullName == people.FullName);

                if (another != null)
                {
                    DisplayRecord(another);
                    while (true)
                    {
                        Console.Write("Another Person with same Fullname exists (U=Use the record, I=Ignore this message and continue, C=Cancel): ");
                        value = Console.ReadLine();
                        if (value.Equals("U", StringComparison.OrdinalIgnoreCase)) return another;
                        if (value.Equals("I", StringComparison.OrdinalIgnoreCase)) break;
                        if (value.Equals("C", StringComparison.OrdinalIgnoreCase)) return null;
                    }
                }
            }

            while (true)
            {
                Console.Write("Date of Birth (dd/MM/YYYY-ddMMyyyy-dd-MM-yyyy): ");
                DateTime dt;
                if (DateTime.TryParseExact(Console.ReadLine(), new string[] { "dd/MM/yyyy", "ddMMyyyy", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    people.DateOfBirth = dt;

                    if (people.IsUnder16)
                    {
                        Console.WriteLine("Persons under 16 cannot be registrated.");
                    }
                    else
                        if (people.IsUnder18)
                    {
                        people.AllowRegistration = CollectYesNo("Parents Authorization? (Y=Yes, N=No): ");
                        if (!people.AllowRegistration.Value)
                            Console.WriteLine("Persons under 18 must be allowed registration.");
                        else
                            break;
                    }
                    else
                    {
                        people.AllowRegistration = null;
                        break;
                    }
                }
            }

            if (partner == null)
            {
                if (CollectYesNo("Married? (Y=Yes, N=No): "))
                {
                    while (true)
                    {
                        if (people.Partner != null)
                        {
                            if (!CollectYesNo(string.Format("Partner is {0}. Want to change? (Y=Yes, N=No): ", people.Partner.FullName)))
                            {
                                break;
                            }

                            people.Partner.Partner = null;
                            people.Partner.MaritalStatus = MaritalStatus.Single;
                            entityPerson.Update(people.Partner);

                            people.Partner = null;
                            people.MaritalStatus = MaritalStatus.Single;
                        }

                        People p = CollectPersonDetails(null, people);

                        if (p != null)
                        {
                            people.MaritalStatus = MaritalStatus.Married;
                            people.Partner = p;
                            p.Partner = people;
                            break;
                        }
                        else
                        {
                            if (!CollectYesNo("Partner was not collected. Try again? (Y=Yes, N=No): "))
                                break;
                        }
                    }
                }

                ValidationStatus validationStatus = entityPerson.Validate(people);
                if (validationStatus.Validated)
                {
                    if (entityPerson.Update(people))
                    {
                        entityPerson.Commit();
                        if (CollectYesNo(string.Format("Person {0} was saved. Create a new record? (Y=Yes, N=No): ", people.FullName)))
                        {
                            Console.Clear();
                            CollectPersonDetails(null, null);
                        }
                    }
                }
                else
                {
                    foreach (var item in validationStatus.Messages)
                        Console.WriteLine(item);

                    if (CollectYesNo("Person was not saved. Change the values? (Y=Yes, N=No): "))
                        CollectPersonDetails(people, null);
                }

            }
            else
            {
                people.MaritalStatus = MaritalStatus.Married;
                if (people.Partner != null)
                {
                    people.Partner.MaritalStatus = MaritalStatus.Single;
                    entityPerson.Update(people.Partner);
                    people.Partner.Partner = null;
                }

                people.Partner = partner;

                ValidationStatus validationStatus = entityPerson.Validate(people);
                if (validationStatus.Validated)
                {
                    entityPerson.Update(people);
                    Console.WriteLine(string.Format("Partner {0} was saved. ", people.FullName));
                }
                else
                {
                    foreach (var item in validationStatus.Messages)
                        Console.WriteLine(item);

                    return null;
                }

            }

            return people;
        }

        /// <summary>
        /// display the person on screen
        /// </summary>
        /// <param name="person"></param>
        static void DisplayRecord(People person)
        {
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("ID: {0}", person.ID));
            Console.WriteLine(string.Format("Full Name: {0}", person.FullName));
            Console.WriteLine(string.Format("Date of Birth: {0}", person.DateOfBirth.Value.ToString("dd/MM/yyyy")));
            Console.WriteLine(string.Format("Authorized: {0}", person.AllowRegistration.HasValue ? person.AllowRegistration.Value.ToString() : ""));
            Console.WriteLine(string.Format("Marital Status: {0}", person.MaritalStatus.ToString()));
            if (person.Partner != null)
                Console.WriteLine(string.Format("Parter: {0}-{1}", person.Partner.ID, person.Partner.FullName));
        }

        /// <summary>
        /// Collect Yes or No fom the user 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        static bool CollectYesNo(string message)
        {
            while (true)
            {
                Console.Write(message);
                string value = Console.ReadLine();
                if (value.Equals("Y", StringComparison.OrdinalIgnoreCase)) return true;
                if (value.Equals("N", StringComparison.OrdinalIgnoreCase)) return false;
            }
        }

        /// <summary>
        /// test performance
        /// </summary>
        static void InjectPeople()
        {
            Console.WriteLine("Injecting 100000 records...");
            var entityPerson = AppController.Get<IEntityPeople>();
            for (var i = 0; i < 100000; i++)
            {
                var obj = entityPerson.New();
                obj.FirstName = string.Format("name{0}", i);
                obj.Surname = string.Format("surname{0}", i);

                DateTime dt;
                if (DateTime.TryParseExact("01012000", new string[] { "ddMMyyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    obj.DateOfBirth = dt;
                }
                obj.AllowRegistration = true;
                entityPerson.Update(obj);
            }

            Console.WriteLine("Commiting 100000 records...");
            entityPerson.Commit();
        }

    }
}