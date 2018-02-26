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
        public static void checkTemp(int temperature, int zipcode, bool isSourKrautAwake, ref bool isFloridaFreezing, ref bool isColoradoFreezing, ref GpioPinValue pinValue, GpioPin pin)
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
                    //isFloridaFreezing = false;
                    if (isFloridaFreezing)
                    {
                        isFloridaFreezing = false;
                    }
                    else
                    {
                        isFloridaFreezing = true;
                    }
                }
                else
                {
                    isColoradoFreezing = false;
                }
                
            }

            if (!isSourKrautAwake)
            {
                if (isFloridaFreezing || isColoradoFreezing)
                {
                    if (pinValue == GpioPinValue.High)
                    {
                        pinValue = GpioPinValue.Low;
                        pin.Write(pinValue);
                    }
                }
                else
                {
                    pinValue = GpioPinValue.High;
                    pin.Write(pinValue);
                }
            }
            else
            {
                pinValue = GpioPinValue.High;
                pin.Write(pinValue);
            }

        }
    }
}
