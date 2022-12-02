using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Android7
{

    public class ClockViewModel : INotifyPropertyChanged
    {
        private UInt16 speed;
        private String gear;

        public event PropertyChangedEventHandler PropertyChanged;

        public ClockViewModel()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(91), () =>
            {
                this.Speed = Convert.ToUInt16(DateTime.Now.Millisecond);
                this.Gear = (DateTime.Now.Second % 7).ToString();
                return true;
            });
        }

        public UInt16 Speed
        {
            set
            {
                if (speed != value)
                {
                    speed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Speed"));
                }
            }
            get
            {
                return speed;
            }
        }

        public String Gear
        {
            set
            {
                if (gear != value)
                {
                    gear = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Gear"));
                }
            }
            get
            {
                return gear;
            }
        }

    }

}
