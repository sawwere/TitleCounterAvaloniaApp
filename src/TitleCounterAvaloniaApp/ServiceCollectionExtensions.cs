using Microsoft.Extensions.DependencyInjection;
using tc.Repository;
using tc.Service;
using tc.ViewModels;
using tc.Views;

namespace tc
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection
                .AddSingleton<IGameRepository, RestGameRepository>()
                .AddSingleton<IFilmRepository, RestFilmRepository>()
                .AddSingleton<GameService>()
                .AddSingleton<FilmService>()
                .AddSingleton<AuthService>()
                .AddSingleton<UserService>()
                .AddSingleton<RestApiClient>();

            collection
                .AddSingleton<MainViewModel>()
                .AddSingleton<HomePageViewModel>()
                .AddSingleton<GamesPageViewModel>()
                .AddTransient<FilmsPageViewModel>()
                .AddTransient<LoginPageViewModel>();

            collection
                .AddSingleton<MainWindow>()
                .AddTransient<HomePageView>()
                .AddTransient<SearchView>()
                .AddSingleton<GamesPageView>()
                .AddSingleton<FilmsPageView>()
                .AddSingleton<LoginPageView>();
        }
    }
}
