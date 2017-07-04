﻿using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Mapping
{
    public class FlightFeeMap : EntityTypeConfiguration<FlightFee>
    {
        public FlightFeeMap()
        {
            this.ToTable("t_FlightFee");
            this.HasKey(c => c.f_ID);
        }
    }
}
