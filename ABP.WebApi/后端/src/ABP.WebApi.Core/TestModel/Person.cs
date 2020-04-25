﻿using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ABP.WebApi.TestModel
{
   // [MessagePackObject(true)]
    [Table("tb_Person")]
    public class Person:Entity
    {

        public virtual string PersonName { get; set; }

        public Person()
        {

        }

        public Person(string personName)
        {
            PersonName = personName;
        }
    }
}
