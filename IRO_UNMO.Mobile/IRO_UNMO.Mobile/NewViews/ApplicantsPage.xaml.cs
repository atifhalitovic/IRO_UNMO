using IRO_UNMO.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IRO_UNMO.Mobile.NewViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ApplicantsPage : ContentPage
	{
        private ApplicantsViewModel model = null;
        public ApplicantsPage()
		{
			InitializeComponent ();
            BindingContext = model = new ApplicantsViewModel(this.Navigation);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await model.Init();
        }
    }
}