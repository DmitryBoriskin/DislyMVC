﻿using System.Linq;
using cms.dbModel;
using cms.dbModel.entity;
using cms.dbase.models;
using LinqToDB;
using System;

namespace cms.dbase.Repository
{
    public class OnlineRegistryRepository : AbstractOnlineRegistryRepository
    {
        private string _context = null;
        
        public OnlineRegistryRepository()
        {
            _context = "registryConnection";
        }

        public OnlineRegistryRepository(string connectionString)
        {
            _context = connectionString;
        }

        /// <summary>
        /// Получаем список врачей
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public override Doctor[] getVDoctors(string oid)
        {
            using (var db = new registry(_context))
            {
                var query = db.V_Doctorss
                    .Where(w => w.C_Hospital_OID.Equals(oid))
                    .Select(s => new Doctor
                    {
                        FIO = s.C_FIO,
                        SNILS = s.C_SNILS,
                        Url = s.C_Registry_URL,
                        HospitalOid = s.C_Hospital_OID,
                        HospitalName = s.C_Hospital_Name
                    });

                if (!query.Any()) return null;
                return query.ToArray();
            }
        }

        /// <summary>
        /// Получаем список докторов
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="snils"></param>
        /// <returns></returns>
        public override Doctor[] getVDoctors(string oid, string snils)
        {
            using (var db = new registry(_context))
            {
                var query = db.V_Doctorss
                    .Where(w => w.C_Hospital_OID.Equals(oid))
                    .Where(w => w.C_SNILS.Equals(snils))
                    .Where(w => w.C_Registry_URL != null)
                    .Select(s => new Doctor
                    {
                        FIO = s.C_FIO,
                        SNILS = s.C_SNILS,
                        District = s.C_District,
                        DistrictType = s.C_DistrictType,
                        Spec = s.C_DoctorSpec,
                        Url = s.C_Registry_URL,
                        HospitalOid = s.C_Hospital_OID,
                        HospitalName = s.C_Hospital_Name
                    });

                if (!query.Any()) return null;
                return query.ToArray();
            }
        }

        /// <summary>
        /// Получаем организацию
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public override Hospital getHospital(string oid)
        {
            using (var db = new registry(_context))
            {
                var query = db.V_Hospitalss
                    .Where(w => w.C_OID.Equals(oid))
                    .Select(s => new Hospital
                    {
                        Id = s.Id,
                        FullName = s.C_FullName,
                        ShortName = s.C_ShortName,
                        Url = s.C_RegistryURL,
                        Oid = s.C_OID
                    });

                if (!query.Any()) return null;
                return query.SingleOrDefault();
            }
        }

        /// <summary>
        /// Получим эл-т карты сайта старой БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override dbModel.entity.Hierarhy getHierarhy(int id)
        {
            using (var db = new registry(_context))
            {
                var query = db.Hierarhys
                    .Where(w => w.id.Equals(id))
                    .Select(s => new dbModel.entity.Hierarhy
                    {
                        Caption = s.caption,
                        Text = s.text,
                        Item = s.item
                    });

                if (!query.Any()) return null;
                return query.SingleOrDefault();
            }
        }
    }
}
