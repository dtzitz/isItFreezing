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
        public static void checkTemp(int temperature, bool isFloridaFreezing, bool isColoradoFreezing, GpioPinValue pinValue, GpioPin pin)
        {
            if (temperature <= 32)
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
    }
}
