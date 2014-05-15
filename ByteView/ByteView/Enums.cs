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
        FourBpp = 4,
        EightBpp = 8,
        SixteenBpp = 16,
        TwentyFourBpp = 24,
        ThirtyTwoBpp = 32
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
