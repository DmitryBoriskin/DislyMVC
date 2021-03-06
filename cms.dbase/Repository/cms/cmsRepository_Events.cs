﻿using System;
using System.Linq;
using cms.dbModel;
using cms.dbModel.entity;
using cms.dbase.models;
using LinqToDB;

namespace cms.dbase
{
    /// <summary>
    /// Репозиторий для работы с новостями
    /// </summary>
    public partial class cmsRepository : abstract_cmsRepository
    {

        #region Events
        private IQueryable<content_events> queryByEventFilter(CMSdb db, EventFilter filtr)
        {
            if (filtr == null)
                throw new Exception("cmsRepository > queryByEventFilter: Filter is null");

            var query = db.content_eventss
                              .AsQueryable();

            if (!string.IsNullOrEmpty(filtr.Domain))
            {
                //Проверка происходит в методе
            }

            if (!string.IsNullOrEmpty(filtr.SearchText))
            {
                query = query.Where(w => w.c_title.Contains(filtr.SearchText));
            }

            if (filtr.RelId.HasValue && filtr.RelId.Value != Guid.Empty)
            {
                //В таблице ищем связи оранизация - контент (новость/событие)
                var objctLinks = db.content_content_links
                    .Where(s => s.f_content == filtr.RelId.Value)
                    .Where(s => s.f_content_type == filtr.RelType.ToString().ToLower())
                    .Where(s => s.f_link_type == ContentLinkType.EVENT.ToString().ToLower());

                if (!objctLinks.Any())
                    query = query.Where(o => o.id == Guid.Empty); //Делаем заранее ложный запрос
                else
                {
                    var objctsId = objctLinks.Select(o => o.f_link);
                    query = query.Where(o => objctsId.Contains(o.id));
                }
            }

            return query;
        }

        public override EventsModel getEvent(Guid id)
        {
            using (var db = new CMSdb(_context))
            {

                var data = db.content_eventss.
                    Where(w => w.id == id).
                    Select(s => new EventsModel
                    {
                        Id = s.id,
                        Num = s.num,
                        Title = s.c_title,
                        Alias = s.c_alias.ToLower(),
                        Place = s.c_place,
                        EventMaker = s.c_organizer,
                        PreviewImage = new Photo()
                        {
                            Url = s.c_preview,
                            Source = s.c_preview_source,
                        },
                        Text = s.c_text,
                        Url = s.c_url,
                        UrlName = s.c_url_name,
                        DateBegin = s.d_date,
                        DateEnd = s.d_date_end,
                        Annually = s.b_annually,
                        KeyW = s.c_keyw,
                        Desc = s.c_desc,
                        Disabled = s.b_disabled,
                        SiteId = getSiteId(s.id),
                        Locked = s.b_locked,
                        //Links  заполняем в контроллере
                    });
                if (!data.Any()) { return null; }
                else { return data.First(); }
            }
        }
        public override EventsList getEventsList(EventFilter filtr)
        {
            using (var db = new CMSdb(_context))
            {

                if (filtr == null)
                    throw new Exception("cmsRepository > queryByEventFilter: Filter is null");

                #region Фильтр
                var query = db.content_eventss
                                  .AsQueryable();

                if (!string.IsNullOrEmpty(filtr.SearchText))
                {
                    query = query.Where(w => w.c_title.Contains(filtr.SearchText));
                }

                //В таблице ищем связи оранизация - контент(новость / событие)
                if (filtr.RelId.HasValue && filtr.RelId.Value != Guid.Empty)
                {
                    var objctLinks = db.content_content_links
                        .Where(s => s.f_content == filtr.RelId.Value)
                        .Where(s => s.f_content_type == filtr.RelType.ToString().ToLower())
                        .Where(s => s.f_link_type == ContentLinkType.EVENT.ToString().ToLower());

                    if (!objctLinks.Any())
                        query = query.Where(o => o.id == Guid.Empty); //Делаем заранее ложный запрос
                    else
                    {
                        var objctsId = objctLinks.Select(o => o.f_link);
                        query = query.Where(o => objctsId.Contains(o.id));
                    }
                }

                //Если указан домен, то выбираем записи, принадлежащие ему
                if (!string.IsNullOrEmpty(filtr.Domain))
                {
                    var contentType = ContentType.EVENT.ToString().ToLower();
                    var events = db.content_content_links.Where(e => e.f_content_type == contentType)
                            .Join(db.cms_sitess.Where(o => o.c_alias.ToLower() == filtr.Domain),
                                    e => e.f_link,
                                    o => o.f_content,
                                    (e, o) => e.f_content
                                    );

                    if (!events.Any())
                        return null;

                    query = query.Where(w => events.Contains(w.id));
                }
                #endregion


                //Сортировка
                query = query.OrderByDescending(w => w.d_date);

                int itemCount = query.Count();

                var List = query
                    .OrderByDescending(s => s.d_date)
                    .Skip(filtr.Size * (filtr.Page - 1))
                    .Take(filtr.Size)
                    .Select(s => new EventsModel
                    {
                        Id = s.id,
                        Num = s.num,
                        //SiteId = s.alias,
                        Title = s.c_title,
                        Alias = s.c_alias.ToLower(),
                        Place = s.c_place,
                        EventMaker = s.c_organizer,
                        PreviewImage = new Photo()
                        {
                            Url = s.c_preview,
                            Source = s.c_preview_source
                        },
                        Text = s.c_text,
                        Url = s.c_url,
                        UrlName = s.c_url_name,
                        DateBegin = s.d_date,
                        DateEnd = s.d_date_end,
                        Annually = s.b_annually,
                        KeyW = s.c_keyw,
                        Desc = s.c_desc,
                        Disabled = s.b_disabled,
                        Locked = s.b_locked
                    });

                if (!List.Any())
                    return null;

                return new EventsList()
                {
                    Data = List.ToArray(),
                    Pager = new Pager()
                    {
                        Page = filtr.Page,
                        Size = filtr.Size,
                        ItemsCount = itemCount,
                        //PageCount = (itemCount % filtr.Size > 0) ? (itemCount / filtr.Size) + 1 : itemCount / filtr.Size
                    }
                };
            }
        }

