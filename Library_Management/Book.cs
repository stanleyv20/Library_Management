/*********************************************************************************************************************************************
 * Written by Stanley Varghese
 * This module (Book.cs) only contains class definition for Book class. 
 * Defined this seperate datatype to store details on Books and serve as components of List that is data source for DataGrid
 * Book is utilized through the applicatino for transforming results from DB queries to in memory object.
 * Also to go from UI input to parameters passed to stored procedures
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProgramming
{
    public class Book
    {
        public int PublicationID { get; set; }

        public string Title { get; set; }

        public string Edition { get; set; }

        public int PageNum { get; set; }

        public string Author { get; set; }

        public string Dewey { get; set; }

        public string ISBN { get; set; }

        public string Publisher { get; set; }

        public int PublisherID { get; set; }

        public DateTime PublicationDate { get; set; }

        public string CongressNumber { get; set; }

        public string PublicationDescription { get; set; }
    }
}
