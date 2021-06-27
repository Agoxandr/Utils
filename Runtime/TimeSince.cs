// https://garry.tv/timesince

//TimeSince ts;

//void Start()
//{
//    ts = 0;
//}

//void Update()
//{
//    if (ts > 10)
//    {
//        DoSomethingAfterTenSeconds();
//    }
//}

using UnityEngine;

namespace Agoxandr.Utils
{
    public struct TimeSince
    {
        private float time;

        public static implicit operator float(TimeSince ts)
        {
            return Time.time - ts.time;
        }

        public static implicit operator TimeSince(float ts)
        {
            return new TimeSince { time = Time.time - ts };
        }
    }
}
