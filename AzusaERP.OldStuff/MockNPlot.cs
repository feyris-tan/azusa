﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AzusaERP.OldStuff;

namespace AzusaERP.OldStuff
{
    class DateTimeAxis
    {
        private DateTime date;
        private DateTime dateTime;

        public DateTimeAxis(DateTime date, DateTime dateTime)
        {
            this.date = date;
            this.dateTime = dateTime;
        }
    }

    class LinearAxis
    {
        private int v1;
        private int v2;

        public LinearAxis(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }

    class PointD
    {
        private long ticks;
        private double v;

        public PointD(long ticks, double v)
        {
            this.ticks = ticks;
            this.v = v;
        }
    }

    class ArrowItem
    {
        private PointD pt;
        private double angle;
        private string msg;

        public ArrowItem(PointD pt, double angle, string msg)
        {
            this.pt = pt;
            this.angle = angle;
            this.msg = msg;
        }
    }

    class LinePlot
    {
        private double[] yValues;
        private DateTime[] xValues;

        public LinePlot(double[] yValues, DateTime[] xValues)
        {
            this.yValues = yValues;
            this.xValues = xValues;
        }
    }
}

namespace NPlot.Windows
{
    class PlotSurface2D : Control
    {
        public void Add(ArrowItem annotation)
        {
            throw new NotImplementedException();
        }

        public bool AutoScaleAutoGeneratedAxes { get; set; }
        public bool AutoScaleTitle { get; set; }
        public bool DateTimeToolTip { get; set; }
        public object Legend { get; set; }
        public int LegendZOrder { get; set; }
        public object RightMenu { get; set; }
        public bool ShowCoordinates { get; set; }
        public SmoothingMode SmoothingMode { get; set; }
        public string Title { get; set; }
        public Font TitleFont { get; set; }
        public object XAxis1 { get; set; }
        public object XAxis2 { get; set; }
        public object YAxis1 { get; set; }
        public object YAxis2 { get; set; }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Add(LinePlot glucosePlot)
        {
            throw new NotImplementedException();
        }
    }
}