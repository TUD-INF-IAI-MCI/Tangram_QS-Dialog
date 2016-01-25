using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

public class GrowLabel : Label
{
    private bool mGrowing;
    public GrowLabel()
    {
        this.AutoSize = false;
    }
    public void ResizeLabel()
    {
        if (mGrowing) return;
        try
        {
            int width = Math.Min(this.Width, (this.Parent != null ? this.Parent.Width : this.Width ));

            mGrowing = true;
            Size sz = new Size(width, Int32.MaxValue);
            sz = TextRenderer.MeasureText(this.Text, this.Font, sz, TextFormatFlags.WordBreak);
            this.Height = sz.Height;
        }
        finally
        {
            mGrowing = false;
        }
    }
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        ResizeLabel();
    }
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        ResizeLabel();
    }
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        ResizeLabel();
    }
}
