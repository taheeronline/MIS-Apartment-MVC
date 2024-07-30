using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageApartment.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string MachineName { get; set; }

        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Exception { get; set; }


    }
}   
