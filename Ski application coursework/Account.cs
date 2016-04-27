using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ski_application_coursework
{

    public interface IAccount
    {

        //Set values.        
        /// <summary>
        /// Sets the user's address.
        /// </summary>
        /// <param name="enteredAddress">The address of the user.</param>
        /// <returns>A string containing issues with the information provided.</returns>
        string setAddress(string enteredAddress);
        /// <summary>
        /// Sets the user's age.
        /// </summary>
        /// <param name="enteredAge">The age of the user.</param>
        /// <returns>A string containing issues with the information provided.</returns>
        string setAge(int enteredAge);
        /// <summary>
        /// Sets the user's blood type.
        /// </summary>
        /// <param name="enteredBloodType">The bloodtype of the user.</param>
        void setBloodType(CelebrityAccount.bloodTypes enteredBloodType); //Void because can't possibly fail without throwing an exception.
        /// <summary>
        /// Sets the user child's name.
        /// </summary>
        /// <param name="enteredChildName">The name of the user's child.</param>
        /// <returns>A string containing issues with the information provided.</returns>
        string setChildName(string enteredChildName);
        /// <summary>
        /// Sets the user's name.
        /// </summary>
        /// <param name="enteredName">The name of the user.</param>
        /// <returns></returns>
        /// 
        string setName(string enteredName);
        /// <summary>
        /// Sets the user's score.
        /// </summary>
        /// <param name="enteredScore">The score which the user acheived.</param>
        /// <returns>A string containing issues with the information provided.</returns>
        string setScore(int enteredScore);
        /// <summary>
        /// Sets the user's sponsor.
        /// </summary>
        /// <param name="enteredSponsor">The sponsor of the user.</param>
        /// <returns>A string containing issues with the information provided.</returns>
        string setSponsor(string enteredSponsor);

        //Get values.
        /// <summary>
        /// Returns the user's ID.
        /// </summary>
        /// <returns>Int containing the user's ID.</returns>
        int getAccountNumber();
        /// <summary>
        /// Returns the user's account type.
        /// </summary>
        /// <returns>StandardAccount.accountType containing the users account type.</returns>
        StandardAccount.accountType getAccountType();
        /// <summary>
        /// Returns the user's address.
        /// </summary>
        /// <returns>String containing the user's address.</returns>
        string getAddress();
        /// <summary>
        /// Returns the user's score.
        /// </summary>
        /// <returns>Int containing the user's age (0 if not supplied).</returns>
        int getAge();
        /// <summary>
        /// Returns the user's bloodtype.
        /// </summary>
        /// <returns>CelebrityAccount.bloodTypes containing the user's bloodtype (CelebrityAccount.bloodTypes.notSupplied if not supplied).</returns>
        CelebrityAccount.bloodTypes getBloodType();
        /// <summary>
        /// Returns the user's child's name.
        /// </summary>
        /// <returns>String containing the user's child's name ("NotSupplied" if no child's name is supplied).</returns>
        string getChildName();
        /// <summary>
        /// Returns the user's entry cost.
        /// </summary>
        /// <returns>decimal containing the user's entry cost.</returns>
        decimal getEntryCost();
        /// <summary>
        /// Returns the user's name.
        /// </summary>
        /// <returns>String containing the user's name.</returns>
        string getName();
        /// <summary>
        /// Returns the user's score.
        /// </summary>
        /// <returns>Int containing the user's score (0 if not provided yet).</returns>
        int getScore();
        /// <summary>
        /// Returns the user's sponsor.
        /// </summary>
        /// <returns>String containing the user's sponsor ("NotSupplied" if no sponsor name is supplied).</returns>
        string getSponsor();

    }

    public class StandardAccount : IAccount
    {

        /// <summary>
        /// Consists of the different types of accounts available.
        /// </summary>
        public enum accountType
        {

            standardAccount,
            celebrityAccount,
            professionalAccount

        }

        //Data about this account.
        private int accountNumber;
        private int score = 0;
        private string name;
        private string address;
        private int age;
        protected decimal entryCost;
        public const int MAX_SCORE = 1000;
        public const int MIN_AGE = 8;
        public const int MAX_AGE = 80;
        protected accountType typeOfAccount;

        /// <summary>
        /// Constructer that creates a standard account with the information provided.
        /// </summary>
        /// <param name="constructorAccountNumber">The account ID.</param>
        /// <param name="constructorName">The name of the user.</param>
        /// <param name="constructorAddress">The address of the user.</param>
        /// <param name="constructorAge">The age of the user.</param>
        public StandardAccount(int constructorAccountNumber, string constructorName, string constructorAddress, int constructorAge = 0, int constructorScore = 0)
        {

            age = constructorAge;
            name = constructorName;
            address = constructorAddress;
            accountNumber = constructorAccountNumber;
            entryCost = 100;
            score = constructorScore;
            typeOfAccount = StandardAccount.accountType.standardAccount;

        }

        //Set info, method comments provided by IAccount interface.
        public string setAddress(string enteredAddress)
        {

            string exceptionToThrow = addressValidation(enteredAddress);

            if (exceptionToThrow == "")
            {

                address = enteredAddress;

            }

            return exceptionToThrow;

        }
        public string setAge(int enteredAge)
        {

            string exceptionToThrow = ageValidation(enteredAge);

            if (exceptionToThrow == "")
            {

                age = enteredAge;
                return exceptionToThrow;

            }

            return exceptionToThrow;

        }
        public virtual void setBloodType(CelebrityAccount.bloodTypes enteredBloodType)
        {
        }
        public virtual string setChildName(string enteredChildName)
        {

            return "";

        }
        public string setName(string enteredName)
        {

            string exceptionToThrow = nameValidation(enteredName);

            if (exceptionToThrow == "")
            {

                name = enteredName;

            }

            return exceptionToThrow;

        }
        public string setScore(int enteredScore)
        {

            if (scoreValidation(enteredScore) == "")
            {

                score = enteredScore;

            }

            return scoreValidation(enteredScore);

        }
        public virtual string setSponsor(string enteredSponsor)
        {

            return "";

        }

        //Get info, method comments provided by IAccount interface.
        public int getAccountNumber()
        {

            return accountNumber;

        }
        public accountType getAccountType()
        {

            return typeOfAccount;

        }
        public string getAddress()
        {

            return address;

        }
        public int getAge()
        {

            return age;

        }
        public virtual CelebrityAccount.bloodTypes getBloodType()
        {

            return CelebrityAccount.bloodTypes.notSupplied;

        }
        public virtual string getChildName()
        {

            return "notSupplied";

        }
        public decimal getEntryCost()
        {

            return entryCost;

        }
        public string getName()
        {

            return name;

        }
        public int getScore()
        {

            return score;

        }
        public virtual string getSponsor()
        {

            return "notSupplied";

        }

        //Validation checks.
        /// <summary>
        /// Checks whether an address fits set criteria.
        /// </summary>
        /// <param name="checkingName">String containing the address to be validated.</param>
        /// <returns>A string containing all failed criteria (empty if valid name)</returns>
        public static string addressValidation(string checkingAddress)
        {

            string[] splitAddress = checkingAddress.Trim().Split(' '); //Splits the address into parts.

            if (splitAddress.Count() > 1) //if(at least 2 words in the provided address)
            {

                return "";

            }

            //else
            return "Address not valid, must contain at least one line and a whitespace.\n";

        }
        /// <summary>
        /// Checks whether an age fits set criteria.
        /// </summary>
        /// <param name="checkingName">Int containing the age to be validated.</param>
        /// <returns>A string containing all failed criteria (empty if valid name)</returns>
        public static string ageValidation(int checkingAge)
        {

            if (checkingAge <= MAX_AGE && checkingAge >= MIN_AGE || checkingAge == 0) //if(age is within acceptable range or 0 (not provided))
            {

                return "";

            }

            //else
            return "Age must be within range " + MIN_AGE + " - " + MAX_AGE + ".\n";


        }
        /// <summary>
        /// Checks whether a name fits set criteria.
        /// </summary>
        /// <param name="checkingName">String containing the name to be validated.</param>
        /// <returns>A string containing all failed criteria (empty if valid name).</returns>
        public static string nameValidation(string checkingName)
        {

            string returnString = ""; //Accumulating string of errors with the name.

            string[] names = checkingName.Split(' '); //Splits the name up into individual parts (first name, surname etc...).

            if (names.Count() < 2) //if(the person hasn't provided at least a first and surname)
            {

                returnString += ("Name \"" + checkingName + "\" must contain at least two indentifyable names.\n"); //Add error informing them that this is not allowed the the return string.

            }

            foreach (string singleName in names) //Cycle through every provided name.
            {

                if (singleName.Length <= 1) //if(a given name isn't at least 2 characters)
                {

                    returnString += ("Name \"" + singleName + "\" must be at least two characters long.\n"); //Add error informing them that this is not allowed the the return string.

                }

            }

            return returnString; //Return string of errors.

        }
        /// <summary>
        /// Checks whether a score fits set criteria.
        /// </summary>
        /// <param name="checkingName">Int containing the score to be validated.</param>
        /// <returns>A string containing all failed criteria (empty if valid name)</returns>
        public static string scoreValidation(int checkingScore)
        {

            if (checkingScore <= MAX_SCORE && checkingScore >= 0) //if(score is within acceptable range)
            {

                return "";

            }

            //else
            return "Score must be between 0 and " + MAX_SCORE + " inclusive.\n";


        }

    }

    public class CelebrityAccount : StandardAccount, IAccount
    {

        /// <summary>
        /// Consists of the different types of blood available.
        /// </summary>
        public enum bloodTypes
        {

            bloodTypeA,
            bloodTypeAB,
            bloodTypeB,
            bloodTypeO,
            notSupplied

        }

        //Extra data about this account type.
        private bloodTypes bloodType;
        private string childName;

        /// <summary>
        /// Constructer that creates a celebrity account with the information provided.
        /// </summary>
        /// <param name="constructorAccountNumber">The account ID.</param>
        /// <param name="constructorName">The name of the user.</param>
        /// <param name="constructorAddress">The address of the user.</param>
        /// <param name="constructorAge">The age of the user.</param>
        /// <param name="constructorBloodType">The bloodtype of the user.</param>
        /// <param name="constructorChildName">The name of the user's child.</param>
        public CelebrityAccount(int constructorAccountNumber, string constructorName, string constructorAddress, string constructorChildName, bloodTypes constructorBloodType, int constructorAge = 0, int constructorScore = 0) :
            base(constructorAccountNumber, constructorName, constructorAddress, constructorAge, constructorScore)
        {

            childName = constructorChildName;
            bloodType = constructorBloodType;
            entryCost = 0;
            typeOfAccount = StandardAccount.accountType.celebrityAccount;

        }

        //Get info, method comments provided by IAccount interface.
        public override bloodTypes getBloodType()
        {

            return bloodType;

        }
        public override string getChildName()
        {

            return childName;

        }

        //Set info, method comments provided by IAccount interface.
        public override void setBloodType(bloodTypes enteredBloodType)
        {

            bloodType = enteredBloodType;

        }
        public override string setChildName(string enteredChildName)
        {

            string exceptionToThrow = StandardAccount.nameValidation(enteredChildName);

            if (exceptionToThrow == "")
            {

                childName = enteredChildName;

            }

            return exceptionToThrow;

        }

    }

    public class ProfessionalAccount : StandardAccount, IAccount
    {

        //Extra data about this account type.
        string sponsor;

        /// <summary>
        /// Constructer that creates a professional account with the information provided.
        /// </summary>
        /// <param name="constructorAccountNumber">The account ID.</param>
        /// <param name="constructorName">The name of the user.</param>
        /// <param name="constructorAddress">The address of the user.</param>
        /// <param name="constructorAge">The age of the user.</param>
        /// <param name="constructorSponsor">The sponser of the user.</param>
        public ProfessionalAccount(int constructorAccountNumber, string constructorName, string constructorAddress, string constructorSponsor, int constructorAge = 0, int constructorScore = 0) :
            base(constructorAccountNumber, constructorName, constructorAddress, constructorAge, constructorScore)
        {

            sponsor = constructorSponsor;
            entryCost = 200;
            typeOfAccount = StandardAccount.accountType.professionalAccount;

        }

        //Get info, method comments provided by IAccount interface.
        public override string getSponsor()
        {

            return sponsor;

        }

        //Set info, method comments provided by IAccount interface.
        public override string setSponsor(string enteredSponsor)
        {

            string exceptionToThrow = ProfessionalAccount.sponsorValidation(enteredSponsor);

            if (exceptionToThrow == "")
            {

                sponsor = enteredSponsor;

            }

            return exceptionToThrow;

        }

        /// <summary>
        /// Checks whether a sponsor name fits set criteria.
        /// </summary>
        /// <param name="checkingName">String containing the sponsor name to be validated.</param>
        /// <returns>A string containing all failed criteria (empty if valid name)</returns>
        public static string sponsorValidation(string checkingSponsor)
        {

            if (checkingSponsor.Length < 2) //if(sponsor is less than 2 characters)
            {

                return "Sponsors must contain at least two characters.\n";

            }

            //else
            return "";

        }

    }

}
