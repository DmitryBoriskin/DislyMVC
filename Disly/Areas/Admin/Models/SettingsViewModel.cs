﻿using cms.dbModel.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disly.Areas.Admin.Models
{
    public class SettingsViewModel : CoreViewModel
    {
        public SettingsModel siteSettings { get; set; }
    }
}
