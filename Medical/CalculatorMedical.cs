using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using UK.CO.Chrisjenx.Calligraphy;
using Android.Preferences;
using Android.Media;

namespace Medical
{
    [Activity(Label = "Calculator Medical" , ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CalculatorMedical : Activity,IDialogInterfaceOnClickListener 
    {
        // Custom tab host 
        TextView TitleHome, TitleDep, TitleSD;TabHost TabHostMain;TabWidget TabW;TypeTabHost tth = TypeTabHost.Home;
        // Widget Home 
        EditText Age, Tall, Weight;Spinner TypeSex, TypeActive;Button BtnCalculator;TextView Result; double ValueActive;int Sex;
        // Widget Dep 
        ListView ListTestDep;Button BtnReload, BtnTest;
        bool[] IBimg = new bool[12];int[] PnTest = new int[12];int PositionListVDep = 0;
        // Widget Diebats
        EditText DbWeight, DbTall, DbAge, DbWCD;Spinner DbTySex;Button DbBtnClaer, DbBtnCalc;
        Switch DbSwitch1, DbSwitch2, DbSwitch3, DbSwitch4, DbSwitch5;int SumDb = 0;int dbSex = -1;

        public enum TypeTabHost
        {
            Home,Dep,Sd
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.ThemeaMain);
            SetContentView(Resource.Layout.CalculatorMedical);
            TabHostRunTimeCustom();
            RunWidgetHome();
            RunWidgetDepress();
            RunWidgetDiabet();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 123, 0, "کمک").SetIcon(Resource.Drawable.HelpRA).SetShowAsAction(ShowAsAction.Always);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 123:
                    LoadMenuHelp();
                    break;
                default:
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        public void LoadMenuHelp()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendLine("این قسمت برنامه از سه قسمت تشکیل شده");
            strb.AppendLine("- محاسبه شاخص ها - تست درصد افسردگی - تست دیابت نوع دوم");
            strb.AppendLine("");
            strb.AppendLine("توضیح محاسبه شاخص ها");
            strb.AppendLine("در این قسمت از شاخص های مهم برای بدن مثل : حجم بدن و سطح بدن و وزن ایده آل و دریافت کاری روزانه پایه و کالری مصرفی روزانه با کمک فعالیت روزانه که این قسمت از وضعیت بدن خبر می دهد");
            strb.AppendLine("");
            strb.AppendLine("توضیح تست افسردگی");
            strb.AppendLine("در بخش از شما سوالاتی پرسیده میشوید که با جواب دادن درصد احتمالی شما به افسردگی مشخص می شود");
            strb.AppendLine("نکته 1 : این نشانه ها براساس این است که بیش از دو هفته تکرار داشته باشد.");
            strb.AppendLine("نکته 2 : از مهم ترین نشانه اول افسردگی ( تغییر خلق و خوی و علاقه و خستگی ) است");
            strb.Append("نکته 3 : ");
            strb.AppendLine("راه های تشخیص معیار اصلی افسردگی");
            strb.AppendLine("تغییر حالت");
            strb.AppendLine("تغییر علاقه");
            strb.AppendLine("خستگی");
            strb.AppendLine("احساس گناه");
            strb.AppendLine("خودکشی");
            strb.AppendLine("عدم تمرکز");
            strb.AppendLine("تغییرات روانی");
            strb.AppendLine("بهم خوردن عادت خواب");
            strb.AppendLine("تغییر وزن و اشتها");
            strb.AppendLine("");
            strb.AppendLine("توضیح بخش تست دیابت");
            strb.AppendLine("این بخش هم با وارد کردن اطلاعات فردی و تاریخچه والدین در خصوص دیابت و سوالات مخصوص به این بخش درصد ابتلا شدن به دیابت نوع دوم را مشخص میکند");
            AlertDialog.Builder Msg = new AlertDialog.Builder(this);
            Msg.SetTitle("راهنما");
            Msg.SetMessage(strb.ToString());
            Msg.SetNeutralButton("لغو", delegate { });
            Msg.SetCancelable(false);Msg.Create();Msg.Show();
        }

        public void RunWidgetHome()
        {
            Age = (EditText)FindViewById(Resource.Id.CalculatorMedicalHEditTextAge);
            Tall = (EditText)FindViewById(Resource.Id.CalculatorMedicalHEditTextTall);
            Weight = (EditText)FindViewById(Resource.Id.CalculatorMedicalHEditTextWeight);
            Result = (TextView)FindViewById(Resource.Id.CalculatorMedicalHTextViewResult);

            TypeSex = (Spinner)FindViewById(Resource.Id.CalculatorMedicalHSpinnerSex);
            ArrayAdapter AdpSex = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, new string[] { "--","مرد","زن" });
            TypeSex.Adapter = AdpSex;
            TypeSex.ItemSelected += TypeSex_ItemSelected;

            TypeActive = (Spinner)FindViewById(Resource.Id.CalculatorMedicalHSpinnerActive);
            ArrayAdapter<string> AdpActive = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, 
                new string[] { "--", "بی حرکت","کمی فعال","فعال","متوسط فعال","بسیار فعال" });
            TypeActive.Adapter = AdpActive;
            TypeActive.ItemSelected += TypeActive_ItemSelected;

