using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using UK.CO.Chrisjenx.Calligraphy;
using Android.Media;
using Android.Preferences;

namespace Medical
{
    [Activity(Label = "Control body position", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ControlBodyPosition : Activity
    {
        // Custom tab host 
        TabHost TabHostMain; TabWidget TabW; TextView TitleHome, TitleSport, TitleWeight, TitleHRB;
        TypeTabHost tth = TypeTabHost.Home; Vibrator vibrator;
        // Tab Home
        ISharedPreferences Isp; ISharedPreferencesEditor EditISP; int CountTVR = 0; string[] Vitam;
        string KeyMain = "ISPE", KeyFood = "CBPFood", KeyWater = "CBPWater", KeyCafi = "CBPCafi",
            KeyRestShab = "CBPSRestShap", KeyRestRooz = "CBPSRestRooz", KeyRestTmp = "CBPResttmp";
        TextView HomeTxtVVitamin, HomeTxtVFood, HomeTxtVWater, HomeTxtVCafe, HomeTxtVSleep;
        // Tab Sprot
        ListView SportListvmd;
        // Tab HR
        ListView HRListvmd;
        // Tab WH
        ListView WHListvmd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.ControlBodyPosition);
            vibrator = (Vibrator)GetSystemService(VibratorService);
            Isp = GetSharedPreferences(KeyMain, FileCreationMode.WorldReadable | FileCreationMode.WorldWriteable);
            EditISP = Isp.Edit();
            LoadTabHost(); LoadHome(); LoadSport(); LoadHRB(); LoadWeightHeight(); AlertMsgVitam();


        }

        public void AlertMsgVitam()
        {
            Vitam = new string[15];
            Vitam[0] = "سردرد و سرگیجه" + '\n' + "نشانه های کمبود ویتامین : " + "B3" + '\n' + "در مواد غذایی زیر یافت میشود " + "کاکائو - شکلات - نارگیل - بادام زمینی - نخود فرنگی";
            Vitam[1] = "افسردگی" + '\n' + "نشانه های کمبود ویتامین : " + "D" + '\n' + "در مواد غذایی زیر یافت میشود " + "تخم مرغ - شیر - ماهی - پنیر";
            Vitam[2] = "اختلال دید شبانه" + '\n' + "نشانه های کمبود ویتامین : " + "A" + '\n' + "در مواد غذایی زیر یافت میشود " + "هلو - فلفل دلمه ای - هویچ - فلفل - میوه های رنگ نارنجی";
            Vitam[3] = "خونریزی لثه و زخم هایی که دیر خوب میشود" + '\n' + "نشانه های کمبود ویتامین : " + "C" + '\n' + "در مواد غذایی زیر یافت میشود " + "توت فرنگی - پرتقال - انگور - گوجه فرنگی - کیوی";
            Vitam[4] = "احساس خستگی" + '\n' + "نشانه های کمبود ویتامین : " + "B12" + '\n' + "در مواد غذایی زیر یافت میشود " + "قارچ - بادام - قهوه - چای - تخم مرغ - محصولات لبنی";
            Vitam[5] = "خشکی پوست" + '\n' + "نشانه های کمبود ویتامین : " + "A,B" + '\n' + "در مواد غذایی زیر یافت میشود " + "گوشت قرمز - گردو - کدو تنبل - کدو - آجیل - سبزیجات برگ دار";
            Vitam[6] = "خواب رفتن و کرختی دست و پا" + '\n' + "نشانه های کمبود ویتامین : " + "B6,B12" + '\n' + "در مواد غذایی زیر یافت میشود " + "مرغ - عسل - موز - ذرت - تخم مرغ - سبزیجات مثل : اسفناج و چغندر";
            Vitam[7] = "درد در انگشت ها و پشت ساق پا و کف پا" + '\n' + "نشانه های کمبود : " + "کلسیم - منیریم - پتاسیم" + '\n' + "در مواد غذایی زیر یافت میشود " + "پیاز - سیب - آناناس - کیوی - سیر - سبزیجات (برگ سبز) - نخود فرنگی - موز";
            Vitam[8] = "ریزش موی شدید" + '\n' + "نشانه های کمبود ویتامین : " + "B" + '\n' + "در مواد غذایی زیر یافت میشود " + "گندم (نان) - گوشت قرمز - سیب زمینی - ماست - سبزیجات (قارچ و گل کلم) - میوه های موز و تمشک";
            Vitam[9] = "بی خوابی" + '\n' + "نشانه های کمبود ویتامین : " + "B5" + '\n' + "در مواد غذایی زیر یافت میشود " + "قارچ - تخم مرغ - سبزیجات - میوه ها";
            Vitam[10] = "شکاف در گوشه لب ها" + '\n' + "نشانه های کمبود ویتامین : " + "B" + '\n' + "در مواد غذایی زیر یافت میشود " + "بادمجان - کلم - لبو - گلابی - بادام زمینی - مرغ - ماهی - حبوبات";
            Vitam[11] = "بی اشتهایی" + '\n' + "نشانه های کمبود ویتامین : " + "B" + '\n' + "در مواد غذایی زیر یافت میشود " + "غلات و گندم - گوشت قرمز - سیب زمینی - میگو - توت فرنگی و تمشک - بادام";
            Vitam[12] = "جوش های قرمز در بازو و گونه ها" + '\n' + "نشانه های کمبود ویتامین : " + "A,D" + '\n' + "در مواد غذایی زیر یافت میشود " + "جگر - پنیر - فلفل دلمه ای - هلو - قارچ - گوجه فرنگی - سبزیجات نارنجی و زرد رنگ" + "مقابل نور آفتاب 15 دقیقه در روز";
            Vitam[13] = "حالت تهوع و یبوست و نفخ" + '\n' + "نشانه های کمبود ویتامین : " + "B12" + '\n' + "در مواد غذایی زیر یافت میشود " + "شیر - ماهی و غذاهای دریایی - قلوه - بوقلمون - جگر - ماهی تن";
            Vitam[14] = "اسهال" + '\n' + "نشانه های کمبود ویتامین : " + "B9" + '\n' + "در مواد غذایی زیر یافت میشود " + "کلم - بامیه - حبوبات - پرتقال - سبزیجات";
            try
            {
                CountTVR = new Random().Next(0, 15);
                HomeTxtVVitamin.Text = Vitam[CountTVR];
            }
            catch { }
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 210, 0, "راهنما").SetIcon(Resource.Drawable.HelpRA).SetShowAsAction(ShowAsAction.Always);
            menu.Add(0, 250, 0, "افزدون داده ورزش");
            menu.Add(0, 251, 0, "افزدون داده ضربان قلب");
            menu.Add(0, 252, 0, "افزدون داده وزن");
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 210:
                    HelpAbout();
                    break;
                case 250:
                    LoadMsgBoxAdddata(0);
                    break;
                case 251:
                    LoadMsgBoxAdddata(1);
                    break;
                case 252:
                    LoadMsgBoxAdddata(2);
                    break;
                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        public void HelpAbout()
        {
            StringBuilder strb = new StringBuilder(); strb.Append("");
            strb.AppendLine("این قسمت برنامه از چهار اصلی تشکیل شده است");
            strb.AppendLine("کنترل وضعیت بدن - یادداشت ورزش - یادداشت ضربان قلب - یادداشت وزن و قد");
            strb.AppendLine("");
            strb.AppendLine("توضیح کنترل وضعیت بدن : ");
            strb.AppendLine("در این قسمت برنامه قسمت های مهم و حیاتی مثل  کالری و آب دریافتی و مصرف چای و قهوه و زمان استراحت بدن در بخش باید در یک 24 ساعتی که مقادیر کالری و آب و چای و قهوه و زمان خواب و بیداری و استراحت بین روز را وارد کنید تا با بررسی میتوان نتیجه گرفت چقدر با ایده آل بودن فاصله دارید");
            strb.AppendLine("");
            strb.AppendLine("یادداشت های مهم برنامه : ");
            strb.AppendLine("ورزش و تعداد گام ها و مشخصات که مد نظر دارید را اضافه کنید تا با برنامه تمرین کنید");
            strb.AppendLine("");
            strb.AppendLine("ضربان قلب خود را کنترل کنید و تعداد و تاریخ آن را یادداشت کنید");
            strb.AppendLine("");
            strb.AppendLine("از مشخصات ظاهری بدن وزن و قد از شاخص های مهم بخش هم میتوانید با اضافه کردن متوجه چاقی و لاغری خود شوید");
            strb.AppendLine("");

            AlertDialog.Builder help = new AlertDialog.Builder(this);
            help.SetTitle("راهنمای کنترل وضعیت بدن");
            help.SetMessage(strb.ToString());
            help.SetNeutralButton("لغو", delegate { });
            help.SetCancelable(false); help.Create(); help.Show();
        }

        public void ClickListViewDataBase(View view, string PMkeyCode, ListView lv)
        {
            TextView TxtTitle = view.FindViewById<TextView>(Resource.Id.LvCusTextViewTitle);
            TextView TxtText = view.FindViewById<TextView>(Resource.Id.LvCusTextViewSummery);
            string TxTitleMp = TxtTitle.Text;
            string TxTextMp = TxtText.Text;
            ISharedPreferences[] SWCSIsp = new ISharedPreferences[50];

            AlertDialog.Builder Mag = new AlertDialog.Builder(this);
            Mag.SetTitle("اعمال تغییرات");
            Mag.SetMessage("آیا مایل هستید تغییرات انجام دهید ؟");
            Mag.SetPositiveButton("حذف", delegate
            {
                for (int i = 0; i < 50; i++)
                {
                    SWCSIsp[i] = GetSharedPreferences(PMkeyCode + i, FileCreationMode.WorldReadable);
                    ISharedPreferencesEditor SIspEdit = SWCSIsp[i].Edit();
                    if (SWCSIsp[i].GetString("KeyTitle", "") == TxTitleMp && SWCSIsp[i].GetString("KeyText", "") == TxTextMp)
                    {
                        SIspEdit.Clear(); SIspEdit.Commit();
                        LoadData(PMkeyCode, lv);
                        Toast.MakeText(this, "اطلاعات حذف شد", ToastLength.Short).Show();
                    }
                }
            });
            Mag.SetNegativeButton("ویرایش", delegate
            {

                View AlView = View.Inflate(this, Resource.Layout.CBPInputDataSHRWT, null);
                EditText EdtTitle = (EditText)AlView.FindViewById(Resource.Id.CBPInputDataSHWTEditTextKeyTitle);
                EditText EdtText = (EditText)AlView.FindViewById(Resource.Id.CBPInputDataSHWTEditTextKeyText);

                string Title = "", HintTilte = "", HintText = "";

                switch (PMkeyCode)
                {
                    case "Sport":
                        Title = "ورزش";
                        HintText = "تعداد گام ها و پیمایش مسافت و کالری";
                        HintTilte = "عنوان اطلاعات"; ;
                        break;
                    case "HR":
                        Title = "ضربان قلب";
                        HintText = "تعداد ضربان قلب";
                        HintTilte = "تاریخ و عنوان";
                        break;
                    case "WH":
                        Title = "وزن و قد";
                        HintText = "مشخصات وزن - قد";
                        HintTilte = "عنوان اطلاعات";
                        break;
                    default:
                        break;
                }
                EdtText.Hint = HintText;
                EdtTitle.Hint = HintTilte;
                EdtText.Text = TxtText.Text;
                EdtTitle.Text = TxtTitle.Text;
                int IDKeyCode = 0;

                for (int i = 0; i < 50; i++)
                {
                    SWCSIsp[i] = GetSharedPreferences(PMkeyCode + i, FileCreationMode.WorldReadable);
                    ISharedPreferencesEditor SIspEdit = SWCSIsp[i].Edit();
                    if (SWCSIsp[i].GetString("KeyTitle", "") == TxtTitle.Text && SWCSIsp[i].GetString("KeyText", "") == TxtText.Text)
                    {
                        EdtText.Text = SWCSIsp[i].GetString("KeyText", "");
                        EdtTitle.Text = SWCSIsp[i].GetString("KeyTitle", "");
                        IDKeyCode = i;
                    }
                }

                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetTitle(Title);
                Msg.SetView(AlView);
                Msg.SetPositiveButton("ثبت ویرایش", delegate
                {
                    if (EdtText.Text != "" && EdtTitle.Text != "")
                    {
                        ISharedPreferences EditIspSHW = GetSharedPreferences(PMkeyCode + IDKeyCode, FileCreationMode.WorldReadable);
                        ISharedPreferencesEditor SIspEdit = EditIspSHW.Edit();
                        SIspEdit.PutString("KeyTitle", EdtTitle.Text);
                        SIspEdit.PutString("KeyText", EdtText.Text);
                        SIspEdit.Commit();
                        Toast.MakeText(this, " اطلاعات ثبت شد ", ToastLength.Short).Show();
                        LoadData(PMkeyCode, lv);
                    }
                });
                Msg.SetNegativeButton("لغو", delegate { });
                Msg.SetCancelable(false); Msg.Create(); Msg.Show();

            });
            Mag.Create(); Mag.Show();
        }

        public void LoadMsgBoxAdddata(int i)
        {
            View view = View.Inflate(this, Resource.Layout.CBPInputDataSHRWT, null);

            EditText EdtTitle = (EditText)view.FindViewById(Resource.Id.CBPInputDataSHWTEditTextKeyTitle);
            EditText EdtText = (EditText)view.FindViewById(Resource.Id.CBPInputDataSHWTEditTextKeyText);

            string Title = "", HintTilte = "", HintText = "", KeyCode = "";

            switch (i)
            {
                case 0:
                    Title = "ورزش";
                    HintText = "تعداد گام ها و پیمایش مسافت و کالری";
                    HintTilte = "عنوان اطلاعات";
                    KeyCode = "Sport";
                    break;
                case 1:
                    Title = "ضربان قلب";
                    HintText = "تعداد ضربان قلب";
                    HintTilte = "تاریخ و عنوان";
                    KeyCode = "HR";
                    break;
                case 2:
                    Title = "وزن و قد";
                    HintText = "مشخصات وزن - قد";
                    HintTilte = "عنوان اطلاعات";
                    KeyCode = "WH";
                    break;
                default:
                    break;
            }

            EdtText.Hint = HintText;
            EdtTitle.Hint = HintTilte;

            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle(Title);
            Msg.SetView(view);
            Msg.SetPositiveButton("اضافه کن", delegate
            {
                if (EdtText.Text != "" && EdtTitle.Text != "")
                {
                    switch (i)
                    {
                        case 0:
                            SetData(KeyCode, SportListvmd, EdtTitle.Text, EdtText.Text);
                            break;
                        case 1:
                            SetData(KeyCode, HRListvmd, EdtTitle.Text, EdtText.Text);
                            break;
                        case 2:
                            SetData(KeyCode, WHListvmd, EdtTitle.Text, EdtText.Text);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Toast.MakeText(this, "لطفا همه فیلد ها پر کنید", ToastLength.Short).Show();
                }
            });
            Msg.SetNegativeButton("لغو", delegate { });
            Msg.SetNeutralButton("حذف همه اطلاعات", delegate
            {
                switch (i)
                {
                    case 0:
                        ISharedPreferences[] SportIsp = new ISharedPreferences[50];
                        for (int j = 0; j < 50; j++)
                        {
                            SportIsp[j] = GetSharedPreferences("Sport" + j, FileCreationMode.WorldReadable);
                            ISharedPreferencesEditor SIspEdit = SportIsp[j].Edit();
                            SIspEdit.Clear(); SIspEdit.Commit();
                        }
                        LoadData("Sport", SportListvmd);
                        break;
                    case 1:
                        ISharedPreferences[] HRIsp = new ISharedPreferences[50];
                        for (int j = 0; j < 50; j++)
                        {
                            HRIsp[j] = GetSharedPreferences("HR" + j, FileCreationMode.WorldReadable);
                            ISharedPreferencesEditor SIspEdit = HRIsp[j].Edit();
                            SIspEdit.Clear(); SIspEdit.Commit();
                        }
                        LoadData("HR", HRListvmd);
                        break;
                    case 2:
                        ISharedPreferences[] WHIsp = new ISharedPreferences[50];
                        for (int j = 0; j < 50; j++)
                        {
                            WHIsp[j] = GetSharedPreferences("WH" + j, FileCreationMode.WorldReadable);
                            ISharedPreferencesEditor SIspEdit = WHIsp[j].Edit();
                            SIspEdit.Clear(); SIspEdit.Commit();
                        }
                        LoadData("WH", WHListvmd);
                        break;
                    default:
                        break;
                }
            });
            Msg.SetCancelable(false); Msg.Create(); Msg.Show();

        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                switch (tth)
                {
                    case TypeTabHost.Home:
                        Finish();
                        break;
                    case TypeTabHost.Sport:
                        tth = TypeTabHost.Home;
                        TabHostMain.CurrentTab = 0;
                        TitleHome.SetTextColor(Color.Maroon);
                        TitleHRB.SetTextColor(Color.WhiteSmoke);
                        TitleSport.SetTextColor(Color.WhiteSmoke);
                        TitleWeight.SetTextColor(Color.WhiteSmoke);
                        break;
                    case TypeTabHost.HRB:
                        tth = TypeTabHost.Sport;
                        TabHostMain.CurrentTab = 1;
                        TitleHome.SetTextColor(Color.WhiteSmoke);
                        TitleHRB.SetTextColor(Color.WhiteSmoke);
                        TitleSport.SetTextColor(Color.Maroon);
                        TitleWeight.SetTextColor(Color.WhiteSmoke);
                        break;
                    case TypeTabHost.Weight:
                        tth = TypeTabHost.HRB;
                        TabHostMain.CurrentTab = 2;
                        TitleHome.SetTextColor(Color.WhiteSmoke);
                        TitleHRB.SetTextColor(Color.Maroon);
                        TitleSport.SetTextColor(Color.WhiteSmoke);
                        TitleWeight.SetTextColor(Color.WhiteSmoke);
                        break;
                    default:
                        break;
                }
                keyCode = Keycode.Unknown;
            }
            return base.OnKeyDown(keyCode, e);
        }

        #region Tab Host & Load Widget H,S,HB,WH

        public enum TypeTabHost
        {
            Home, Sport, HRB, Weight
        }

        public void LoadTabHost()
        {
            TabHostMain = (TabHost)FindViewById(Resource.Id.CBPTabHostMain); TabHostMain.Setup();
            TabW = (TabWidget)FindViewById(Android.Resource.Id.Tabs);
            //Tab Home
            TabHost.TabSpec TabHome = TabHostMain.NewTabSpec("Home");
            TabHome.SetContent(Resource.Id.CBPLinearLayoutTp1);
            View ViewHome = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgHome = ViewHome.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgHome.SetImageResource(Resource.Drawable.CPBHome);
            TitleHome = (TextView)ViewHome.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleHome.Text = "صفحه اصلی"; TitleHome.SetTextColor(Color.Maroon);
            TabHome.SetIndicator(ViewHome);
            TabHostMain.AddTab(TabHome);
            ViewHome.Click += async delegate
            {
                Title = "Control body position";
                tth = TypeTabHost.Home;
                TitleHome.SetTextColor(Color.Maroon);
                TitleHRB.SetTextColor(Color.WhiteSmoke);
                TitleSport.SetTextColor(Color.WhiteSmoke);
                TitleWeight.SetTextColor(Color.WhiteSmoke);
                TabHostMain.CurrentTab = 0;
                vibrator.Vibrate(25);
                imgHome.ImageAlpha = 128;
                await System.Threading.Tasks.Task.Delay(300);
                imgHome.ImageAlpha = 255;
            };

            //Tab Sport
            TabHost.TabSpec TabSport = TabHostMain.NewTabSpec("Sport");
            TabSport.SetContent(Resource.Id.CBPLinearLayoutTp2);
            View ViewSport = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgSport = ViewSport.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgSport.SetImageResource(Resource.Drawable.CPBReport);
            TitleSport = (TextView)ViewSport.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleSport.Text = "ورزش"; TitleSport.SetTextColor(Color.WhiteSmoke);
            TabSport.SetIndicator(ViewSport);
            TabHostMain.AddTab(TabSport);
            ViewSport.Click += async delegate
            {
                Title = "Sport";
                tth = TypeTabHost.Sport;
                TitleHome.SetTextColor(Color.WhiteSmoke);
                TitleHRB.SetTextColor(Color.WhiteSmoke);
                TitleSport.SetTextColor(Color.Maroon);
                TitleWeight.SetTextColor(Color.WhiteSmoke);
                TabHostMain.CurrentTab = 1;
                vibrator.Vibrate(25);
                imgSport.ImageAlpha = 128;
                await System.Threading.Tasks.Task.Delay(300);
                imgSport.ImageAlpha = 255;
            };

            //Tab HRB
            TabHost.TabSpec TabHRB = TabHostMain.NewTabSpec("HRB");
            TabHRB.SetContent(Resource.Id.CBPLinearLayoutTp3);
            View ViewHRB = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgHRB = ViewHRB.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgHRB.SetImageResource(Resource.Drawable.CPBHeartRateBlood);
            TitleHRB = (TextView)ViewHRB.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleHRB.Text = "ضربان قلب"; TitleHRB.SetTextColor(Color.WhiteSmoke);
            TabHRB.SetIndicator(ViewHRB);
            TabHostMain.AddTab(TabHRB);
            ViewHRB.Click += async delegate
            {
                Title = "Heart rate & Blood pressure";
                tth = TypeTabHost.HRB;
                TitleHome.SetTextColor(Color.WhiteSmoke);
                TitleHRB.SetTextColor(Color.Maroon);
                TitleSport.SetTextColor(Color.WhiteSmoke);
                TitleWeight.SetTextColor(Color.WhiteSmoke);
                TabHostMain.CurrentTab = 2;
                vibrator.Vibrate(25);
                imgHRB.ImageAlpha = 128;
                await System.Threading.Tasks.Task.Delay(300);
                imgHRB.ImageAlpha = 255;
            };

            //Tab Weight
            TabHost.TabSpec TabWeight = TabHostMain.NewTabSpec("Weight");
            TabWeight.SetContent(Resource.Id.CBPLinearLayoutTp4);
            View ViewWeight = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgWeight = ViewWeight.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgWeight.SetImageResource(Resource.Drawable.CPBWeight);
            TitleWeight = (TextView)ViewWeight.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleWeight.Text = "وزن و قد"; TitleWeight.SetTextColor(Color.WhiteSmoke);
            TabWeight.SetIndicator(ViewWeight);
            TabHostMain.AddTab(TabWeight);
            ViewWeight.Click += async delegate
            {
                Title = "Weight & Height";
                tth = TypeTabHost.Weight;
                TitleHome.SetTextColor(Color.WhiteSmoke);
                TitleHRB.SetTextColor(Color.WhiteSmoke);
                TitleSport.SetTextColor(Color.WhiteSmoke);
                TitleWeight.SetTextColor(Color.Maroon);
                TabHostMain.CurrentTab = 3;
                vibrator.Vibrate(25);
                imgWeight.ImageAlpha = 128;
                await System.Threading.Tasks.Task.Delay(300);
                imgWeight.ImageAlpha = 255;
            };


        }

        public void LoadHome()
        {
            HomeTxtVVitamin = (TextView)FindViewById(Resource.Id.CBPTextViewVitaem);
            HomeTxtVFood = (TextView)FindViewById(Resource.Id.CBPTextViewFood);
            if (Isp.GetInt(KeyFood, 0) != 0)
            {
                HomeTxtVFood.Text = Isp.GetInt(KeyFood, 0) + " کالری ";
            }
            HomeTxtVWater = (TextView)FindViewById(Resource.Id.CBPTextViewWater);
            if (Isp.GetInt(KeyWater, 0) != 0)
            {
                HomeTxtVWater.Text = Isp.GetInt(KeyWater, 0) + " تعداد لیوان " + '\n' + Isp.GetInt(KeyWater, 0) * 0.2 + " لتیر ";
            }
            HomeTxtVCafe = (TextView)FindViewById(Resource.Id.CBPTextViewCafi);
            if (Isp.GetInt(KeyCafi, 0) != 0)
            {
                HomeTxtVCafe.Text = Isp.GetInt(KeyCafi, 0) + " تعداد فنجان ";
            }
            HomeTxtVSleep = (TextView)FindViewById(Resource.Id.CBPTextViewSleep);
            if (Isp.GetInt(KeyRestShab, 0) != 0 || Isp.GetInt(KeyRestRooz, 0) != 0 || Isp.GetInt(KeyRestTmp, 0) != 0)
            {
                HomeTxtVSleep.Text = "زمان استراحت ثبت شده";
            }

            HomeTxtVVitamin.Click += delegate
            {
                if (CountTVR < 15)
                {
                    HomeTxtVVitamin.Text = Vitam[CountTVR++];
                }
                else
                {
                    CountTVR = 0;
                    HomeTxtVVitamin.Text = Vitam[CountTVR];
                }
            };

            ActionMenuView AmvFood = (ActionMenuView)FindViewById(Resource.Id.CBPActionMenuViewFood);
            AmvFood.Menu.Add("ثبت کالری دریافت شده");
            AmvFood.Menu.Add("حذف اطلاعات");
            AmvFood.MenuItemClick += AmvFood_MenuItemClick;

            ActionMenuView AmvWater = (ActionMenuView)FindViewById(Resource.Id.CBPActionMenuViewWater);
            AmvWater.Menu.Add("افزدون نوشیدن آب");
            AmvWater.Menu.Add("حذف اطلاعات");
            AmvWater.MenuItemClick += AmvWater_MenuItemClick;

            ActionMenuView AmvCafi = (ActionMenuView)FindViewById(Resource.Id.CBPActionMenuViewCafi);
            AmvCafi.Menu.Add("افزدون نوشیدنی چای و قهوه");
            AmvCafi.Menu.Add("حذف اطلاعات");
            AmvCafi.MenuItemClick += AmvCafi_MenuItemClick;

            ActionMenuView AmvSleep = (ActionMenuView)FindViewById(Resource.Id.CBPActionMenuViewSleep);
            AmvSleep.Menu.Add("افزدون زمان استراحت");
            AmvSleep.Menu.Add("نمایش زمان ها");
            AmvSleep.Menu.Add("حذف اطلاعات");
            AmvSleep.MenuItemClick += AmvSleep_MenuItemClick;

            Button BtnCalcHome = (Button)FindViewById(Resource.Id.CBPButtonCalc);
            BtnCalcHome.SetTextColor(Color.WhiteSmoke);
            BtnCalcHome.Background.SetColorFilter(Color.Rgb(180, 40, 40), PorterDuff.Mode.Multiply);
            BtnCalcHome.Click += delegate
            {
                LoadButtonCalculater();
            };

            Button BtnClaerHome = (Button)FindViewById(Resource.Id.CBPButtonClaerData);
            BtnClaerHome.Background.SetColorFilter(Color.Rgb(180, 40, 40), PorterDuff.Mode.Multiply);
            BtnClaerHome.SetTextColor(Color.WhiteSmoke);
            BtnClaerHome.Click += delegate
            {
                ClaerDataISP();
            };


        }

        public void LoadButtonCalculater()
        {
            // Calc
            StringBuilder strb = new StringBuilder(); bool TmpResultView = false; double BEE = 0;
            ISharedPreferences DataProfile = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);

            // Get data profile
            int Age = DataProfile.GetInt("Age", 0);
            int Tall = DataProfile.GetInt("Tall", 0);
            int Weight = DataProfile.GetInt("Weight", 0);
            int Sex = DataProfile.GetInt("Sex", 0);
            string Active = DataProfile.GetString("Active", "");
            // Get data CBP
            int Calery = Isp.GetInt(KeyFood, 0);
            double Water = Isp.GetInt(KeyWater, 0);
            int Cafi = Isp.GetInt(KeyCafi, 0);
            int SleepShab = Isp.GetInt(KeyRestShab, 0);
            int SleepRooz = Isp.GetInt(KeyRestRooz, 0);
            int SleepTmp = Isp.GetInt(KeyRestTmp, 0);

            if (Calery != 0 && Water != 0 && Cafi != 0 && SleepShab != 0 && SleepRooz != 0)
            {
                TmpResultView = true;
            }
            else
            {
                TmpResultView = false;
            }

            switch (Sex)
            {
                case 0:
                    BEE = 66.5 + (((13.75 * (Weight) + (5.003 * Tall)) - (6.775 * Age)));
                    break;
                case 1:
                    BEE = 665.1 + (((9.563 * (Weight) + (1.850 * Tall)) - (4.676 * Age)));
                    break;
                default:
                    break;
            }
            switch (Active)
            {
                case "بی حرکت":
                    BEE *= 1.25;
                    break;
                case "کمی فعال":
                    BEE *= 1.375;
                    break;
                case "متوسط فعال":
                    BEE *= 1.55;
                    break;
                case "فعال":
                    BEE *= 1.725;
                    break;
                case "بسیار فعال":
                    BEE *= 1.9;
                    break;
                default:
                    BEE *= 1;
                    break;
            }
            // 1 )
            strb.AppendLine("* در این قسمت باید برای تمام روز 24 ساعته در نظر گرفته شود");
            strb.AppendLine("");
            strb.AppendLine("قسمت کالری : ");
            strb.AppendLine("- مقدار کالری مصرفی روزانه : " + BEE);
            strb.AppendLine("- مقدار کالری ثبت شده روزانه : " + Calery);
            strb.Append("نتیجه : "); string Pmx = "";
            if (Calery == BEE)
            {
                Pmx = ("مقدار کالری دریافتی مناسب بوده");
            }
            if (BEE < Calery)
            {
                Pmx = ("مقدار کالری بیشتر از حد مجاز بوده");
            }
            if (BEE > Calery)
            {
                Pmx = ("کالری دریافتی کمتر از حد مجاز بوده");
            }
            if (((BEE + 100) >= Calery) && (BEE - 100) <= Calery)
            {
                Pmx = ("مقدار کالری دریافتی مناسب بوده");
            }
            if (((BEE - 100) <= Calery) && BEE > Calery)
            {
                Pmx = ("مقدار کالری کمی کمتر از حد مجاز بوده");
            }
            strb.AppendLine(Pmx);
            strb.AppendLine("");
            // 2 ) type  water : cc
            double CmWater = (double)(Weight * 30); CmWater /= 1000; Water *= 0.2;
            strb.AppendLine("قسمت آب مصرفی : ");
            strb.AppendLine("- کل مقدار آب مورد نیاز : " + (CmWater) + " لیتر ");
            strb.AppendLine("- آب دریافتی بدن : " + Water + " لیتر ");
            strb.Append("نتیجه : "); string Mxp = "";

            if (CmWater == Water)
            {
                Mxp = "میزان آب دریافتی و پایه یکسان است";
            }
            if (CmWater > Water)
            {
                Mxp = "میزان آب دریافتی بدن کم است";
            }
            if (CmWater < Water)
            {
                Mxp = "میزان آب دریافتی بدن کافی است";
            }

            strb.AppendLine(Mxp);
            strb.AppendLine("");
            // 3 )
            strb.AppendLine("قسمت کافئین : ");
            strb.AppendLine("- تعداد فنجان های مصرف نوشیدنی : " + Cafi);
            strb.AppendLine("");
            // 4 )
            strb.AppendLine("قسمت استراحت و خواب : ");
            // age 6 to 13 ==> 9-11
            // age 14 to 17 ==> 8-10
            // age 18 to 25 ==> 7-9
            // age 26 to 64 ==> 7-9
            // age top 65 ==> 7-8 
            double SumSleep = 0; string HourAndMinuteSp = "";
            if (SleepShab / 60 > 12)
            {
                SumSleep = ((1440 - SleepShab) + SleepRooz) + SleepTmp;
            }
            else
            {
                SumSleep = (SleepRooz - SleepShab) + SleepTmp;
            }

            if (SumSleep % 60 < 30)
            {
                HourAndMinuteSp = SumSleep % 60 + " : " + Math.Round(SumSleep / 60, 0);
            }
            else if (SumSleep % 60 >= 30)
            {
                HourAndMinuteSp = SumSleep % 60 + " : " + (Math.Round(SumSleep / 60, 0) - 1);
            }

            int Ayear = Age;
            if (Ayear >= 6 && Ayear <= 13)
            {
                strb.Append("- ");
                strb.AppendLine("- گروه سنی کودک , خواب مورد نیاز بین 9 تا 11 ساعت");
                strb.AppendLine("- ساعات خواب  " + HourAndMinuteSp + " ساعت ");
            }
            else if (Ayear >= 14 && Ayear <= 17)
            {
                strb.Append("- ");
                strb.AppendLine("گروه سنی نوجوان , خواب مورد نیاز بین 8 تا 10 ساعت");
                strb.AppendLine("- ساعات خواب  " + HourAndMinuteSp + " ساعت ");
            }
            else if (Ayear >= 18 && Ayear <= 25)
            {
                strb.Append("- ");
                strb.AppendLine("گروه سنی جوان , خواب مورد نیاز بین 7 تا 9 ساعت");
                strb.AppendLine("- ساعات خواب  " + HourAndMinuteSp + " ساعت ");
            }
            else if (Ayear >= 26 && Ayear <= 64)
            {
                strb.Append("- ");
                strb.AppendLine("گروه سنی بزرگ سال , خواب مورد نیاز بین 7 تا 9 ساعت");
                strb.AppendLine("- ساعات خواب  " + HourAndMinuteSp + " ساعت ");
            }
            else if (Ayear >= 65)
            {
                strb.Append("- ");
                strb.AppendLine("گروه سنی بازنشسته , خواب مورد نیاز بین 7 تا 8 ساعت");
                strb.AppendLine("- ساعات خواب  " + HourAndMinuteSp + " ساعت ");
            }
            strb.AppendLine("");
            // End calc
            if (TmpResultView)
            {
                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetIcon(Resource.Drawable.ReportFWCS);
                Msg.SetTitle("گزارشی از عملکرد روزانه");
                Msg.SetMessage(strb.ToString());
                Msg.SetNeutralButton("لغو", delegate { });
                Msg.SetCancelable(false); Msg.Create(); Msg.Show();
            }
            else
            {
                Toast.MakeText(this, "تمام قسمت های مختلف باید کامل شود", ToastLength.Short).Show();
            }
        }

