//
// Copyright (c) 2010-2012 Frank A. Krueger
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Collections.Generic;
//using System.Drawing;

namespace CrossGraphics
{
	public interface IGraphics
	{
		void BeginEntity(object entity);

		void SetFont(Font f);

		void SetColor(Color c);

		void FillRect(float x,float y,float width, float height);

		void DrawRect(float x, float y, float width, float height, float w);

		void FillRoundedRect(float x, float y, float width, float height, float radius);

		void DrawRoundedRect(float x, float y, float width, float height, float radius, float w);

		void FillOval(float x, float y, float width, float height);

		void DrawOval(float x, float y, float width, float height, float w);

		void BeginLines(bool rounded);

		void DrawLine(float sx, float sy, float ex, float ey, float w);

		void EndLines();

		void FillArc(float cx, float cy, float radius, float startAngle, float endAngle);
		
		void DrawArc(float cx, float cy, float radius, float startAngle, float endAngle, float w);

		void DrawImage(IImage img, float x, float y);
		void DrawImage(IImage img, float x, float y, float width, float height);

		void DrawString(string s, 
			float x,
			float y,
			float width,
			float height,
			LineBreakMode lineBreak, 
			TextAlignment horizontal_align, 
			TextAlignment vertical_align
		);
		
		void SaveState();
		
		void SetClippingRect (float x, float y, float width, float height);
		
		void Translate(float dx, float dy);
		
		void Scale(float sx, float sy);
		
		void RestoreState();

		void BeginOffscreen (float width, float height, IImage prev);
		IImage EndOffscreen ();
	}

	public enum LineBreakMode {
		None,
		Clip,
		WordWrap,
	}

	public enum TextAlignment {
		Start,
		Center,
		End,
		//Justified
	}

	public interface IImage
	{
		void Destroy();
	}

	[Flags]
	public enum FontOptions
	{
		None = 0,
		Bold = 1,
		Italic = 2, // TODO
	}

	public class Font
	{
		public string FontFamily { get; private set; }
		
		public FontOptions Options { get; private set; }

		public int Size { get; private set; } // TODO why is this an int?

		public object Tag { get; set; }
		
		public bool IsBold { get { return (Options & FontOptions.Bold) != 0; } }

		public Font (string fontFamily, FontOptions options, int size)
		{
			FontFamily = fontFamily;
			Options = options;
			Size = size;
		}
		
		static Font[] _boldSystemFonts = new Font[0];
		static Font[] _systemFonts = new Font[0];
		static Font[] _userFixedPitchFonts = new Font[0];
		static Font[] _boldUserFixedPitchFonts = new Font[0];

		public static Font BoldSystemFontOfSize (int size) {
			if (size >= _boldSystemFonts.Length) {
				return new Font ("SystemFont", FontOptions.Bold, size);
			}
			else {
				var f = _boldSystemFonts[size];
				if (f == null) {
					f = new Font ("SystemFont", FontOptions.Bold, size);
					_boldSystemFonts[size] = f;
				}
				return f;
			}
		}
		public static Font SystemFontOfSize (int size) {
			if (size >= _systemFonts.Length) {
				return new Font ("SystemFont", FontOptions.None, size);
			}
			else {
				var f = _systemFonts[size];
				if (f == null) {
					f = new Font ("SystemFont", FontOptions.None, size);
					_systemFonts[size] = f;
				}
				return f;
			}
		}
		public static Font UserFixedPitchFontOfSize (int size) {
			if (size >= _userFixedPitchFonts.Length) {
				return new Font ("Monospace", FontOptions.None, size);
			}
			else {
				var f = _userFixedPitchFonts[size];
				if (f == null) {
					f = new Font ("Monospace", FontOptions.None, size);
					_userFixedPitchFonts[size] = f;
				}
				return f;
			}
		}
		public static Font BoldUserFixedPitchFontOfSize (int size) {
			if (size >= _boldUserFixedPitchFonts.Length) {
				return new Font ("Monospace", FontOptions.Bold, size);
			}
			else {
				var f = _boldUserFixedPitchFonts[size];
				if (f == null) {
					f = new Font ("Monospace", FontOptions.Bold, size);
					_boldUserFixedPitchFonts[size] = f;
				}
				return f;
			}
		}
		
		public static Font FromName (string name, int size) {
			return new Font (name, FontOptions.None, size);
		}

		public override string ToString()
		{
			return string.Format ("[Font: FontFamily={0}, Options={1}, Size={2}, Tag={3}]", FontFamily, Options, Size, Tag);
		}
	}

	public class Color
	{
		public readonly int Red, Green, Blue, Alpha;
		public object Tag;

		public float RedValue {
			get { return Red / 255.0f; }
		}

		public float GreenValue {
			get { return Green / 255.0f; }
		}

		public float BlueValue {
			get { return Blue / 255.0f; }
		}

		public float AlphaValue {
			get { return Alpha / 255.0f; }
		}

		public Color (int red, int green, int blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = 255;
		}

		public Color (int red, int green, int blue, int alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		public Color (double red, double green, double blue)
		{
			Red = (int) (red * 255);
			Green = (int) (green * 255);
			Blue = (int) (blue * 255);
			Alpha = 255;
		}

		public Color (double red, double green, double blue, double alpha)
		{
			Red = (int) (red * 255);
			Green = (int) (green * 255);
			Blue = (int) (blue * 255);
			Alpha = (int) (alpha * 255);
		}

		public int Intensity { get { return (Red + Green + Blue) / 3; } }

		public Color GetInvertedColor()
		{
			return new Color (255 - Red, 255 - Green, 255 - Blue, Alpha);
		}

		public static bool AreEqual(Color a, Color b)
		{
			if (a == null && b == null)
				return true;
			if (a == null && b != null)
				return false;
			if (a != null && b == null)
				return false;
			return (a.Red == b.Red && a.Green == b.Green && a.Blue == b.Blue && a.Alpha == b.Alpha);
		}

		public bool IsWhite {
			get { return (Red == 255) && (Green == 255) && (Blue == 255); }
		}

		public bool IsBlack {
			get { return (Red == 0) && (Green == 0) && (Blue == 0); }
		}

		public Color WithAlpha(int aa)
		{
			return new Color (Red, Green, Blue, aa);
		}

        public override bool Equals (object obj)
        {
            var o = obj as Color;
            return (o != null) && (o.Red == Red) && (o.Green == Green) && (o.Blue == Blue) && (o.Alpha == Alpha);
        }

        public override int GetHashCode ()
        {
            return (Red + Green + Blue + Alpha).GetHashCode ();
        }

		public override string ToString()
		{
			return string.Format ("[Color: RedValue={0}, GreenValue={1}, BlueValue={2}, AlphaValue={3}]", RedValue, GreenValue, BlueValue, AlphaValue);
		}
	}

	public static class Colors
	{
		public static readonly Color Yellow = new Color (255, 255, 0);
		public static readonly Color Red = new Color (255, 0, 0);
		public static readonly Color Green = new Color (0, 255, 0);
		public static readonly Color Blue = new Color (0, 0, 255);
		public static readonly Color White = new Color (255, 255, 255);
		public static readonly Color Cyan = new Color (0, 255, 255);
		public static readonly Color Black = new Color (0, 0, 0);
		public static readonly Color LightGray = new Color (212, 212, 212);
		public static readonly Color Gray = new Color (127, 127, 127);
		public static readonly Color DarkGray = new Color (64, 64, 64);
	}

}

