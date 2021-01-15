using DataBaseStaff.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBaseStaff.Service
{
    public class HashTagService
    {
        //private EfDbContext _service;

        public HashTagService()
        {
            //_service = service;
        }

        public async Task SyncHashTags(string text, Publication publication)
        {
            var taskList = new ConcurrentQueue<Task>();
            var hashtagsArray = GetAllHashtags(text);

            var hashTags = new List<HashTag>();
            foreach (var item in hashtagsArray)
            {
                EfDbContext _service = new EfDbContext();
                taskList.Enqueue(Task.Run(async()=>
                {
                    var hashtag = await _service.HashaTags.FirstOrDefaultAsync(x => x.HashTagContent == item);
                    if (hashtag == null)
                    {
                        var publications = new List<Publication>();
                        publications.Add(publication);
                        hashtag = new HashTag()
                        {
                            Id = Guid.NewGuid(),
                            HashTagContent = item,
                            Publications = publications
                        };
                        await _service.HashaTags.AddAsync(hashtag);
                        await _service.SaveChangesAsync();
                    }
                    else
                    {
                        var newPublications = hashtag.Publications.ToList();
                        newPublications.Add(publication);
                        hashtag.Publications = newPublications;

                        _service.HashaTags.Update(hashtag);
                        await _service.SaveChangesAsync();
                    }
                }));
            }
            if (taskList.Any())
            {
                await Task.WhenAll(taskList);
                taskList.Clear();
            }
        }




        private string[] GetAllHashtags(string text)
        {
            Regex regex = new Regex(@"\#\w+");

            var collection = regex.Matches(text).AsEnumerable().ToArray();

            int i = 0;

            string[] container = new string[collection.Count()];
            foreach (var item in collection)
            {

                container[i] = item.ToString().ToLower();
                i++;
            }
            return container;
        }
    }
}
