﻿using KPP_Alpha1.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPP_Alpha1.Models
{
    internal class KarticeUlazModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Napomena { get; set; }
        public string Aktivno { get; set; }
        public int KorisnikId { get; set; } = LoginHelper.StaticId;
        public DateTime DateEdited { get; set; } = DateTime.Now.Date;
    }
}