            BtnCalculator = (Button)FindViewById(Resource.Id.CalculatorMedicalHBtnCalc);
            BtnCalculator.Background.SetColorFilter(Color.Rgb(160, 0, 0), PorterDuff.Mode.Multiply);
            BtnCalculator.SetTextColor(Color.WhiteSmoke);
            BtnCalculator.Click += BtnCalculator_Click;
            GetDataPf();        
            
        }

        public void GetDataPf()
        {
            ISharedPreferences Isp = GetSharedPreferences("Profile", FileCreationMode.WorldReadable);

            Age.Text = Isp.GetInt("Age", 0) != 0 ? Isp.GetInt("Age", 0).ToString() : "";
            Tall.Text = Isp.GetInt("Tall", 0) != 0 ? Isp.GetInt("Tall", 0).ToString() : "";
            Weight.Text = Isp.GetInt("Weight", 0) != 0 ? Isp.GetInt("Weight", 0).ToString() : "";
            //Sp Sex
            int TfSex = Isp.GetInt("Sex", -1);
            string[] ItemSex = new string[3];
            switch (TfSex)
            {
                case -1:
                    ItemSex = new string[] { "--", "مرد", "زن" };
                    break;
                case 0:
                    ItemSex = new string[] { "مرد", "زن" };
                    break;
                case 1:
                    ItemSex = new string[] { "زن", "مرد" };
                    break;
                default:
                    break;
            }
            ArrayAdapter<string> AdapterSex = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ItemSex);
            TypeSex.Adapter = AdapterSex;
            //Sp Active
            string TfActive = Isp.GetString("Active", "0");
            string[] ItemActive = new string[6];
            switch (TfActive)
            {
                case "1.25":
                    ItemActive = new string[] { "بی حرکت", "کمی فعال", "متوسط فعال", "فعال", "بسیار فعال" };
                    break;
                case "1.375":
                    ItemActive = new string[] { "کمی فعال", "بی حرکت", "متوسط فعال", "فعال", "بسیار فعال" };
                    break;
                case "1.55":
                    ItemActive = new string[] { "متوسط فعال", "بی حرکت", "کمی فعال", "فعال", "بسیار فعال" };
                    break;
                case "1.725":
                    ItemActive = new string[] { "فعال", "بی حرکت", "کمی فعال", "متوسط فعال", "بسیار فعال" };
                    break;
                case "1.9":
                    ItemActive = new string[] { "بسیار فعال", "بی حرکت", "کمی فعال", "متوسط فعال", "فعال" };
                    break;
                default:
                    ItemActive = new string[] { "--", "بی حرکت", "کمی فعال", "متوسط فعال", "فعال", "بسیار فعال" };
                    break;
            }

            ArrayAdapter<string> AdapterActive = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ItemActive);
            TypeActive.Adapter = AdapterActive;

        }

        public void RunWidgetDepress()
        {
            ListTestDep = (ListView)FindViewById(Resource.Id.CMDepListViewMain);
            BtnReload = (Button)FindViewById(Resource.Id.CMDepButtonAgainTest);
            BtnTest = (Button)FindViewById(Resource.Id.CMDepButtonResult);
            BtnReload.Background.SetColorFilter(Color.Rgb(200, 0, 0), PorterDuff.Mode.Multiply);
            BtnTest.Background.SetColorFilter(Color.Rgb(200, 0, 0), PorterDuff.Mode.Multiply);
            BtnReload.SetTextColor(Color.WhiteSmoke);
            BtnTest.SetTextColor(Color.WhiteSmoke);
            BtnReload.Click += BtnReload_Click;
            BtnTest.Click += BtnTest_Click;

            List<TypeItemList> list = new List<TypeItemList>();

            int Icon = Resource.Drawable.CmNo;
            list.Add(new TypeItemList() { Title = "ناراحتی", Text = "آیا احساس ناراحتی و غمگینی دارید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "علاقه", Text = "آیا شما علاقه به فعالیت های روزانه خود را از دست داده اید ؟",Icon = Icon });
            list.Add(new TypeItemList() { Title = "انرژی", Text = "آیا شما احساس  فاقد انرژی و قدرت دارید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "اعتماد به نفس", Text = "آیا شما کمتر احساس اعتماد به نفس دارید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "عذاب وجدان", Text = "آیا شما عذاب وجدان و یا احساس گناه دارید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "زندگی", Text = "آیا شما احساس می کنید زندگی ارزش زیستن را ندارد ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "تمرکز", Text = "آیا شما مشکل در تمرکز کردن دارید ؟ (هنگام خواندن روزنامه یا تماشا تلویویزن )", Icon = Icon });
            list.Add(new TypeItemList() { Title = "بیقراری", Text = "آیا شما احساس بیقراری دارید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "تسخیر بودن", Text = "آیا شما احساس در تسخیر بودن ' بسته بودن یا بدون تغییر و یکنواخت ' دارید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "خواب", Text = "مشکل در خواب شب داشته اید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "اشتها", Text = "آیا شما از کاهش اشتها رنج می برید ؟", Icon = Icon });
            list.Add(new TypeItemList() { Title = "اشتها", Text = "آیا شما از افزایش اشتها رنج می برید ؟", Icon = Icon });

            ListTestDep.Adapter = new ItemAddList(this, list);
            ListTestDep.ItemClick += ListTestDep_ItemClick;

        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            int Count = 0;
            for (int i = 0; i < 12; i++)
            {
                if (IBimg[i] == true)
                {
                    Count++;
                }
            }
            if (Count == 12)
            {
                int A = (int)System.Math.Max(PnTest[3], PnTest[4]);
                int B = (int)System.Math.Max(PnTest[7], PnTest[8]);
                int C = (int)System.Math.Max(PnTest[10], PnTest[11]);
                int SumDep = A + B + C + (PnTest[0] + PnTest[1] + PnTest[2] + PnTest[5] + PnTest[6] + PnTest[9]);

                for (int i = 0; i < 12; i++)
                {
                    IBimg[i] = false;
                    PnTest[i] = 0;
                }
                RetrunLVDep();
                System.Text.StringBuilder StrB = new System.Text.StringBuilder();
                if (SumDep < 20)
                {
                    StrB.AppendLine("امتیاز شما از این تست : " + " % " + SumDep * 2.22);
                    StrB.AppendLine("وضعیت روحی : سالم");
                }
                else if (20 <= SumDep && SumDep <= 24)
                {
                    StrB.AppendLine("امتیاز شما از این تست : " + " % " + SumDep * 2.22);
                    StrB.AppendLine("وضعیت روحی : افسردگی ضعیف");
                }
                else if (25 <= SumDep && SumDep <= 29)
                {
                    StrB.AppendLine("امتیاز شما از این تست : " + " % " + SumDep * 2.22);
                    StrB.AppendLine("وضعیت روحی : افسردگی متوسط");
                }
                else if (30 <= SumDep)
                {
                    StrB.AppendLine("امتیاز شما از این تست : " + " % " + SumDep * 2.22);
                    StrB.AppendLine("وضعیت روحی : افسردگی شدید");
                }

                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetTitle("نتیجه تست افسردگی");
                Msg.SetMessage(StrB.ToString());
                Msg.SetNeutralButton("لغو", delegate { });
                Msg.SetCancelable(false);
                Msg.Create();Msg.Show();
            }
            else
            {
                Toast.MakeText(this, "لطفا به همه سوالات پاسخ دهید", ToastLength.Short).Show();
            }
 
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder Mg = new AlertDialog.Builder(this);
            Mg.SetTitle("تست دوباره");
            Mg.SetMessage("آیا میخواهید تست قبلی را حذف کنید ؟");
            Mg.SetPositiveButton("بله", delegate {
                for (int i = 0; i < 12; i++)
                {
                    IBimg[i] = false;
                    PnTest[i] = 0;
                }
                RetrunLVDep();
            });
            Mg.SetNegativeButton("خیر", delegate { });
            Mg.Create();Mg.Show();
        }

        public void RetrunLVDep()
        {
            List<TypeItemList> list = new List<TypeItemList>();
            int icon = Resource.Drawable.CmNo;
            list.Add(new TypeItemList() { Title = "ناراحتی", Text = "آیا احساس ناراحتی و غمگینی دارید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "علاقه", Text = "آیا شما علاقه به فعالیت های روزانه خود را از دست داده اید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "انرژی", Text = "آیا شما احساس  فاقد انرژی و قدرت دارید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "اعتماد به نفس", Text = "آیا شما کمتر احساس اعتماد به نفس دارید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "عذاب وجدان", Text = "آیا شما عذاب وجدان و یا احساس گناه دارید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "زندگی", Text = "آیا شما احساس می کنید زندگی ارزش زیستن را ندارد ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "تمرکز", Text = "آیا شما مشکل در تمرکز کردن دارید ؟ (هنگام خواندن روزنامه یا تماشا تلویویزن )", Icon = icon });
            list.Add(new TypeItemList() { Title = "بیقراری", Text = "آیا شما احساس بیقراری دارید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "تسخیر بودن", Text = "آیا شما احساس در تسخیر بودن ' بسته بودن یا بدون تغییر و یکنواخت ' دارید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "خواب", Text = "مشکل در خواب شب داشته اید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "اشتها", Text = "آیا شما از کاهش اشتها رنج می برید ؟", Icon = icon });
            list.Add(new TypeItemList() { Title = "اشتها", Text = "آیا شما از افزایش اشتها رنج می برید ؟", Icon = icon });
            ListTestDep.Adapter = new ItemAddList(this, list);
        }

        public void SetCheckImgDep(int position)
        {
            IBimg[position] = true;
            List<TypeItemList> list = new List<TypeItemList>();
            int icon = Resource.Drawable.CmNo;

            if (IBimg[0] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "ناراحتی", Text = "آیا احساس ناراحتی و غمگینی دارید ؟", Icon = icon });
            if (IBimg[1] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "علاقه", Text = "آیا شما علاقه به فعالیت های روزانه خود را از دست داده اید ؟", Icon = icon });
            if (IBimg[2] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "انرژی", Text = "آیا شما احساس  فاقد انرژی و قدرت دارید ؟", Icon = icon });
            if (IBimg[3] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "اعتماد به نفس", Text = "آیا شما کمتر احساس اعتماد به نفس دارید ؟", Icon = icon });
            if (IBimg[4] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "عذاب وجدان", Text = "آیا شما عذاب وجدان و یا احساس گناه دارید ؟", Icon = icon });
            if (IBimg[5] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "زندگی", Text = "آیا شما احساس می کنید زندگی ارزش زیستن را ندارد ؟", Icon = icon });
            if (IBimg[6] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "تمرکز", Text = "آیا شما مشکل در تمرکز کردن دارید ؟ (هنگام خواندن روزنامه یا تماشا تلویویزن )", Icon = icon });
            if (IBimg[7] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "بیقراری", Text = "آیا شما احساس بیقراری دارید ؟", Icon = icon });
            if (IBimg[8] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "تسخیر بودن", Text = "آیا شما احساس در تسخیر بودن ' بسته بودن یا بدون تغییر و یکنواخت ' دارید ؟", Icon = icon });
            if (IBimg[9] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "خواب", Text = "مشکل در خواب شب داشته اید ؟", Icon = icon });
            if (IBimg[10] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "اشتها", Text = "آیا شما از کاهش اشتها رنج می برید ؟", Icon = icon });
            if (IBimg[11] == true) { icon = Resource.Drawable.CmYes; } else { icon = Resource.Drawable.CmNo; }
            list.Add(new TypeItemList() { Title = "اشتها", Text = "آیا شما از افزایش اشتها رنج می برید ؟", Icon = icon });
            ListTestDep.Adapter = new ItemAddList(this, list);
        }

        private void ListTestDep_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView TitleList = (TextView)e.View.FindViewById(Resource.Id.LvDepTextViewText);
            if (IBimg[e.Position] == false)
            {
                PositionListVDep = e.Position;
                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetTitle(" ' " + TitleList.Text + " ' ");
                Msg.SetItems(new string[] { "درهیچ زمان","برخی از زمان","کمی کمتر از نیمی از زمان",
                    "کمی بیش از از نیمی از زمان","بسیاری از اوقات","همیشه" }, this);
                Msg.SetCancelable(true);
                Msg.SetNeutralButton("لغو", delegate { });
                Msg.Create();Msg.Show();
            }
            else
            {
                Toast.MakeText(this, "این مورد را قبلا پاسخ دادید", ToastLength.Short).Show();
            }
           
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            PnTest[PositionListVDep] = which;
            SetCheckImgDep(PositionListVDep);
        }

        public void RunWidgetDiabet()
        {
            DbWeight = (EditText)FindViewById(Resource.Id.CMDiabetEditTextWeight);
            DbTall = (EditText)FindViewById(Resource.Id.CMDiabetEditTextTall);
            DbAge = (EditText)FindViewById(Resource.Id.CMDiabetEditTextAge);
            DbWCD = (EditText)FindViewById(Resource.Id.CMDiabetEditTextWCD);
            DbTySex = (Spinner)FindViewById(Resource.Id.CMDiabetSpinnerSex);
            DbTySex.ItemSelected += DbTySex_ItemSelected;
            ArrayAdapter ItemAdp = new ArrayAdapter(this, Android.Resource.
                Layout.SimpleSpinnerDropDownItem, new string[] { "--", "مرد", "زن" });
            DbTySex.Adapter = ItemAdp;
            DbSwitch1 = (Switch)FindViewById(Resource.Id.CMDiabetSwitchParnet);
            DbSwitch2 = (Switch)FindViewById(Resource.Id.CMDiabetSwitchHisBd);
            DbSwitch3 = (Switch)FindViewById(Resource.Id.CMDiabetSwitchDaPb);
            DbSwitch4 = (Switch)FindViewById(Resource.Id.CMDiabetSwitchSomk);
            DbSwitch5 = (Switch)FindViewById(Resource.Id.CMDiabetSwitchActive);

            DbBtnCalc = (Button)FindViewById(Resource.Id.CMDiabetButtonCalc);
            DbBtnCalc.Text = "محاسبه درصد دیابت";
            DbBtnClaer = (Button)FindViewById(Resource.Id.CMDiabetButtonClaer);
            DbBtnClaer.Text = "اصلاح مقادیر";

            DbBtnCalc.Background.SetColorFilter(Color.Rgb(200, 0, 0), PorterDuff.Mode.Multiply);
            DbBtnCalc.SetTextColor(Color.WhiteSmoke);

            DbBtnClaer.Background.SetColorFilter(Color.Rgb(200, 0, 0), PorterDuff.Mode.Multiply);
            DbBtnClaer.SetTextColor(Color.WhiteSmoke);

            DbBtnCalc.Click += DbBtnCalc_Click;
            DbBtnClaer.Click += DbBtnClaer_Click;
        }

        private void DbTySex_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            dbSex = e.Position - 1;
        }

        private void DbBtnClaer_Click(object sender, EventArgs e)
        {
            DbWeight.Text = "";DbTall.Text = "";
            DbAge.Text = "";DbWCD.Text = "";
            ArrayAdapter ItemAdp = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new string[] { "--", "مرد", "زن" });
            DbTySex.Adapter = ItemAdp;
            DbSwitch1.Checked = false;
            DbSwitch2.Checked = false;
            DbSwitch3.Checked = false;
            DbSwitch4.Checked = false;
            DbSwitch5.Checked = false;
        }

        private void DbBtnCalc_Click(object sender, EventArgs e)
        {
            if (DbAge.Text != "" && DbTall.Text != "" && DbWeight.Text != ""
                && DbWCD.Text != "" && DbTySex.SelectedItem.ToString() != "")
            {
                int age = int.Parse(DbAge.Text);
                int tall = int.Parse(DbTall.Text);
                int weight = int.Parse(DbWeight.Text);
                int wcd = int.Parse(DbWCD.Text);

                if (age >= 25 && age <= 34)
                    SumDb += 0;
                else if (age >= 35 && age <= 44)
                    SumDb += 2;
                else if (age >= 45 && age <= 54)
                    SumDb += 4;
                else if (age >= 55 && age <= 64)
                    SumDb += 6;
                else if (age >= 65)
                    SumDb += 8;

                SumDb += dbSex == 0 ? 3 : 0;
                SumDb += DbSwitch1.Checked ? 3 : 0;
                SumDb += DbSwitch2.Checked ? 6 : 0;
                SumDb += DbSwitch3.Checked ? 2 : 0;
                SumDb += DbSwitch4.Checked ? 2 : 0;
                SumDb += DbSwitch5.Checked ? 2 : 0;

                double bmi = Math.Round((((double)weight / (tall * tall))), 3);

                if (bmi < 25)
                    SumDb += 0;
                else if (bmi >= 25 && bmi <= 29.99)
                    SumDb += 3;
                else if (bmi >= 30 && bmi <= 34.99)
                    SumDb += 6;
                else if (bmi >= 35)
                    SumDb += 8;

                switch (dbSex)
                {
                    case 0:
                        if (wcd <= 90)
                            SumDb += 0;
                        else if (wcd >= 90 && wcd <= 99)
                            SumDb += 4;
                        else if (wcd >= 100)
                            SumDb += 7;
                        break;
                    case 1:
                        if (wcd <= 80)
                            SumDb += 0;
                        else if (wcd >= 80 && wcd <= 89)
                            SumDb += 4;
                        else if (wcd >= 90)
                            SumDb += 7;
                        break;
                    default:
                        break;
                }

                StringBuilder strb = new StringBuilder();
  
                if (SumDb <= 5)
                {
                    strb.AppendLine("درصد ابتلا به دیابت : " + "%" + " 1" );
                    strb.AppendLine("تناسب ابتلا به دیابت : " + "%" + " 100 : 1");
                }
                else if (SumDb >= 6 && SumDb <= 8)
                {
                    strb.AppendLine("درصد ابتلا به دیابت : " + "%" + " 2" );
                    strb.AppendLine("تناسب ابتلا به دیابت : " + "%" + " 50 : 1");
                }
                else if (SumDb >= 9 && SumDb <= 11)
                {
                    strb.AppendLine("درصد ابتلا به دیابت : " + "%" + " 3.33" );
                    strb.AppendLine("تناسب ابتلا به دیابت : " + "%" + " 30 : 1");
                }
                else if (SumDb >= 12 && SumDb <= 15)
                {
                    strb.AppendLine("درصد ابتلا به دیابت : " + "%" + " 7.15" );
                    strb.AppendLine("تناسب ابتلا به دیابت : " + "%" + " 14 : 1" );
                }
                else if (SumDb >= 16 && SumDb <= 19)
                {
                    strb.AppendLine("درصد ابتلا به دیابت : " + "%" + " 14.28");
                    strb.AppendLine("تناسب ابتلا به دیابت : " + "%" + " 7 : 1" );
                }
                else if (SumDb >= 20)
                {
                    strb.AppendLine("درصد ابتلا به دیابت : " + "%" + " 33.33" );
                    strb.AppendLine("تناسب ابتلا به دیابت : " + "%" + " 3 : 1");
                }

                AlertDialog.Builder Msg = new AlertDialog.Builder(this);
                Msg.SetTitle("تست دیابت نوع دوم");
                Msg.SetMessage(strb.ToString());
                Msg.SetNeutralButton("لغو", delegate { });
                Msg.SetCancelable(false); Msg.Create(); Msg.Show(); SumDb = 0;

            }
            else
            {
                Toast.MakeText(this, "لطفا همه مقادیرها را تکمیل کنید", ToastLength.Short).Show();
            }


        }

        private void TypeActive_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch (e.Parent.SelectedItem.ToString())
            {
                case "بی حرکت":
                    ValueActive = 1.25;
                    break;
                case "کمی فعال":
                    ValueActive = 1.375;
                    break;
                case "متوسط فعال":
                    ValueActive = 1.55;
                    break;
                case "فعال":
                    ValueActive = 1.725;
                    break;
                case "بسیار فعال":
                    ValueActive = 1.9;
                    break;
                default:
                    ValueActive = 1;
                    break;
            }
        }

        private void TypeSex_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch (e.Parent.SelectedItem.ToString())
            {
                case "مرد":
                    Sex = 0;
                    break;
                case "زن":
                    Sex = 1;
                    break;
                default:
                    Sex = -1;
                    break;
            }
        }

        private void BtnCalculator_Click(object sender, EventArgs e)
        {
            if (Age.Text != "" && Tall.Text != "" && Weight.Text != "" && ValueActive != 0 && Sex != -1)
            {
                BMI(); BSA(); IBW(); BMR(); BEE();WaterBody();SleepBody();
            }
        }

        #region Calculator BMI,BSA,IBW,BMR,BEE,Sleep,Water

        public void BMI()
        {
            int Wkg = int.Parse(Weight.Text);
            double Tm = double.Parse(Tall.Text) / 100; // Value (cm) / 100 = Value (m)
            double Bmi = Wkg / (System.Math.Pow(Tm, 2));

            Result.Text = "شاخص حجم بدن (BMI) : " + (System.Math.Round(Bmi, 3)) + " kg/m2"; // BMI (Kg/m2)
            Result.Text += " = ";
            if (Bmi < 18.5)
            {
                Result.Text += "کمبود وزن";
            }
            else if (Bmi >= 18.5 && Bmi <= 24.99)
            {
                Result.Text += "وزن طبیعی";
            }
            else if (Bmi >= 25 && Bmi <= 29.99)
            {
                Result.Text += "اضافه وزن";
            }
            else if (Bmi >= 30)
            {
                Result.Text += "چاق هستید";
            }
            Result.Text += "\n"; 
        }

        public void BSA()
        {
            int Hcm = int.Parse(Tall.Text);
            int Wkg = int.Parse(Weight.Text);
            double Bsi = System.Math.Sqrt(((Hcm * Wkg) / 3600));
            Result.Text += "سطح بدن (BSI) : " + System.Math.Round(Bsi, 3) + " m2";
            Result.Text += System.Environment.NewLine;
        }

        public void IBW()
        {
            double Tin = (double.Parse(Tall.Text) / 2.5);
            double ConstMI = 0;
            switch (Sex)
            {
                case 0:
                    ConstMI = 50;
                    break;
                case 1:
                    ConstMI = 45.5;
                    break;
                default:
                    break;
            }
            double IBW = ConstMI + (2.3 * (Tin - 60));
            Result.Text += "وزن ایده آل (IBW) : " + System.Math.Round(IBW, 3) + " kg";
            Result.Text += System.Environment.NewLine;
        } 

        public void BMR()
        {
            double Wkg = double.Parse(Weight.Text);
            double Tcm = double.Parse(Tall.Text);
            int Ayear = int.Parse(Age.Text);
            double BMR = 0;
            switch (Sex)
            {
                case 0:
                    BMR = 66 + (((13.7 * (Wkg) + (5 * Tcm)) - (6.8 * Ayear)));
                    break;
                case 1:
                    BMR = 665 + (((9.6 * (Wkg) + (1.8 * Tcm)) - (4.7 * Ayear)));
                    break;
                default:
                    break;
            }
            Result.Text += "دریافت کالری روزانه پایه (BMR) : " + System.Math.Round(BMR, 3) + " kj";
            Result.Text += System.Environment.NewLine;
        }

        public void BEE()
        {
            double Wkg = double.Parse(Weight.Text);
            double Tcm = double.Parse(Tall.Text);
            int Ayear = int.Parse(Age.Text);
            double BEE = 0;double TotalBEE = 0;
            switch (Sex)
            {
                case 0:
                    BEE = 66.5 + (((13.75 * (Wkg) + (5.003 * Tcm)) - (6.775 * Ayear)));
                    break;
                case 1:
                    BEE = 665.1 + (((9.563 * (Wkg) + (1.850 * Tcm)) - (4.676 * Ayear)));
                    break;
                default:
                    break;
            }
            TotalBEE = BEE * ValueActive;
            Result.Text += "کالری مصرفی روزانه (BEE) : " + System.Math.Round(TotalBEE, 3) + " kj/day";
            Result.Text += System.Environment.NewLine;
        }

        public void WaterBody()
        {
            double Wkg = double.Parse(Weight.Text);
            Result.Text += "مقدار آب مورد نیاز بدن : " + Math.Round((Wkg * 30) / 1000, 2) + " لیتر ";
            Result.Text += System.Environment.NewLine;
        }

        public void SleepBody()
        {
            int Ayear = int.Parse(Age.Text);
            if (Ayear >= 6 && Ayear <= 13)
            {
                Result.Text +="گروه سنی کودک - خواب مورد نیاز بین 9 تا 11 ساعت";
                Result.Text += System.Environment.NewLine;
            }
            else if (Ayear >= 14 && Ayear <= 17)
            {
                Result.Text += "گروه سنی نوجوان - خواب مورد نیاز بین 8 تا 10 ساعت"; 
                Result.Text += System.Environment.NewLine;
            }
            else if (Ayear >= 18 && Ayear <= 25)
            {
                Result.Text += "گروه سنی جوان - خواب مورد نیاز بین 7 تا 9 ساعت";
                Result.Text += System.Environment.NewLine;
            }
            else if (Ayear >= 26 && Ayear <= 64)
            {
                Result.Text += "گروه سنی بزرگ سال - خواب مورد نیاز بین 7 تا 9 ساعت";
                Result.Text += System.Environment.NewLine;
            }
            else if (Ayear >= 65)
            {
                Result.Text += "گروه سنی بازنشسته - خواب مورد نیاز بین 7 تا 8 ساعت";
                Result.Text += System.Environment.NewLine;
            }

        }

        #endregion

        public void TabHostRunTimeCustom()
        {
            TabHostMain = (TabHost)FindViewById(Resource.Id.CalculatorMedicalTabHostMain);
            TabHostMain.Setup();
            TabW = (TabWidget)FindViewById(Android.Resource.Id.Tabs);
            Vibrator vibrator = GetSystemService(VibratorService) as Vibrator;

            // Tab Home
            TabHost.TabSpec TabOne = TabHostMain.NewTabSpec("One");
            TabOne.SetContent(Resource.Id.CalculatorMedicalLinearLayoutTp1);
            View ViewHome = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgHome = ViewHome.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgHome.SetImageResource(Resource.Drawable.MCHome);
            TitleHome = (TextView)ViewHome.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleHome.SetTextColor(Color.Maroon);
            TitleHome.Text = "صفحه اصلی";
            TabOne.SetIndicator(ViewHome);
            TabHostMain.AddTab(TabOne);
            ViewHome.Click += async delegate
            {
                tth = TypeTabHost.Home;
                TabHostMain.CurrentTab = 0;
                TitleHome.SetTextColor(Color.Maroon);
                TitleDep.SetTextColor(Color.WhiteSmoke);
                TitleSD.SetTextColor(Color.WhiteSmoke);
                vibrator.Vibrate(25); 
                imgHome.ImageAlpha = 100;
                await Task.Delay(200);
                imgHome.ImageAlpha = 255;
            };
            // Tab Dep...
            TabHost.TabSpec TabTwo = TabHostMain.NewTabSpec("Two");
            TabTwo.SetContent(Resource.Id.CalculatorMedicalLinearLayoutTp2);
            View ViewDep = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgDep = ViewDep.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgDep.SetImageResource(Resource.Drawable.MCDep);
            TitleDep = (TextView)ViewDep.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleDep.SetTextColor(Color.WhiteSmoke);
            TitleDep.Text = "افسردگی";
            TabTwo.SetIndicator(ViewDep);
            TabHostMain.AddTab(TabTwo);
            ViewDep.Click += async delegate
            {
                tth = TypeTabHost.Dep;
                TabHostMain.CurrentTab = 1;
                TitleHome.SetTextColor(Color.WhiteSmoke);
                TitleDep.SetTextColor(Color.Maroon);
                TitleSD.SetTextColor(Color.WhiteSmoke);
                vibrator.Vibrate(25);
                imgDep.ImageAlpha = 100;
                await Task.Delay(600);
                imgDep.ImageAlpha = 255;
            };
            // Tab SD
            TabHost.TabSpec TabThree = TabHostMain.NewTabSpec("Three");
            TabThree.SetContent(Resource.Id.CalculatorMedicalLinearLayoutTp3);
            View ViewSD = View.Inflate(this, Resource.Layout.CalculatorMedicalTabHostCustom, null);
            ImageView imgSD = ViewSD.FindViewById<ImageView>(Resource.Id.CMTabHostImageViewPic);
            imgSD.SetImageResource(Resource.Drawable.MCSB);
            TitleSD = (TextView)ViewSD.FindViewById(Resource.Id.CMTabHostTextViewTilte);
            TitleSD.SetTextColor(Color.WhiteSmoke);
            TitleSD.Text = "دیابت";
            TabThree.SetIndicator(ViewSD);
            TabHostMain.AddTab(TabThree);
            ViewSD.Click += async delegate 
            {
                tth = TypeTabHost.Sd;
                TabHostMain.CurrentTab = 2;
                TitleHome.SetTextColor(Color.WhiteSmoke);
                TitleDep.SetTextColor(Color.WhiteSmoke);
                TitleSD.SetTextColor(Color.Maroon);
                vibrator.Vibrate(25); 
                imgSD.ImageAlpha = 100;
                await Task.Delay(200);
                imgSD.ImageAlpha = 255;
            };
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                switch (tth)
                {
                    case TypeTabHost.Home:
                        Finish();
                        break;
                    case TypeTabHost.Dep:
                        tth = TypeTabHost.Home;
                        TabHostMain.CurrentTab = 0;
                        TitleHome.SetTextColor(Color.Maroon);
                        TitleDep.SetTextColor(Color.WhiteSmoke);
                        TitleSD.SetTextColor(Color.WhiteSmoke);
                        keyCode = Keycode.A;
                        break;
                    case TypeTabHost.Sd:
                        tth = TypeTabHost.Dep;
                        TabHostMain.CurrentTab = 1;
                        TitleHome.SetTextColor(Color.WhiteSmoke);
                        TitleDep.SetTextColor(Color.Maroon);
                        TitleSD.SetTextColor(Color.WhiteSmoke);
                        keyCode = Keycode.A;
                        break;
                    default:
                        break;
                }

            }
            return base.OnKeyDown(keyCode, e);
        }

        protected override void AttachBaseContext(Android.Content.Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

}

    public class ItemAddList : BaseAdapter
    {
        private Activity context;
        private List<TypeItemList> list;

        public ItemAddList (Activity mcontext, List<TypeItemList> mlist)
        {
            context = mcontext;
            list = mlist;
        }

        public override int Count
        {
            get
            {
                return list.Count;
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
            // get data
            var cd = list[position];
            // get view
            var cv = context.LayoutInflater.Inflate(Resource.Layout.ListViewCMDep, parent, false);

            TextView TextNumber = (TextView)cv.FindViewById(Resource.Id.LvDepTextViewNumber);
            TextNumber.Text = (position + 1).ToString();

            ImageView img = (ImageView)cv.FindViewById(Resource.Id.LvDepImageViewOC);
            img.SetImageResource(cd.Icon);

            TextView Title = (TextView)cv.FindViewById(Resource.Id.LvDepTextViewTitle);
            Title.Text = cd.Title; 

            TextView TextSu = (TextView)cv.FindViewById(Resource.Id.LvDepTextViewText);
            TextSu.Text = cd.Text;

            return cv;
        }
    }

    public class TypeItemList
    {
        public int Icon { get; set; }
        public string Title { get; set; }
        public string  Text { get; set; }
    }
}