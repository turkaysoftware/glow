// ======================================================================================================
// Türkay Software - C# Custom Graphics UI Library
// © Eray Türkay
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
        private int borderRadius = 20;
        // Colors
        private Color borderColor = Color.DodgerBlue;
        // Properties
        [Category("TS Appearance")]
        [Description("Gets or sets the border size.")]
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; this.Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the border radius size.")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; this.Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the border color.")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the background color.")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("TS Appearance")]
        [Description("Gets or sets the fore color.")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }
        // Constructor
        public TSCustomButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.DodgerBlue;
            this.ForeColor = Color.White;
            this.Resize += Button_Resize;
        }
        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }
        private float ScaleFactor => this.DeviceDpi / 96f;
        // Overridden Methods
        private GraphicsPath GetFigurePath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            int scaledBorderSize = (int)(borderSize * ScaleFactor);
            int scaledBorderRadius = (int)(borderRadius * ScaleFactor);
            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -scaledBorderSize, -scaledBorderSize);
            int smoothSize = scaledBorderSize > 0 ? scaledBorderSize : 2;
            if (scaledBorderRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, scaledBorderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, scaledBorderRadius - scaledBorderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, scaledBorderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    this.Region = new Region(pathSurface);
                    pevent.Graphics.DrawPath(penSurface, pathSurface);
                    if (scaledBorderSize >= 1)
                    {
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                this.Region = new Region(rectSurface);
                if (scaledBorderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, scaledBorderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += Container_BackColorChanged;
        }
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
    #endregion
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
        private Color _hoverButtonColor = SystemColors.ControlDark;
        // Events
        private bool _isHovering = false;
        // Constructor
        public TSCustomComboBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.MouseEnter += (s, e) => { _isHovering = true; Invalidate(); };
            this.MouseLeave += (s, e) => { _isHovering = false; Invalidate(); };
            this.DrawItem += TSCustomComboBox_DrawItem;
        }
        // Properties
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color of the ComboBox.")]
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
        private bool ShouldSerializeBackColor() => _backColor != SystemColors.Window;
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the foreground color of the ComboBox.")]
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
        private bool ShouldSerializeForeColor() => _foreColor != SystemColors.WindowText;
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the border color of the ComboBox.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the dropdown button color of the ComboBox.")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set { _buttonColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the color of the dropdown arrow.")]
        public Color ArrowColor
        {
            get => _arrowColor;
            set { _arrowColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the arrow color when the ComboBox is disabled.")]
        public Color DisabledArrowColor
        {
            get => _disabledArrowColor;
            set { _disabledArrowColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color when the ComboBox is disabled.")]
        public Color DisabledBackColor
        {
            get => _disabledBackColor;
            set { _disabledBackColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the foreground color when the ComboBox is disabled.")]
        public Color DisabledForeColor
        {
            get => _disabledForeColor;
            set { _disabledForeColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the dropdown button color when the ComboBox is disabled.")]
        public Color DisabledButtonColor
        {
            get => _disabledButtonColor;
            set { _disabledButtonColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the border color when the ComboBox has focus.")]
        public Color FocusedBorderColor
        {
            get => _focusedBorderColor;
            set { _focusedBorderColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color when the mouse hovers over the ComboBox.")]
        public Color HoverBackColor
        {
            get => _hoverBackColor;
            set { _hoverBackColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the dropdown button color when the mouse hovers over the ComboBox.")]
        public Color HoverButtonColor
        {
            get => _hoverButtonColor;
            set { _hoverButtonColor = value; Invalidate(); }
        }
        // Overridden Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = this.ClientRectangle;
            bool rtl = (this.RightToLeft == RightToLeft.Yes);
            float scale = this.DeviceDpi / 96f;
            int buttonWidth = (int)(20 * scale);
            int padding = (int)(2 * scale);
            Rectangle buttonRect = rtl ? new Rectangle(0, 0, buttonWidth, rect.Height) : new Rectangle(rect.Width - buttonWidth, 0, buttonWidth, rect.Height);
            Rectangle textRect = rtl ? new Rectangle(buttonRect.Right + padding, 0, rect.Width - buttonRect.Width - padding, rect.Height) : new Rectangle(padding, 0, rect.Width - buttonRect.Width - padding, rect.Height);
            Color effectiveBack = !this.Enabled ? _disabledBackColor : _isHovering ? _hoverBackColor : _backColor;
            Color effectiveFore = !this.Enabled ? _disabledForeColor : _foreColor;
            Color effectiveButton = !this.Enabled ? _disabledButtonColor : _isHovering ? _hoverButtonColor : _buttonColor;
            using (SolidBrush b = new SolidBrush(effectiveBack))
                e.Graphics.FillRectangle(b, rect);
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | (rtl ? TextFormatFlags.Right : TextFormatFlags.Left);
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, textRect, effectiveFore, flags);
            using (SolidBrush b = new SolidBrush(effectiveButton))
                e.Graphics.FillRectangle(b, buttonRect);
            float arrowWidth = 8 * scale;
            float arrowHeight = 5 * scale;
            PointF middle = new PointF(buttonRect.Left + buttonRect.Width / 2f, buttonRect.Top + buttonRect.Height / 2f);
            PointF[] arrow = {
                new PointF(middle.X - arrowWidth / 2f, middle.Y - arrowHeight / 2f),
                new PointF(middle.X + arrowWidth / 2f, middle.Y - arrowHeight / 2f),
                new PointF(middle.X, middle.Y + arrowHeight / 2f)
            };
            Color effectiveArrow = !this.Enabled ? _disabledArrowColor : _arrowColor;
            using (SolidBrush arrowBrush = new SolidBrush(effectiveArrow))
                e.Graphics.FillPolygon(arrowBrush, arrow);
            using (Pen pen = new Pen(this.Focused ? _focusedBorderColor : _borderColor))
                e.Graphics.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
            if (this.Focused && this.ShowFocusCues && this.Enabled)
                ControlPaint.DrawFocusRectangle(e.Graphics, rect);
        }
        private void TSCustomComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = selected ? SystemColors.Highlight : this.BackColor;
            Color fore = selected ? SystemColors.HighlightText : this.ForeColor;
            using (SolidBrush b = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }
            string text = this.Items[e.Index].ToString();
            TextRenderer.DrawText(e.Graphics, text, this.Font, e.Bounds, fore, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            e.DrawFocusRectangle();
        }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }
        protected override void OnRightToLeftChanged(EventArgs e) { base.OnRightToLeftChanged(e); Invalidate(); }
    }
    #endregion
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
            this.SetStyle(ControlStyles.UserPaint, true);
            UpdateCalendarColors();
        }
        // Properties
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color.")]
        public override Color BackColor
        {
            get => _backColor;
            set { _backColor = value; UpdateCalendarColors(); Invalidate(); }
        }
        private bool ShouldSerializeBackColor() => _backColor != SystemColors.Window;
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the foreground color.")]
        public override Color ForeColor
        {
            get => _foreColor;
            set { _foreColor = value; UpdateCalendarColors(); Invalidate(); }
        }
        private bool ShouldSerializeForeColor() => _foreColor != SystemColors.WindowText;
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the border color.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the dropdown button color.")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set { _buttonColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the background color when disabled.")]
        public Color DisabledBackColor
        {
            get => _disabledBackColor;
            set { _disabledBackColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the foreground color when disabled.")]
        public Color DisabledForeColor
        {
            get => _disabledForeColor;
            set { _disabledForeColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the dropdown button color when disabled.")]
        public Color DisabledButtonColor
        {
            get => _disabledButtonColor;
            set { _disabledButtonColor = value; Invalidate(); }
        }
        [Browsable(true)]
        [Category("TS Appearance")]
        [Description("Gets or sets the border color when focused.")]
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
}