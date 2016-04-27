using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;

namespace Ski_application_coursework
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //The account that we are updating/reading.
        IAccount currentlyEdittingAccount;

        //The class where all accounts are stored/managed.
        AccountStorage accounts;

        public MainWindow()
        {

            InitializeComponent();

            accounts = new AccountStorage();
            accounts.loadAccounts();

            NameTextBox.IsEnabled = false;
            AgeTextBox.IsEnabled = false;
            AddressTextBox.IsEnabled = false;
            SponsorBox.IsEnabled = false;
            NextOfKinBox.IsEnabled = false;
            bloodComboBox.IsEnabled = false;
            ScoreTextBox.IsEnabled = false;
            SaveButton.IsEnabled = false;

            SponsorTextBox.IsEnabled = false;
            newBloodComboBox.IsEnabled = false;
            newBloodComboBox.SelectedIndex = 0;
            bloodComboBox.SelectedIndex = 4;
            NextOfKinTypeTextBox.IsEnabled = false;

        }

        /// <summary>
        /// Prepares the account for editting and displays the current account data.
        /// </summary>
        /// <param name="accountID">The ID of the account which you wish to load.</param>
        private void loadAccount(int accountID)
        {

            currentlyEdittingAccount = accounts.getAccount(accountID);

            NameTextBox.Text = currentlyEdittingAccount.getName();
            edittingAge();
            edittingSponsor();
            AddressTextBox.Text = currentlyEdittingAccount.getAddress();
            edittingNextOfKin();
            edittingBloodType();
            ScoreTextBox.Text = currentlyEdittingAccount.getScore().ToString();
            CompetitorNumberTextBox.Text = accountID.ToString();

            SaveButton.IsEnabled = true;
            NameTextBox.IsEnabled = true;
            AddressTextBox.IsEnabled = true;
            ScoreTextBox.IsEnabled = true;

        }

        /// <summary>
        /// Searches through the list of stored accounts for users who's name contains the given string.
        /// </summary>
        /// <param name="searchingName">The string that to search for within names.</param>
        /// <returns></returns>
        private int searchByName(string searchingName)
        {

            List<int> matchedIDs = new List<int>(); //List of IDs of accounts that have names which match with the search string.

            for (int i = 0; i < accounts.getAccountsCount(); i++) //Lood through each account.
            {

                if (accounts.getAccount(i).getName().Contains(searchingName)) //If(the account's name contains the search string)
                {

                    matchedIDs.Add(i); //Add the account ID to the list of matches.

                }

            }

            if (matchedIDs.Count == 1) //If (there's only one match)
            {

                return matchedIDs[0]; //Return that value, no need for list of selectable accounts.

            }

            else if (matchedIDs.Count == 0) //If(no matches)
            {

                return -1; //return -1, an unobtainable ID.

            }

            else //If (multiple matches)
            {

                List<nameIDAndAddress> matchedNameIDAndAddress = new List<nameIDAndAddress>(); //List of matched accounts containing their addresses and names.

                IAccount currentlyLoadingAccount; //The account that is being loaded in the next loop.

                foreach (int ID in matchedIDs) //Cycle through all matched IDs.
                {

                    currentlyLoadingAccount = accounts.getAccount(ID);
                    matchedNameIDAndAddress.Add(new nameIDAndAddress(currentlyLoadingAccount.getName(), currentlyLoadingAccount.getAddress(), ID)); //Add the matched accounts into a new list containing their address and names.

                }

                var nameSearchWindow = new NameSearchPage(matchedNameIDAndAddress); //Declare the new window to be opened and sending it the list of accounts.
                nameSearchWindow.ShowDialog(); //Show the new window.
                return selectedAccountID.selectedAccount; //Return the selected account ID from the new window.

            }

        }

        /// <summary>
        /// Checks that all of the values relating to the provided account type are valid.
        /// </summary>
        /// <param name="name">Name of the user.</param>
        /// <param name="address">Adress of the user.</param>
        /// <param name="age">Age of the user.</param>
        /// <param name="accountType">Account type of the user.</param>
        /// <param name="sponsor">Sponsor of the user.</param>
        /// <param name="bloodType">Blood type of the user.</param>
        /// <param name="nextOfKin">Name of the user's child.</param>
        /// <returns>A string containing information about invalid data. Returns empty string if completely valid ("").</returns>
        private string accountValidityCheck(string name, string address, string age, StandardAccount.accountType accountType, string sponsor, CelebrityAccount.bloodTypes bloodType, string nextOfKin)
        {

            string returnString = ""; //Accumulating string of errors.
            int parsedAge;

            returnString += StandardAccount.nameValidation(name); //Add issues with the provided name to the string of errors.

            if (age == "") //Empty if statement to prevent integer parsing issues.
            {

                //This is a valid empty age with an assumed value of 0. 

            }

            else if (int.TryParse(age, out parsedAge)) //else if(age can be parsed)
            {

                returnString += StandardAccount.ageValidation(parsedAge); //Add issues with the provided age to the string of errors.

            }

            else
            {

                returnString += "Age not valid, only numerics 0-9 accepted.\n"; //Add error due to un-parsable age value.

            }

            returnString += StandardAccount.addressValidation(address); //Add issues with the provided address to the string of errors.

            if (accountType == StandardAccount.accountType.professionalAccount)
            {

                returnString += ProfessionalAccount.sponsorValidation(sponsor); //Add issues with the provided sponsor to the string of errors.

            }

            else if (accountType == StandardAccount.accountType.celebrityAccount)
            {

                returnString += StandardAccount.nameValidation(nextOfKin); //Add issues with the provided next of kin to the string of errors.

                if (bloodType == CelebrityAccount.bloodTypes.notSupplied)
                {

                    returnString += "Bloodtype \"NotSupplied\" is reserved for non-celebrity accounts."; //Add issues with the provided blood type to the string of errors.

                }

            }

            return returnString; //Return all the errors found (empty string if completely valid account).

        }

        /// <summary>
        /// Restricts the entered string to the list of characters provided.
        /// </summary>
        /// <param name="checkString">The string to be checked for matches.</param>
        /// <param name="regexString">The characters that are "allowed".</param>
        /// <returns>The checked string with any non-allowed characters removed.</returns>
        private string stringRestriction(string checkString, string regexString = "[^A-z ]")
        {

            string returnString = "";

            //Cycle through each character in the string.
            for (int i = 0; i < checkString.Count(); i++)
            {

                //If the current character being checked is valid, add it to the return string. If it isn't valid, it will not be added to the return string.
                if (!new Regex(regexString).Match(checkString[i].ToString()).Success)
                {

                    returnString += checkString[i];

                }

            }

            //Return all valid characters in their original order.
            return returnString;

        }

        /// <summary>
        /// Disabled fields related to other account types.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AmateurCheckBox_Click(object sender, RoutedEventArgs e)
        {

            disableCelebrityCreation();
            disableProfessionalCreation();

        }
        /// <summary>
        /// Disabled fields related to other account types. Enables nextofkin box and bloodtype drop down list which are related to celebrities.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CelebrityCheckBox_Click(object sender, RoutedEventArgs e)
        {

            NextOfKinTypeTextBox.IsEnabled = true;
            newBloodComboBox.IsEnabled = true;

            disableProfessionalCreation();

        }
        /// <summary>
        /// Disabled fields related to other account types. Enabled sponsor box that is related to professionals.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfessionalCheckBox_Click(object sender, RoutedEventArgs e)
        {

            SponsorTextBox.IsEnabled = true;

            disableCelebrityCreation();

        }

        /// <summary>
        /// Enables or disables fields regarding a given account's age.
        /// </summary>
        private void edittingAge()
        {

            int age = currentlyEdittingAccount.getAge();

            if (age == 0)
            {

                AgeTextBox.Text = "notSupplied";
                AgeTextBox.IsEnabled = false;

            }

            else
            {

                AgeTextBox.Text = age.ToString();
                AgeTextBox.IsEnabled = true;

            }

        }
        /// <summary>
        /// Enables or disables fields regarding a given account's blood type depending on the account type.
        /// </summary>
        private void edittingBloodType()
        {

            CelebrityAccount.bloodTypes bloodType = currentlyEdittingAccount.getBloodType();

            if (currentlyEdittingAccount.getAccountType() == StandardAccount.accountType.celebrityAccount)
            {

                bloodComboBox.IsEnabled = true;

            }

            else
            {

                bloodComboBox.IsEnabled = false;

            }

            if (bloodType == CelebrityAccount.bloodTypes.bloodTypeA)
            {

                bloodComboBox.SelectedIndex = 0;

            }

            else if (bloodType == CelebrityAccount.bloodTypes.bloodTypeAB)
            {

                bloodComboBox.SelectedIndex = 1;

            }

            else if (bloodType == CelebrityAccount.bloodTypes.bloodTypeB)
            {

                bloodComboBox.SelectedIndex = 2;

            }

            else if (bloodType == CelebrityAccount.bloodTypes.bloodTypeO)
            {

                bloodComboBox.SelectedIndex = 3;

            }

            else
            {

                bloodComboBox.SelectedIndex = 4;

            }

        }
        /// <summary>
        /// Enables or disables fields regarding a given account's next of kin depending on the account type.
        /// </summary>
        private void edittingNextOfKin()
        {

            if (currentlyEdittingAccount.getChildName() == "notSupplied")
            {

                NextOfKinBox.IsEnabled = false;

            }

            else
            {

                NextOfKinBox.IsEnabled = true;

            }

            NextOfKinBox.Text = currentlyEdittingAccount.getChildName();

        }
        /// <summary>
        /// Enables or disables fields regarding a given account's sponsor depending on the account type.
        /// </summary>
        private void edittingSponsor()
        {

            if (currentlyEdittingAccount.getSponsor() == "notSupplied")
            {

                SponsorBox.IsEnabled = false;

            }

            else
            {

                SponsorBox.IsEnabled = true;

            }

            SponsorBox.Text = currentlyEdittingAccount.getSponsor();

        }

        /// <summary>
        /// Disables blood type drop down list and next of kin textbox associated with the celebrity account type.
        /// </summary>
        private void disableCelebrityCreation()
        {

            newBloodComboBox.IsEnabled = false;
            NextOfKinTypeTextBox.IsEnabled = false;

        }
        /// <summary>
        /// Disables sponsor textbox associated with the professional account type.
        /// </summary>
        private void disableProfessionalCreation()
        {

            SponsorTextBox.IsEnabled = false;

        }

        /// <summary>
        /// Adds a competitor with the information provided.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCompetitorButton_Click(object sender, RoutedEventArgs e)
        {

            string errorsToShow; //Accumulation of issues with entered data that is to be displayed if needed.

            //Values entered by the user.
            string name = NewNameTextBox.Text;
            string age = NewAgeTextBox.Text;
            string address = NewAddressTextBox.Text;
            string sponsor = SponsorTextBox.Text;
            string nextOfKin = NextOfKinTypeTextBox.Text;
            CelebrityAccount.bloodTypes bloodType;
            Enum.TryParse(newBloodComboBox.Text, out bloodType);

            int ageInt; //integer verson of age.

            if (!int.TryParse(age, out ageInt) && age == "") //Parses the age string to an integer, if the age box is empty then set the age to an assumed value of 0.
            {

                ageInt = 0;

            }

            StandardAccount.accountType accountType;

            if (AmateurCheckBox.IsChecked == true)
            {

                accountType = StandardAccount.accountType.standardAccount;

            }

            else if (ProfessionalCheckBox.IsChecked == true)
            {

                accountType = StandardAccount.accountType.professionalAccount;

            }

            else
            {

                accountType = StandardAccount.accountType.celebrityAccount;

            }

            if ((errorsToShow = accountValidityCheck(name, address, age, accountType, sponsor, bloodType, nextOfKin)) == "") //If (there are no errors to return for an account with the given information.)
            {

                if (accountType == StandardAccount.accountType.standardAccount) //Create standard account.
                {

                    accounts.addAccount(name, address, ageInt);

                }

                else if (accountType == StandardAccount.accountType.professionalAccount) //Create professional account.
                {

                    accounts.addAccount(name, address, sponsor, ageInt);

                }

                else //Create celebrity account.
                {

                    accounts.addAccount(name, address, bloodType, nextOfKin, ageInt);

                }

                loadAccount(accounts.getAccountsCount() - 1); //Load the account into the editting section for easy display.

                MessageBox.Show("Account created. Your ID is: " + (accounts.getAccountsCount() - 1)); //Show the user their account ID.

            }

            else //If(there are errors)
            {

                MessageBox.Show(errorsToShow);

            }

        }
        /// <summary>
        /// Deletes all stored accounts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {

            accounts.clearAccounts();
            MessageBox.Show("Accounts deleted.");

        }
        /// <summary>
        /// Creates reports based on account info and displays it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateReportsButton_Click(object sender, RoutedEventArgs e)
        {

            
            if (accounts.getAccountsCount() > 0) //if(there are accounts to build a report on)
            {

                TopThreeScoresTextBox.Text = accounts.getTopThreeCompetitors();
                EntriesTextBlock.Text = accounts.getAmountOfEachAccountType().ToString();
                TotalScoresTextBlock.Text = accounts.getTotalScores();
                IncomeTextBlock.Text = accounts.getTotalIncome();
                AverageAgeTextBlock.Text = accounts.getAverageAge();
                ModalAgeTextBlock.Text = accounts.getModalAge();
                RangeOfAgeTextBlock.Text = accounts.getAgeRange();

            }

            else //if(there are no accounts to report on)
            {

                MessageBox.Show("No accounts available for use in generating reports.");

            }

        }
        /// <summary>
        /// Creates a multitude of generated accounts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateTestDataButton_Click(object sender, RoutedEventArgs e)
        {

            accounts.generateTestData();
            MessageBox.Show("120 test accounts created");

        }
        /// <summary>
        /// Searches for a user based on ID if a valid number is entered; searches for a number based on name otherwise. Found user is then selected and displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {

            int searchingAccountID;

            if (int.TryParse(SearchTextBox.Text, out searchingAccountID)) //If(a number is entered, parse it and continue)
            {

                if (searchingAccountID >= 0 && searchingAccountID < accounts.getAccountsCount()) //If(number is within the range of accounts)
                {

                    loadAccount(searchingAccountID);

                }

                else
                {

                    MessageBox.Show("That is not a valid account ID (0 - " + (accounts.getAccountsCount() - 1) + ").");

                }

            }

            else //If(a parseable number isn't entered)
            {

                try
                {

                    loadAccount(searchByName(SearchTextBox.Text)); //Search for a name containing the entered string.

                }

                catch
                {

                    MessageBox.Show("No entries found with that name.");

                }

            }

        }
        /// <summary>
        /// Checks the validity of new entered data and updates the stored data if it is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            string errorsToShow; //Accumulation of issues with entered data that is to be displayed if needed.

            string name = NameTextBox.Text;
            string age = AgeTextBox.Text;
            string address = AddressTextBox.Text;
            string sponsor = SponsorBox.Text;
            string nextOfKin = NextOfKinBox.Text;
            CelebrityAccount.bloodTypes bloodType;
            Enum.TryParse(bloodComboBox.Text, out bloodType);
            string score = ScoreTextBox.Text;
            StandardAccount.accountType accountType = currentlyEdittingAccount.getAccountType();

            int ageInt;

            if (!int.TryParse(age, out ageInt) && age == "") //Parses the age string to an integer, if the age box is empty then set the age to an assumed value of 0.
            {

                ageInt = 0;

            }

            int scoreInt;

            if (!int.TryParse(score, out scoreInt) && score == "") //Parses the score string to an integer, if the age box is empty then set the age to an assumed value of 0.
            {

                scoreInt = 0;

            }

            if ((errorsToShow = accountValidityCheck(name, address, age, accountType, sponsor, bloodType, nextOfKin)) == "") //If (there are no errors to return for an account with the given information.)
            {

                //Update account settings.
                currentlyEdittingAccount.setName(name);
                currentlyEdittingAccount.setAge(ageInt);
                currentlyEdittingAccount.setAddress(address);
                currentlyEdittingAccount.setSponsor(sponsor);
                currentlyEdittingAccount.setChildName(nextOfKin);
                currentlyEdittingAccount.setBloodType(bloodType);
                currentlyEdittingAccount.setScore(int.Parse(score));

                MessageBox.Show("Account information saved.");

            }

            else //If(there are errors)
            {

                MessageBox.Show(errorsToShow); //Display the errors.

            }

        }

        /// <summary>
        /// Loads the bloodtype dropdown menu's options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newBloodComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            newBloodComboBox.ItemsSource = Enum.GetNames(typeof(CelebrityAccount.bloodTypes));

        }
        /// <summary>
        /// Loads the bloodtype dropdown menu's options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bloodComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            bloodComboBox.ItemsSource = Enum.GetNames(typeof(CelebrityAccount.bloodTypes));

        }

        /// <summary>
        /// Prevents entering invalid characters in the address bod. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            AddressTextBox.Text = stringRestriction(AddressTextBox.Text, "[^A-z 0-9'\r]"); //Restricts a textbox to the given character range.

            if (StandardAccount.addressValidation(AddressTextBox.Text) == "") //if(address is valid)
            {

                AddressTextBox.Background = Brushes.GreenYellow;

            }

            else
            {

                AddressTextBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering non-numerics in the age box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            AgeTextBox.Text = stringRestriction(AgeTextBox.Text, "[^0-9]"); //Restricts a textbox to the given character range.

            string enteredAgeText = AgeTextBox.Text;

            if (enteredAgeText.Length > 0 && enteredAgeText.Length < 4) //if(age is between 1 and 3 digits) this prevents parse attempts of numbers too large to be integer value types.
            {

                if (StandardAccount.ageValidation(int.Parse(enteredAgeText)) == "")//if(the age is valid)
                {

                    AgeTextBox.Background = Brushes.GreenYellow;

                }

                else
                {

                    AgeTextBox.Background = Brushes.White;

                }

            }

            else if (enteredAgeText.Length == 0)
            {

                AgeTextBox.Background = Brushes.GreenYellow;

            }

        }
        /// <summary>
        /// Only allows the entering of letters and whitespaces. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            NameTextBox.Text = stringRestriction(NameTextBox.Text); //Restricts a textbox to the given character range.

            if (StandardAccount.nameValidation(NameTextBox.Text) == "") //if(name is valid)
            {

                NameTextBox.Background = Brushes.GreenYellow;

            }

            else
            {

                NameTextBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering non-numerics in the age box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewAgeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            NewAgeTextBox.Text = stringRestriction(NewAgeTextBox.Text, "[^0-9]"); //Restricts a textbox to the given character range.

            string enteredAgeText = NewAgeTextBox.Text;

            if (enteredAgeText.Length > 0 && enteredAgeText.Length < 4) //If(age is between 1 and 3 digits) this prevents parse attempts of numbers too large to be integer value types.
            {

                if (StandardAccount.ageValidation(int.Parse(enteredAgeText)) == "") //If(age is valid)
                {

                    NewAgeTextBox.Background = Brushes.GreenYellow;

                }

                else
                {

                    NewAgeTextBox.Background = Brushes.White;

                }

            }

            else if (enteredAgeText.Length == 0) //If(age testbox is empty i.e 0 value)
            {

                NewAgeTextBox.Background = Brushes.GreenYellow;

            }

        }
        /// <summary>
        /// Prevents entering invalid characters in the address bod. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            NewAddressTextBox.Text = stringRestriction(NewAddressTextBox.Text, "[^A-z 0-9'\r]"); //Restricts a textbox to the given character range.

            if (StandardAccount.addressValidation(NewAddressTextBox.Text) == "") //If(address is valid)
            {

                NewAddressTextBox.Background = Brushes.GreenYellow;

            }

            else
            {

                NewAddressTextBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering non-alphabetic characters in the name box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            NewNameTextBox.Text = stringRestriction(NewNameTextBox.Text); //Restricts a textbox to the given character range.

            if (StandardAccount.nameValidation(NewNameTextBox.Text) == "") //If(name is valid)
            {

                NewNameTextBox.Background = Brushes.GreenYellow;

            }

            else
            {

                NewNameTextBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering non-alphabetic characters in the next of kin box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextOfKinBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            NextOfKinBox.Text = stringRestriction(NextOfKinBox.Text); //Restricts a textbox to the given character range.


            if (StandardAccount.nameValidation(NextOfKinBox.Text) == "")
            {

                NextOfKinBox.Background = Brushes.GreenYellow;

            }

            else
            {

                NextOfKinBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering non-alphabetic characters in the next of kin box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextOfKinTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


            NextOfKinTypeTextBox.Text = stringRestriction(NextOfKinTypeTextBox.Text); //Restricts a textbox to the given character range.


            if (StandardAccount.nameValidation(NextOfKinTypeTextBox.Text) == "")
            {

                NextOfKinTypeTextBox.Background = Brushes.GreenYellow;

            }

            else
            {

                NextOfKinTypeTextBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering non-numerics in the score box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScoreTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            ScoreTextBox.Text = stringRestriction(ScoreTextBox.Text, "[^0-9]"); //Restricts a textbox to the given character range.

            string enteredScoreText = ScoreTextBox.Text;

            if (enteredScoreText.Length > 0 && enteredScoreText.Length < 5) //if(age is between 1 and 4 digits) this prevents parse attempts of numbers too large to be integer value types.
            {

                if (StandardAccount.scoreValidation(int.Parse(enteredScoreText)) == "") //if(score is valid)
                {

                    ScoreTextBox.Background = Brushes.GreenYellow;

                }

                else
                {

                    ScoreTextBox.Background = Brushes.White;

                }

            }

            else if (enteredScoreText.Length == 0) //if(score isn't provided)
            {

                ScoreTextBox.Background = Brushes.GreenYellow;

            }

        }
        /// <summary>
        /// Prevents entering of certain characters in the sponsor box. Makes the colour of the box reflect the validity of the value inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SponsorBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            SponsorBox.Text = stringRestriction(SponsorBox.Text, "[^A-z 0-9']"); //Restricts a textbox to the given character range.

            if (ProfessionalAccount.sponsorValidation(SponsorBox.Text) == "")
            {

                SponsorBox.Background = Brushes.GreenYellow;

            }

            else
            {

                SponsorBox.Background = Brushes.White;

            }

        }
        /// <summary>
        /// Prevents entering of certain characters in the sponsor box. Makes the colour of the box reflect the validity of the value inside
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SponsorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            SponsorTextBox.Text = stringRestriction(SponsorTextBox.Text, "[^A-z 0-9']"); //Restricts a textbox to the given character range.

            if (ProfessionalAccount.sponsorValidation(SponsorTextBox.Text) == "")
            {

                SponsorTextBox.Background = Brushes.GreenYellow;

            }

            else
            {

                SponsorTextBox.Background = Brushes.White;

            }

        }

        /// <summary>
        /// Saves the accounts when the main window is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            accounts.saveAccounts("");

        }

    }

}