        public void ClaerDataISP()
        {
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetIcon(Resource.Drawable.DelSS);
            Msg.SetTitle("بهینه سازی");
            Msg.SetMessage("آیا میخواهید همه مقادیر ذخیره شده را پاک کنید");
            Msg.SetPositiveButton("بله", delegate
            {

                EditISP.Clear(); EditISP.Commit();
                Intent intent = new Intent(this, typeof(ControlBodyPosition));
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                StartActivity(intent);
            });
            Msg.SetNegativeButton("خیر", delegate { return; });
            Msg.SetCancelable(false);
            Msg.Create(); Msg.Show();
        }

        private void AmvSleep_MenuItemClick(object sender, ActionMenuView.MenuItemClickEventArgs e)
        {
            switch (e.Item.TitleFormatted.ToString())
            {
                case "افزدون زمان استراحت":

                    View view = View.Inflate(this, Resource.Layout.CPBInputSleepy, null);
                    TimePicker TmPk = (TimePicker)view.FindViewById(Resource.Id.CBPTimePickerSleepy);
                    TmPk.SetIs24HourView(Java.Lang.Boolean.True);
                    AlertDialog.Builder msg = new AlertDialog.Builder(this);

                    msg.SetTitle("زمان خواب و بیداری خود را وارد کنید"); msg.SetView(view); msg.SetIcon(Resource.Drawable.Sleepy);
                    msg.SetPositiveButton("خواب شب", delegate
                    {
                        int hour = TmPk.CurrentHour.IntValue(); int Minute = TmPk.CurrentMinute.IntValue();
                        EditISP.PutInt(KeyRestShab, ((hour * 60) + Minute)); EditISP.Commit();
                        HomeTxtVSleep.Text = " زمان خواب شب " + Minute + " : " + hour;
                    });
                    msg.SetNegativeButton("بیداری صبح", delegate
                    {
                        int hour = TmPk.CurrentHour.IntValue(); int Minute = TmPk.CurrentMinute.IntValue();
                        EditISP.PutInt(KeyRestRooz, ((hour * 60) + Minute)); EditISP.Commit();
                        HomeTxtVSleep.Text = " زمان بیدار شدن " + Minute + " : " + hour;
                    });
                    msg.SetNeutralButton("استراحت روزانه", delegate
                    {
                        int hour = TmPk.CurrentHour.IntValue(); int Minute = TmPk.CurrentMinute.IntValue();
                        EditISP.PutInt(KeyRestTmp, ((hour * 60) + Minute)); EditISP.Commit();
                        HomeTxtVSleep.Text = " زمان استراحت " + ((hour * 60) + Minute) + " دقیقه ";
                    });
                    msg.Create(); msg.Show();
                    break;
                case "حذف اطلاعات":
                    AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                    Msg.SetTitle("حذف اطلاعات زمان استراحت");
                    Msg.SetMessage("آیا میخواهید اطلاعات این بخش پاک شود ؟");
                    Msg.SetPositiveButton("پاک کن", delegate
                    {
                        EditISP.PutInt(KeyRestShab, 0);
                        EditISP.PutInt(KeyRestRooz, 0);
                        EditISP.PutInt(KeyRestTmp, 0); EditISP.Commit();
                        // read & write data
                        HomeTxtVSleep.Text = "میانگین استراحت روزانه";
                    });
                    Msg.SetNegativeButton("لغو", delegate { });
                    Msg.SetCancelable(false); Msg.Create(); Msg.Show();
                    break;
                case "نمایش زمان ها":

                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("نمایش زمان بیدارشدن و خوابیدن");
                    alert.SetNeutralButton("لغو", delegate { });
                    StringBuilder strb = new StringBuilder();
                    double TimeShab = Isp.GetInt(KeyRestShab, 0);
                    double TimeRooz = Isp.GetInt(KeyRestRooz, 0);
                    double TimeRest = Isp.GetInt(KeyRestTmp, 0);

                    strb.Append("زمان خوابیدن : ");

                    if (TimeShab % 60 < 30)
                    {
                        strb.Append((TimeShab % 60) + " : " + (Math.Round(TimeShab / 60, 0)));
                    }
                    else if (TimeShab % 60 >= 30)
                    {
                        strb.Append((TimeShab % 60) + " : " + (Math.Round(TimeShab / 60, 0) - 1));
                    }

                    strb.AppendLine("");
                    strb.Append("زمان بیدار شدن : ");

                    if (TimeRooz % 60 < 30)
                    {
                        strb.Append((TimeRooz % 60) + " : " + (Math.Round(TimeRooz / 60, 0)));
                    }
                    else if (TimeRooz % 60 >= 30)
                    {
                        strb.Append((TimeRooz % 60) + " : " + (Math.Round(TimeRooz / 60, 0) - 1));
                    }

                    strb.AppendLine("");

                    strb.Append("زمان استراحت : ");
                    strb.Append(TimeRest + " دقیقه ");
                    strb.AppendLine("");

                    alert.SetMessage(strb.ToString());
                    alert.SetCancelable(false); alert.Create(); alert.Show();
                    break;
                default:
                    break;
            }

        }

