using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace isItFreezing
{
    class ToggleLight
    {
        public static void checkTemp(int temperature, int zipcode, bool isFloridaFreezing, bool isColoradoFreezing, GpioPinValue pinValue, GpioPin pin)
        {
            if (temperature <= 32)
            {
                if (zipcode == 33709)
                {
                    isFloridaFreezing = true;
                }
                else
                {
                    isColoradoFreezing = true;
                }
                
            }
            else
            {
                if (zipcode == 33709)
                {
                    isFloridaFreezing = false;
                }
                else
                {
                    isColoradoFreezing = false;
                }
                
            }

            if (isFloridaFreezing || isColoradoFreezing)
            {
                if (pinValue == GpioPinValue.High)
                {
                    pinValue = GpioPinValue.Low;
                    pin.Write(pinValue);
                }
            }

        }
    }
}
