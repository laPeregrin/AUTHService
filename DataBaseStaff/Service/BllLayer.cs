using DataBaseStaff.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseStaff.Service
{
    public class BllLayer
    {
        private readonly EfDbContext DataContextService;
        private readonly HashTagService _hashtagService;
        public BllLayer(EfDbContext service, HashTagService hashtagService)
        {
            DataContextService = service;
            _hashtagService = hashtagService;
        }

        public async Task AddUser(User obj)
        {
            DataContextService.Users.Add(obj);
            await DataContextService.SaveChangesAsync();
        }


        public async Task AddPublication(Publication publication)
        {
            try
            {
                DataContextService.Publications.Add(publication);
                await DataContextService.SaveChangesAsync();

                await _hashtagService.SyncHashTags(publication.Text, publication);
            }
            catch(Exception e) { }
        }


        public async Task<domainObject> GetById<T>(Guid id) where T : domainObject
        {
            return await DataContextService.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<User> GetUserByExpression(Expression<Func<User, bool>> expression)
        {
            return await DataContextService.Users.FirstOrDefaultAsync(expression);
        }

        public async Task RemovePostById(Guid id)
        {
            var post = await DataContextService.Publications.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                throw new ArgumentNullException();

            DataContextService.Remove(post);
            await DataContextService.SaveChangesAsync();
        }

        public async Task<IEnumerable<Publication>> GetByHashTag(string content)
        {
            var hashtag = await DataContextService.HashaTags.Include(x => x.Publications).FirstOrDefaultAsync(x => x.HashTagContent == content);
            if (hashtag == null)
                throw new KeyNotFoundException(content);

            return hashtag.Publications;
        }

        public async Task<IEnumerable<Publication>> GetAllPublication()
        {
            return await DataContextService.Publications.AsNoTracking().ToListAsync();
        }



    }
}
