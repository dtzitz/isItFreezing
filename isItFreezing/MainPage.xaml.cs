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

        private int DELETEME = 35;

        private const int LED_PIN = 5;
        public GpioPin pin;
        private GpioPinValue pinValue;
        private bool isLightOn;

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                //uhh
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
            flHumidity.Text = "Humidity: " + (myWeather.main.humidity).ToString();
            flCondition.Text = myWeather.weather[0].description;

            //Florida Timer
            TimeSpan period = TimeSpan.FromMinutes(5);
            ThreadPoolTimer PerodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (Florida_Loaded) => {
                myWeather = await OpenWeatherMapProxy.GetWeatherAsync(zipcode);
                
                if ((int)myWeather.main.temp <= 32 && pinValue == GpioPinValue.High)
                {
                    pinValue = GpioPinValue.Low;
                    pin.Write(pinValue);
                }
                if ((int)myWeather.main.temp > 32 && pinValue == GpioPinValue.Low)
                {
                    
                    pinValue = GpioPinValue.High;
                    pin.Write(pinValue);
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
                    flHumidity.Text = "Humidity: " + (myWeather.main.humidity).ToString();
                    flCondition.Text = myWeather.weather[0].description;
                    flTimeUpdated.Text = "Last Updated on: " + DateTime.Now.ToShortTimeString();

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
            coHumidity.Text = "Humidity: " + (coWeather.main.humidity).ToString();
            coCondition.Text = coWeather.weather[0].description;

            //Refresh timer
            TimeSpan period = TimeSpan.FromMinutes(5);
            ThreadPoolTimer PerodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (Colorado_loaded) => {
                coWeather = await OpenWeatherMapProxy.GetWeatherAsync(zipcode);

                if ((int)coWeather.main.temp <= 32 && pinValue == GpioPinValue.High)
                {
                    pinValue = GpioPinValue.Low;
                    pin.Write(pinValue);
                }
                if ((int)coWeather.main.temp > 32 && pinValue == GpioPinValue.Low)
                {

                    pinValue = GpioPinValue.High;
                    pin.Write(pinValue);
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
                    coHumidity.Text = "Humidity: " + (coWeather.main.humidity).ToString();
                    coCondition.Text = coWeather.weather[0].description;
                    coTimeUpdated.Text = "Last Updated on: " + DateTime.Now.ToShortTimeString() + " | Colorado Time: " + DateTime.Now.AddHours(-2).ToShortTimeString();

                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }, period);
        }


    }
}
