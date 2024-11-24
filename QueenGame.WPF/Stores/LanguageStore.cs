using System.Globalization;
using System.Windows;

namespace QueenGame.WPF.Stores
{
    public class LanguageStore
    {
        private readonly string FILEPATH_PATTERN = "Resources/Langs/lang.{0}.xaml";
        private readonly string FILEPATH_DEFAULT = "Resources/Langs/lang.xaml";
        private readonly string FILEPATH_STARTSWITH = "Resources/Langs/lang.";

        private CultureInfo DefaultCultureInfo = new CultureInfo("en-US");

        private List<CultureInfo> _Languages; 
        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                return _Languages;
            }
        }

        private bool AvailableCultureInfo(CultureInfo cultureInfo) 
        { 
            return Languages.Any(v => v.Equals(cultureInfo));
        }

        public LanguageStore(IEnumerable<string> langs, string? prefferedLang = null)
        {
            _Languages = new List<CultureInfo>();
            foreach (string language in langs) 
            {
                try
                {
                    _Languages.Add(new CultureInfo(language));
                }
                catch (Exception ex) { }
            }
            
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture; 
            if (prefferedLang != null)
            {
                try
                {
                    cultureInfo = new CultureInfo(prefferedLang);
                    if (!AvailableCultureInfo(cultureInfo))
                    {
                        cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    }
                }
                catch (Exception ex) { }
            }
            if (!AvailableCultureInfo(cultureInfo))
            {
                cultureInfo = DefaultCultureInfo;
            }

            CurrentLanguage = cultureInfo;
        }

        //Евент для оповещения всех окон приложения
        public event EventHandler? LanguageChanged;

        protected void OnLanguageChangedChanged()
        {
            LanguageChanged?.Invoke(Application.Current, new EventArgs());
        }


        public CultureInfo CurrentLanguage
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                //1. Меняем язык приложения:
                Thread.CurrentThread.CurrentUICulture = value;

                //2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();

                CultureInfo? cultureInfo = Languages.FirstOrDefault(v => v.Name == value.Name);
                if (cultureInfo != null)
                {
                    try 
                    { 
                        dict.Source = new Uri(String.Format(FILEPATH_PATTERN, value.Name), UriKind.Relative);                    
                    }
                    catch 
                    { 
                        dict.Source = new Uri(FILEPATH_DEFAULT, UriKind.Relative);                        
                    }
                }
                else 
                { 
                    dict.Source = new Uri(FILEPATH_DEFAULT, UriKind.Relative);
                }

                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary? oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null 
                                                    && d.Source.OriginalString.StartsWith(FILEPATH_STARTSWITH)
                                              select d).FirstOrDefault();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                //4. Вызываем евент для оповещения всех 
                OnLanguageChangedChanged();
            }
        }
    }
}
