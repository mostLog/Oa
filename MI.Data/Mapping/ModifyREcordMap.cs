﻿using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
   public class ModifyREcordMap : EntityTypeConfiguration<ModifyRecord>
    {
        public ModifyREcordMap()
        {
            this.ToTable("ModifyRecord");
            this.HasKey(m=>m.f_Id);

        }
    }
}
