using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Models
{
    class PrijsModel
    {
        public string TweeD { get; set; }
        public string TweeDVip { get; set; }
        public string DrieD { get; set; }
        public string DrieDVip { get; set; }
        public string ImaxTweeD { get; set; }
        public string ImaxTweeDVip { get; set; }
        public string ImaxDrieD { get; set; }
        public string ImaxDrieDVip { get; set; }

        public PrijsModel() { }
        public PrijsModel(string tweeD, string tweeDVip, string drieD, string drieDVip, string imaxTweeD, string imaxTweeDVip, string imaxDrieD, string imaxDrieDVip)
        {
            TweeD = tweeD;
            TweeDVip = tweeDVip;
            DrieD = drieD;
            DrieDVip = drieDVip;
            ImaxTweeD = imaxTweeD;
            ImaxTweeDVip = imaxTweeDVip;
            ImaxDrieD = imaxDrieD;
            ImaxDrieDVip = imaxDrieDVip;
        }

    }
}
