/*
 * Enums and types that are used commonly throughout the library
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using System;
using System.ComponentModel;
using System.Globalization;

namespace StompyBlondie.Common.Types
{
    /*
     * Four directions
     */
    public enum Direction
    {
        Down,
        Left,
        Up,
        Right
    };

    /*
     * Eight directions
     */
    public enum EightDirection
    {
        Down,
        DownLeft,
        Left,
        LeftUp,
        Up,
        UpRight,
        Right,
        RightDown
    };

    /*
     * A rotation in a direction
     */
    public enum RotationalDirection
    {
        Clockwise,
        AntiClockwise
    }

    /*
     * TypeConvertor for Pos objects so they can be explicity converted to and from a string representation.
     */
    internal class PosTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string)
            {
                var strVal = (string) value;
                foreach(var c in new []{"<", ">", " "})
                    strVal = strVal.Replace(c, string.Empty);
                var parts = strVal.Split(',');
                var pos = new Pos(
                    x:float.Parse(parts[0]),
                    y:float.Parse(parts[1]),
                    layer:float.Parse(parts[2])
                );
                return pos;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    /*
     * Generic representation of a position on a 3D tilemap
     */
    [TypeConverter(typeof(PosTypeConverter))]
    public struct Pos
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Layer;

        public Pos(float x, float y, float layer)
        {
            X = x;
            Y = y;
            Layer = layer;
        }

        public override string ToString()
        {
            return $"<{X}, {Y}, {Layer}>";
        }

        public static bool operator ==(Pos a, Pos b)
        {
            return (System.Math.Abs(a.X - b.X) < .005f) &&
                   (System.Math.Abs(a.Y - b.Y) < .005f) &&
                   (System.Math.Abs(a.Layer - b.Layer) < .005f);
        }

        public static bool operator !=(Pos a, Pos b)
        {
            return !(a == b);
        }

        public override bool Equals(System.Object obj)
        {
            return this == (Pos)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Layer.GetHashCode();
        }
    }

}