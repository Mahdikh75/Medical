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
    [Activity(Label = "Help About", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class HelpAbout : Activity
    {
        ImageView Mimg; TextView TxtMain; TypeImg img = TypeImg.SP;

        public enum TypeImg
        {
            SP, HR, CPB, AM, MG, ST, info
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.HelpAbout);

            Mimg = (ImageView)FindViewById(Resource.Id.AHImageViewMain);
            Mimg.SetImageResource(Resource.Drawable.SP);
            Mimg.Click += Mimg_Click;
            TxtMain = (TextView)FindViewById(Resource.Id.AHTextViewSummery);

            TxtMain.Text = "سلامتی" + "\n";
            TxtMain.Text += "در این بخش برنامه به ورزش بپردازید و با کنترل تعداد گام ها در طبیعت قدم بزنید و همین طور از لذت کوه نوردی با صعود قله بهره مند بشوید." + "\n";
            TxtMain.Text += "قسمت سلامتی امکانات این قسمت 1- شمارش گام ها 2- مشخصات چی پی اس 3- ارتفاع صعود قله";
        }

        protected override void OnStart()
        {
            base.OnStart();
            Toast.MakeText(this, "لطفا روی تصاویر لمس کنید تا از مزایای برنامه اطلاع یابید", ToastLength.Long).Show();
        }

        private void Mimg_Click(object sender, EventArgs e)
        {
            switch (img)
            {
                case TypeImg.SP:
                    Mimg.SetImageResource(Resource.Drawable.HR);
                    img = TypeImg.HR;
                    TxtMain.Text = "ضربان قلب" + "\n";
                    TxtMain.Text += "ضربان قلب خود را روزانه اندازه گیری کنید و آن ها را در برنامه یادداشت کنید و از محدوده ضربان قلب خود آگاه شوید" + "\n";
                    TxtMain.Text += "در این قسمت می توانید ضربان قلب خود را اندازه کنید و مشخصات ضربان مطلع شوید.";
                    AnimationImgMul();
                    break;
                case TypeImg.HR:
                    Mimg.SetImageResource(Resource.Drawable.CBP);
                    img = TypeImg.CPB;
                    TxtMain.Text = "کنترل وضعیت بدن" + "\n";
                    TxtMain.Text += "برنامه غذایی روزانه و فعالیت های روزانه خود را ذخیره نمایید و در آخر روز آن را با شاخص ها قیاس کنید" + "\n";
                    TxtMain.Text += "این قسمت برنامه یادداشتی از دریافت کالری روزانه و آب و کافئین و استراحت روزانه و مقادیر قابل ذخیره تعداد ضربان قلب و وزن و قد و ورزش های روزانه";
                    AnimationImgMul();
                    break;
                case TypeImg.CPB:
                    Mimg.SetImageResource(Resource.Drawable.AM);
                    img = TypeImg.AM;
                    TxtMain.Text = "یادآور دارو" + '\n';
                    TxtMain.Text += "در هنگام بیماری نگران زمان خوردن دارو ها نباشید با یک برنامه ریزی دقیق و زمان بندی شده دارو های خود را مصرف کنید" + '\n';
                    TxtMain.Text += "این قسمت برنامه یادآوری دارو و یادداشتی از کاربرد آن دارد";
                    AnimationImgMul();
                    break;
                case TypeImg.AM:
                    Mimg.SetImageResource(Resource.Drawable.MG);
                    img = TypeImg.MG;
                    TxtMain.Text = "ماساژور" + '\n';
                    TxtMain.Text += "در هنگام خستگی و ناراحتی کمی ماساژ می تواند حسابی روحیه و انرژی به فعالیت های شما دهد" + '\n';
                    TxtMain.Text += "این قسمت هم وظیفه ماساژ دادن با استفاده ویبره گوشی میباشد البته استفاده زیاد هم میتواند برای این سیستم مشکل ساز شود" + '\n';
                    AnimationImgMul();
                    break;
                case TypeImg.MG:
                    Mimg.SetImageResource(Resource.Drawable.ST);
                    img = TypeImg.ST;
                    TxtMain.Text = "تست - شاخص ها" + '\n';
                    TxtMain.Text += "دانستن برخی اطلاعات اساسی بدن ضروری است مثل : مقدار دریافت کالری و وزن آیده آل و مقدار آب دریافتی روزانه و ساعت خواب و مشخصات بدن" + '\n';
                    TxtMain.Text += "تست افسردگی و تست قند خون نوع دوم هم میتواند شما را از برخی مشکلات و یا برخی از بیماری درونی آگاه کند" + '\n';
                    AnimationImgMul();
                    break;
                case TypeImg.ST:
                    Mimg.SetImageResource(Resource.Drawable.InfoApp);
                    img = TypeImg.info;
                    TxtMain.Text = "درباره ما" + '\n';
                    TxtMain.Text += "با تشکر از حسن انتخاب شما این برنامه از قابلیت های متعددی که پزشکی رایج و مهم است بهره مند است" + '\n';
                    TxtMain.Text += "منابع تست ها و شاخص ها : www.mdcalc.com" + "\n";
                    TxtMain.Text += "با تشکر - مهدی خیامدار" + '\n';
                    AnimationImgMul();
                    break;
                case TypeImg.info:
                    Mimg.SetImageResource(Resource.Drawable.SP);
                    img = TypeImg.SP;
                    TxtMain.Text = "سلامتی" + "\n";
                    TxtMain.Text += "در این بخش برنامه به ورزش بپردازید و با کنترل تعداد گام ها در طبیعت قدم بزنید و همین طور از لذت کوه نوردی با صعود قله بهره مند بشوید." + "\n";
                    TxtMain.Text += "قسمت سلامتی امکانات این قسمت 1- شمارش گام ها 2- مشخصات چی پی اس 3- ارتفاع صعود قله";
                    AnimationImgMul();
                    break;
                default:
                    break;
            }

        }

        public async void AnimationImgMul()
        {
            for (int i = 0; i < 25; i++)
            {
                Mimg.ImageAlpha = (i * 10) + 15;
                await System.Threading.Tasks.Task.Delay(15);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 120, 0, "ایمیل بزنید");
            menu.Add(0, 121, 0, "نظر دادن در مایکت");
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            if (item.ItemId == 120)
            {
                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetTitle("ایمیل");
                Msg.SetMessage("آیا میخواهید نظر خود را با ما در میان بگذارید ؟");
                Msg.SetPositiveButton("بله", delegate
                {
                    Intent Email = new Intent(Intent.ActionSend);
                    Email.SetType("text/plain");
                    Email.PutExtra(Intent.ExtraEmail, new string[] { "DeveloperAndroid4444@gmail.com" });
                    Email.PutExtra(Intent.ExtraSubject, "برنامه پزشکی");

                    string AndroidSdk = "Number Sdk : " + Android.OS.Build.VERSION.Sdk + " - " + Build.VERSION.SdkInt;
                    string NameSmartPhone = "Model : " + Build.Model + " - " + Build.Manufacturer;

                    Email.PutExtra(Intent.ExtraText, " اطلاعات لازم : " + '\n' + AndroidSdk + '\n' + NameSmartPhone);
                    StartActivity(Intent.CreateChooser(Email, "ایمیل"));
                });
                Msg.SetNegativeButton("خیر", delegate { });
                Msg.Create(); Msg.Show();
            }
            else if (item.ItemId == 121)
            {
                //https://myket://comment?id=Com.Mkh75_Medical
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://myket.ir/app/Com.Mkh75_Medical/?lang=fa"));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(Intent.CreateChooser(intent, "مایکت را انتخاب کنید"));
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }
    }
}