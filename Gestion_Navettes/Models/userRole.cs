using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Navettes.Models
{
    public class userRole
    {
        [Required(ErrorMessage = "Le champs est obligatoire")]
        public int usertype { get; set; }

        [Required(ErrorMessage = "Le champs est obligatoire")]
        public string login { get; set; }
        [Required(ErrorMessage = "Le champs est obligatoire")]
        public string mdp { get; set; }
    }
}