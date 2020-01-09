using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gateway.API.Admin.Web.Contracts.Response
{
    public class User
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Last Names
        /// </summary>
        public string LastNames { get; set; }
        
        /// <summary>
        /// Birth date
        /// </summary>
        public DateTime BirthDate { get; set; }
        
        public IEnumerable<Phone> Phones { get; set; }
    }
}
