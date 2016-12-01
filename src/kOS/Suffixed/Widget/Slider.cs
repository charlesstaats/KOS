﻿using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;

namespace kOS.Suffixed
{
    [kOS.Safe.Utilities.KOSNomenclature("Slider")]
    public class Slider : Widget
    {
        private bool horizontal { get; set; }
        private float value { get; set; }
        private float valueVisible { get; set; }
        private float min { get; set; }
        private float max { get; set; }
        private GUIStyle thumbStyle;

        public Slider(Box parent, bool h_not_v, float v, float from, float to) : base(parent)
        {
            RegisterInitializer(InitializeSuffixes);
            horizontal = h_not_v;
            value = v;
            min = from;
            max = to;
            if (horizontal) { SetStyle.margin.top = 8; SetStyle.margin.bottom = 8; } // align better with labels.
            thumbStyle = new GUIStyle(horizontal ? HighLogic.Skin.horizontalSliderThumb : HighLogic.Skin.verticalSliderThumb);
        }

        protected override GUIStyle BaseStyle() { return horizontal ? HighLogic.Skin.horizontalSlider : HighLogic.Skin.verticalSlider; }

        private void InitializeSuffixes()
        {
            AddSuffix("VALUE", new SetSuffix<ScalarValue>(() => value, v => { if (value != v) { value = v; Communicate(() => valueVisible = v); } }));
            AddSuffix("MIN", new SetSuffix<ScalarValue>(() => min, v => min = v));
            AddSuffix("MAX", new SetSuffix<ScalarValue>(() => max, v => max = v));
        }

        public override void DoGUI()
        {
            float newvalue;
            if (horizontal)
                newvalue = GUILayout.HorizontalSlider(valueVisible, min, max, Style, thumbStyle);
            else
                newvalue = GUILayout.VerticalSlider(valueVisible, min, max, Style, thumbStyle);
            if (newvalue != valueVisible) {
                valueVisible = newvalue;
                Communicate(() => value = newvalue);
            }
        }

        public override string ToString()
        {
            return string.Format("SLIDER({0:0.00})",value);
        }
    }
}
