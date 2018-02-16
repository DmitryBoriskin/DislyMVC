﻿using cms.dbase.models;
using cms.dbModel;
using cms.dbModel.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using cms.dbModel.entity.cms;
using LinqToDB;

namespace cms.dbase.Mapping
{
    internal static class DbQueryMap
    {
        internal static IQueryable<People> MapSearch(this IQueryable<content_people> query, IQueryable<cms_sites> sites)
        {
            return query
                    .OrderBy(o => o.c_surname)
                    .Select(s => new People
                    {
                        Id = s.id,
                        FIO = s.c_surname + " " + s.c_name + " " + s.c_patronymic,
                        Photo = s.c_photo,
                        Posts = s.employeespostspeoples
                                    .Where(a => !a.b_dissmissed)
                                    .Select(p => new EmployeePost
                                    {
                                        Id = p.f_post,
                                        Name = p.employeespostsspecializations.c_name,
                                        Type = p.n_type,
                                        Org = new OrgsShortModel
                                        {
                                            Id = p.contentemployeespostsorgs.id,
                                            Title = p.contentemployeespostsorgs.c_title,
                                            Url = sites.Where(w => w.f_content.Equals(p.contentemployeespostsorgs.id))
                                            .Select(r => r.fksitesdomainss.FirstOrDefault().c_domain)
                                            .SingleOrDefault()
                                        }
                                    })
                        //Posts = s.contentpeoplepostscontentpeoples
                        //            .Where(w => w.contentpeoplepostscontentpeople.contentpeopleorglinks.Any(a => !a.b_dismissed))
                        //            .Select(p => new EmployeePost
                        //            {
                        //                Id = p.f_post,
                        //                Name = p.contentpeoplepostscontentspecializations.c_name,
                        //                Type = p.n_type,
                        //                Org = new OrgsModel
                        //                {
                        //                    Id = p.contentpeoplepostscontentorgs.id,
                        //                    Title = p.contentpeoplepostscontentorgs.c_title,
                        //                    Url = sites.Where(w => w.f_content.Equals(p.contentpeoplepostscontentorgs.id))
                        //                   .Select(r => r.fksitesdomainss.FirstOrDefault().c_domain)
                        //                   .SingleOrDefault()
                        //                }
                        //            })
                    });
        }
    }
}
