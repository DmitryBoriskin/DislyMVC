﻿using cms.dbModel.entity;
using cms.dbModel.entity.cms;

namespace Disly.Models
{
    /// <summary>
    /// Модель докторов для типовой страницы 
    /// </summary>
    public class DoctorsViewModel : PageViewModel
    {
        /// <summary>
        /// Список докторов
        /// </summary>
        public People[] DoctorsList { get; set; }

        /// <summary>
        /// Единичная запись доктора
        /// </summary>
        public People DoctorsItem { get; set; }

        /// <summary>
        /// Список структур
        /// </summary>
        public StructureModel[] DepartmentsSelectList { get; set; }

        /// <summary>
        /// Список должностей
        /// </summary>
        public PeoplePost[] PeoplePosts { get; set; }
    }
}
