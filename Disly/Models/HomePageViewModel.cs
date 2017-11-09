﻿using cms.dbModel.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disly.Models
{
    /// <summary>
    /// Модель для главной страницы
    /// </summary>
    public class HomePageViewModel : PageViewModel
    {
        public List<MaterialFrontModule> Materials { get; set; }
    }
}