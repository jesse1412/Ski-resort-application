using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ski_application_coursework
{

    class AccountStorage
    {

        private List<IAccount> accountList = new List<IAccount>(); //List of accounts.

        /// <summary>
        /// Adds an ametuer account.
        /// </summary>
        /// <param name="constructorName">User's name.</param>
        /// <param name="constructorAddress">User's address.</param>
        /// <param name="constructorAge">User's age.</param>
        /// <param name="constructorScore">User's score.</param>
        public void addAccount(string constructorName, string constructorAddress, int constructorAge = 0, int constructorScore = 0)
        {

            genericAccountCreationExceptionHandler(constructorName, constructorAddress, constructorAge);

            accountList.Add(new StandardAccount(accountList.Count(), constructorName, constructorAddress, constructorAge, constructorScore));

        }
        /// <summary>
        /// Adds a celebrity account.
        /// </summary>
        /// <param name="constructorName">User's name.</param>
        /// <param name="constructorAddress">User's address.</param>
        /// <param name="constructorBloodtype">user's bloodtype.</param>
        /// <param name="constructorChildName">user's child's name.</param>
        /// <param name="constructorAge">User's age.</param>
        /// <param name="constructorScore">User's score.</param>
        public void addAccount(string constructorName, string constructorAddress, CelebrityAccount.bloodTypes constructorBloodtype, string constructorChildName, int constructorAge = 0, int constructorScore = 0)
        {

            genericAccountCreationExceptionHandler(constructorName, constructorAddress, constructorAge, constructorScore, StandardAccount.nameValidation(constructorChildName));

            accountList.Add(new CelebrityAccount(accountList.Count(), constructorName, constructorAddress, constructorChildName, constructorBloodtype, constructorAge, constructorScore));

        }
        /// <summary>
        /// Adds a professional account.
        /// </summary>
        /// <param name="constructorName">User's name.</param>
        /// <param name="constructorAddress">User's address.</param>
        /// <param name="constructorSponsor">User's sponsor.</param>
        /// <param name="constructorAge">User's age.</param>
        /// <param name="constructorScore">User's score.</param>
        public void addAccount(string constructorName, string constructorAddress, string constructorSponsor, int constructorAge = 0, int constructorScore = 0)
        {

            genericAccountCreationExceptionHandler(constructorName, constructorAddress, constructorAge, constructorScore, ProfessionalAccount.sponsorValidation(constructorSponsor));

            accountList.Add(new ProfessionalAccount(accountList.Count(), constructorName, constructorAddress, constructorSponsor, constructorAge, constructorScore));

        }

        /// <summary>
        /// Throws an exception if invalid data is entered, data should already be verified by this point.
        /// </summary>
        /// <param name="constructorName">User's name.</param>
        /// <param name="constructorAddress">User's address.</param>
        /// <param name="constructorAge">User's age.</param>
        /// <param name="constructorScore">User's score.</param>
        /// <param name="specializedExceptions">Exception strings handed down for non ameruer classes which are to be added on to generic exceptions.</param>
        private void genericAccountCreationExceptionHandler(string constructorName, string constructorAddress, int constructorAge = 0, int constructorScore = 0, string specializedExceptions = "")
        {

            string exceptionToThrow = specializedExceptions; //Start by taking specialized exceptions from non-ametuer checks and adding it to the thrown exception.

            //Add exception information using the standard validation methods.
            exceptionToThrow += StandardAccount.nameValidation(constructorName);
            exceptionToThrow += StandardAccount.addressValidation(constructorAddress);
            exceptionToThrow += StandardAccount.ageValidation(constructorAge);

            if (exceptionToThrow != "") //if(there is an error)
            {

                throw new Exception(exceptionToThrow);

            }

        }

        /// <summary>
        /// Returns an account of type account.
        /// </summary>
        /// <param name="accountID">ID of the account to be fetched.</param>
        /// <returns>The desired account (if found).</returns>
        public IAccount getAccount(int accountID)
        {

            //No in range checks, if the program asks for an out of range account number then the user isn't being guided/restricted well enough.
            return accountList[accountID];

        }
        /// <summary>
        /// Gets the amount of accounts stored.
        /// </summary>
        /// <returns>Account count.</returns>
        public int getAccountsCount()
        {

            return accountList.Count;

        }
        /// <summary>
        /// Gets the range of ages.
        /// </summary>
        /// <returns>A string containing the age range "(x - y)".</returns>
        public string getAgeRange()
        {

            int lowAge = StandardAccount.MAX_AGE;
            int highAge = StandardAccount.MIN_AGE;
            int checkingAge;

            foreach (IAccount account in accountList) //Cycle through all accounts.
            {

                checkingAge = account.getAge();

                if (checkingAge > 0) //if(the given user has entered an age)
                {

                    if (checkingAge > highAge) //if(the current checking age is greater than the previous highest age).
                    {

                        highAge = checkingAge; //Set highest age to current checking age.

                    }

                    if (checkingAge < lowAge) //if(the current checking age is less than the previous lowest age).
                    {

                        lowAge = checkingAge; //Set lowest age to current checking age.

                    }

                }

            }

            return lowAge + " to " + highAge; //Combine age range into one string and return.

        }
        /// <summary>
        /// Gets the amount of accounts of each respective type seperated out onto three lines.
        /// </summary>
        /// <returns>Three line string containing the amount of each account type.</returns>
        public string getAmountOfEachAccountType()
        {

            //Current count of each account type.
            int standardCount = 0;
            int professionalCount = 0;
            int celebrityCount = 0;

            foreach (IAccount account in accountList) //Cycle through every account.
            {

                if (account.getAccountType() == StandardAccount.accountType.standardAccount) //if(the current account is a standard account)
                {

                    standardCount++;

                }

                else if (account.getAccountType() == StandardAccount.accountType.professionalAccount) //if(the current account is a professional account)
                {

                    professionalCount++;

                }

                else //if(the current account is a celebrity account)
                {

                    celebrityCount++;

                }

            }

            return "Standard: " + standardCount.ToString() + "\nProfessional: " + professionalCount.ToString() + "\nCelebrity: " + celebrityCount.ToString(); //Combine all the information and return.

        }
        /// <summary>
        /// Gets the average age of competitors.
        /// </summary>
        /// <returns>String containing the average age of competitors.</returns>
        public string getAverageAge()
        {

            int totalAge = 0;
            int accountsWithAge = 0;

            foreach (IAccount account in accountList) //Cycle through every account.
            {

                if (account.getAge() > 0) //if(the user has actually entered their age)
                {

                    totalAge += account.getAge();
                    accountsWithAge++;

                }

            }

            if (accountsWithAge > 0) //if(there are accounts that provided their age (prevents divide by 0))
            {

                return Math.Round((totalAge + 0.5) / accountsWithAge).ToString(); //Return totalAge/accountsWithAge rounded.

            }

            else //if(no accounts provided their age)
            {

                return "No ages disclosed";

            }

        }
        /// <summary>
        /// Gets the most common age.
        /// </summary>
        /// <returns>String containing the modal age.</returns>
        public string getModalAge()
        {

            int[] ageCounts = new int[StandardAccount.MAX_AGE + 1]; //Array containing a slot for each year within the valid range. Each slot will be incremented to represent the amount of people of that age.

            foreach(IAccount account in accountList) //Cycle through every account.
            {

                ageCounts[account.getAge()]++; //Add one to the slot in the age count array that represents the count for a given year.

            }

            int currentModalAge = 0; //The current modal age.
            int currentModalAgeAmount = 0; //The amount of people with the current modal age.

            for(int i = StandardAccount.MIN_AGE; i < ageCounts.Count(); i++) //Cycle through the array of age counts.
            {

                if(ageCounts[i] > currentModalAgeAmount) //if(the currently checking age is more popular than the previously thought most popular age)
                {

                    currentModalAgeAmount = ageCounts[i]; //Set the most popular count to the newly found most popular count.
                    currentModalAge = i; //Set the currently most popluar age to the newly found most popular age.

                }

            }

            return currentModalAge.ToString(); //Return the most popular age.

        }
        /// <summary>
        /// Gets a string consisting of the top 3 scores from each account type seperated out onto three lines.
        /// </summary>
        /// <returns>Three line string containing the three top scores of each account type.</returns>
        public string getTopThreeCompetitors()
        {

            //Lists of scores for accounts of respective types. These are used in order to sort the scores into order.
            List<int> amatuerScores = new List<int>();
            List<int> professionalScores = new List<int>();
            List<int> celebrityScores = new List<int>();

            string returnString = "Amats: "; //String containing the information.

            foreach(IAccount account in accountList) //Cycle through all accounts.
            {

                if(account.getAccountType() == StandardAccount.accountType.standardAccount) //if(account currently being cycled is a standard account)
                {

                    amatuerScores.Add(account.getScore()); //Add the score of the account to the list of amatuer scores.

                }

                else if (account.getAccountType() == StandardAccount.accountType.professionalAccount) //else if(account currently being cycled is a professional account)
                { 

                    professionalScores.Add(account.getScore()); //Add the score of the account to the list of professional scores.

                }

                else //if(account currently being cycled is a celebrity account)
                {

                    celebrityScores.Add(account.getScore()); //Add the score of the account to the list of celebrity scores.

                }

            }

            //Sort each list of scores respectively.
            amatuerScores.Sort();
            professionalScores.Sort();
            celebrityScores.Sort();

            for (int i = amatuerScores.Count - 1; i >= amatuerScores.Count - 3; i--) //Cycle through the 0 - 3 best ametuer scores.
            {

                if(i < 0) //if(there are 0 ametuer scores, end the loop)
                {

                    break;

                }

                returnString += amatuerScores[i] + ", "; //Add the score to the return string.

            }

            //else
            returnString += "\nProfs: "; //Add professional line.

            for (int i = professionalScores.Count - 1; i >= professionalScores.Count - 3; i--) //Cycle through the 0 - 3 best professional scores.
            {

                if (i < 0) //if(there are 0 professional scores, end the loop)
                {

                    break;

                }

                //else
                returnString += professionalScores[i] + ", "; //Add the score to the return string.

            }

            returnString += "\nCelebs: "; //Add celebrity line.

            for (int i = celebrityScores.Count - 1; i >= celebrityScores.Count - 3; i--)
            {

                if (i < 0) //if(there are 0 celebrity scores, end the loop)
                {

                    break;

                }

                //else
                returnString += celebrityScores[i] + ", "; //Add the score to the return string.

            }

            return returnString;

        }
        /// <summary>
        /// Gets the accumulated score of all contestants.
        /// </summary>
        /// <returns>String of the value of all competitor scores accumulated.</returns>
        public string getTotalScores()
        {

            int totalScore = 0;

            foreach(IAccount account in accountList) //Cycle through every account.
            {

                totalScore += account.getScore(); //Add the score from the current account to the total.

            }

            return totalScore.ToString();

        }
        /// <summary>
        /// Gets the total income from competitors.
        /// </summary>
        /// <returns>String containing the profit earned across all competitors.</returns>
        public string getTotalIncome()
        {

            Decimal income = 0;

            foreach (IAccount account in accountList) //Loop through each account.
            {

                income += account.getEntryCost(); //Add the entry cost of the current account to the total.

            }

            return income.ToString();

        }

        /// <summary>
        /// Generates 120 test accounts. Screw this method.
        /// </summary>
        public void generateTestData()
        {

            Random rand = new Random();

            int randType;

            int randNameLength1;
            int randNameLength2;
            string randName;

            int randScore;

            string randChildName;

            int randAddressLength;
            string randAddress;

            int randSponsorLength;
            string randSponsor;

            int randAge;

            CelebrityAccount.bloodTypes randBloodType;

            char[] consonants = new char[21];

            char[] vowels = new char[5];

            string[] placeEndings = new string[5];

            string[] sponsorEndings = new string[5];

            CelebrityAccount.bloodTypes[] bloodTypes = new CelebrityAccount.bloodTypes[4];

            consonants[0] = 'b';
            consonants[1] = 'c';
            consonants[2] = 'd';
            consonants[3] = 'f';
            consonants[4] = 'g';
            consonants[5] = 'h';
            consonants[6] = 'j';
            consonants[7] = 'k';
            consonants[8] = 'l';
            consonants[9] = 'm';
            consonants[10] = 'n';
            consonants[11] = 'p';
            consonants[12] = 'q';
            consonants[13] = 'r';
            consonants[14] = 's';
            consonants[15] = 't';
            consonants[16] = 'v';
            consonants[17] = 'w';
            consonants[18] = 'x';
            consonants[19] = 'y';
            consonants[20] = 'z';

            vowels[0] = 'a';
            vowels[1] = 'e';
            vowels[2] = 'i';
            vowels[3] = 'o';
            vowels[4] = 'u';

            placeEndings[0] = " road";
            placeEndings[1] = " ville";
            placeEndings[2] = " street";
            placeEndings[3] = " avenue";
            placeEndings[4] = " close";

            sponsorEndings[0] = " industries";
            sponsorEndings[1] = " & co";
            sponsorEndings[2] = " limited";
            sponsorEndings[3] = " corp";
            sponsorEndings[4] = " studios";

            bloodTypes[0] = CelebrityAccount.bloodTypes.bloodTypeA;
            bloodTypes[1] = CelebrityAccount.bloodTypes.bloodTypeB;
            bloodTypes[2] = CelebrityAccount.bloodTypes.bloodTypeAB;
            bloodTypes[3] = CelebrityAccount.bloodTypes.bloodTypeO;

            for (int i = 0; i < 120; i++)
            {

                randName = "";
                randAddress = "";
                randSponsor = "";

                randScore = rand.Next(0, 1001);

                randType = rand.Next(0, 3);

                randAge = rand.Next(0, StandardAccount.MAX_AGE + 1);

                randNameLength1 = rand.Next(1, 4) * 2 + 1;
                randNameLength2 = rand.Next(1, 4) * 2 + 1;

                randAddressLength = rand.Next(2, 6) * 2 + 1;

                randSponsorLength = rand.Next(1, 6) * 2 + 1;

                if (randAge < StandardAccount.MIN_AGE)
                {

                    randAge = 0;

                }

                for(int j = 0; j < randNameLength1; j++)
                {

                    if(j % 2 == 0)
                    {

                        randName += consonants[rand.Next(0, 21)];

                    }

                    else
                    {

                        randName += vowels[rand.Next(0, 5)];

                    }

                }

                randName += " ";

                for (int j = 0; j < randNameLength2; j++)
                {

                    if (j % 2 == 0)
                    {

                        randName += consonants[rand.Next(0, 5)];

                    }

                    else
                    {

                        randName += vowels[rand.Next(0, 5)];

                    }

                }

                for (int j = 0; j < randAddressLength; j++)
                {

                    if (j % 2 == 0)
                    {

                        randAddress += consonants[rand.Next(0, 5)];

                    }

                    else
                    {

                        randAddress += vowels[rand.Next(0, 5)];

                    }

                }

                randAddress += placeEndings[rand.Next(0, 5)];

                //Standard
                if (randType == 0)
                {

                    this.addAccount(randName, randAddress, randAge);

                }

                //Professional
                else if(randType == 1)
                {

                    for (int j = 0; j < randSponsorLength; j++)
                    {

                        if (j % 2 == 0)
                        {

                            randSponsor += consonants[rand.Next(0, 5)];

                        }

                        else
                        {

                            randSponsor += vowels[rand.Next(0, 5)];

                        }

                    }

                    randSponsor += sponsorEndings[rand.Next(0, 5)];

                    this.addAccount(randName, randAddress, randSponsor, randAge);

                }

                //Celebrity
                else
                {

                    randBloodType = bloodTypes[rand.Next(0,4)];

                    randChildName = "";

                    for (int j = 0; j < randNameLength1; j++)
                    {

                        if (j % 2 == 0)
                        {

                            randChildName += consonants[rand.Next(0, 5)];

                        }

                        else
                        {

                            randChildName += vowels[rand.Next(0, 5)];

                        }

                    }

                    randChildName += (" " + randName.Split(' ')[1]);

                    this.addAccount(randName, randAddress, randBloodType, randChildName, randAge);

                }

                this.getAccount(this.getAccountsCount() - 1).setScore(randScore);

            }

        }

        /// <summary>
        /// Saves all currently entered accounts.
        /// </summary>
        /// <param name="directory">Directory to save account to.</param>
        /// <returns>Whether the save was successful.</returns>
        public bool saveAccounts(string directory)
        {

            StreamWriter writer = new StreamWriter("Accounts.txt");

            foreach(IAccount account in accountList) //Cycle through each account.
            {

                if (account.getAccountType() == StandardAccount.accountType.standardAccount) //if(the account is a standard account)
                {

                    writer.WriteLine(account.getAccountType().ToString() + "<" + account.getName() + "<" + account.getAddress() + "<" + account.getAge() + "<" + account.getScore()); //Write the account using standard format.

                }

                else if(account.getAccountType() == StandardAccount.accountType.celebrityAccount) //if(the account is a celebrity account)
                {

                    writer.WriteLine(account.getAccountType().ToString() + "<" + account.getName() + "<" + account.getAddress() + "<" + account.getAge() + "<" + account.getBloodType().ToString() + "<" + account.getChildName() + "<" + account.getScore()); //Write the account using celebrity format.

                }

                else //if(the account is a professional account)
                {

                    writer.WriteLine(account.getAccountType().ToString() + "<" + account.getName() + "<" + account.getAddress() + "<" + account.getAge() + "<" + account.getSponsor() + "<" + account.getScore()); //Write the account using professional format.

                }

            }

            writer.Close();
            return true;

        }
        /// <summary>
        /// Load accounts from the current directory.
        /// </summary>
        /// <returns>Whether the load was successful.</returns>
        public bool loadAccounts()
        {

            accountList = new List<IAccount>();

            if(!File.Exists(Directory.GetCurrentDirectory() + "\\Accounts.txt")) //if(no account file in current directory, create an empty one)
            {

                StreamWriter writer = new StreamWriter("Accounts.txt");

                writer.Write("");

                writer.Close();

            }

            StreamReader reader = new StreamReader("Accounts.txt");

            string currentLine = ""; //string of the line currently being read.

            string[] info; //Current lines split up into parts at each '<' character, allows data to be seperated and read easily.

            while((currentLine = reader.ReadLine()) != null) //While there are still lines to read.
            {

                info = currentLine.Split('<'); //Split the line into its constituent parts.

                if(info[0] == StandardAccount.accountType.standardAccount.ToString()) //if(the account on the current line is a standard account.
                {

                    this.addAccount(info[1], info[2], int.Parse(info[3]), int.Parse(info[4])); //Add a standard account to the account list using the information provided.

                }

                else if (info[0] == StandardAccount.accountType.celebrityAccount.ToString()) //if(the account on the current line is a celebrity account.
                {

                    CelebrityAccount.bloodTypes bloodType;

                    Enum.TryParse(info[4], out bloodType); //Convert the string representation of the blood type into the required enum format.

                    this.addAccount(info[1], info[2], bloodType, info[5], int.Parse(info[3]), int.Parse(info[6])); //Add a celebrity account to the account list using the information provided.

                }

                else //if(the account on the current line is a professional account.
                {

                    this.addAccount(info[1], info[2], info[4], int.Parse(info[3]), int.Parse(info[5])); //Add a professional account to the account list using the information provided.

                }

            }

            reader.Close();
            return true;

        }
        /// <summary>
        /// Deletes all accounts.
        /// </summary>
        public void clearAccounts()
        {

            accountList = new List<IAccount>();

        }

    }

}
