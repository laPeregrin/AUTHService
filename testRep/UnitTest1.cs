using DataBaseStaff;
using DataBaseStaff.Models;
using DataBaseStaff.Service;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace testRep
{
    public class Tests
    {
        private BllLayer _service;
        private  EfDbContext _context;
        private HashTagService hservice;
        [SetUp]
        public void Setup()
        {
            _context = new EfDbContext();
            hservice = new HashTagService(_context);
            _service = new BllLayer(_context, hservice);
           
        }

        [Test]
        public async Task Test1()
        {
            var user = await _service.GetUserByExpression(x => x.UserName == "reader");
            var publciation = new Publication()
            {
                Id = Guid.NewGuid(),
                Text = "А я бычок подниму, горький дым затяну,Покурю и полезу домо-ой.Не жалейте меня, я прекрасно живу,Только кушать охота порой.А я бычок подниму, #горький дым затяну,Люк открою, полезу домо-ой.Не #жалейте меня, я прекрасно живу,Только кушать охота #порой.",
                User = user,
                UserId = user.Id
            };
            await _service.AddPublication(publciation);
            return;
        }

        [Test]
        public async Task GetByHashTag_content_returnCollectionPublicationWithPublications()
        {
            string content = "#горький";

            var collection = await _service.GetByHashTag(content);

            return;
        }
        [Test]
        public async Task RemovePostById_id_Success()
        {
           await _service.RemovePostById(new Guid("89b507c8-aa3a-4169-a53e-6158537ae9ba"));
            return;
        }
    }
}