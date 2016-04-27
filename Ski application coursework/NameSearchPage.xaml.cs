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
using System.Windows.Shapes;

namespace Ski_application_coursework
{
    /// <summary>
    /// Interaction logic for NameSearchPage.xaml
    /// </summary>
    public partial class NameSearchPage : Window
    {

        List<nameIDAndAddress> matches; //List of matched accounts containing their name, address and ID.

        /// <summary>
        /// Takes a list of matched accounts and displays them in a list for the user to select.
        /// </summary>
        /// <param name="constructorMatches"></param>
        public NameSearchPage(List<nameIDAndAddress> constructorMatches)
        {

            InitializeComponent();

            matches = constructorMatches;
            MatchesListView.ItemsSource = matches;

        }

        /// <summary>
        /// Closes the window and publicly displays the selected userID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            selectedAccountID.selectedAccount = matches[MatchesListView.SelectedIndex].ID;
            this.Close();

        }

    }

    /// <summary>
    /// Static class to display a static value to other classes without initializing a class.
    /// </summary>
    public static class selectedAccountID 
    {

        public static int selectedAccount; //ID of the selected account, displayed publicly.

    }

    /// <summary>
    /// Class containing a list of accounts including their name, address and ID.
    /// </summary>
    public class nameIDAndAddress
    {

        private string name;
        private string address;
        private int iD;

        public nameIDAndAddress(string constructorName, string constructorAddress, int constructorID)
        {

            Name = constructorName;
            Address = constructorAddress;
            ID = constructorID;

        }

        public string Name
        {

            get
            {

                return name;

            }

            set
            {

                name = value;

            }

        }

        public string Address
        {

            get
            {

                return address;

            }

            set
            {

                address = value;

            }

        }

        public int ID
        {

            get
            {

                return iD;

            }

            set
            {

                iD = value;

            }

        }

    }

}
