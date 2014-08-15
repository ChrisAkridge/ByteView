using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteView
{
    public enum BitDepth
    {
        Invalid,
        OneBpp,
        TwoBpp,
        FourBpp,
        EightBpp,
        SixteenBpp,
        TwentyFourBpp,
        ThirtyTwoBpp,
    }

    public enum ColorMode
    {
        Invalid,
        Grayscale,
        RGB,
        ARGB,
        Paletted
    }
}
