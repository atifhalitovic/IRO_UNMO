using IRO_UNMO.Mobile.Models;
using IRO_UNMO.Mobile.NewViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using IRO_UNMO.Model.Requests;
using IRO_UNMO.Model;

namespace IRO_UNMO.Mobile.ViewModels
{
    public class ApplicantsViewModel : BaseViewModel
    {
        private readonly APIService _serviceApplicant = new APIService("Applicant");
        private readonly APIService _serviceUniversity = new APIService("University");
        public ObservableCollection<Applicant> ApplicantList { get; set; } = new ObservableCollection<Applicant>();

        private readonly INavigation Navigation;
        public ICommand NavigateToSearchPageCommand { get; set; }
        public ICommand InitCommand { get; set; }
        public ApplicantSearchRequest _search;

        public ApplicantsViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            InitCommand = new Command(async () => await Init());
        }

        public ApplicantsViewModel(INavigation navigation, ApplicantSearchRequest request)
        {
            _search = request;
            this.Navigation = navigation;
            InitCommand = new Command(async () => await Init());
        }

        public ObservableCollection<University> UniversityList { get; set; } = new ObservableCollection<University>();

        University _selectedUniversity = null;

        public University SelectedUniversity
        {
            get { return _selectedUniversity; }
            set
            {
                SetProperty(ref _selectedUniversity, value);
                if (value != null)
                {
                    InitCommand.Execute(null);
                }
            }
        }

        public async Task Init()
        {
            if (UniversityList.Count == 0)
            {
                var universityList = await _serviceUniversity.Get<List<University>>(null);

                foreach (var k in universityList)
                {
                    UniversityList.Add(k);
                }
            }

            if (SelectedUniversity != null)
            {
                var listApplicants = await _serviceApplicant.Get<List<Applicant>>(_search);
               
                ApplicantList.Clear();

                foreach (var m in listApplicants)
                {
                    if(m.UniversityId == SelectedUniversity.UniversityId)
                    {
                        ApplicantList.Add(m);
                    }
                }
            }
            else
            {
                var listApplicants = await _serviceApplicant.Get<List<Applicant>>(_search);

                ApplicantList.Clear();
                foreach (var x in listApplicants)
                {
                    ApplicantList.Add(x);
                }
            }
        }
    }
}
