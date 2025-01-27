using DineSync.Controls;
using DineSync.Data;

namespace DineSync
{
    public partial class App : Application
    {
        private readonly DbConfig _DbConfig;
        public App(DbConfig dbConfig)
        {
            InitializeComponent();

            MainPage = new AppShell();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            _DbConfig = dbConfig;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await _DbConfig.InitializeDatabaseAsync();
        }

    }
}
