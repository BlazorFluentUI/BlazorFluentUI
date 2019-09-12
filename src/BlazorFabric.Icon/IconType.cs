using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public enum IconType
    {
        /// <summary>
        /// Render using the fabric icon font.
        /// </summary>
        Default = 0,

        /// <summary>
        ///  Render using an image, where imageProps would be used.
        /// </summary>
        Image = 1,
    }
}