        private void AmvCafi_MenuItemClick(object sender, ActionMenuView.MenuItemClickEventArgs e)
        {
            HomeTxtVCafe.TextAlignment = TextAlignment.Center;
            switch (e.Item.TitleFormatted.ToString())
            {
                case "افزدون نوشیدنی چای و قهوه":

                    View view = View.Inflate(this, Resource.Layout.CPBInputWaterCafi, null);
                    NumberPicker Nmp = (NumberPicker)view.FindViewById(Resource.Id.CPBNumberPickerWCS);
                    Nmp.MaxValue = 20; Nmp.MinValue = 1;

                    AlertDialog.Builder TmMg = new AlertDialog.Builder(this);
                    TmMg.SetTitle("تعداد فنجون ها را وارد کنید");
                    TmMg.SetView(view);
                    TmMg.SetIcon(Resource.Drawable.CaiTea);
                    TmMg.SetPositiveButton("ثبت", delegate
                    {

                        int After = Isp.GetInt(KeyCafi, 0);
                        EditISP.PutInt(KeyCafi, (Nmp.Value) + After);
                        EditISP.Commit();
                        HomeTxtVCafe.Text = Isp.GetInt(KeyCafi, 0) + " تا فنجون " + '\n';

                    });
                    TmMg.SetNegativeButton("لغو", delegate { });
                    TmMg.SetCancelable(false); TmMg.Create(); TmMg.Show();
                    break;
                case "حذف اطلاعات":
                    AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                    Msg.SetTitle("حذف اطلاعات نوشیدنی چای و قهوه");
                    Msg.SetMessage("آیا میخواهید اطلاعات این بخش پاک شود ؟");
                    Msg.SetPositiveButton("پاک کن", delegate
                    {
                        EditISP.PutInt(KeyCafi, 0);
                        EditISP.Commit();
                        // read & write data
                        HomeTxtVCafe.Text = "مقدار مصرف چای و قهوه";
                    });
                    Msg.SetNegativeButton("لغو", delegate { });
                    Msg.SetCancelable(false); Msg.Create(); Msg.Show();
                    break;
                default:
                    break;
            }
        }

