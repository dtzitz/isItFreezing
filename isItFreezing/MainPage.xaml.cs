using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static isItFreezing.OpenWeatherProxy;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace isItFreezing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitGPIO();
            
        }

        private bool isColoradoFreezing;
        private bool isFloridaFreezing;
        private bool isPiConnected = true;
        private bool isSourKrautAwake = false;

        private const int LED_PIN = 5;
        public GpioPin pin;
        private GpioPinValue pinValue;

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                isPiConnected = false;
                return;
            }

            pin = gpio.OpenPin(LED_PIN);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);

        }


        private async void Florida_Loaded(object sender, RoutedEventArgs e)
        {
            int zipcode = 33709;
            RootObject myWeather = await OpenWeatherMapProxy.GetWeatherAsync(zipcode);

            //make the background
            string flWeatherIcon = String.Format("ms-appx:///Assets/Florida/{0}.png", myWeather.weather[0].icon);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(flWeatherIcon, UriKind.Absolute));
            ib.Stretch = Stretch.UniformToFill;
            Florida.Background = ib;

            //weather info
            flName.Text = myWeather.name;
            flTemp.Text = "Temp: " + ((int)myWeather.main.temp).ToString()+ " F";
            flCondition.Text = myWeather.weather[0].description;

            //sunrise and sunset
            var sunrise = DateTimeOffset.FromUnixTimeSeconds(myWeather.sys.sunrise);
            sunrise += TimeSpan.FromHours(-5);
            var sunset = DateTimeOffset.FromUnixTimeSeconds(myWeather.sys.sunset);
            sunset += TimeSpan.FromHours(-5);
            flSunrise.Text = "sunrise: " + sunrise.ToString("hh':'mm") + " AM" ;
            flSunset.Text = "sunset: " + sunset.ToString("hh':'mm") + " PM";

            //Florida Timer
            TimeSpan period = TimeSpan.FromMinutes(10);
            ThreadPoolTimer PerodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (Florida_Loaded) => {
                myWeather = await OpenWeatherMapProxy.GetWeatherAsync(zipcode);

                var moment = DateTime.Now;
                if (moment.Hour > 22 || moment.Hour < 6)
                {
                    isSourKrautAwake = true;
                }

                if (isPiConnected && !isSourKrautAwake)
                {
                    int _flCurrentTemp = (int)myWeather.main.temp;
                    if (_flCurrentTemp <= 32)
                    {
                        isFloridaFreezing = true;
                    }
                    else
                    {
                        isFloridaFreezing = false;
                    }

                    if (isFloridaFreezing && pinValue == GpioPinValue.High)
                    {
                        pinValue = GpioPinValue.Low;
                        pin.Write(pinValue);
                    }
                    if (!isFloridaFreezing && pinValue == GpioPinValue.Low)
                    {
                        if (!isColoradoFreezing)
                        {
                            pinValue = GpioPinValue.High;
                            pin.Write(pinValue);
                        }

                    }

                }


#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {


                    //make the background
                    flWeatherIcon = String.Format("ms-appx:///Assets/Florida/{0}.png", myWeather.weather[0].icon);
                    ib.ImageSource = new BitmapImage(new Uri(flWeatherIcon, UriKind.Absolute));
                    ib.Stretch = Stretch.UniformToFill;
                    Florida.Background = ib;

                    //weather info
                    flName.Text = myWeather.name;
                    flTemp.Text = "Temp: " + ((int)myWeather.main.temp).ToString() + " F";
                    flCondition.Text = myWeather.weather[0].description;
                    flTimeUpdated.Text = "Last Updated on: " + DateTime.Now.ToShortTimeString();

                    //sunrise and sunset
                    sunrise = DateTimeOffset.FromUnixTimeSeconds(myWeather.sys.sunrise);
                    sunrise += TimeSpan.FromHours(-5);
                    sunset = DateTimeOffset.FromUnixTimeSeconds(myWeather.sys.sunset);
                    sunset += TimeSpan.FromHours(-5);
                    flSunrise.Text = "sunrise: " + sunrise.ToString("hh':'mm") + " AM";
                    flSunset.Text = "sunset: " + sunset.ToString("hh':'mm") + " PM";

                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }, period);


        }

        private async void Colorado_Loaded(object sender, RoutedEventArgs e)
        {
            int zipcode = 80918;
            RootObject coWeather = await OpenWeatherMapProxy.GetWeatherAsync(zipcode);

            //make the background
            string coWeatherIcon = String.Format("ms-appx:///Assets/Colorado/{0}.png", coWeather.weather[0].icon);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(coWeatherIcon, UriKind.Absolute));
            ib.Stretch = Stretch.UniformToFill;
            Colorado.Background = ib;

            //weather info
            coName.Text = coWeather.name;
            coTemp.Text = "Temp: " + ((int)coWeather.main.temp).ToString() + " F";
            coCondition.Text = coWeather.weather[0].description;

            //sunrise and sunset
            var sunrise = DateTimeOffset.FromUnixTimeSeconds(coWeather.sys.sunrise);
            sunrise += TimeSpan.FromHours(-7);
            var sunset = DateTimeOffset.FromUnixTimeSeconds(coWeather.sys.sunset);
            sunset += TimeSpan.FromHours(-7);
            coSunrise.Text = "sunrise: " + sunrise.ToString("hh':'mm") + " AM";
            coSunset.Text = "sunset: " + sunset.ToString("hh':'mm") + " PM";

            //Colorado timer
            TimeSpan period = TimeSpan.FromMinutes(20);
            ThreadPoolTimer PerodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (Colorado_loaded) => {
                coWeather = await OpenWeatherMapProxy.GetWeatherAsync(zipcode);

                var moment = DateTime.Now;
                if (moment.Hour > 22 || moment.Hour < 6)
                {
                    isSourKrautAwake = true;
                }

                if (isPiConnected && !isSourKrautAwake)
                {
                    int _coCurrentTemp = (int)coWeather.main.temp;
                    if (_coCurrentTemp <= 32)
                    {
                        isColoradoFreezing = true;
                    }
                    else
                    {
                        isColoradoFreezing = false;
                    }

                    if (isColoradoFreezing && pinValue == GpioPinValue.High)
                    {
                        pinValue = GpioPinValue.Low;
                        pin.Write(pinValue);
                    }
                    if (!isColoradoFreezing && pinValue == GpioPinValue.Low)
                    {
                        if (!isFloridaFreezing)
                        {
                            pinValue = GpioPinValue.High;
                            pin.Write(pinValue);
                        }
                    }
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {

                    //make the background
                    coWeatherIcon = String.Format("ms-appx:///Assets/Colorado/{0}.png", coWeather.weather[0].icon);
                    ib.ImageSource = new BitmapImage(new Uri(coWeatherIcon, UriKind.Absolute));
                    ib.Stretch = Stretch.UniformToFill;
                    Colorado.Background = ib;

                    //weather info
                    coName.Text = coWeather.name;
                    coTemp.Text = "Temp: " + ((int)coWeather.main.temp).ToString() + " F";
                    coCondition.Text = coWeather.weather[0].description;
                    coTimeUpdated.Text = "Last Updated on: " + DateTime.Now.ToShortTimeString() + " | Colorado Time: " + DateTime.Now.AddHours(-2).ToShortTimeString();

                    //sunrise and sunset
                    sunrise = DateTimeOffset.FromUnixTimeSeconds(coWeather.sys.sunrise);
                    sunrise += TimeSpan.FromHours(-7);
                    sunset = DateTimeOffset.FromUnixTimeSeconds(coWeather.sys.sunset);
                    sunset += TimeSpan.FromHours(-7);
                    coSunrise.Text = "sunrise: " + sunrise.ToString("hh':'mm") + " AM";
                    coSunset.Text = "sunset: " + sunset.ToString("hh':'mm") + " PM";

                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }, period);
        }


    }
}
