﻿#region Imports
using Shared.Codes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks; 
#endregion

namespace Microservice.Services {
    public class UsersService {

        #region GetAll
        public List<EUser> GetAll(int listCount = -1, int pageNumber = 0, string orderBy="id asc") {
            List<EUser> list = null;
            using var context = new SMySQLContext();
            if (listCount == -1) list = context.Users.ToList();
            else list = context.Users.OrderBy(orderBy).Skip(pageNumber * listCount).Take(listCount).ToList();
            return list;
        }
        #endregion

        #region GetByID
        public EUser GetByID(Int64 id) {
            using var context = new SMySQLContext();
            var e = context.Users.SingleOrDefault(x => x.id == id);
            return e;
        }
        #endregion

        #region SaveAsync
        public async Task<Int64> SaveAsync(EUser eUser) {
            //eUser.ModificationDateUTC = DateTime.UtcNow;
            await using var context = new SMySQLContext();
            if (eUser.id < 1) {
                //eUser.CreationDateUTC = eUser.ModificationDateUTC = DateTime.UtcNow;
                var e = await context.Users.AddAsync(eUser);
                await context.SaveChangesAsync();
                return e.Entity.id;
            } else {
                var e = context.Users.Update(eUser);
                await context.SaveChangesAsync();
                return e.Entity.id;
            }
        }
        #endregion

        #region InsertAsync
        public async Task<Int64> InsertAsync(EUser eUser) {
            await using var context = new SMySQLContext();
            var e = await context.Users.AddAsync(eUser);
            await context.SaveChangesAsync();
            return e.Entity.id;
        }
        #endregion

        #region UpdateAsync
        public async Task<Int64> UpdateAsync(EUser eUser) {
            await using var context = new SMySQLContext();
            var e = context.Users.Update(eUser);
            await context.SaveChangesAsync();
            return e.Entity.id;
        } 
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Int64 id) {
            await using var context = new SMySQLContext();
            var e = context.Users.SingleOrDefault(x => x.id == id);
            if (e == null) return false;
            context.Remove(e);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}