        private void AmvWater_MenuItemClick(object sender, ActionMenuView.MenuItemClickEventArgs e)
        {
            switch (e.Item.TitleFormatted.ToString())
            {
                case "افزدون نوشیدن آب":

                    View view = View.Inflate(this, Resource.Layout.CPBInputWaterCafi, null);
                    NumberPicker Nmp = (NumberPicker)view.FindViewById(Resource.Id.CPBNumberPickerWCS);
                    Nmp.MaxValue = 20; Nmp.MinValue = 1;

                    AlertDialog.Builder TmMg = new AlertDialog.Builder(this);
                    TmMg.SetTitle("تعداد لیوان ها را وارد کنید");
                    TmMg.SetView(view);
                    TmMg.SetIcon(Resource.Drawable.Water);
                    TmMg.SetPositiveButton("ثبت", delegate
                    {

                        int After = Isp.GetInt(KeyWater, 0);
                        EditISP.PutInt(KeyWater, (Nmp.Value) + After);
                        EditISP.Commit();
                        HomeTxtVWater.Text = Isp.GetInt(KeyWater, 0) + " تا لیوان " + '\n' + Isp.GetInt(KeyWater, 0) * 0.2 + " لتیر ";

                    });
                    TmMg.SetNegativeButton("لغو", delegate { });
                    TmMg.SetCancelable(false); TmMg.Create(); TmMg.Show();
                    break;
                case "حذف اطلاعات":
                    AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                    Msg.SetTitle("حذف اطلاعات نوشیدن آب");
                    Msg.SetMessage("آیا میخواهید اطلاعات این بخش پاک شود ؟");
                    Msg.SetPositiveButton("پاک کن", delegate
                    {
                        EditISP.PutInt(KeyWater, 0);
                        EditISP.Commit();
                        // read & write data
                        HomeTxtVWater.Text = "افزدون نوشیدن آب";
                    });
                    Msg.SetNegativeButton("لغو", delegate { });
                    Msg.SetCancelable(false); Msg.Create(); Msg.Show();
                    break;
                default:
                    break;
            }

        }

