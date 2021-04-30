using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using UK.CO.Chrisjenx.Calligraphy;
namespace Medical
{
    [Activity(Label = "Alarm Medicine")]
    public class AlarmMedicine : Activity
    {
        AlarmManager AlarmMg;PendingIntent PendIntnet;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);


        }

        protected override void OnStart()
        {
            base.OnStart();

        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }
    }


}