        public override EventsShortModel[] getLastEventsListWithCheckedFor(EventFilter filtr)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_eventss
                                .Where(s => s.id != filtr.RelId); // Само на себя событие не может ссылаться - это зацикливание


                var List = query
                    .OrderByDescending(s => s.d_date)
                    .Take(filtr.Size)
                    .Select(s => new EventsShortModel()
                    {
                        Id = s.id,
                        Title = s.c_title,
                        DateBegin = s.d_date,
                        Text = s.c_text,
                        Checked = ContentLinkExists(filtr.RelId.Value, filtr.RelType, s.id, ContentLinkType.EVENT),
                        Origin = ContentLinkOrigin(filtr.RelId.Value, filtr.RelType, s.id, ContentLinkType.EVENT)
                    });

                    if (List.Any())
                        return List.ToArray();

                return null;

            }
        }

        public override bool insertCmsEvent(EventsModel eventData)
        {
            try
            {
                using (var db = new CMSdb(_context))
                {
                    using (var tran = db.BeginTransaction())
                    {
                        content_events cdEvent = db.content_eventss
                                                        .Where(p => p.id == eventData.Id)
                                                        .SingleOrDefault();
                        if (cdEvent != null)
                        {
                            throw new Exception("Запись с таким Id уже существует");
                        }

                        var EndDate = (eventData.DateEnd.HasValue) ? eventData.DateEnd.Value : eventData.DateBegin;
                        cdEvent = new content_events
                        {
                            id = eventData.Id,                               
                            c_alias = eventData.Alias.ToLower(),
                            c_title = eventData.Title,
                            c_text = eventData.Text,
                            c_place = eventData.Place,
                            c_organizer = eventData.EventMaker,
                            c_preview = (eventData.PreviewImage != null) ? eventData.PreviewImage.Url : null,
                            c_preview_source = (eventData.PreviewImage != null) ? eventData.PreviewImage.Source : null,
                            c_desc = eventData.Desc,
                            c_keyw = eventData.KeyW,
                            b_annually = eventData.Annually,
                            b_disabled = eventData.Disabled,
                            d_date = eventData.DateBegin,
                            d_date_end = EndDate,
                            c_url = eventData.Url,
                            c_url_name = eventData.UrlName,
                            n_date_begin_day = int.Parse(eventData.DateBegin.ToString("MMdd")),
                            n_date_end_day = int.Parse(EndDate.ToString("MMdd")),

                            f_content_origin = eventData.ContentLink,
                            c_content_type_origin = eventData.ContentLinkType,
                            b_locked = eventData.Locked
                        };
                        if (!eventData.Annually)
                        { 
                            cdEvent.n_date_begin_year = eventData.DateBegin.Year;
                            if (eventData.DateEnd.HasValue)
                                cdEvent.n_date_end_year = eventData.DateEnd.Value.Year;
                        }


                        var cdContentLink = new content_content_link()
                        {
                            id = Guid.NewGuid(),
                            f_content = eventData.Id,
                            f_content_type = ContentType.EVENT.ToString().ToLower(),
                            f_link = eventData.ContentLink,
                            f_link_type = eventData.ContentLinkType,
                            b_origin = true
                        };
                        var query_str = db.LastQuery;
                        db.Insert(cdEvent);
                       

                        db.Insert(cdContentLink);

                        //логирование
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Events,
                            Action = LogAction.insert,
                            PageId = eventData.Id,
                            PageName = eventData.Title,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to log ex
                return false;
            }
        }
        public override bool updateCmsEvent(EventsModel eventData)
        {
            try
            {
                using (var db = new CMSdb(_context))
                {
                    using (var tran = db.BeginTransaction())
                    {
                        content_events cdEvent = db.content_eventss
                                                .Where(p => p.id == eventData.Id)
                                                .SingleOrDefault();
                        if (cdEvent == null)
                        {
                            throw new Exception("Запись с таким Id не найдена");
                        }

                        var EndDate = (eventData.DateEnd.HasValue) ? eventData.DateEnd.Value : eventData.DateBegin;

                        cdEvent.c_alias = eventData.Alias.ToLower();
                        cdEvent.c_title = eventData.Title;
                        cdEvent.c_text = eventData.Text;
                        cdEvent.c_place = eventData.Place;
                        cdEvent.c_organizer = eventData.EventMaker;
                        if (eventData.PreviewImage != null)
                        {
                            cdEvent.c_preview = eventData.PreviewImage.Url;
                            cdEvent.c_preview_source = eventData.PreviewImage.Source;
                        }
                        else
                        {
                            cdEvent.c_preview = null;
                            cdEvent.c_preview_source = null;
                        }

                        cdEvent.c_desc = eventData.Desc;
                        cdEvent.c_keyw = eventData.KeyW;
                        cdEvent.b_annually = eventData.Annually;
                        cdEvent.b_disabled = eventData.Disabled;
                        cdEvent.d_date = eventData.DateBegin;
                        cdEvent.d_date_end = EndDate;
                        cdEvent.c_url = eventData.Url;
                        cdEvent.c_url_name = eventData.UrlName;
                        cdEvent.n_date_begin_day = int.Parse(eventData.DateBegin.ToString("MMdd"));
                        cdEvent.n_date_end_day = int.Parse(EndDate.ToString("MMdd"));
                        cdEvent.b_locked = eventData.Locked;

                        if (!eventData.Annually)
                        {
                            cdEvent.n_date_begin_year = eventData.DateBegin.Year;
                            cdEvent.n_date_end_year = eventData.DateEnd.Value.Year;
                        }
                        else
                        {
                            cdEvent.n_date_begin_year = null;
                            cdEvent.n_date_end_year = null;
                        }

                        db.Update(cdEvent);

                        //логирование
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Events,
                            Action = LogAction.update,
                            PageId = eventData.Id,
                            PageName = eventData.Title,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to log ex
                return false;
            }
        }
        public override bool deleteCmsEvent(Guid id)
        {
            try
            {
                using (var db = new CMSdb(_context))
                {
                    using (var tran = db.BeginTransaction())
                    {
                        var data = db.content_eventss
                                                .Where(p => p.id == id);
                                                
                        if (!data.Any())
                        {
                            throw new Exception("Запись с таким Id не найдена");
                        }

                        var cdEvent = data.SingleOrDefault();
                        
                        //Delete links to other objects
                        var q2 = db.content_content_links
                             .Where(s => s.f_content == id)
                             .Delete();

                        db.Delete(cdEvent);

                        //логирование
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Events,
                            Action = LogAction.delete,
                            PageId = cdEvent.id,
                            PageName = cdEvent.c_title,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to log ex
                return false;
            }
        }
        #endregion

    }
}