        private void AmvFood_MenuItemClick(object sender, ActionMenuView.MenuItemClickEventArgs e)
        {
            switch (e.Item.TitleFormatted.ToString())
            {
                case "ثبت کالری دریافت شده":
                    //https://irancook.ir/calories/

                    View view = View.Inflate(this, Resource.Layout.CPBInputHome, null);
                    EditText Edtxt = (EditText)view.FindViewById(Resource.Id.CBPEditTextValueFWCS);
                    AlertDialog.Builder TmMg = new AlertDialog.Builder(this);
                    TmMg.SetTitle("اضافه کردن مقدار کالری دریافتی");
                    TmMg.SetView(view);
                    TmMg.SetIcon(Resource.Drawable.Food);
                    TmMg.SetNeutralButton("لیست کالری ها", delegate
                    {
                        Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://irancook.ir/calories/"));
                        browserIntent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(browserIntent);
                    });
                    TmMg.SetPositiveButton("ثبت", delegate
                    {

                        int After = Isp.GetInt(KeyFood, 0);
                        EditISP.PutInt(KeyFood, (int.Parse(Edtxt.Text)) + After);
                        EditISP.Commit();
                        HomeTxtVFood.Text = Isp.GetInt(KeyFood, 0) + " کالری ";

                    });
                    TmMg.SetNegativeButton("لغو", delegate { });
                    TmMg.SetCancelable(false); TmMg.Create(); TmMg.Show();
                    break;
                case "حذف اطلاعات":
                    AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                    Msg.SetTitle("حذف اطلاعات غذا");
                    Msg.SetMessage("آیا میخواهید اطلاعات این بخش پاک شود ؟");
                    Msg.SetPositiveButton("پاک کن", delegate
                    {
                        EditISP.PutInt(KeyFood, 0);
                        EditISP.Commit();
                        // read & write data
                        HomeTxtVFood.Text = "مقدار کالری دریافتی روزانه";
                    });
                    Msg.SetNegativeButton("لغو", delegate { });
                    Msg.SetCancelable(false); Msg.Create(); Msg.Show();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public void LoadSport()
        {
            SportListvmd = (ListView)FindViewById(Resource.Id.SportListViewMain);
            LoadData("Sport", SportListvmd);
            SportListvmd.ItemClick += SportListvmd_ItemClick;
        }

        private void SportListvmd_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ClickListViewDataBase(e.View, "Sport", SportListvmd);
        }

        public void LoadHRB()
        {
            HRListvmd = (ListView)FindViewById(Resource.Id.HRBListViewData);
            LoadData("HR", HRListvmd);
            HRListvmd.ItemClick += HRListvmd_ItemClick;
        }

        private void HRListvmd_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ClickListViewDataBase(e.View, "HR", HRListvmd);
        }

