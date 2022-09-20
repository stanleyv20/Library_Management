/*********************************************************************************************************************************************
 * Written by Stanley Varghese
 * This module (ErrorStatus.cs) only contains class definition for ErrorStatus class. 
 * Defined this seperate datatype to error/debug information to be passed to/from methods
 * ErrorStatus class is primarily used to pass information on success/failure/exceptions between DataAccessor class/validation methods 
 * and LibraryConsole class for handling or displaying issues to UI for user awareness
 * This class could be implemented much differently in a real life setting but this basic approach works fine for this simple application
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProgramming
{
    public class ErrorStatus
    {
        public int StatusCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}
