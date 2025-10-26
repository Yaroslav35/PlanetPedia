namespace PlanetPedia
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void planetb_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new planets());
        }

        private void moons_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new moons());
        }

        private void settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new settings());
        }

        private async void camera_Clicked(object sender, EventArgs e)
        {
            var cameraPermission = await CheckAndRequestPermission<Permissions.Camera>();
            if (cameraPermission == PermissionStatus.Granted) await Navigation.PushAsync(new camera());
        }
        public async Task<PermissionStatus> CheckAndRequestPermission<T>() where T : Permissions.BasePermission, new()
        {
            var status = await Permissions.CheckStatusAsync<T>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<T>();
            return status;
        }
    }
}
