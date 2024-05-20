﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CVJoyMobile
{
    public class Gauge
    {
        public AbsoluteLayout absoluteLayout;
        BoxView needle;
        public int valueMin;
        public int valueMax;
        public int angleMin;
        public int angleMax;
        public int valueRedLine;
        public Double needleWidth;
        public enumGaugeRadiusSize radiusSize;
        public enumSpecialModes specialMode;

        public Gauge(AbsoluteLayout pAbsoluteLayout, enumSpecialModes pSpecialMode = enumSpecialModes.Normal)
        {
            absoluteLayout = pAbsoluteLayout;
            specialMode = pSpecialMode;
        }

        public enum enumGaugeRadiusSize
        {
            Fit,
            ExpandStart,
            ExpandCenter,
            ExpandEnd
        }

        public enum enumSpecialModes
        {
            Normal,
            BmwM2Speed
        }


        public void Init(int pValueMin, int pValueRedLine, int pValueMax, Color ticks1Color, Color ticks2Color, Color ticks3Color, Color needleColor, Double pNeedleWidth, int pAngleMin, int pAngleMax, enumGaugeRadiusSize pRadiusSize, Color needleCenterColor)
        {
            valueMin = pValueMin;
            valueRedLine = pValueRedLine;
            valueMax = pValueMax;
            angleMin = pAngleMin;
            angleMax = pAngleMax;
            needleWidth = pNeedleWidth;
            radiusSize = pRadiusSize;

            absoluteLayout.Children.Clear();

            // Get the center and radius of the AbsoluteLayout
            Point center = new Point(absoluteLayout.Width / 2, absoluteLayout.Height / 2);
            Double radius;
            if (radiusSize != enumGaugeRadiusSize.Fit)
            {
                if (absoluteLayout.Width > absoluteLayout.Height)
                {
                    radius = 0.5 * absoluteLayout.Width;
                    if (radiusSize == enumGaugeRadiusSize.ExpandStart)
                    {
                        center.Y = radius;
                    }
                    else if (radiusSize == enumGaugeRadiusSize.ExpandEnd)
                    {
                        center.Y = absoluteLayout.Height - radius;
                    }
                }
                else
                {
                    radius = 0.5 * absoluteLayout.Height;
                    if (radiusSize == enumGaugeRadiusSize.ExpandStart)
                    {
                        center.X = radius;
                    }
                    else if (radiusSize == enumGaugeRadiusSize.ExpandEnd)
                    {
                        center.X = absoluteLayout.Width - radius;
                    }
                }
            }
            else
            {
                radius = 0.5 * Math.Min(absoluteLayout.Width, absoluteLayout.Height);
            }

            int valueBigStep = 1000;
            if (pValueMax - valueMin <= 200)
            {
                valueBigStep = 10;
            }
            else if (pValueMax - valueMin < 500)
            {
                valueBigStep = 20;
            }

            Double bigStepsCount = (valueMax - valueMin) / valueBigStep;
            Double angleBigStep = (angleMax - angleMin) / bigStepsCount;
            int redLineAngle = angleMin + (int)((double)(pValueRedLine - valueMin) / (valueMax - valueMin) * (angleMax - angleMin));

            int value = valueMin;
            for (Double angle = angleMin; angle <= angleMax + 5; angle += angleBigStep)
            {
                // big ticks:
                if (ticks1Color != Color.Transparent)
                {
                    Double x = center.X + radius * Math.Sin(angle / 180 * Math.PI);
                    Double y = center.Y - radius * Math.Cos(angle / 180 * Math.PI);
                    absoluteLayout.Children.Add(new BoxView
                    {
                        AnchorY = 0,
                        TranslationX = x,
                        TranslationY = y,
                        WidthRequest = 3,
                        HeightRequest = 30,
                        Rotation = angle,
                        Color = value >= pValueRedLine && pValueRedLine != valueMax ? Color.Red : ticks1Color
                    });
                    // text:
                    const int textSize = 25;
                    Double textX = center.X + (radius - 30 - 5 - textSize) * Math.Sin(angle / 180 * Math.PI);
                    Double textY = center.Y - (radius - 30 - 5 - textSize) * Math.Cos(angle / 180 * Math.PI);
                    absoluteLayout.Children.Add(new Label
                    {
                        TranslationX = textX - textSize * value.ToString().Length / 3.8,
                        TranslationY = textY - textSize / 1.5,
                        TextColor = value >= pValueRedLine && pValueRedLine != valueMax ? Color.Red : Color.White,
                        FontSize = textSize,
                        Text = value.ToString()
                    });
                    //absoluteLayout.Children.Add(new BoxView
                    //{
                    //    TranslationX = textX ,
                    //    TranslationY = textY ,
                    //    WidthRequest = 2,
                    //    HeightRequest = 2,
                    //    Color = Color.Pink
                    //});
                }

                if (ticks2Color != Color.Transparent)
                {
                    if (angle + 2 < angleMax)
                    {
                        // small ticks:
                        Double subAngle = angle + angleBigStep * .5;
                        Double subX = center.X + radius * Math.Sin(subAngle / 180 * Math.PI);
                        Double subY = center.Y - radius * Math.Cos(subAngle / 180 * Math.PI);
                        absoluteLayout.Children.Add(new BoxView
                        {
                            AnchorY = 0,
                            TranslationX = subX,
                            TranslationY = subY,
                            WidthRequest = 2,
                            HeightRequest = 20,
                            Rotation = subAngle,
                            Color = subAngle > redLineAngle ? Color.Red : ticks2Color
                        });
                    }

                    if (ticks3Color != Color.Transparent)
                    {
                        // tiny ticks:
                        Double subAngle = angle + angleBigStep * .25;
                        Double subX = center.X + radius * Math.Sin(subAngle / 180 * Math.PI);
                        Double subY = center.Y - radius * Math.Cos(subAngle / 180 * Math.PI);
                        absoluteLayout.Children.Add(new BoxView
                        {
                            AnchorY = 0,
                            TranslationX = subX,
                            TranslationY = subY,
                            WidthRequest = 1,
                            HeightRequest = 10,
                            Rotation = subAngle,
                            Color = subAngle > redLineAngle ? Color.Red : ticks3Color
                        });
                        subAngle = angle + angleBigStep * .75;
                        subX = center.X + radius * Math.Sin(subAngle / 180 * Math.PI);
                        subY = center.Y - radius * Math.Cos(subAngle / 180 * Math.PI);
                        absoluteLayout.Children.Add(new BoxView
                        {
                            AnchorY = 0,
                            TranslationX = subX,
                            TranslationY = subY,
                            WidthRequest = 1,
                            HeightRequest = 10,
                            Rotation = subAngle,
                            Color = subAngle > redLineAngle ? Color.Red : ticks3Color
                        });
                    }
                }

                value += valueBigStep;
            }

            // red line:
            if (pValueRedLine < valueMax)
            {
                Double redLineX = center.X + radius * Math.Sin((double)redLineAngle / 180 * Math.PI);
                Double redLineY = center.Y - radius * Math.Cos((double)redLineAngle / 180 * Math.PI);
                absoluteLayout.Children.Add(new BoxView
                {
                    AnchorY = 0,
                    TranslationX = redLineX,
                    TranslationY = redLineY,
                    WidthRequest = 3,
                    HeightRequest = radius * .75,
                    Rotation = redLineAngle,
                    Color = Color.Red
                });
            }

            // needle:
            int needleCenterRadius = 7 + (int)(needleWidth * 2);
            Button needleCenterBox = new Button
            {
                TranslationX = center.X - needleCenterRadius,
                TranslationY = center.Y - needleCenterRadius,
                WidthRequest = needleCenterRadius * 2,
                HeightRequest = needleCenterRadius * 2,
                CornerRadius = needleCenterRadius,
                BackgroundColor = needleCenterColor
            };
            absoluteLayout.Children.Add(needleCenterBox);
            needleCenterBox.Clicked += NeedleCenterBox_Clicked;

            const Double needleOffset = 0.85;
            Double needleHeight = .9 * radius;
            needle = new BoxView
            {
                AnchorY = needleOffset,
                TranslationX = center.X - .5 * needleWidth,
                TranslationY = center.Y - needleOffset * needleHeight,
                WidthRequest = needleWidth,
                HeightRequest = needleHeight,
                Rotation = angleMin,
                Color = needleColor,
                CornerRadius = 3
            };
            absoluteLayout.Children.Add(needle);
        }

        private void NeedleCenterBox_Clicked(object sender, EventArgs e)
        {
            PageGaugeProps detailPage = new PageGaugeProps(this);
            absoluteLayout.Navigation.PushModalAsync(detailPage);
        }

        public void needleValue(int value)
        {
            if (specialMode == enumSpecialModes.BmwM2Speed)
            {
                int rotation ;
                if (value <= 120)
                {
                    rotation = angleMin + (int)((double)(value ) / 120 * (angleMax - angleMin)/2);
                }
                else
                {
                    rotation = angleMin + (angleMax - angleMin) / 2 + (int)((double)(value - 120) / (valueMax - 120) * (angleMax - angleMin)/2);
                }
                needle.Rotation = rotation;
            }
            else
            {
                int rotation = angleMin + (int)((double)(value - valueMin) / (valueMax - valueMin) * (angleMax - angleMin));
                needle.Rotation = rotation;
            }
        }
    }
}