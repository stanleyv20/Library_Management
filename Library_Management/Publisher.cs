/*********************************************************************************************************************************************
 * Written by Stanley Varghese
 * This module (Publisher.cs) only contains class definition for Publisher class. 
 * Defined this seperate datatype to store details on publishers and serve as data source for Publisher ComboBox dropdown shown on UI
 * When updating or saving an item, PublisherID for selected element in drop down is passed behind the scenes through code to stored procedure
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProgramming
{
    public class Publisher
    {
        public string PublisherID { get; set; }

        public string PublisherName { get; set; }
    }
}
