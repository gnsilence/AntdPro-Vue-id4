﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ABP.WebApi.Entities
{
   public abstract class AuditEntity
    {
        [Column(Order = 100)]
        public DateTime CreatedOn { get; set; }
        [Column(Order = 101)]
        public Guid? CreatedByUserGuid { get; set; }
        [Column(Order = 102)]
        public string CreatedByUserName { get; set; }
        [Column(Order = 103)]
        public DateTime? ModifiedOn { get; set; }
        [Column(Order = 104)]
        public Guid? ModifiedByUserGuid { get; set; }
        [Column(Order = 105)]
        public string ModifiedByUserName { get; set; }
    }
}
