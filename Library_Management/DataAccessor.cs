/*******************************************************************************************************************************
    Written by Stanley Varghese
    This module (DataAccessor.cs) is the data access layer of the LibraryConsole application.
    Methods interacting with the SQL Server Database will do so by invoking stored procedures via methods defined in this class.
*******************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DBProgramming
{
    public class DataAccessor
    {
        //ConnectionString for my DB is stored in App.config file. 
        //Retrieved and assigned to dbConnectionString variable that is accessable to all methods in this class for easy future reference
        string dbConnectionString = ConfigurationManager.ConnectionStrings["LibraryDBConectionString"].ConnectionString;

        /*
            Executes SP to retrieve all books on initial load of application and to update DataGrid after save,update, delete operation is invoked
            Input: No input paramters
            Output: Returns list of Book objects reflecting current entries in DB which is then assigned as datasource of DataGrid to update UI
         */
        public List<Book> GetBookList() //string SearchCriteria 
        {
            DataTable dtBook = new DataTable();
            /*
             * Code section below is used for connecting to DB, executing GetBookInfo stored procedure and loading results into DataTable variable for processing
             * SqlConnection implements IDisposable interface so DB connection is closed when exiting using block. Do not need to explicitly close the connection
             */
            using (SqlConnection dbConnection = new SqlConnection(dbConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.GetBookInfo", dbConnection);
                command.CommandType = CommandType.StoredProcedure;
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dtBook.Load(reader);
            }

            List<Book> BookList = new List<Book>();
            /* 
             * Below code block declares/initializes new Book object and converts each column value from DataTable row and assigns it as field of Book object. 
             * Values are trimmed to remove white space and type conversions performed if needed to confirm to Book's member variable types
             * Each resulting book object is added to BookList output from this method
             */
            BookList = (from DataRow dtRow in dtBook.Rows
                        select new Book()
                        {
                            PublicationID = int.Parse(dtRow["PublicationID"].ToString().Trim()),
                            Title = dtRow["Title"].ToString().Trim(),
                            //Author = (dtRow["PublicationAuthor"].ToString().Trim().Split(',').Length - 1 == 1) ? 
                            Author = dtRow["PublicationAuthor"].ToString().Trim(),//.Replace(";", "; ").Replace(",,", ","),
                            PageNum = dtRow["PageNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dtRow["PageNumber"]),
                            Edition = dtRow["Edition"] == DBNull.Value || dtRow["Edition"].ToString() == "0" ? "N/A" : dtRow["Edition"].ToString().Trim(),
                            Dewey = dtRow["DeweyNumber"].ToString().Trim(),
                            ISBN = dtRow["ISBN"].ToString().Trim(),
                            CongressNumber = dtRow["CongressNumber"].ToString(),
                            PublicationDate = (DateTime)dtRow["PublicationDate"],
                            Publisher = dtRow["PublisherName"].ToString().Trim(),
                            PublicationDescription = dtRow["Description"].ToString().Trim()
                        }).ToList();

            return BookList;
        }

        /*
         * Executes stored procedure to retrieve Publisher names and Id's for populating ComboBox element that functions as drop down on UI
         * Input: No input paramters
         * Output: Returns list of Publisher objects
         */
        public List<Publisher> GetPublisherList()
        {
            DataTable dtPublisher = new DataTable();
            SqlCommand command = null;
            SqlConnection dbConnection = null;
            List<Publisher> publisherList = new List<Publisher>();

            try
            {
                /*
                 * Code section below is used for connecting to DB, executing GetPublishers stored procedure and loading results into DataTable variable for processing
                 * SqlConnection implements IDisposable interface so DB connection is closed when exiting using block. Do not need to explicitly close the connection
                 */
                using (dbConnection = new SqlConnection(dbConnectionString))
                {
                    command = new SqlCommand("dbo.GetPublishers", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    dbConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dtPublisher.Load(reader);
                }

                /* 
                 * Below code block declares/initializes new publisher object and converts each column value from DataTable row and assigns it as field of Publisher object. 
                 * Each resulting publisher object is added to BookList output from this method
                 */
                publisherList = (from DataRow dtRow in dtPublisher.Rows
                                 select new Publisher()
                                 {
                                     PublisherID = dtRow["PublisherID"].ToString(),
                                     PublisherName = dtRow["PublisherName"].ToString().Trim()
                                 }).ToList();

                return publisherList;
            }
            //Used catch block for debugging only but in production would likely be used for error handling, logging etc. 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return publisherList;
            }
        }

        /*
         * Executes stored procedure to insert new Book along with associated meta data into Database
         * Input: Book object with desired fields for insertion
         * Output: ErrorStatus object indicating success/failure and error description in case a SQL exception was caught
         */
        public ErrorStatus SaveNewBook(Book newBook)
        {
            SqlCommand command = null;
            SqlConnection dbConnection = null;
            int insertRowCount = 0;

            ErrorStatus objErrStatus = new ErrorStatus
            {
                ErrorDescription = "",
                StatusCode = -1
            };

            try
            {
                /*
                 * Code section below is used for connecting to DB, creating/assigning procedure parameters, and executing InsertBook stored procedure.
                 * Stored procedure logic is implemented within a transaction, if any exception occurs during execution then it is rolled back within catch block.
                 * So there is no partial success scenario for this stored procededure ensuring DB integrity is maintained
                 * Originally implemented retrieval of exceptions using DebugString Output parameter but ended up implementing exception handling via RAISEERROR in SQL Server
                 * If catch block is entered during stored procedure execution then SqlException catch block will find metadata on exception (ERROR_STATE, ERROR_SEVERITY, ERROR_LINE, ERROR_MESSAGE)
                 * SqlConnection implements IDisposable interface so DB connection is closed when exiting using block. Do not need to explicitly close the connection
                 */
                using (dbConnection = new SqlConnection(dbConnectionString))
                {
                    command = new SqlCommand("dbo.InsertBook", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Title", newBook.Title));
                    command.Parameters.Add(new SqlParameter("@PublisherID", newBook.PublisherID));
                    command.Parameters.Add(new SqlParameter("@ISBN", newBook.ISBN));
                    command.Parameters.Add(new SqlParameter("@Authors", newBook.Author));
                    command.Parameters.Add(new SqlParameter("@Edition", newBook.Edition));
                    command.Parameters.Add(new SqlParameter("@PageNum", newBook.PageNum));
                    command.Parameters.Add(new SqlParameter("@DeweyDecimalNumber", newBook.Dewey));
                    command.Parameters.Add(new SqlParameter("@PublicationDate", newBook.PublicationDate));
                    command.Parameters.Add(new SqlParameter("@CongressCatalogNumber", newBook.CongressNumber));
                    command.Parameters.Add(new SqlParameter("@PublicationDescription", newBook.PublicationDescription));
                    command.Parameters.Add(new SqlParameter("@DebugString", ""));
                    dbConnection.Open();
                    insertRowCount = command.ExecuteNonQuery();
                }

                //Used for debugging but is not needed
                if (insertRowCount > 0)
                {
                    objErrStatus.StatusCode = 0;
                    objErrStatus.ErrorDescription = String.Format("Rows Inserted: {0}", insertRowCount);
                }
                return objErrStatus;
            }
            //Specific catch block for SQL Exceptions caught in stored procedure. Used to inform user via UI of which fields do not comply with DB constraints
            catch (SqlException sqlEx)
            {
                //Multiple errors are possible so appending each one at a time to ErrorStatus object
                for (int errorIncrementer = 0; errorIncrementer < sqlEx.Errors.Count; errorIncrementer++)
                {
                    objErrStatus.ErrorDescription += sqlEx.Message + " ";
                }
                return objErrStatus;
            }
            //General Exception handler. Only used for debugging but in production setting would likely have error handling/logging functionality
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                objErrStatus.ErrorDescription += ex.ToString();
                return objErrStatus;
            }
        }

        /*
         * Executes stored procedure to update details for an existing Book along with associated meta data in database
         * Input: Book object with desired fields for update
         * Output: ErrorStatus object indicating success/failure and error description in case a SQL exception or general exception was caught
         */
        public ErrorStatus UpdateBook(Book updateBook)
        {
            SqlCommand command = null;
            SqlConnection dbConnection = null;
            int insertRowCount = 0;

            ErrorStatus objErrStatus = new ErrorStatus
            {
                ErrorDescription = "",
                StatusCode = -1
            };

            try
            {
                /*
                 * Code section below is used for connecting to DB, creating/assigning procedure parameters, and executing UpdateBook stored procedure.
                 * Originally implemented retrieval of exceptions using DebugString Output parameter but ended up implementing exception handling via RAISEERROR in SQL Server
                 * If catch block is entered during stored procedure execution then SqlException catch block will find metadata on exception (ERROR_STATE, ERROR_SEVERITY, ERROR_LINE, ERROR_MESSAGE)
                 * SqlConnection implements IDisposable interface so DB connection is closed when exiting using block. Do not need to explicitly close the connection
                 */
                using (dbConnection = new SqlConnection(dbConnectionString))
                {
                    command = new SqlCommand("dbo.UpdateBook", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (!String.IsNullOrEmpty(updateBook.Edition) && updateBook.Edition != "N/A")
                    {
                        command.Parameters.Add(new SqlParameter("@Edition", Convert.ToInt32(updateBook.Edition)));
                    }

                    command.Parameters.Add(new SqlParameter("@Title", updateBook.Title));
                    command.Parameters.Add(new SqlParameter("@PublisherID", updateBook.PublisherID));
                    command.Parameters.Add(new SqlParameter("@ISBN", updateBook.ISBN));
                    command.Parameters.Add(new SqlParameter("@Authors", updateBook.Author));
                    command.Parameters.Add(new SqlParameter("@PageNum", updateBook.PageNum));
                    command.Parameters.Add(new SqlParameter("@DeweyDecimalNumber", updateBook.Dewey));
                    command.Parameters.Add(new SqlParameter("@PublicationDate", updateBook.PublicationDate));
                    command.Parameters.Add(new SqlParameter("@CongressCatalogNumber", updateBook.CongressNumber));
                    command.Parameters.Add(new SqlParameter("@PublicationDescription", updateBook.PublicationDescription));
                    command.Parameters.Add(new SqlParameter("@PublicationID", updateBook.PublicationID));
                    SqlParameter errorMessageOut = new SqlParameter("@DebugString", "");
                    errorMessageOut.Direction = ParameterDirection.Output;
                    command.Parameters.Add(errorMessageOut);
                    dbConnection.Open();
                    insertRowCount = command.ExecuteNonQuery();
                }
                //Used for debugging but is not needed
                if (insertRowCount > 0)
                {
                    objErrStatus.StatusCode = 0;
                    objErrStatus.ErrorDescription = String.Format("Rows Inserted: {0}", insertRowCount);
                }
                return objErrStatus;
            }
            //Specific catch block for SQL Exceptions caught in stored procedure. Used to inform user via UI of which fields do not comply with DB constraints
            catch (SqlException sqlEx)
            {
                for (int errorIncrementer = 0; errorIncrementer < sqlEx.Errors.Count; errorIncrementer++)
                {
                    objErrStatus.ErrorDescription += sqlEx.Message + " ";
                }
                return objErrStatus;
            }
            //General Exception handler. Only used for debugging but in production setting would likely have error handling/logging functionality
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                objErrStatus.ErrorDescription += ex.ToString();
                return objErrStatus;
            }
        }

        /*
         * Executes stored procedure to retrieve books based on provided input parameters
         * SP was implemented using dynamic SQL generated based on which parameters are not null. Only fields eligible for search are Title,Author,ISBN, and DeweyDecimalNumber
         * LIKE is used within WHERE clause for Title and Author fields so partial matches will be returned
         * = is used within WHERE clause for ISBN and DeweyDecimalNumber so ONLY exact results are returned. Search parameters can be combined as desired 
         * Input: Book object with desired search fields
         * Output: List containing Book objects reflecting items in database meeting specified search criteria
         */
        public List<Book> SearchBook(Book bookSearch)
        {
            SqlCommand command = null;
            SqlConnection dbConnection = null;
            List<Book> BookList = new List<Book>();

            ErrorStatus objErrStatus = new ErrorStatus
            {
                ErrorDescription = "",
                StatusCode = -1
            };

            try
            {
                /*
                 * Code section below is used for connecting to DB, creating/assigning procedure parameters, and executing SearchBooks stored procedure.
                 * SqlConnection implements IDisposable interface so DB connection is closed when exiting using block. Do not need to explicitly close the connection
                 * Only non null fields from input variable bookSearch are pased to stored procedure. Default values for paramters is NULL and they are not including in search query if not provided
                 */
                DataTable dtBook = new DataTable();
                using (dbConnection = new SqlConnection(dbConnectionString))
                {
                    command = new SqlCommand("dbo.SearchBooks", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;

                    if(bookSearch.Title != null && !String.IsNullOrEmpty(bookSearch.Title))
                        command.Parameters.Add(new SqlParameter("@Title", bookSearch.Title));

                    if (bookSearch.Author != null && !String.IsNullOrEmpty(bookSearch.Author))
                        command.Parameters.Add(new SqlParameter("@Authors", bookSearch.Author));
                    
                    if (bookSearch.ISBN != null && !String.IsNullOrEmpty(bookSearch.ISBN))
                        command.Parameters.Add(new SqlParameter("@ISBN", bookSearch.ISBN));

                    if (bookSearch.Dewey != null && !String.IsNullOrEmpty(bookSearch.Dewey))
                        command.Parameters.Add(new SqlParameter("@DeweyDecimalNumber", bookSearch.Dewey));

                    dbConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dtBook.Load(reader);
                }

                /* 
                 * Below code block declares/initializes new Book object and converts each column value from DataTable row returnred from executing SearchBooks and assigns it as field of Book object. 
                 * Values are trimmed to remove white space and type conversions performed if needed to confirm to Book's member variable types
                 * Each resulting book object is added to BookList output from this method
                 */
                BookList = (from DataRow dtRow in dtBook.Rows
                            select new Book()
                            {
                                PublicationID = int.Parse(dtRow["PublicationID"].ToString().Trim()),
                                Title = dtRow["Title"].ToString().Trim(),
                                //Author = (dtRow["PublicationAuthor"].ToString().Trim().Split(',').Length - 1 == 1) ? 
                                Author = dtRow["PublicationAuthor"].ToString().Trim(),//.Replace(";", "; ").Replace(",,", ","),
                                PageNum = dtRow["PageNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dtRow["PageNumber"]),
                                Edition = dtRow["Edition"] == DBNull.Value || dtRow["Edition"].ToString() == "0" ? "N/A" : dtRow["Edition"].ToString().Trim(),
                                Dewey = dtRow["DeweyNumber"].ToString().Trim(),
                                ISBN = dtRow["ISBN"].ToString().Trim(),
                                CongressNumber = dtRow["CongressNumber"].ToString(),
                                PublicationDate = (DateTime)dtRow["PublicationDate"],
                                Publisher = dtRow["PublisherName"].ToString().Trim(),
                                PublicationDescription = dtRow["Description"].ToString().Trim()
                            }).ToList();

                return BookList;
            }
            catch(Exception ex)
            {
                return BookList;
            }
        }

        /*
         * Executes stored procedure to delete books with provided details. 
         * Since deletion is done after selecting a row from DataGrid, PublisherID is included in request for book deletion and passed to stored proc
         * Input: Details on book to be deleted from Database
         * Output: ErrorStatus object indicating success/failure and error description in case an exception occurs
         */
        public ErrorStatus DeleteBook(int publicationID)
        {
            SqlCommand command = null;
            SqlConnection dbConnection = null;
            int deleteRowCount = 0;

            ErrorStatus objErrStatus = new ErrorStatus
            {
                ErrorDescription = "",
                StatusCode = -1
            };

            try
            {
                /*
                 * Code section below is used for connecting to DB, creating/assigning procedure parameters, and executing SearchBooks stored procedure.
                 * SqlConnection implements IDisposable interface so DB connection is closed when exiting using block. Do not need to explicitly close the connection
                 * Only non null fields from input variable bookSearch are pased to stored procedure. Default values for paramters is NULL and they are not including in search query if not provided
                 */
                using (dbConnection = new SqlConnection(dbConnectionString))
                {
                    command = new SqlCommand("dbo.DeleteBook", dbConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
                    dbConnection.Open();
                    deleteRowCount = command.ExecuteNonQuery();
                }

                if (deleteRowCount >= 3)
                {
                    objErrStatus.StatusCode = 0;
                    objErrStatus.ErrorDescription = String.Format("Rows deleted: {0}", deleteRowCount);
                }
                return objErrStatus;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return objErrStatus;
            }
        }
    }
}
