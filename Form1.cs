using FontAwesome.Sharp;
using GoogleTranslateFreeApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HumanInterface
{
    public partial class MainForm : Form
    {
        WebBrowser wb = new WebBrowser();
        Button wb_BackButton = new Button();
        Button wb_RefreshButton = new Button();
        Button main_ChangeCity = new Button();
        Button main_ShowMap = new Button();
        Button main_Theme = new Button();
        Button city_Change = new Button();
        Button city_Back = new Button();
        UserLocation location;
        Panel Main = new Panel();
        Label currentCity = new Label();
        Panel WeatherPanel = new Panel();
        Label DateText = new Label();
        Label DayNight = new Label();
        Label Temperature = new Label();
        Label SunRiseAndSet = new Label();
        Label AddInfo = new Label();
        Label EnterCity = new Label();
        PictureBox currentCloud = new PictureBox();
        TextBox CityName = new TextBox();
        Color accentColor = Color.Black;
        Color temperatureColor = Color.Blue;
        Color background = Color.WhiteSmoke;
        Color mainColor = Color.Turquoise;
        Color buttonColor = Color.Teal;
        Panel selectCity = new Panel();
        Weather weather = new Weather();
        MenuStrip menuStrip = new MenuStrip();
        ToolStripMenuItem HelpMenuTool = new ToolStripMenuItem();
        ToolStripMenuItem InfoMenuTool = new ToolStripMenuItem();
        public MainForm()
        {
            InitializeComponent();

            //Menu
            menuStrip.Items.AddRange(new ToolStripItem[] { HelpMenuTool, InfoMenuTool });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip1";
            menuStrip.Size = new Size(404, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";

            HelpMenuTool.Name = "HelpMenuTool";
            HelpMenuTool.Size = new Size(68, 20);
            HelpMenuTool.Text = "Справка";
            HelpMenuTool.Click += (e,x) => MessageBox.Show("\tПрограмма \"Погода\" разработана для информационного осведомления о погодных условиях в определенной локации (городе).\n\n\tПриложение разработал Молчанов Артём, студент группы 3-42В Ивановского государственного энергетического университета им. Ленина в 2023 году.","Справка",MessageBoxButtons.OK,MessageBoxIcon.Information);

            InfoMenuTool.Name = "InfoMenuTool";
            InfoMenuTool.Size = new Size(172, 20);
            InfoMenuTool.Text = "Информация о WebBrowser";
            InfoMenuTool.Click += (e, x) => MessageBox.Show("\tWebBrowser - элемент, предоставляющий функции интернет-браузера, позволяя загружать и отображать контент из сети интернет, но важно понимать, что данный элемент не является полноценным веб-браузером, и возможности по его настройки и изменению довольно ограничены.\n\n\tОсновные свойства:\nCanGoBack: определяет, может ли веб-браузер переходить назад по истории просмотров\nCanGoForward: определяет, может ли веб-браузер переходить вперед\nDocument: возвращает открытый в веб-браузере документ\nDocumentText: возвращает текстовое содержание документа\nDocumentTitle: возвращает заголовок документа\nDocumentType: возвращает тип документа\nIsOffline: возвращает true, если отсутствует подключение к интернету\nScriptErrorsSuppressed: указывает, будут ли отображаться ошибки javascript в диалоговом окне\nScrollBarsEnabled: определяет, будет ли использоваться прокрутка\nURL: возвращает или устанавливает URL документа в веб-браузере\n\n\tОсновные методы:\nGoBack(): осуществляет переход к предыдущей странице в истории навигации (если таковая имеется)\nGoForward(): осуществляет переход к следующей странице в истории навигации\nGoHome(): осуществляет переход к домашней странице веб-браузера\nGoSearch(): осуществляет переход к странице поиска\nNavigate(url): осуществляет переход к определенному адресу в сети интернет", "Информация о WebBrowser", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Controls.Add(menuStrip);
            //Home
            this.BackColor = background;
            this.Width = 420;
            this.Height = 500;

            Main.Dock = DockStyle.Fill;
            Main.Visible = false;

            currentCity.Text = "Текущий город: ";
            currentCity.ForeColor = accentColor;
            currentCity.Size = new Size(300, 20);
            currentCity.TextAlign = ContentAlignment.MiddleRight;
            currentCity.Location = new Point(this.ClientSize.Width - currentCity.Width - 10, 20);
            currentCity.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            Main.Controls.Add(currentCity);

            main_ChangeCity.BackColor = mainColor;
            main_ChangeCity.ForeColor = accentColor;
            main_ChangeCity.BackgroundImageLayout = ImageLayout.None;
            main_ChangeCity.Cursor = Cursors.Hand;
            main_ChangeCity.FlatAppearance.BorderColor = mainColor;
            main_ChangeCity.FlatAppearance.BorderSize = 0;
            main_ChangeCity.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            main_ChangeCity.FlatAppearance.MouseOverBackColor = Color.Cyan;
            main_ChangeCity.FlatStyle = FlatStyle.Flat;
            main_ChangeCity.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            main_ChangeCity.Size = new Size(80, 24);
            main_ChangeCity.Location = new Point(this.ClientSize.Width - main_ChangeCity.Width - 10, 45);
            main_ChangeCity.Name = "main_ChangeCity";
            main_ChangeCity.TabIndex = 1;
            main_ChangeCity.Click += ChangeCityButton;
            main_ChangeCity.Text = "Изменить";
            main_ChangeCity.UseVisualStyleBackColor = false;
            Main.Controls.Add(main_ChangeCity);

            main_Theme.BackColor = mainColor;
            main_Theme.ForeColor = accentColor;
            main_Theme.BackgroundImageLayout = ImageLayout.None;
            main_Theme.Cursor = Cursors.Hand;
            main_Theme.FlatAppearance.BorderColor = mainColor;
            main_Theme.FlatAppearance.BorderSize = 0;
            main_Theme.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            main_Theme.FlatAppearance.MouseOverBackColor = Color.Cyan;
            main_Theme.FlatStyle = FlatStyle.Flat;
            main_Theme.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            main_Theme.Size = new Size(32, 32);
            main_Theme.Location = new Point(20, 25);
            main_Theme.Name = "main_Theme";
            main_Theme.TabIndex = 1;
            main_Theme.Click += ChangeTheme;
            main_Theme.Image = FontAwesome.Sharp.IconChar.Sun.ToBitmap(IconFont.Auto, 32, buttonColor);
            main_Theme.UseVisualStyleBackColor = false;
            Main.Controls.Add(main_Theme);

            main_ShowMap.BackColor = mainColor;
            main_ShowMap.ForeColor = accentColor;
            main_ShowMap.BackgroundImageLayout = ImageLayout.None;
            main_ShowMap.Cursor = Cursors.Hand;
            main_ShowMap.FlatAppearance.BorderColor = mainColor;
            main_ShowMap.FlatAppearance.BorderSize = 0;
            main_ShowMap.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            main_ShowMap.FlatAppearance.MouseOverBackColor = Color.Cyan;
            main_ShowMap.FlatStyle = FlatStyle.Flat;
            main_ShowMap.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            main_ShowMap.Size = new Size(120, 30);
            main_ShowMap.Location = new Point(this.ClientSize.Width / 2 - main_ShowMap.Width / 2, this.ClientSize.Height - 50);
            main_ShowMap.Name = "main_ShowMap";
            main_ShowMap.TabIndex = 1;
            main_ShowMap.Text = "Показать карту";
            main_ShowMap.UseVisualStyleBackColor = false;
            main_ShowMap.Click += ShowMapButton;
            Main.Controls.Add(main_ShowMap);

            WeatherPanel.BackColor = mainColor;
            WeatherPanel.Size = new Size(this.ClientSize.Width - 20, 300);
            WeatherPanel.Location = new Point(10, 100);

            DateText.Text = "Сегодня, " + DateTime.Now.ToString("dd MMMM yyyy");
            DateText.ForeColor = accentColor;
            DateText.Size = new Size(200, 20);
            DateText.TextAlign = ContentAlignment.MiddleLeft;
            DateText.Location = new Point(10, 10);
            DateText.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            WeatherPanel.Controls.Add(DateText);

            Temperature.Text = "-10°";
            Temperature.ForeColor = temperatureColor;
            Temperature.Size = new Size(180, 80);
            Temperature.TextAlign = ContentAlignment.MiddleCenter;
            Temperature.Location = new Point(20, 30);
            Temperature.Font = new Font("Segoe UI", 48F, FontStyle.Bold, GraphicsUnit.Point);
            WeatherPanel.Controls.Add(Temperature);

            DayNight.Text = "Днем -9°\nНочью -10°";
            DayNight.ForeColor = accentColor;
            DayNight.ForeColor = temperatureColor;
            DayNight.Size = new Size(180, 40);
            DayNight.TextAlign = ContentAlignment.TopLeft;
            DayNight.Location = new Point(Temperature.Location.X, Temperature.Location.Y + Temperature.Height);
            DayNight.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            WeatherPanel.Controls.Add(DayNight);

            currentCloud.Size = new Size(80, 80);
            currentCloud.SizeMode = PictureBoxSizeMode.StretchImage;
            currentCloud.Image = FontAwesome.Sharp.IconChar.CloudShowersHeavy.ToBitmap(IconFont.Auto, 80, temperatureColor);
            currentCloud.Location = new Point(WeatherPanel.ClientSize.Width - currentCloud.Size.Width - 80, Temperature.Location.Y+30);
            WeatherPanel.Controls.Add(currentCloud);

            SunRiseAndSet.Text = "Восход: 05:15\nЗакат: 16:15";
            SunRiseAndSet.ForeColor = accentColor;
            SunRiseAndSet.ForeColor = temperatureColor;
            SunRiseAndSet.Size = new Size(150, 80);
            SunRiseAndSet.TextAlign = ContentAlignment.TopLeft;
            SunRiseAndSet.Location = new Point(20, 180);
            SunRiseAndSet.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            WeatherPanel.Controls.Add(SunRiseAndSet);

            AddInfo.Text = "Давление: 150 мм.рт.ст.\nВлажность: 80%\nОщущается как -9°\nСкорость ветра 5 м/с";
            AddInfo.Size = new Size(250, 80);
            AddInfo.TextAlign = ContentAlignment.TopLeft;
            AddInfo.Location = new Point(180, 180);
            AddInfo.ForeColor = accentColor;
            AddInfo.ForeColor = temperatureColor;
            AddInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            WeatherPanel.Controls.Add(AddInfo);

            Main.Controls.Add(WeatherPanel);
            this.Controls.Add(Main);
            //Map
            wb.Dock = DockStyle.Fill;
            wb.ScriptErrorsSuppressed = true;
            wb.AllowNavigation = false;
            wb.DocumentCompleted += Wb_DocumentCompleted;
            BrowserPanel.Controls.Add(wb);
            BrowserPanel.Visible = false;

            wb_BackButton.BackColor = mainColor;
            wb_BackButton.ForeColor = accentColor;
            wb_BackButton.FlatAppearance.BorderColor = mainColor;
            wb_BackButton.BackgroundImageLayout = ImageLayout.None;
            wb_BackButton.Cursor = Cursors.Hand;
            wb_BackButton.FlatAppearance.BorderSize = 0;
            wb_BackButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            wb_BackButton.FlatAppearance.MouseOverBackColor = Color.Cyan;
            wb_BackButton.FlatStyle = FlatStyle.Flat;
            wb_BackButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            wb_BackButton.Location = new Point(12, 10);
            wb_BackButton.Name = "wb_BackButton";
            wb_BackButton.Size = new Size(32, 32);
            wb_BackButton.TabIndex = 1;
            wb_BackButton.Click += BackToHome;
            wb_BackButton.Image = FontAwesome.Sharp.IconChar.ArrowLeft.ToBitmap(IconFont.Auto, 32, buttonColor);
            wb_BackButton.UseVisualStyleBackColor = false;
            wb_BackButton.Visible = false;

            wb_RefreshButton.BackColor = mainColor;
            wb_RefreshButton.ForeColor = accentColor;
            wb_RefreshButton.BackgroundImageLayout = ImageLayout.None;
            wb_RefreshButton.Cursor = Cursors.Hand;
            wb_RefreshButton.FlatAppearance.BorderColor = mainColor;
            wb_RefreshButton.FlatAppearance.BorderSize = 0;
            wb_RefreshButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            wb_RefreshButton.FlatAppearance.MouseOverBackColor = Color.Cyan;
            wb_RefreshButton.FlatStyle = FlatStyle.Flat;
            wb_RefreshButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            wb_RefreshButton.Location = new Point(12 + 32 + 10, 10);
            wb_RefreshButton.Name = "wb_RefreshButton";
            wb_RefreshButton.Size = new Size(32, 32);
            wb_RefreshButton.TabIndex = 1;
            wb_RefreshButton.Image = FontAwesome.Sharp.IconChar.Refresh.ToBitmap(IconFont.Auto, 32, buttonColor);
            wb_RefreshButton.Click += MapRefresh;
            wb_RefreshButton.UseVisualStyleBackColor = false;
            wb_RefreshButton.Visible = false;

            this.Controls.Add(wb_BackButton);
            this.Controls.Add(wb_RefreshButton);

            this.Width = 300;
            this.Height = 400;
            selectCity.Width = this.Width;
            selectCity.Height = this.Height;
            selectCity.Dock = DockStyle.Fill;
            selectCity.Visible = false;


            EnterCity.Text = "Введите название города";
            EnterCity.Size = new Size(300, 100);
            EnterCity.TextAlign = ContentAlignment.MiddleCenter;
            EnterCity.Location = new Point(0, 20);
            EnterCity.ForeColor = accentColor;
            EnterCity.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            selectCity.Controls.Add(EnterCity);

            CityName.Size = new Size(200, 60);
            CityName.Location = new Point(selectCity.Width / 2 - CityName.Width / 2, selectCity.Height / 3 - 20);
            CityName.ForeColor = accentColor;
            CityName.BackColor = mainColor;
            CityName.TextAlign = HorizontalAlignment.Center;
            CityName.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            CityName.BorderStyle = BorderStyle.FixedSingle;
            selectCity.Controls.Add(CityName);

            city_Change.BackColor = mainColor;
            city_Change.ForeColor = accentColor;
            city_Change.BackgroundImageLayout = ImageLayout.None;
            city_Change.Cursor = Cursors.Hand;
            city_Change.FlatAppearance.BorderColor = mainColor;
            city_Change.FlatAppearance.BorderSize = 0;
            city_Change.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            city_Change.FlatAppearance.MouseOverBackColor = Color.Cyan;
            city_Change.FlatStyle = FlatStyle.Flat;
            city_Change.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            city_Change.Size = new Size(100, 30);
            city_Change.Location = new Point(selectCity.Width / 2 - city_Change.Width / 2, CityName.Location.Y + 80);
            city_Change.Name = "city_Change";
            city_Change.TabIndex = 1;
            city_Change.Text = "Выбрать";
            city_Change.Click += SelectCity;
            city_Change.UseVisualStyleBackColor = false;
            selectCity.Controls.Add(city_Change);

            city_Back.BackColor = mainColor;
            city_Back.ForeColor = accentColor;
            city_Back.BackgroundImageLayout = ImageLayout.None;
            city_Back.Cursor = Cursors.Hand;
            city_Back.FlatAppearance.BorderColor = mainColor;
            city_Back.FlatAppearance.BorderSize = 0;
            city_Back.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255);
            city_Back.FlatAppearance.MouseOverBackColor = Color.Cyan;
            city_Back.FlatStyle = FlatStyle.Flat;
            city_Back.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            city_Back.Size = new Size(100, 30);
            city_Back.Location = new Point(selectCity.Width / 2 - city_Back.Width / 2, city_Change.Location.Y + 60);
            city_Back.Name = "city_Back";
            city_Back.TabIndex = 1;
            city_Back.Text = "Назад";
            city_Back.Click += BackToHome;
            city_Back.UseVisualStyleBackColor = false;
            selectCity.Controls.Add(city_Back);

            this.Controls.Add(selectCity);
            location = GetCurrentUserCity();
            ShowHomeView();
            GetWeatherData();
            return;
        }

        private void SelectCity(object? sender, EventArgs e)
        {
            location.city = CityName.Text;
            GetWeatherData();
        }

        private void ChangeCityButton(object? sender, EventArgs e)
        {
            this.Height = 400;
            this.Width = 300;

            EnterCity.Text = "Введите название города";
            CityName.Text = location.city;

            selectCity.Visible = true;
            Main.Visible = false;
            BrowserPanel.Visible = false;
            wb_BackButton.Visible = false;
            wb_RefreshButton.Visible = false;
            menuStrip.Visible = false;
        }

        private void ChangeTheme(object? sender, EventArgs e)
        {
            if (background == Color.WhiteSmoke)
            {
                accentColor = Color.LightGray;
                temperatureColor = Color.Blue;
                background = Color.Gray;
                mainColor = Color.DarkCyan;
                buttonColor = Color.Turquoise;
                main_Theme.Image = FontAwesome.Sharp.IconChar.Moon.ToBitmap(IconFont.Auto, 32, buttonColor);
            }
            else
            {
                accentColor = Color.Black;
                temperatureColor = Color.Blue;
                background = Color.WhiteSmoke;
                mainColor = Color.Turquoise;
                buttonColor = Color.Teal;
                main_Theme.Image = FontAwesome.Sharp.IconChar.Sun.ToBitmap(IconFont.Auto, 32, buttonColor);
            }
            ChangeColor();
        }
        private void ChangeColor()
        {
            this.BackColor = background;
            menuStrip.BackColor = background;

            EnterCity.ForeColor = accentColor;
            main_Theme.Image = FontAwesome.Sharp.IconChar.Sun.ToBitmap(IconFont.Auto, 32, buttonColor);
            wb_RefreshButton.Image = FontAwesome.Sharp.IconChar.Refresh.ToBitmap(IconFont.Auto, 32, buttonColor);
            wb_BackButton.Image = FontAwesome.Sharp.IconChar.ArrowLeft.ToBitmap(IconFont.Auto, 32, buttonColor);
            main_Theme.Image = FontAwesome.Sharp.IconChar.Sun.ToBitmap(IconFont.Auto, 32, buttonColor);

            currentCity.ForeColor = accentColor;

            main_ChangeCity.BackColor = mainColor;
            main_ChangeCity.ForeColor = accentColor;

            CityName.ForeColor = accentColor;
            CityName.BackColor = mainColor;

            main_Theme.BackColor = mainColor;
            main_Theme.ForeColor = accentColor;

            main_ShowMap.BackColor = mainColor;
            main_ShowMap.ForeColor = accentColor;

            city_Change.BackColor = mainColor;
            city_Change.ForeColor = accentColor;

            city_Back.BackColor = mainColor;
            city_Back.ForeColor = accentColor;

            WeatherPanel.BackColor = mainColor;

            DateText.ForeColor = temperatureColor;

            Temperature.ForeColor = temperatureColor;

            DayNight.ForeColor = temperatureColor;

            SunRiseAndSet.ForeColor = temperatureColor;

            AddInfo.ForeColor = temperatureColor;

            wb_BackButton.BackColor = mainColor;
            wb_BackButton.ForeColor = accentColor;
            wb_BackButton.FlatAppearance.BorderColor = mainColor;

            wb_RefreshButton.BackColor = mainColor;
            wb_RefreshButton.ForeColor = accentColor;

        }
        private void BackToHome(object? sender, EventArgs e) => ShowHomeView();

        private void ShowMapButton(object? sender, EventArgs e) => ShowWeatherMap();

        private void ShowHomeView()
        {
            this.Width = 420;
            this.Height = 500;

            selectCity.Visible = false;
            BrowserPanel.Visible = false;
            wb_BackButton.Visible = false;
            wb_RefreshButton.Visible = false;
            menuStrip.Visible = true;

            currentCity.Text = "Текущий город: " + location.city;

            Main.Visible = true;
        }
        private void MapRefresh(object? sender, EventArgs e) => ShowWeatherMap();

        private void Wb_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HideElement(wb.Document.GetElementById("header"));
            foreach (HtmlElement item in wb.Document.GetElementsByTagName("menu"))
                HideElement(item);
            wb.AllowNavigation = false;
        }
        private void HideElement(HtmlElement? element)
        {
            if (element != null)
                element.Style = "display:none";
        }
        private void ShowWeatherMap()
        {
            Main.Visible = false;
            selectCity.Visible = false;
            menuStrip.Visible = false;
            this.Width = 753;
            this.Height = 596;
            string page = string.Format("https://www.ventusky.com/?p={0};{1};11&l=rain-1h", location.coordinate[0], location.coordinate[1]);
            wb.AllowNavigation = true;
            wb.Navigate(page);
            BrowserPanel.Visible = true;
            wb_BackButton.Visible = true;
            wb_RefreshButton.Visible = true;
        }
        private UserLocation GetCurrentUserCity()
        {
            WebRequest request = WebRequest.Create("https://ipinfo.io/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                return JsonConvert.DeserializeObject<UserLocation>(stream.ReadToEnd());
        }

        class UserLocation
        {
            public string city { get; set; }
            public string loc { get; set; }
            public string[] coordinate { get { return (loc.Split(",")).Select(i => Convert.ToDouble(i, CultureInfo.InvariantCulture).ToString().Replace(',', '.')).ToArray(); } }
        }
        private void GetWeatherData(bool second_try = false)
        {
            string uri = string.Format("http://api.weatherapi.com/v1/forecast.xml?key=91e291f9f50541eeaf2143550231712&q={0}&days=1&aqi=no&alerts=no", location.city);

            try
            {
                XDocument doc = XDocument.Load(uri);

                weather.maxTemp = (int)Math.Round(Convert.ToDouble(doc.Descendants("forecastday").FirstOrDefault().Descendants("day").FirstOrDefault().Descendants("maxtemp_c").FirstOrDefault().Value.Replace('.',',')));
                weather.minTemp = (int)Math.Round(Convert.ToDouble(doc.Descendants("forecastday").FirstOrDefault().Descendants("day").FirstOrDefault().Descendants("mintemp_c").FirstOrDefault().Value.Replace('.',',')));

                weather.sunRise = DateTime.Parse(doc.Descendants("astro").FirstOrDefault().Descendants("sunrise").FirstOrDefault().Value).ToString("HH:mm");
                weather.sunSet = DateTime.Parse(doc.Descendants("astro").FirstOrDefault().Descendants("sunset").FirstOrDefault().Value).ToString("HH:mm");

                var current = doc.Descendants("current");
                weather.currentTemp = (int)Math.Round(Convert.ToDouble(current.Descendants("temp_c").First().Value.Replace('.',',')));

                weather.pressure = (int)Math.Round(Convert.ToDouble(current.Descendants("pressure_mb").FirstOrDefault().Value.Replace(".", ",")) / 1.333);
                weather.humidity = Int32.Parse(current.Descendants("humidity").FirstOrDefault().Value);
                weather.feels = (int)Math.Round(Convert.ToDouble(current.Descendants("feelslike_c").FirstOrDefault().Value.Replace(".", ",")));
                weather.wind_speed = (int)Math.Round(Convert.ToDouble(current.Descendants("wind_kph").FirstOrDefault().Value.Replace(".", ","))/3.6);

                currentCloud.Image = Image.FromStream(new WebClient().OpenRead("http:"+current.Descendants("condition").First().Descendants("icon").First().Value));

                DayNight.Text = string.Format("Днем {0}°\nНочью {1}°", weather.maxTemp, weather.minTemp);
                Temperature.Text = weather.currentTemp + "°";
                SunRiseAndSet.Text = string.Format("Восход: {0}\nЗакат: {1}", weather.sunRise, weather.sunSet);
                AddInfo.Text = string.Format("Давление: {0} мм.рт.ст.\nВлажность: {1}%\nОщущается как {2}°\nСкорость ветра {3} м/с",
                    weather.pressure,
                    weather.humidity,
                    weather.feels,
                    weather.wind_speed);

                location.loc = doc.Descendants("location").FirstOrDefault().Descendants("lat").FirstOrDefault().Value + "," + doc.Descendants("location").FirstOrDefault().Descendants("lon").FirstOrDefault().Value;
                EnterCity.Text = "Город успешно выбран";
            }
            catch(HttpRequestException e)
            {
                EnterCity.Text = "Неверные данные";
                MessageBox.Show("Введенные данные неверны.\nПопробуйте указать другой город.\n\nКод ошибки: " + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                location = GetCurrentUserCity();
                ChangeCityButton(null,new EventArgs());
            }
            catch(Exception e)
            {
                EnterCity.Text = "Отствует подключение к интернету";
                if (!second_try)
                {
                    MessageBox.Show("Отсутствует подключение к интернету, либо погодный сервис недоступен\n\nКод ошибки: " + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GetWeatherData(true);
                    return;
                }
                Application.Exit();
            }
            return;

        }

        public struct Weather
        {
            public int minTemp;
            public int maxTemp;
            public int currentTemp;
            public string sunRise;
            public string sunSet;
            public int pressure;
            public int humidity;
            public int feels;
            public int wind_speed;
        }
    }
}
