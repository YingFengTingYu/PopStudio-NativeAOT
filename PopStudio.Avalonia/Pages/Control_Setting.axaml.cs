using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PopStudio.Language.Languages;
using PopStudio.Platform;
using System.Reactive.Subjects;

namespace PopStudio.Avalonia.Pages
{
    public partial class Control_Setting : UserControl
    {
        public Control_Setting()
        {
            InitializeComponent();
            LoadControl();
            LoadFont();
            MAUIStr.OnLanguageChanged += LoadFont;
            Setting.CanSave = false;
            InitDzPackSetting();
            InitPakPS3PackSetting();
            InitRsbPackSetting();
            InitPTXDecodeSetting();
            InitCdatKeySetting();
            InitRTONKeySetting();
            InitImageStringSetting();
            InitXflSetting();
            switch_ad.IsChecked = Setting.OpenProgramAD;
            xfllabelname.SelectionChanged += xfllabel_TextChanged;
            Setting.CanSave = true;
        }

        ~Control_Setting()
        {
            MAUIStr.OnLanguageChanged -= LoadFont;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void LoadControl()
        {
            label_introduction = this.Get<TextBlock>("label_introduction");
            label_itemlanguage = this.Get<TextBlock>("label_itemlanguage");
            button_chooselanguage = this.Get<Button>("button_chooselanguage");
            label_itemdz = this.Get<TextBlock>("label_itemdz");
            label_introdz = this.Get<TextBlock>("label_introdz");
            Setting_Dz = this.Get<ListBox>("Setting_Dz");
            button_dz_1 = this.Get<Button>("button_dz_1");
            button_dz_2 = this.Get<Button>("button_dz_2");
            label_itempak = this.Get<TextBlock>("label_itempak");
            label_intropak = this.Get<TextBlock>("label_intropak");
            Setting_PakPS3 = this.Get<ListBox>("Setting_PakPS3");
            button_pak_1 = this.Get<Button>("button_pak_1");
            button_pak_2 = this.Get<Button>("button_pak_2");
            label_itemrsb = this.Get<TextBlock>("label_itemrsb");
            label_introrsb = this.Get<TextBlock>("label_introrsb");
            rsbptx1 = this.Get<ToggleSwitch>("rsbptx1");
            rsbptx2 = this.Get<ToggleSwitch>("rsbptx2");
            label_itemptx = this.Get<TextBlock>("label_itemptx");
            label_introptx = this.Get<TextBlock>("label_introptx");
            ptx1 = this.Get<ToggleSwitch>("ptx1");
            ptx2 = this.Get<ToggleSwitch>("ptx2");
            label_itemcdat = this.Get<TextBlock>("label_itemcdat");
            label_introcdat = this.Get<TextBlock>("label_introcdat");
            cdat = this.Get<TextBox>("cdat");
            label_itemrton = this.Get<TextBlock>("label_itemrton");
            label_introrton = this.Get<TextBlock>("label_introrton");
            rton = this.Get<TextBox>("rton");
            label_itemcompiled = this.Get<TextBlock>("label_itemcompiled");
            label_introcompiled = this.Get<TextBlock>("label_introcompiled");
            imagestring = this.Get<TextBlock>("imagestring");
            button_compiled_1 = this.Get<Button>("button_compiled_1");
            button_compiled_2 = this.Get<Button>("button_compiled_2");
            label_itemxfl = this.Get<TextBlock>("label_itemxfl");
            label_introxfl = this.Get<TextBlock>("label_introxfl");
            label_xflwidth = this.Get<TextBlock>("label_xflwidth");
            xflwidth = this.Get<TextBox>("xflwidth");
            label_xflheight = this.Get<TextBlock>("label_xflheight");
            xflheight = this.Get<TextBox>("xflheight");
            label_xfllabelname = this.Get<TextBlock>("label_xfllabelname");
            xfllabelname = this.Get<ComboBox>("xfllabelname");
            label_xflscalex = this.Get<TextBlock>("label_xflscalex");
            xflscalex = this.Get<TextBox>("xflscalex");
            label_xflscaley = this.Get<TextBlock>("label_xflscaley");
            xflscaley = this.Get<TextBox>("xflscaley");
            label_ad = this.Get<TextBlock>("label_ad");
            switch_ad = this.Get<ToggleSwitch>("switch_ad");
            button_recover = this.Get<Button>("button_recover");
            button_cdat = this.Get<Button>("button_cdat");
            button_rton = this.Get<Button>("button_rton");
            button_xflwidth = this.Get<Button>("button_xflwidth");
            button_xflheight = this.Get<Button>("button_xflheight");
            button_xflscalex = this.Get<Button>("button_xflscalex");
            button_xflscaley = this.Get<Button>("button_xflscaley");
        }

        void LoadFont()
        {
            label_introduction.Text = MAUIStr.Obj.Setting_Introduction;
            label_itemlanguage.Text = string.Format(MAUIStr.Obj.Setting_ItemLanguage, Setting.LanguageName[Setting.AppLanguage]);
            button_chooselanguage.Content = MAUIStr.Obj.Setting_SetLanguage;
            label_itemdz.Text = MAUIStr.Obj.Setting_ItemDz;
            label_introdz.Text = MAUIStr.Obj.Setting_IntroDz;
            button_dz_1.Content = MAUIStr.Obj.Setting_Add;
            button_dz_2.Content = MAUIStr.Obj.Setting_Clear;
            label_itempak.Text = MAUIStr.Obj.Setting_ItemPak;
            label_intropak.Text = MAUIStr.Obj.Setting_IntroPak;
            button_pak_1.Content = MAUIStr.Obj.Setting_Add;
            button_pak_2.Content = MAUIStr.Obj.Setting_Clear;
            label_itemrsb.Text = MAUIStr.Obj.Setting_ItemRsb;
            label_introrsb.Text = MAUIStr.Obj.Setting_IntroRsb;
            label_itemptx.Text = MAUIStr.Obj.Setting_ItemPtx;
            label_introptx.Text = MAUIStr.Obj.Setting_IntroPtx;
            label_itemcdat.Text = MAUIStr.Obj.Setting_ItemCdat;
            label_introcdat.Text = MAUIStr.Obj.Setting_IntroCdat;
            label_itemrton.Text = MAUIStr.Obj.Setting_ItemRTON;
            label_introrton.Text = MAUIStr.Obj.Setting_IntroRTON;
            label_itemcompiled.Text = MAUIStr.Obj.Setting_ItemCompiled;
            label_introcompiled.Text = MAUIStr.Obj.Setting_IntroCompiled;
            label_itemxfl.Text = MAUIStr.Obj.Setting_ItemXfl;
            label_introxfl.Text = MAUIStr.Obj.Setting_IntroXfl;
            label_xflwidth.Text = MAUIStr.Obj.Setting_XflWidth;
            label_xflheight.Text = MAUIStr.Obj.Setting_XflHeight;
            label_xfllabelname.Text = MAUIStr.Obj.Setting_XflLabelName;
            label_xflscalex.Text = MAUIStr.Obj.Setting_XflScaleX;
            label_xflscaley.Text = MAUIStr.Obj.Setting_XflScaleY;
            label_ad.Text = MAUIStr.Obj.Setting_AD;
            button_compiled_1.Content = MAUIStr.Obj.Setting_Load;
            button_compiled_2.Content = MAUIStr.Obj.Setting_Unload;
            button_recover.Content = MAUIStr.Obj.Setting_Recover;
            button_cdat.Content = MAUIStr.Obj.Setting_Submit;
            button_rton.Content = MAUIStr.Obj.Setting_Submit;
            button_xflwidth.Content = MAUIStr.Obj.Setting_Submit;
            button_xflheight.Content = MAUIStr.Obj.Setting_Submit;
            button_xflscalex.Content = MAUIStr.Obj.Setting_Submit;
            button_xflscaley.Content = MAUIStr.Obj.Setting_Submit;
        }

        /// <summary>
        /// DZ Setting Begin
        /// </summary>
        List<ListInfo> dzpackinfo;

        private void Button_Dz_1_Clicked(object sender, RoutedEventArgs e)
        {
            AddDzPackSetting();
        }

        private void Button_Dz_2_Clicked(object sender, RoutedEventArgs e)
        {
            ClearDzPackSetting();
        }

        void InitDzPackSetting()
        {
            dzpackinfo = new List<ListInfo>();
            Dictionary<string, Package.Dz.CompressFlags> dzpacksetting = Setting.DzCompressDictionary;
            Dictionary<Package.Dz.CompressFlags, string> dzcompressname = Setting.DzCompressMethodName;
            dzpackinfo.Add(new ListInfo("default", dzcompressname[Setting.DzDefaultCompressMethod]));
            foreach (KeyValuePair<string, Package.Dz.CompressFlags> keyValuePair in dzpacksetting)
            {
                dzpackinfo.Add(new ListInfo(keyValuePair.Key, dzcompressname[keyValuePair.Value]));
            }
            Setting_Dz.Items = dzpackinfo;
        }

        async void AddDzPackSetting()
        {
            try
            {
                string cancel = MAUIStr.Obj.Setting_Cancel;
                string ok = MAUIStr.Obj.Setting_OK;
                string ans = await this.DisplayPromptAsync(MAUIStr.Obj.Setting_EnterExtension, MAUIStr.Obj.Setting_EnterExtensionText, ok, cancel);
                if (!string.IsNullOrEmpty(ans))
                {
                    string itemname = ans;
                    if (!itemname.StartsWith('.'))
                    {
                        itemname = '.' + itemname;
                    }
                    string item1 = "Store";
                    string item2 = "Lzma";
                    string item3 = "Gzip";
                    string item4 = "Bzip2";
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2, item3, item4);
                    if (ans != cancel && ans != ok)
                    {
                        Dictionary<string, Package.Dz.CompressFlags> tempdic = Setting.DzCompressDictionary;
                        if (tempdic.ContainsKey(itemname))
                        {
                            tempdic[itemname] = Setting.DzCompressMethodEnum[ans];
                        }
                        else
                        {
                            tempdic.Add(itemname, Setting.DzCompressMethodEnum[ans]);
                        }
                        InitDzPackSetting();
                        Setting.SaveAsXml(Permission.GetSettingPath());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        void ClearDzPackSetting()
        {
            Setting.DzCompressDictionary.Clear();
            InitDzPackSetting();
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        async void ChangeDzPackSetting(string itemname)
        {
            try
            {
                string cancel = MAUIStr.Obj.Setting_Cancel;
                string ok = MAUIStr.Obj.Setting_OK;
                string choose1 = MAUIStr.Obj.Setting_CompressItem1;
                string choose2 = MAUIStr.Obj.Setting_CompressItem2;
                string ans;
                if (itemname == "default")
                {
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1);
                }
                else
                {
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1, choose2);
                }
                if (ans == choose1)
                {
                    string item1 = "Store";
                    string item2 = "Lzma";
                    string item3 = "Gzip";
                    string item4 = "Bzip2";
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2, item3, item4);
                    if (ans != cancel && ans != ok)
                    {
                        if (itemname == "default")
                        {
                            Setting.DzDefaultCompressMethod = Setting.DzCompressMethodEnum[ans];
                        }
                        else
                        {
                            Setting.DzCompressDictionary[itemname] = Setting.DzCompressMethodEnum[ans];
                        }
                        InitDzPackSetting();
                        Setting.SaveAsXml(Permission.GetSettingPath());
                    }
                }
                else if (ans == choose2)
                {
                    if (Setting.DzCompressDictionary.ContainsKey(itemname))
                    {
                        Setting.DzCompressDictionary.Remove(itemname);
                        InitDzPackSetting();
                        Setting.SaveAsXml(Permission.GetSettingPath());
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Setting_Dz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lst = sender as ListBox;
            if (lst == null || lst.SelectedIndex == -1 || e.AddedItems.Count == 0) return;
            ChangeDzPackSetting((e.AddedItems[0] as ListInfo)?.ItemName);
            lst.SelectedIndex = -1;
        }

        /// <summary>
        /// PakPS3 Setting Begin
        /// </summary>
        List<ListInfo> pakps3packinfo;

        private void Button_PakPS3_1_Clicked(object sender, RoutedEventArgs e)
        {
            AddPakPS3PackSetting();
        }

        private void Button_PakPS3_2_Clicked(object sender, RoutedEventArgs e)
        {
            ClearPakPS3PackSetting();
        }

        void InitPakPS3PackSetting()
        {
            pakps3packinfo = new List<ListInfo>();
            Dictionary<string, Package.Pak.CompressFlags> pakps3packsetting = Setting.PakPS3CompressDictionary;
            Dictionary<Package.Pak.CompressFlags, string> pakps3compressname = Setting.PakPS3CompressMethodName;
            pakps3packinfo.Add(new ListInfo("default", pakps3compressname[Setting.PakPS3DefaultCompressMethod]));
            foreach (KeyValuePair<string, Package.Pak.CompressFlags> keyValuePair in pakps3packsetting)
            {
                pakps3packinfo.Add(new ListInfo(keyValuePair.Key, pakps3compressname[keyValuePair.Value]));
            }
            Setting_PakPS3.Items = pakps3packinfo;
        }

        async void AddPakPS3PackSetting()
        {
            try
            {
                string cancel = MAUIStr.Obj.Setting_Cancel;
                string ok = MAUIStr.Obj.Setting_OK;
                string ans = await this.DisplayPromptAsync(MAUIStr.Obj.Setting_EnterExtension, MAUIStr.Obj.Setting_EnterExtensionText, ok, cancel);
                if (!string.IsNullOrEmpty(ans))
                {
                    string itemname = ans;
                    if (!itemname.StartsWith('.'))
                    {
                        itemname = '.' + itemname;
                    }
                    string item1 = "Store";
                    string item2 = "Zlib";
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2);
                    if (ans != cancel && ans != ok)
                    {
                        Dictionary<string, Package.Pak.CompressFlags> tempdic = Setting.PakPS3CompressDictionary;
                        if (tempdic.ContainsKey(itemname))
                        {
                            tempdic[itemname] = Setting.PakPS3CompressMethodEnum[ans];
                        }
                        else
                        {
                            tempdic.Add(itemname, Setting.PakPS3CompressMethodEnum[ans]);
                        }
                        InitPakPS3PackSetting();
                        Setting.SaveAsXml(Permission.GetSettingPath());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        void ClearPakPS3PackSetting()
        {
            Setting.PakPS3CompressDictionary.Clear();
            InitPakPS3PackSetting();
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        async void ChangePakPS3PackSetting(string itemname)
        {
            try
            {
                string cancel = MAUIStr.Obj.Setting_Cancel;
                string ok = MAUIStr.Obj.Setting_OK;
                string choose1 = MAUIStr.Obj.Setting_CompressItem1;
                string choose2 = MAUIStr.Obj.Setting_CompressItem2;
                string ans;
                if (itemname == "default")
                {
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1);
                }
                else
                {
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseItem, cancel, ok, choose1, choose2);
                }
                if (ans == choose1)
                {
                    string item1 = "Store";
                    string item2 = "Zlib";
                    ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseCompressMode, cancel, ok, item1, item2);
                    if (ans != cancel && ans != ok)
                    {
                        if (itemname == "default")
                        {
                            Setting.PakPS3DefaultCompressMethod = Setting.PakPS3CompressMethodEnum[ans];
                        }
                        else
                        {
                            Setting.PakPS3CompressDictionary[itemname] = Setting.PakPS3CompressMethodEnum[ans];
                        }
                        InitPakPS3PackSetting();
                        Setting.SaveAsXml(Permission.GetSettingPath());
                    }
                }
                else if (ans == choose2)
                {
                    if (Setting.PakPS3CompressDictionary.ContainsKey(itemname))
                    {
                        Setting.PakPS3CompressDictionary.Remove(itemname);
                        InitPakPS3PackSetting();
                        Setting.SaveAsXml(Permission.GetSettingPath());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void Setting_PakPS3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lst = sender as ListBox;
            if (lst == null || lst.SelectedIndex == -1 || e.AddedItems.Count == 0) return;
            ChangePakPS3PackSetting((e.AddedItems[0] as ListInfo)?.ItemName);
            lst.SelectedIndex = -1;
        }

        /// <summary>
        /// RsbPTX Setting Begin
        /// </summary>
        void InitRsbPackSetting()
        {
            rsbptx1.IsChecked = Setting.RsbPtxABGR8888Mode;
            rsbptx2.IsChecked = Setting.RsbPtxARGB8888PaddingMode;
        }

        private void Switch_RsbPtx_1_Toggled(object sender, RoutedEventArgs e)
        {
            Setting.RsbPtxABGR8888Mode = rsbptx1.IsChecked == true;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private void Switch_RsbPtx_2_Toggled(object sender, RoutedEventArgs e)
        {
            Setting.RsbPtxARGB8888PaddingMode = rsbptx2.IsChecked == true;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        /// <summary>
        /// PTX Setting Begin
        /// </summary>
        void InitPTXDecodeSetting()
        {
            ptx1.IsChecked = Setting.PtxABGR8888Mode;
            ptx2.IsChecked = Setting.PtxARGB8888PaddingMode;
        }

        private void Switch_Ptx_1_Toggled(object sender, RoutedEventArgs e)
        {
            Setting.PtxABGR8888Mode = ptx1.IsChecked == true;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private void Switch_Ptx_2_Toggled(object sender, RoutedEventArgs e)
        {
            Setting.PtxARGB8888PaddingMode = ptx2.IsChecked == true;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        /// <summary>
        /// Cdat Setting Begin
        /// </summary>
        void InitCdatKeySetting()
        {
            cdat.Text = Setting.CdatKey;
        }

        private void Button_Cdat_Click(object sender, RoutedEventArgs e)
        {
            Setting.CdatKey = cdat.Text;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        /// <summary>
        /// RTON Setting Begin
        /// </summary>
        void InitRTONKeySetting()
        {
            rton.Text = Setting.RTONKey;
        }

        private void Button_RTON_Click(object sender, RoutedEventArgs e)
        {
            Setting.RTONKey = rton.Text;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        /// <summary>
        /// ImageString Setting Begin
        /// </summary>
        void InitImageStringSetting()
        {
            imagestring.Text = Setting.ImageConvertName;
        }

        private async void Button_ImageString_1_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = (await new OpenFileDialog().ShowAsync(MainWindow.Singleten))?[0];
                if (string.IsNullOrEmpty(path)) return;
                Setting.LoadImageConvertXml(path);
                InitImageStringSetting();
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
            catch (Exception)
            {

            }
        }

        private void Button_ImageString_2_Clicked(object sender, RoutedEventArgs e)
        {
            Setting.ClearImageConvertXml();
            InitImageStringSetting();
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        /// <summary>
        /// Xfl Setting Begin
        /// </summary>
        void InitXflSetting()
        {
            xflwidth.Text = Setting.ReanimXflWidth.ToString();
            xflheight.Text = Setting.ReanimXflHeight.ToString();
            xfllabelname.Items = new List<string>
            {
                "Image Name",
                "Short Image Name",
                "Label Name"
            };
            xfllabelname.SelectedIndex = Setting.ReanimXflLabelName + 1;
            //xfllabelname.Text = Setting.ReanimXflLabelName.ToString();
            xflscalex.Text = Setting.ReanimXflScaleX.ToString();
            xflscaley.Text = Setting.ReanimXflScaleY.ToString();
        }

        private void Entry_XflSize_Width_TextChanged(object sender, RoutedEventArgs e)
        {
            string n = xflwidth.Text;
            try
            {
                Setting.ReanimXflWidth = Convert.ToSingle(n);
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
            catch (Exception)
            {
            }
        }

        private void Entry_XflSize_Height_TextChanged(object sender, RoutedEventArgs e)
        {
            string n = xflheight.Text;
            try
            {
                Setting.ReanimXflHeight = Convert.ToSingle(n);
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
            catch (Exception)
            {
            }
        }

        private void xfllabel_TextChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = xfllabelname.SelectedIndex - 1;
            try
            {
                Setting.ReanimXflLabelName = n;
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
            catch (Exception)
            {
            }
        }

        private void xflscalex_TextChanged(object sender, RoutedEventArgs e)
        {
            string n = xflscalex.Text;
            try
            {
                Setting.ReanimXflScaleX = Convert.ToDouble(n);
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
            catch (Exception)
            {
            }
        }

        private void xflscaley_TextChanged(object sender, RoutedEventArgs e)
        {
            string n = xflscaley.Text;
            try
            {
                Setting.ReanimXflScaleY = Convert.ToDouble(n);
                Setting.SaveAsXml(Permission.GetSettingPath());
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Reset All(Without language)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_ResetSetting_Clicked(object sender, RoutedEventArgs e)
        {
            string cancel = MAUIStr.Obj.Setting_Cancel;
            string ok = MAUIStr.Obj.Setting_OK;
            bool sure = await this.DisplayAlert(MAUIStr.Obj.Setting_SureRecover, MAUIStr.Obj.Setting_SureRecoverText, ok, cancel);
            if (sure)
            {
                Setting.ResetXml(Permission.GetSettingPath());
                await this.DisplayAlert(MAUIStr.Obj.Setting_FinishRecover, MAUIStr.Obj.Setting_FinishRecoverText, ok);
                Environment.Exit(0);
            }
        }

        async void ChangeLanguageSetting()
        {
            try
            {
                string cancel = MAUIStr.Obj.Setting_Cancel;
                string ok = MAUIStr.Obj.Setting_OK;
                string item1 = "\u7B80\u4F53\u4E2D\u6587";
                string item2 = "English";
                string ans = await this.DisplayActionSheet(MAUIStr.Obj.Setting_ChooseLanguage, cancel, ok, item1, item2);
                if (ans != cancel && ans != ok && Setting.LanguageEnum[ans] != Setting.AppLanguage)
                {
                    Setting.AppLanguage = Setting.LanguageEnum[ans];
                    Setting.SaveAsXml(Permission.GetSettingPath());
                    //await this.DisplayAlert(MAUIStr.Obj.Setting_FinishChooseLanguage, MAUIStr.Obj.Setting_FinishChooseLanguageText, ok);
                    MAUIStr.LoadLanguage();
                }
            }
            catch (Exception)
            {

            }
        }

        private void button_chooselanguage_Clicked(object sender, RoutedEventArgs e)
        {
            ChangeLanguageSetting();
        }

        private void Switch_AD_1_Toggled(object sender, RoutedEventArgs e)
        {
            Setting.OpenProgramAD = ((ToggleSwitch)sender).IsChecked == true;
            Setting.SaveAsXml(Permission.GetSettingPath());
        }

        private class ListInfo
        {
            public string ItemName { get; set; }
            public string ItemValue { get; set; }

            public ListInfo()
            {

            }

            public ListInfo(string itemName, string itemValue)
            {
                ItemName = itemName;
                ItemValue = itemValue;
            }
        }
    }
}
