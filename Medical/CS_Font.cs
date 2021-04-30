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
    [Application]
    public class CSFont : Application
    {
        public CSFont(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder().SetDefaultFontPath
                ("fonts/iran_sans.ttf").SetFontAttrId(Resource.Attribute.fontPath).Build());
        }

    }
}