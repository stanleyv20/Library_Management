/*
    Written by Stanley Varghese
    This module (LibraryConsole.cs) is the code behind for my LibraryConsole Windows Form application.
    Module contains methods for handling events triggered via interactions with form elements and input validation.
    Events include initial load, button clicks, row selection changes from DataGrid etc.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace DBProgramming
{
    public partial class LibraryConsole : Form
    {
        public List<Book> BookList = new List<Book>();
        public List<Publisher> PublisherList = new List<Publisher>();
        public int selectedPublisherIndex;
        public string publisherValue;

        /*
         * Constructor for LibraryConsole application
         * Initializes all necessary items for form
         * Makes initial call to DB through DataAccessor to retrieve data to populate DataGrid with Book information and populate ComboBox dropdown with available publishers
         * Input: No Input
         * Output: Void
         */
        public LibraryConsole()
        {
            InitializeComponent();
            DataAccessor dbAccessor = new DataAccessor();
            BookList = dbAccessor.GetBookList();
            PublisherList = dbAccessor.GetPublisherList();
            DataGrid.DataSource = BookList;
            DataGrid.Columns["PublisherID"].Visible = false; //PublisherID does not need to be visible to users, but is needed for subsequent calls to DB
            DataGrid.Columns["PublicationID"].Visible = false;
            PublisherList.ForEach(publisher => PublisherComboBox.Items.Add(publisher));//Populate combobox with data from GetPublisher stored proc
            PublisherComboBox.DisplayMember = "PublisherName";
            PublisherComboBox.ValueMember = "PublisherID";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataAccessor dbAccessor = new DataAccessor();
            BookList = dbAccessor.GetBookList();
        }

        /*
        * Event handler for click of search button element
        * This method starts by validating the four available search criteria: Title, Author, ISBN, Dewey. Messagebox is displayed on UI if issue identified with provided input
        * The method then creates a Book item with search criteria populated based on text provided within Textboxes and passes to DataAccessor
        * Current DataGrid selection is cleared along with textbox values on UI. Entries autopopulate after a new row selection is made via UI
        * Lastly, StatusStrip text is updated to reflect that current mode involves searching for Books
        */
        private void SearchButton_Click(object sender, EventArgs e)
        {
            ErrorStatus objErrorStatus = null;
            objErrorStatus = ValidateBookForSearch(TitleTextBox.Text, AuthorTextBox.Text, IsbnTextBox.Text, DeweyTextBox.Text);
            if (objErrorStatus.StatusCode == -1)
            {
                MessageBox.Show(objErrorStatus.ErrorDescription);
                return;
            }

            //If no values are provided to any of the 4 search parameters, new call is made to GetBookList which simply retrieves all books from DB and brings user back to same UI as Form load
            DataAccessor dbAccessor = new DataAccessor();
            if (!String.IsNullOrEmpty(AuthorTextBox.Text) || !String.IsNullOrEmpty(TitleTextBox.Text) || !String.IsNullOrEmpty(IsbnTextBox.Text) || !String.IsNullOrEmpty(DeweyTextBox.Text))
            {
                //New Book object created and populated with current text values provided
                Book BookToSearch = new Book
                {
                    Author = !String.IsNullOrEmpty(AuthorTextBox.Text) ? AuthorTextBox.Text : null,
                    Title = !String.IsNullOrEmpty(TitleTextBox.Text) ? TitleTextBox.Text : null,
                    ISBN = !String.IsNullOrEmpty(IsbnTextBox.Text) ? IsbnTextBox.Text : null,
                    Dewey = !String.IsNullOrEmpty(DeweyTextBox.Text) ? DeweyTextBox.Text : null,
                };
                BookList = dbAccessor.SearchBook(BookToSearch);
                DataGrid.DataSource = BookList;
                
                //Code section below clears DataGrid selection and textbox elements so user can make a new selection from DataGrid after it is populated with search results
                this.DataGrid.ClearSelection();
                TitleTextBox.Clear();
                AuthorTextBox.Clear();
                IsbnTextBox.Clear();
                DeweyTextBox.Clear();
                PublisherComboBox.ResetText();
                DateTextBox.Clear();
                PageNumTextBox.Clear();
                EditionTextBox.Clear();
                congressTextBox.Clear();
                DescriptionTextBox.Clear();
                toolStripStatusLabel1.Text = "Mode:Searching for Books";
            }
            else
            {
                BookList = dbAccessor.GetBookList();
                DataGrid.DataSource = BookList;
            }
            //Try catch was removed after debugging but not a good practice 
        }

        //Event for updating selection made in Publisher ComboBox (dropdown)
        private void PublisherComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPublisherIndex = PublisherComboBox.SelectedIndex;
        }

        /*
         * Event handler for when row selection is changed via DataGrid
         * Updates textboxes with column values for selected row to facilitate update operation
         * Conversion is made for Date textbox and Text for SaveButton is changed to Update for user awareness
         */
        private void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Mode:Modifying Selected Book";
            TitleTextBox.Text = this.DataGrid.CurrentRow.Cells["Title"].Value.ToString();
            AuthorTextBox.Text = this.DataGrid.CurrentRow.Cells["Author"].Value.ToString();
            IsbnTextBox.Text = this.DataGrid.CurrentRow.Cells["ISBN"].Value.ToString();
            DeweyTextBox.Text = this.DataGrid.CurrentRow.Cells["Dewey"].Value.ToString();
            PageNumTextBox.Text = this.DataGrid.CurrentRow.Cells["PageNum"].Value.ToString();
            PublisherComboBox.Text = this.DataGrid.CurrentRow.Cells["Publisher"].Value.ToString();
            DateTextBox.Text = Convert.ToDateTime(this.DataGrid.CurrentRow.Cells["PublicationDate"].Value).ToString("MM/dd/yyyy"); 
            EditionTextBox.Text = this.DataGrid.CurrentRow.Cells["Edition"].Value.ToString() == "N/A" ? "0" : this.DataGrid.CurrentRow.Cells["Edition"].Value.ToString();
            congressTextBox.Text = this.DataGrid.CurrentRow.Cells["CongressNumber"].Value.ToString().Trim();
            DescriptionTextBox.Text = this.DataGrid.CurrentRow.Cells["PublicationDescription"].Value.ToString();
            SaveButton.Text = "Update";
        }

        /*
         * Event handler for when clear button is preset
         * Deselects DataGrid row selection if one was in place, clears all textboxes, resets publisher ComboBox (dropdown list), updates statusStrip text, and updates button text to "Save"
         */
        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.DataGrid.ClearSelection();
            TitleTextBox.Clear();
            AuthorTextBox.Clear();
            IsbnTextBox.Clear();
            DeweyTextBox.Clear();
            PublisherComboBox.ResetText();
            DateTextBox.Clear();
            PageNumTextBox.Clear();
            EditionTextBox.Clear();
            congressTextBox.Clear();
            DescriptionTextBox.Clear();
            toolStripStatusLabel1.Text = "Mode:Entering New Book";
            SaveButton.Text = "Save";
        }

        /*
        * Event handler for when save button is clicked. Logic varies based on current mode (Save vs Update) that application is in. This mode is reflected in status strip and button text shown on UI
        * Data validation is performed first by passing all data text fields to ValidateBook method
        * If data validation is succesful, method attempts to save or update a Book via call through DataAccessor class based on mode indicated by button text. 
        * Lastly, DataGrid is updated by calling GetBookList (similar to initial load) to display updated entries to UI
        */
        private void SaveButton_Click(object sender, EventArgs e)
        {
            DataAccessor dbAccessor = new DataAccessor();
            Publisher selectedPublisher = (Publisher)PublisherComboBox.SelectedItem;
            try
            {
                ErrorStatus objErrorStatus = null;

                //If data validation fails then StatusCode will be -1 and message is displayed to UI and method exists without invoking DataAccessor
                objErrorStatus = ValidateBook(TitleTextBox.Text, AuthorTextBox.Text, IsbnTextBox.Text, EditionTextBox.Text, congressTextBox.Text, DeweyTextBox.Text, PageNumTextBox.Text, DateTextBox.Text, DescriptionTextBox.Text);
                if(objErrorStatus.StatusCode == -1)
                {
                    MessageBox.Show(objErrorStatus.ErrorDescription);
                    return;
                }

                if (SaveButton.Text == "Save")
                {
                    Book BookToInsert = new Book
                    {
                        Author = AuthorTextBox.Text,
                        Title = TitleTextBox.Text,
                        PublisherID = Convert.ToInt32(selectedPublisher.PublisherID),
                        Edition = EditionTextBox.Text,
                        PageNum = Convert.ToInt32(PageNumTextBox.Text),
                        PublicationDate = DateTime.Parse(DateTextBox.Text),
                        ISBN = IsbnTextBox.Text,
                        Dewey = DeweyTextBox.Text,
                        CongressNumber = congressTextBox.Text,
                        PublicationDescription = DescriptionTextBox.Text
                    };

                    ErrorStatus saveStatus = new ErrorStatus();
                    saveStatus = dbAccessor.SaveNewBook(BookToInsert);
                    //If save failed due to a constraint violation, then it is output to display for user awareness
                    if (!String.IsNullOrEmpty(saveStatus.ErrorDescription) && saveStatus.ErrorDescription.Contains("Violation"))
                    {
                        MessageBox.Show(saveStatus.ErrorDescription);
                    }
                }

                if (SaveButton.Text == "Update")
                {
                    /*
                     * Code section below creates new Book object to be passed to data accessor. Before assigning fields, check is made to compare current text value in TextBox to column value in selected DataRow
                     * If there is no difference between textbox and corresponding dataRow column value then value from DataGrid is passed to DataAccessor
                     * Readability could be improved here but it works fine as is
                     */
                    Book BookToUpdate = new Book
                    {
                        Author = this.DataGrid.CurrentRow.Cells["Author"].Value.ToString() != AuthorTextBox.Text ? AuthorTextBox.Text : this.DataGrid.CurrentRow.Cells["Author"].Value.ToString(),
                        Title = this.DataGrid.CurrentRow.Cells["Title"].Value.ToString() != TitleTextBox.Text ? TitleTextBox.Text : this.DataGrid.CurrentRow.Cells["Title"].Value.ToString(),
                        PublisherID = this.DataGrid.CurrentRow.Cells["PublisherID"].Value.ToString() != selectedPublisher.PublisherID ? Convert.ToInt32(selectedPublisher.PublisherID) : Convert.ToInt32(this.DataGrid.CurrentRow.Cells["PublisherID"].Value.ToString()),
                        Edition = this.DataGrid.CurrentRow.Cells["Edition"].Value.ToString() != EditionTextBox.Text ? EditionTextBox.Text : this.DataGrid.CurrentRow.Cells["PublisherID"].Value.ToString(),
                        PageNum = Convert.ToInt32(this.DataGrid.CurrentRow.Cells["PageNum"].Value.ToString()) != Convert.ToInt32(PageNumTextBox.Text) ? Convert.ToInt32(PageNumTextBox.Text) : Convert.ToInt32(this.DataGrid.CurrentRow.Cells["PageNum"].Value.ToString()),
                        PublicationDate = DateTime.Parse(this.DataGrid.CurrentRow.Cells["PublicationDate"].Value.ToString()) != DateTime.Parse(DateTextBox.Text) ? DateTime.Parse(DateTextBox.Text) : DateTime.Parse(this.DataGrid.CurrentRow.Cells["PublicationDate"].Value.ToString()),
                        ISBN = this.DataGrid.CurrentRow.Cells["ISBN"].Value.ToString() != IsbnTextBox.Text ? IsbnTextBox.Text : this.DataGrid.CurrentRow.Cells["ISBN"].Value.ToString(),
                        Dewey = this.DataGrid.CurrentRow.Cells["Dewey"].Value.ToString() != DeweyTextBox.Text ? DeweyTextBox.Text : this.DataGrid.CurrentRow.Cells["Dewey"].Value.ToString(),
                        CongressNumber = this.DataGrid.CurrentRow.Cells["CongressNumber"].Value.ToString() != congressTextBox.Text ? congressTextBox.Text : this.DataGrid.CurrentRow.Cells["CongressNumber"].Value.ToString(),
                        PublicationDescription = this.DataGrid.CurrentRow.Cells["PublicationDescription"].Value.ToString() != DescriptionTextBox.Text ? DescriptionTextBox.Text : this.DataGrid.CurrentRow.Cells["PublicationDescription"].Value.ToString(),
                        PublicationID = Convert.ToInt32(this.DataGrid.CurrentRow.Cells["PublicationID"].Value)
                    };
                    ErrorStatus updateStatus = new ErrorStatus();

                    updateStatus = dbAccessor.UpdateBook(BookToUpdate);
                    //If update failed due to a constraint violation, then it is output to display for user awareness
                    if (!String.IsNullOrEmpty(updateStatus.ErrorDescription) && updateStatus.ErrorDescription.Contains("Violation"))
                    {
                        MessageBox.Show(updateStatus.ErrorDescription);
                    }
                }
                //Invoke GetBookList to refresh data rows on UI
                BookList = dbAccessor.GetBookList();
                DataGrid.DataSource = BookList;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /*
         * Event handler for click of delete button. On click, column value for PublicationID (not visible in DataGrid but present and retrieved when populating data) is passed into DataAccessor method DeleteBook
         * After invoking DataAccessor method, call is made to GetBookList (just like form load) to retrieve updated data for display
         */
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int selectedPublicationID = Convert.ToInt32(this.DataGrid.CurrentRow.Cells["PublicationID"].Value);
            DataAccessor dbAccessor = new DataAccessor();
            dbAccessor.DeleteBook(selectedPublicationID);
            BookList = dbAccessor.GetBookList();
            DataGrid.DataSource = BookList;
        }

        /*
         * Method used for validating text fields populated by user on UI. Invoked on click of Save/Update button before invokation of DataAccessor methods. 
         * DataAccessors are not invoked if data is found to be missing on invalid
         * Various techniques are used to validate data integrity. 
         * Many alternatives such as Regex exist and may be more efficient/flexible but I am not able to use Regex with confidence quickly for this use case. My hacks still work
         * Also lack subject matter expertise on fields such as Dewey decimal numer, convress number, and ISBN so assumptions were made regarding what is expected format
         */
        private ErrorStatus ValidateBook(string Title, string Author, string ISBN, string Edition, string Congress, string Dewey, string PageNum, string DateCheck, string Description)
        {
            ErrorStatus objErrorStatus = null;

            try
            {
                objErrorStatus = new ErrorStatus
                {
                    StatusCode = -1,
                    ErrorDescription = ""
                };

                if (String.IsNullOrEmpty(ISBN) || String.IsNullOrEmpty(Edition) || String.IsNullOrEmpty(Congress) || String.IsNullOrEmpty(DateCheck) || String.IsNullOrEmpty(Dewey)
                    || String.IsNullOrEmpty(Title) || String.IsNullOrEmpty(Author) || String.IsNullOrEmpty(Description) || String.IsNullOrEmpty(PageNum))
                {
                    objErrorStatus.ErrorDescription = "A required field is missing. Please try again";
                    return objErrorStatus;
                }

                DateTime dtParseResult;
                if (!String.IsNullOrEmpty(DateCheck) && DateTime.TryParse(DateCheck, out dtParseResult))
                {
                    //Do nothing and proceed with input validation if date parse is succesful indicating valid entry
                }
                else
                {
                    objErrorStatus.ErrorDescription = "Provided publication date is invalid. Please try again";
                    return objErrorStatus;
                }

                if(String.IsNullOrEmpty(ISBN) || !ISBN.All(char.IsDigit))
                {
                    objErrorStatus.ErrorDescription = "Provided ISBN is invalid. Please try again";
                    return objErrorStatus;
                }

                //Explore using Regex to validate Dewey formats
                if (!String.IsNullOrEmpty(Dewey))
                {
                    if (!Dewey.Contains('.'))
                    {
                        objErrorStatus.ErrorDescription = "Invalid Dewey Decimal Format - no decimal point. Please try again";
                        return objErrorStatus;
                    }
                    int decimalCount = Dewey.Split('.').Length - 1;
                    if(decimalCount > 1)
                    {
                        objErrorStatus.ErrorDescription = "Invalid Dewey Decimal Format - only one decimal point should be provided. Please try again";
                        return objErrorStatus;
                    }

                    string[] DeweySplit = Dewey.Split('.');
                    foreach(var deweyElement in DeweySplit)
                    {
                        if (!deweyElement.All(char.IsLetterOrDigit))
                        {
                            objErrorStatus.ErrorDescription = "Invalid character in provided Dewey Decimal Number. Please try again";
                            return objErrorStatus;
                        }
                    }
                }

                if (String.IsNullOrEmpty(PageNum) || !PageNum.All(char.IsDigit))
                {
                    objErrorStatus.ErrorDescription = "Provided PageNum is invalid. Please try again";
                    return objErrorStatus;
                }

                if (String.IsNullOrEmpty(Edition) || !Edition.All(char.IsDigit))
                {
                    objErrorStatus.ErrorDescription = "Provided Edition is invalid. Please try again";
                    return objErrorStatus;
                }

                if (String.IsNullOrEmpty(Congress) || !Congress.All(char.IsDigit))
                {
                    objErrorStatus.ErrorDescription = "Invalid character in provided Congress Catalog Number. Please try again";
                    return objErrorStatus;
                }
                objErrorStatus.StatusCode = 0;
                return objErrorStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                objErrorStatus.StatusCode = -1;
                objErrorStatus.ErrorDescription = ex.ToString();
                return objErrorStatus;
            }
        }

        /*
        * Method used for validating text fields involved in search populated by user on UI. Invoked only on click of search button before invokation of SearchBook method in DataAccessor 
        * Since no restrictions are placed on acceptable values for Title or Author, they are not validated. However these checks could be added to this method in the future
        * DataAccessors are not invoked if data is found to be invalid.
        * I lack subject matter expertise on fields such as Dewey decimal number and ISBN number so assumptions were made regarding what is expected format
        */
        private ErrorStatus ValidateBookForSearch(string Title, string Author, string ISBN, string Dewey)
        {
            ErrorStatus objErrorStatus = null;

            try
            {
                objErrorStatus = new ErrorStatus
                {
                    StatusCode = -1,
                    ErrorDescription = ""
                };

                if (!String.IsNullOrEmpty(ISBN) && (!ISBN.All(char.IsDigit) || ISBN.Length > 13))
                {
                    if(!ISBN.All(char.IsDigit))
                        objErrorStatus.ErrorDescription = "Provided ISBN is invalid. Please try again";
                    if (ISBN.Length > 13)
                        objErrorStatus.ErrorDescription = "Provided ISBN is too long. 13 digits expected";
                    return objErrorStatus;
                }

                if (!String.IsNullOrEmpty(Dewey))
                {
                    if (!Dewey.Contains('.'))
                    {
                        objErrorStatus.ErrorDescription = "Invalid Dewey Decimal Format - no decimal point. Please try again";
                        return objErrorStatus;
                    }
                    int decimalCount = Dewey.Split('.').Length - 1;
                    if (decimalCount > 1)
                    {
                        objErrorStatus.ErrorDescription = "Invalid Dewey Decimal Format - only one decimal point should be provided. Please try again";
                        return objErrorStatus;
                    }

                    string[] DeweySplit = Dewey.Split('.');
                    foreach (var deweyElement in DeweySplit)
                    {
                        if (!deweyElement.All(char.IsLetterOrDigit))
                        {
                            objErrorStatus.ErrorDescription = "Invalid character in provided Dewey Decimal Number. Please try again";
                            return objErrorStatus;
                        }
                    }
                }

                objErrorStatus.StatusCode = 0;
                return objErrorStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                objErrorStatus.StatusCode = -1;
                objErrorStatus.ErrorDescription = ex.ToString();
                return objErrorStatus;
            }
        }
    }
}