        public void LoadWeightHeight()
        {
            WHListvmd = (ListView)FindViewById(Resource.Id.WHListViewData);
            LoadData("WH", WHListvmd);
            WHListvmd.ItemClick += WHListvmd_ItemClick;
        }

        private void WHListvmd_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ClickListViewDataBase(e.View, "WH", WHListvmd);
        }

        public void SetData(string keycode, ListView lv, string title, string text)
        {
            ISharedPreferences[] SWCSIsp = new ISharedPreferences[50];
            for (int i = 0; i < 50; i++)
            {
                SWCSIsp[i] = GetSharedPreferences(keycode + i, FileCreationMode.WorldReadable);
                ISharedPreferencesEditor SIspEdit = SWCSIsp[i].Edit();
                if (SWCSIsp[i].GetString("KeyTitle", "") == "" && SWCSIsp[i].GetString("KeyText", "") == "")
                {
                    SIspEdit.PutString("KeyTitle", title);
                    SIspEdit.PutString("KeyText", text);
                    SIspEdit.Commit();
                    Toast.MakeText(this, (i + 1) + " اطلاعات ثبت شد ", ToastLength.Short).Show();
                    LoadData(keycode, lv);
                    break;
                }
                else
                {
                    if (i == 49)
                        Toast.MakeText(this, "حافظه پایگاه داده پر است", ToastLength.Short).Show();

                }
            }
        }

        public void LoadData(string keycode, ListView lv)
        {
            ISharedPreferences[] SWCSIsp = new ISharedPreferences[50];
            List<TypeDataListCus> list = new List<TypeDataListCus>();

            for (int i = 0; i < 50; i++)
            {
                SWCSIsp[i] = GetSharedPreferences(keycode + i, FileCreationMode.WorldReadable);
                if (SWCSIsp[i].GetString("KeyTitle", "") != "" && SWCSIsp[i].GetString("KeyText", "") != "")
                {
                    string Title = SWCSIsp[i].GetString("KeyTitle", "");
                    string Text = SWCSIsp[i].GetString("KeyText", "");
                    list.Add(new TypeDataListCus() { Title = Title, Text = Text });
                }
                else
                {
                    break;
                }
            }
            lv.Adapter = new CustomList(this, list);

        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

    }

    public class CustomList : BaseAdapter
    {
        Activity context; List<TypeDataListCus> list;

        public CustomList(Activity mc, List<TypeDataListCus> listdata)
        {
            context = mc;
            list = listdata;
        }

        public override int Count
        {
            get
            {
                return list.Count();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var cd = list[position];
            var cv = context.LayoutInflater.Inflate(Resource.Layout.LayoutCBPLSV, parent, false);

            TextView TxtTitle = cv.FindViewById<TextView>(Resource.Id.LvCusTextViewTitle);
            TxtTitle.Text = cd.Title;

            TextView TxtText = cv.FindViewById<TextView>(Resource.Id.LvCusTextViewSummery);
            TxtText.Text = cd.Text;

            return cv;

        }

    }

    public class TypeDataListCus
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }

}





//Uri uri = Uri.parse("android.resource://com.xxx.appname/raw/pdfId");
//var uri = Android.Net.Uri.Parse(("file:///android_asset/ListFood.pdf"));


//   var uri = Android.Net.Uri.Parse("android.resurce://" + PackageName + "/raw/ListFood.pdf");
//   Intent i = new Intent(Intent.ActionSend);
//   i.SetType("*/*");
//   share image to apps [imgae gets]
//   Java.IO.File file = new Java.IO.File("android.resurce://" + PackageName + "/" + Resource.Raw.ListFood);
//   Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + file.AbsolutePath);
//   i.PutExtra(Intent.ExtraStream, uri);
//   i.SetFlags(ActivityFlags.NewTask);
//   start share to apps
//   StartActivity(Intent.CreateChooser(i, "Pdf ..."));

//   WebView webview = new WebView(this);
//   SetContentView(webview);
//   webview.Settings.JavaScriptEnabled = true;
//   webview.LoadUrl("https://irancook.ir/calories/");

//   Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("file://"+ "/res/raw/ListFood.pdf"));
//   browserIntent.SetFlags(ActivityFlags.NewTask);
//   StartActivity(browserIntent);