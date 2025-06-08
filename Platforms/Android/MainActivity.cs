using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AI_Translator_Mobile_App
{
    [Activity(
        //Theme = "@style/Maui.SplashTheme",  // Comment out this line
        Theme = "@style/Maui.MainTheme.NoActionBar",  // Use this instead
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : MauiAppCompatActivity
    {
    }

}
