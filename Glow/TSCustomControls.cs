// ======================================================================================================
// Türkay Software - C# Custom Graphics UI Library
// Library Version: v26.2
// © Eray Türkay
// ======================================================================================================

// ======================================================================================================
// Current TS Custom Graphics UI Library Control Items
// ---------------------------------------------------------
// - Button
// - CheckBox
// - DateTimePicker
// - FlowLayoutPanel
// - ListBox
// - RadioButton
// - TrackBar
// ======================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Glow
{

    #region TS Custom Button
    public class TSCustomButton : Button
    {
        // Fields
        private int borderSize = 0;
        private int borderRadius = 5;
        // Colors
        private Color borderColor = Color.DodgerBlue;
        // Properties
        [Category("TS Appearance")]
        [Description("Gets or sets the width of the control's border. Must be 0 or a positive value.")]
        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = Math.Max(0, value);
                Invalidate();
            }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the radius of the control's corners. A higher value makes the corners more rounded.")]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the control's border.")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the control.")]
        public Color BackgroundColor
        {
            get => BackColor;
            set => BackColor = value;
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the text displayed on the control.")]
        public Color TextColor
        {
            get => ForeColor;
            set => ForeColor = value;
        }
        // Constructor
        public TSCustomButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            BackColor = Color.DodgerBlue;
            ForeColor = Color.White;
            Resize += Button_Resize;
        }
        private void Button_Resize(object sender, EventArgs e)
        {
            if (BorderRadius > Height)
                BorderRadius = Height;
        }
        private float ScaleFactor => DeviceDpi / 96f;
        // Graphics
        private GraphicsPath GetFigurePath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2f;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Parent == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int scaledBorderSize = (int)(borderSize * ScaleFactor);
            int scaledBorderRadius = (int)(borderRadius * ScaleFactor);
            Rectangle rectSurface = ClientRectangle;
            RectangleF rectBorder = new RectangleF(scaledBorderSize / 2f, scaledBorderSize / 2f, rectSurface.Width - scaledBorderSize, rectSurface.Height - scaledBorderSize);
            int smoothSize = scaledBorderSize > 0 ? scaledBorderSize : 2;
            if (scaledBorderRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, scaledBorderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(Rectangle.Round(rectBorder), Math.Max(1, scaledBorderRadius - scaledBorderSize)))
                using (Pen penSurface = new Pen(Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, scaledBorderSize))
                {
                    this.Region = new Region(pathSurface);
                    g.DrawPath(penSurface, pathSurface);
                    if (scaledBorderSize >= 1)
                    {
                        g.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                this.Region = new Region(rectSurface);
                if (scaledBorderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, scaledBorderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        g.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
                }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (Parent != null)
                Parent.BackColorChanged += Container_BackColorChanged;
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (Parent != null)
                Parent.BackColorChanged -= Container_BackColorChanged;
            base.OnHandleDestroyed(e);
        }
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom CheckBox
    public class TSCustomCheckBox : CheckBox
    {
        [Category("TS Appearance")]
        [Description("The color used when the control is checked.")]
        public Color CheckedColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("The color of the check mark inside the control.")]
        public Color CheckMarkColor { get; set; } = Color.White;
        [Category("TS Appearance")]
        [Description("The background color of the control when it is unchecked.")]
        public Color UncheckedBackColor { get; set; } = Color.Transparent;
        [Category("TS Appearance")]
        [Description("Determines whether the unchecked background fill is drawn.")]
        public bool DrawUncheckedFill { get; set; } = false;
        [Category("TS Appearance")]
        [Description("The thickness of the control's border.")]
        public float BorderThickness { get; set; } = 2f;
        [Category("TS Appearance")]
        [Description("The border color of the control when it is unchecked.")]
        public Color UncheckedBorderColor { get; set; } = Color.Gray;
        [Category("TS Appearance")]
        [Description("The radius of the control's border corners.")]
        public float BorderRadius { get; set; } = 2f;
        [Category("TS Appearance")]
        [Description("The maximum allowable thickness for the control's border.")]
        public float MaxBorderThickness { get; set; } = 4f;
        [Category("TS Appearance")]
        [Description("The maximum allowable radius for the control's border corners.")]
        public float MaxBorderRadius { get; set; } = 8f;
        public TSCustomCheckBox()
        {
            AutoSize = true;
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
            PerformLayout();
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            float dpi = DeviceDpi / 96f;
            int boxSize = (int)(16 * dpi);
            int padding = (int)(6 * dpi);
            int margin = 2;
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            Size textSize = TextRenderer.MeasureText(Text, Font, new Size(int.MaxValue, int.MaxValue), flags);
            int width = textSize.Width + boxSize + padding + (margin * 2);
            int height = Math.Max(textSize.Height, boxSize) + 4;
            return new Size(width, height);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            float dpi = DeviceDpi / 96f;
            float boxSize = 16f * dpi;
            float padding = 6f * dpi;
            float margin = 2f;
            bool checkOnRight = CheckAlign == ContentAlignment.MiddleRight || CheckAlign == ContentAlignment.TopRight || CheckAlign == ContentAlignment.BottomRight;
            float boxY = (Height - boxSize) / 2f;
            float boxX;
            Rectangle textRect;
            if (checkOnRight)
            {
                boxX = Width - boxSize - margin;
                textRect = new Rectangle(0, 0, (int)(Width - boxSize - padding - margin), Height);
            }
            else
            {
                boxX = margin;
                int textX = (int)(boxSize + padding + margin);
                textRect = new Rectangle(textX, 0, Width - textX, Height);
            }
            RectangleF boxRect = new RectangleF(boxX, boxY, boxSize, boxSize);
            float radius = Math.Min(BorderRadius, MaxBorderRadius) * dpi;
            radius = Math.Min(radius, boxSize / 2f);
            if (Checked || (DrawUncheckedFill && UncheckedBackColor.A > 0))
            {
                Color fillColor = !Enabled ? SystemColors.Control : (Checked ? CheckedColor : UncheckedBackColor);
                using (var path = GetRoundedRectPath(boxRect, radius))
                using (var brush = new SolidBrush(fillColor))
                    g.FillPath(brush, path);
            }
            float border = Math.Min(BorderThickness, MaxBorderThickness) * dpi;
            if (border > 0.5f)
            {
                RectangleF borderRect = new RectangleF(boxRect.X + border / 2f, boxRect.Y + border / 2f, boxRect.Width - border, boxRect.Height - border);
                Color borderColor = !Enabled ? SystemColors.ControlDark : (Checked ? CheckedColor : UncheckedBorderColor);
                using (var path = GetRoundedRectPath(borderRect, radius))
                using (var pen = new Pen(borderColor, border))
                    g.DrawPath(pen, path);
            }
            if (Checked && Enabled)
            {
                using (var pen = new Pen(CheckMarkColor, boxRect.Width * 0.18f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    pen.LineJoin = LineJoin.Round;
                    float cx = boxRect.Left + boxRect.Width / 2f;
                    float cy = boxRect.Top + boxRect.Height / 2f;
                    float s = boxRect.Width * 0.5f;
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx - s * 0.5f, cy),
                        new PointF(cx - s * 0.1f, cy + s * 0.4f),
                        new PointF(cx + s * 0.5f, cy - s * 0.4f)
                    });
                }
            }
            TextFormatFlags textFlags = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter;
            textFlags |= checkOnRight ? TextFormatFlags.Right : TextFormatFlags.Left;
            TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? ForeColor : SystemColors.GrayText, textFlags);
        }
        private GraphicsPath GetRoundedRectPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.5f)
            {
                path.AddRectangle(rect);
                return path;
            }
            float d = radius * 2f;
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom ComboBox
    public class TSCustomComboBox : ComboBox
    {
        // Colors
        private Color _backColor = SystemColors.Window;
        private Color _foreColor = SystemColors.WindowText;
        private Color _buttonColor = SystemColors.ControlDark;
        private Color _arrowColor = SystemColors.WindowText;
        private Color _borderColor = SystemColors.ControlDark;
        //
        private Color _disabledBackColor = SystemColors.Control;
        private Color _disabledForeColor = SystemColors.GrayText;
        private Color _disabledButtonColor = SystemColors.ControlDark;
        private Color _disabledArrowColor = SystemColors.GrayText;
        //
        private Color _focusedBorderColor = Color.DodgerBlue;
        private Color _hoverBackColor = SystemColors.Window;
        private Color _hoverForeColor = SystemColors.WindowText;
        private Color _hoverButtonColor = SystemColors.ControlDark;
        //
        private bool _isHovering;
        //
        public TSCustomComboBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            MouseEnter += (s, e) => { _isHovering = true; Invalidate(); };
            MouseLeave += (s, e) => { _isHovering = false; Invalidate(); };
            DrawItem += TSCustomComboBox_DrawItem;
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the control.")]
        public override Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                base.BackColor = value;
                Invalidate();
            }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the foreground (text) color of the control.")]
        public override Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                base.ForeColor = value;
                Invalidate();
            }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the border color of the control.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the button portion of the control.")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set { _buttonColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the arrow displayed in the control.")]
        public Color ArrowColor
        {
            get => _arrowColor;
            set { _arrowColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the arrow when the control is disabled.")]
        public Color DisabledArrowColor
        {
            get => _disabledArrowColor;
            set { _disabledArrowColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the background color when the control is disabled.")]
        public Color DisabledBackColor
        {
            get => _disabledBackColor;
            set { _disabledBackColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the text color when the control is disabled.")]
        public Color DisabledForeColor
        {
            get => _disabledForeColor;
            set { _disabledForeColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the button color when the control is disabled.")]
        public Color DisabledButtonColor
        {
            get => _disabledButtonColor;
            set { _disabledButtonColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the border color when the control is focused.")]
        public Color FocusedBorderColor
        {
            get => _focusedBorderColor;
            set { _focusedBorderColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the background color when the mouse hovers over the control.")]
        public Color HoverBackColor
        {
            get => _hoverBackColor;
            set { _hoverBackColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the text color when the mouse hovers over the control.")]
        public Color HoverForeColor
        {
            get => _hoverForeColor;
            set { _hoverForeColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the button color when the mouse hovers over the control.")]
        public Color HoverButtonColor
        {
            get => _hoverButtonColor;
            set { _hoverButtonColor = value; Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the selected item in the dropdown list.")]
        public Color SelectedBackColor { get; set; } = SystemColors.Highlight;
        [Category("TS Appearance")]
        [Description("Gets or sets the text color of the selected item in the dropdown list.")]
        public Color SelectedForeColor { get; set; } = SystemColors.HighlightText;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;
            bool rtl = RightToLeft == RightToLeft.Yes;
            float scale = DeviceDpi / 96f;
            int buttonWidth = (int)(20 * scale);
            int padding = (int)(4 * scale);
            Rectangle buttonRect = rtl ? new Rectangle(0, 0, buttonWidth, rect.Height) : new Rectangle(rect.Width - buttonWidth, 0, buttonWidth, rect.Height);
            Rectangle textRect = rtl ? new Rectangle(buttonRect.Right + padding, 1, rect.Width - buttonRect.Width - padding, rect.Height - 2) : new Rectangle(padding, 1, rect.Width - buttonRect.Width - padding, rect.Height - 2);
            Color back = !Enabled ? _disabledBackColor : _isHovering ? _hoverBackColor : _backColor;
            Color fore = !Enabled ? _disabledForeColor : _isHovering ? _hoverForeColor : _foreColor;
            Color button = !Enabled ? _disabledButtonColor : _isHovering ? _hoverButtonColor : _buttonColor;
            using (SolidBrush b = new SolidBrush(back))
                e.Graphics.FillRectangle(b, rect);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | (rtl ? TextFormatFlags.Right : TextFormatFlags.Left));
            using (SolidBrush b = new SolidBrush(button))
                e.Graphics.FillRectangle(b, buttonRect);
            float aw = 8 * scale;
            float ah = 5 * scale;
            PointF c = new PointF(buttonRect.Left + buttonRect.Width / 2f, buttonRect.Top + buttonRect.Height / 2f);
            PointF[] arrow =
            {
                new PointF(c.X - aw / 2, c.Y - ah / 2),
                new PointF(c.X + aw / 2, c.Y - ah / 2),
                new PointF(c.X, c.Y + ah / 2)
            };
            using (SolidBrush b = new SolidBrush(!Enabled ? _disabledArrowColor : _arrowColor))
                e.Graphics.FillPolygon(b, arrow);
            using (Pen p = new Pen(Focused ? _focusedBorderColor : _borderColor))
                e.Graphics.DrawRectangle(p, 0, 0, rect.Width - 1, rect.Height - 1);
        }
        private void TSCustomComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) != 0;
            Color back = selected ? SelectedBackColor : (_isHovering ? _hoverBackColor : BackColor);
            Color fore = selected ? SelectedForeColor : (_isHovering ? _hoverForeColor : ForeColor);
            using (SolidBrush b = new SolidBrush(back))
                e.Graphics.FillRectangle(b, e.Bounds);
            TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(), Font, e.Bounds, fore, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }
        protected override void OnRightToLeftChanged(EventArgs e) { base.OnRightToLeftChanged(e); Invalidate(); }
        protected override void OnPaintBackground(PaintEventArgs e) { }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom DateTimePicker
    public class TSCustomDateTimePicker : DateTimePicker
    {
        // Colors
        private Color _backColor = SystemColors.Window;
        private Color _foreColor = SystemColors.WindowText;
        private Color _buttonColor = SystemColors.ControlDark;
        private Color _borderColor = SystemColors.ControlDark;
        //
        private Color _disabledBackColor = SystemColors.Control;
        private Color _disabledForeColor = SystemColors.GrayText;
        private Color _disabledButtonColor = SystemColors.ControlDark;
        //
        private Color _focusedBorderColor = Color.DodgerBlue;
        // Constructor
        public TSCustomDateTimePicker()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            UpdateCalendarColors();
        }
        // Properties
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the control. Updates the calendar colors when changed.")]
        public override Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                UpdateCalendarColors();
                Invalidate();
            }
        }
        private bool ShouldSerializeBackColor() => _backColor != SystemColors.Window;
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the foreground (text) color of the control. Updates the calendar colors when changed.")]
        public override Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                UpdateCalendarColors();
                Invalidate();
            }
        }
        private bool ShouldSerializeForeColor() => _foreColor != SystemColors.WindowText;
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the border color of the control.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the dropdown button.")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set { _buttonColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the control when it is disabled.")]
        public Color DisabledBackColor
        {
            get => _disabledBackColor;
            set { _disabledBackColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the text color of the control when it is disabled.")]
        public Color DisabledForeColor
        {
            get => _disabledForeColor;
            set { _disabledForeColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the dropdown button color when the control is disabled.")]
        public Color DisabledButtonColor
        {
            get => _disabledButtonColor;
            set { _disabledButtonColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the border color when the control is focused.")]
        public Color FocusedBorderColor
        {
            get => _focusedBorderColor;
            set { _focusedBorderColor = value; Invalidate(); }
        }
        private void UpdateCalendarColors()
        {
            this.CalendarForeColor = _foreColor;
            this.CalendarMonthBackground = _backColor;
        }
        // Overridden Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = this.ClientRectangle;
            bool rtl = this.RightToLeft == RightToLeft.Yes;
            float scale = this.DeviceDpi / 96f;
            int buttonWidth = (int)(20 * scale);
            int padding = (int)(2 * scale);
            Rectangle buttonRect = rtl ? new Rectangle(0, 0, buttonWidth, rect.Height) : new Rectangle(rect.Width - buttonWidth, 0, buttonWidth, rect.Height);
            Rectangle textRect = rtl ? new Rectangle(buttonRect.Right + padding, 0, rect.Width - buttonRect.Width - padding, rect.Height) : new Rectangle(padding, 0, rect.Width - buttonRect.Width - padding, rect.Height);
            Color effectiveBack = this.Enabled ? _backColor : _disabledBackColor;
            Color effectiveFore = this.Enabled ? _foreColor : _disabledForeColor;
            Color effectiveButton = this.Enabled ? _buttonColor : _disabledButtonColor;
            using (SolidBrush b = new SolidBrush(effectiveBack))
            {
                e.Graphics.FillRectangle(b, rect);
            }
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | (rtl ? TextFormatFlags.Right : TextFormatFlags.Left);
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, textRect, effectiveFore, flags);
            using (SolidBrush b = new SolidBrush(effectiveButton))
            {
                e.Graphics.FillRectangle(b, buttonRect);
            }
            int arrowWidth = (int)(8 * scale);
            int arrowHeight = (int)(5 * scale);
            Point middle = new Point(buttonRect.Left + buttonRect.Width / 2, buttonRect.Top + buttonRect.Height / 2);
            Point[] arrow = {
                new Point(middle.X - arrowWidth / 2, middle.Y - arrowHeight / 2),
                new Point(middle.X + arrowWidth / 2, middle.Y - arrowHeight / 2),
                new Point(middle.X, middle.Y + arrowHeight / 2)
            };
            using (SolidBrush arrowBrush = new SolidBrush(effectiveFore))
            {
                e.Graphics.FillPolygon(arrowBrush, arrow);
            }
            using (Pen pen = new Pen(this.Focused ? _focusedBorderColor : _borderColor))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
            }
            if (this.Focused && this.ShowFocusCues && this.Enabled)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, rect);
            }
        }
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);
            Invalidate();
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Invalidate();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            Invalidate();
        }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom FlowLayoutPanel
    public class TSCustomFLP : FlowLayoutPanel
    {
        private Point _savedScrollPosition;
        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (!DesignMode)
            {
                var current = AutoScrollPosition;
                _savedScrollPosition = new Point(-current.X, -current.Y);
            }
            base.OnLayout(levent);
            if (!DesignMode)
                AutoScrollPosition = _savedScrollPosition;
        }
        protected override void OnResize(EventArgs e)
        {
            if (!DesignMode)
            {
                var current = AutoScrollPosition;
                _savedScrollPosition = new Point(-current.X, -current.Y);
            }
            base.OnResize(e);
            if (!DesignMode)
                AutoScrollPosition = _savedScrollPosition;
        }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom ListBox
    public class TSCustomListBox : ListBox
    {
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the selected item in the control.")]
        public Color SelectedBackColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("Gets or sets the text color of the selected item in the control.")]
        public Color SelectedForeColor { get; set; } = Color.White;
        public TSCustomListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DoubleBuffered = true;
            UpdateItemHeight();
        }
        private void UpdateItemHeight()
        {
            this.ItemHeight = this.Font.Height + 5;
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateItemHeight();
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateItemHeight();
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= Items.Count) return;
            bool selected = (e.State & DrawItemState.Selected) != 0;
            Color backColor = selected ? SelectedBackColor : this.BackColor;
            Color foreColor = selected ? SelectedForeColor : this.ForeColor;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            Rectangle textBounds = new Rectangle(e.Bounds.X + 3, e.Bounds.Y, e.Bounds.Width - 3, e.Bounds.Height);
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix;
            TextRenderer.DrawText(e.Graphics, this.Items[e.Index].ToString(), this.Font, textBounds, foreColor, flags);
        }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom RadioButton
    public class TSCustomRadioButton : RadioButton
    {
        [Category("TS Appearance")]
        [Description("The color displayed when the radio button is checked.")]
        public Color CheckedColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("The color displayed when the radio button is unchecked.")]
        public Color UnCheckedColor { get; set; } = Color.Gray;
        public TSCustomRadioButton()
        {
            AutoSize = true;
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
            PerformLayout();
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            float dpi = DeviceDpi / 96f;
            int rbSize = (int)(18 * dpi);
            int padding = (int)(8 * dpi);
            int margin = 2;
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            Size textSize = TextRenderer.MeasureText(Text, Font, new Size(int.MaxValue, int.MaxValue), flags);
            int width = textSize.Width + rbSize + padding + (margin * 2);
            int height = Math.Max(textSize.Height, rbSize) + 6;
            return new Size(width, height);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            float dpi = DeviceDpi / 96f;
            float rbSize = 18f * dpi;
            float checkSize = 10f * dpi;
            float padding = 8f * dpi;
            float margin = 2f;
            bool rightAligned = CheckAlign == ContentAlignment.MiddleRight || CheckAlign == ContentAlignment.TopRight || CheckAlign == ContentAlignment.BottomRight;
            float rbX;
            Rectangle textRect;
            if (rightAligned)
            {
                rbX = Width - rbSize - margin;
                textRect = new Rectangle(0, 0, (int)(Width - rbSize - padding - margin), Height);
            }
            else
            {
                rbX = margin;
                int textX = (int)(rbSize + padding + margin);
                textRect = new Rectangle(textX, 0, Width - textX, Height);
            }
            RectangleF rbRect = new RectangleF(rbX, (Height - rbSize) / 2f, rbSize, rbSize);
            RectangleF checkRect = new RectangleF(rbRect.X + (rbRect.Width - checkSize) / 2f, rbRect.Y + (rbRect.Height - checkSize) / 2f, checkSize, checkSize);
            using (Pen borderPen = new Pen(Checked ? CheckedColor : UnCheckedColor, 1.6f * dpi))
            using (SolidBrush checkBrush = new SolidBrush(CheckedColor))
            {
                g.DrawEllipse(borderPen, rbRect);
                if (Checked)
                    g.FillEllipse(checkBrush, checkRect);
            }
            TextFormatFlags textFlags = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter;
            textFlags |= rightAligned ? TextFormatFlags.Right : TextFormatFlags.Left;
            TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? ForeColor : SystemColors.GrayText, textFlags);
        }
    }
    #endregion
    // ======================================================================================================
    #region TS Custom TrackBar
    public class TSCustomTrackBar : Control
    {
        [Category("TS Appearance")]
        [Description("The color of the track (background) of the control.")]
        public Color TrackColor { get; set; } = Color.LightGray;
        [Category("TS Appearance")]
        [Description("The color of the filled portion of the track.")]
        public Color TrackFillColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("The height of the track.")]
        public float TrackHeight { get; set; } = 8f;
        [Category("TS Appearance")]
        [Description("The radius of the track corners.")]
        public float TrackRadius { get; set; } = 5f;
        [Category("TS Appearance")]
        [Description("The color of the thumb (slider knob).")]
        public Color ThumbColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("The color of the thumb when the mouse hovers over it.")]
        public Color ThumbHoverColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("The color of the thumb when it is pressed.")]
        public Color ThumbPressedColor { get; set; } = Color.DodgerBlue;
        [Category("TS Appearance")]
        [Description("The border color of the thumb.")]
        public Color ThumbBorderColor { get; set; } = Color.DimGray;
        [Category("TS Appearance")]
        [Description("The radius of the thumb corners.")]
        public float ThumbRadius { get; set; } = 10f;
        [Category("TS Appearance")]
        [Description("The thickness of the thumb border.")]
        public float ThumbBorderThickness { get; set; } = 0f;
        [Category("TS Appearance")]
        [Description("The minimum value of the control.")]
        public int Minimum { get; set; } = 0;
        [Category("TS Appearance")]
        [Description("The maximum value of the control.")]
        public int Maximum { get; set; } = 100;
        private int _value = 0;
        [Category("TS Appearance")]
        [Description("The current value of the control.")]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(Minimum, Math.Min(Maximum, value));
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        [Category("TS Appearance")]
        [Description("Determines whether the control is displayed vertically.")]
        public bool Vertical { get; set; } = false;
        private bool _hover = false;
        private bool _pressed = false;
        public event EventHandler ValueChanged;
        public TSCustomTrackBar()
        {
            DoubleBuffered = true;
            this.Cursor = Cursors.Hand;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        }
        protected override void OnMouseDown(MouseEventArgs e) { _pressed = true; UpdateValueFromMouse(e.Location); base.OnMouseDown(e); }
        protected override void OnMouseMove(MouseEventArgs e) { if (_pressed) UpdateValueFromMouse(e.Location); base.OnMouseMove(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        private void UpdateValueFromMouse(Point location)
        {
            float dpiScale = DeviceDpi / 96f;
            float thumbR = ThumbRadius * dpiScale;
            float bThick = ThumbBorderThickness * dpiScale;
            float margin = thumbR + (bThick / 2f) + 2f;
            if (Vertical)
            {
                float usableHeight = Height - (2 * margin);
                float pos = location.Y - margin;
                float ratio = 1f - (pos / usableHeight);
                Value = Minimum + (int)Math.Round(Math.Max(0, Math.Min(1, ratio)) * (Maximum - Minimum));
            }
            else
            {
                float usableWidth = Width - (2 * margin);
                float pos = location.X - margin;
                float ratio = pos / usableWidth;
                Value = Minimum + (int)Math.Round(Math.Max(0, Math.Min(1, ratio)) * (Maximum - Minimum));
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            float dpiScale = DeviceDpi / 96f;
            float thumbR = ThumbRadius * dpiScale;
            float bThick = ThumbBorderThickness * dpiScale;
            float trackH = TrackHeight * dpiScale;
            float trackRad = TrackRadius * dpiScale;
            float margin = thumbR + (bThick / 2f) + 2f;
            RectangleF trackRect = Vertical ? new RectangleF((Width - trackH) / 2f, margin, trackH, Height - (2 * margin)) : new RectangleF(margin, (Height - trackH) / 2f, Width - (2 * margin), trackH);
            using (GraphicsPath trackPath = GetRoundedRectPath(trackRect, trackRad))
            using (SolidBrush br = new SolidBrush(TrackColor))
                g.FillPath(br, trackPath);
            float ratio = (Maximum == Minimum) ? 0 : (float)(Value - Minimum) / (Maximum - Minimum);
            if (ratio > 0)
            {
                RectangleF fillRect = Vertical ? new RectangleF(trackRect.X, trackRect.Bottom - (trackRect.Height * ratio), trackRect.Width, trackRect.Height * ratio) : new RectangleF(trackRect.X, trackRect.Y, trackRect.Width * ratio, trackRect.Height);
                using (GraphicsPath fillPath = GetRoundedRectPath(fillRect, trackRad))
                using (SolidBrush br = new SolidBrush(TrackFillColor))
                    g.FillPath(br, fillPath);
            }
            PointF thumbCenter = Vertical ? new PointF(Width / 2f, trackRect.Bottom - (trackRect.Height * ratio))  : new PointF(trackRect.X + (trackRect.Width * ratio), Height / 2f);
            RectangleF thumbRect = new RectangleF(thumbCenter.X - thumbR, thumbCenter.Y - thumbR, thumbR * 2, thumbR * 2);
            Color activeThumbColor = _pressed ? ThumbPressedColor : (_hover ? ThumbHoverColor : ThumbColor);
            using (SolidBrush br = new SolidBrush(activeThumbColor))
                g.FillEllipse(br, thumbRect);
            if (bThick > 0)
            {
                using (Pen pen = new Pen(ThumbBorderColor, bThick))
                {
                    pen.Alignment = PenAlignment.Inset;
                    g.DrawEllipse(pen, thumbRect);
                }
            }
        }
        private GraphicsPath GetRoundedRectPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.1f) { path.AddRectangle(rect); return path; }
            float d = radius * 2;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
    #endregion

}