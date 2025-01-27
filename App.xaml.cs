using DineSync.Data;
using DineSync.ViewModels;

namespace DineSync
{
    public partial class App : Application
    {
        private readonly DbConfig _DbConfig;
        public App(DbConfig dbConfig)
        {
            InitializeComponent();

            MainPage = new AppShell();

            _DbConfig = dbConfig;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await _DbConfig.InitializeDatabaseAsync();
        }

    }
}
