﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cms.dbModel.entity
{

    public class IndexModel
    {
        public BannersModel[] BannerArrayIndex { get; set; }
        public IEnumerable<SiteMapModel> SitemapPlate { get; set; }
        public IEnumerable<BannersModel> BenifitBanners { get; set; }
        public IEnumerable<BannersModel> Slider { get; set; }

        #region пресс центр
        //public IEnumerable<MaterialFrontModule> ModuleAnnouncement { get; set; }
        //public IEnumerable<MaterialFrontModule> ModuleNews { get; set; }
        //public IEnumerable<MaterialFrontModule> ModuleEvents { get; set; }
        //public IEnumerable<MaterialFrontModule> ModuleActual { get; set; }

        //public MaterialFrontModule ModulePhoto { get; set; }
        //public MaterialFrontModule ModuleVideo { get; set; }

        //public MaterialFrontModule ModuleNewsWorld { get; set; }
        //public MaterialFrontModule ModuleNewsRus { get; set; }
        //public MaterialFrontModule ModuleNewsChuv { get; set; }

        //public MaterialFrontModule ImportantMaterials { get; set; }
        #endregion

    }

}
