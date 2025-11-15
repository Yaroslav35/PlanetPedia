using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace PlanetPedia
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Безопасная настройка аппаратного ускорения
            SetHardwareAcceleration();
        }

        private void SetHardwareAcceleration()
        {
            try
            {
                // Проверяем, что Window не null
                if (Window != null)
                {
                    // Устанавливаем флаги только если Window доступен
                    Window.SetFlags(
                        WindowManagerFlags.HardwareAccelerated,
                        WindowManagerFlags.HardwareAccelerated
                    );

                    // Дополнительные настройки для видео
                    Window.SetFormat(Android.Graphics.Format.Rgba8888);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Hardware acceleration error: {ex.Message}");
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            SetHardwareAcceleration(); // Повторно при возобновлении
        }
    }
}
