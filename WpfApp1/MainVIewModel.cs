using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Services;
using WpfApp1.Wraps;

namespace WpfApp1
{
    public class MainVIewModel : BindableBase
    {
        public string login { get; set; }
        public string password { get; set; }
        public string seacrhingData { get; set; }
        public string NewTextPost { get; set; }

        public Post SelectedPost { get; set; }
        public ObservableCollection<Post> Posts {get;set;}


        private WEBApiService _apiService;
        public MainVIewModel()
        {
            _apiService = new WEBApiService();
        }


        public ICommand AddNewPost => new AsyncCommand(async () =>
        {
            await _apiService.AddPost(NewTextPost);
        });

        public ICommand Remove => new AsyncCommand(async () =>
        {
            if (SelectedPost == null)
                return;
            await _apiService.RemovePostById(SelectedPost.id);
        });

        public ICommand GetPost => new AsyncCommand(async () =>
        {
            var coll = await _apiService.GetPost(seacrhingData);
            Posts = new ObservableCollection<Post>(coll);
        });
        public ICommand LOGIN => new AsyncCommand(async () =>
        {
            await _apiService.login(login, password);
        });
    }
}
