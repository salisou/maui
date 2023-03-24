package com.microsoft.maui;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.PorterDuff;
import android.graphics.Rect;
import android.util.AttributeSet;
import android.view.View;

import androidx.annotation.NonNull;

public abstract class PlatformWrapperView extends PlatformContentViewGroup {
    public PlatformWrapperView(Context context) {
        super(context);
        this.viewBounds = new Rect();
    }

    public PlatformWrapperView(Context context, AttributeSet attrs) {
        super(context, attrs);
        this.viewBounds = new Rect();
    }

    public PlatformWrapperView(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
        this.viewBounds = new Rect();
    }

    public PlatformWrapperView(Context context, AttributeSet attrs, int defStyle, int defStyleRes) {
        super(context, attrs, defStyle, defStyleRes);
        this.viewBounds = new Rect();
    }

    private final Rect viewBounds;
    private boolean hasShadow;

    protected void setHasShadow(boolean hasShadow) {
        this.hasShadow = hasShadow;
        postInvalidate();
    }

    @Override
    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        if (getChildCount() == 0) {
            super.onMeasure(widthMeasureSpec, heightMeasureSpec);
            return;
        }

        viewBounds.set(0, 0, MeasureSpec.getSize(widthMeasureSpec), MeasureSpec.getSize(heightMeasureSpec));
        View child = getChildAt(0);
        child.measure(widthMeasureSpec, heightMeasureSpec);
        setMeasuredDimension(child.getMeasuredWidth(), child.getMeasuredHeight());
    }

    @Override
    protected void dispatchDraw(Canvas canvas) {
        if (hasShadow) {
            int viewWidth = viewBounds.width();
            int viewHeight = viewBounds.height();

            if (getChildCount() > 0) {
                View child = getChildAt(0);
                if (viewWidth == 0)
                    viewWidth = child.getMeasuredWidth();
                if (viewHeight == 0)
                    viewHeight = child.getMeasuredHeight();
            }

            drawShadow(canvas, viewWidth, viewHeight);
        }

        super.dispatchDraw(canvas);
    }

    /**
     * Overridden in C#, for custom logic around shadows
     * @param canvas
     * @param viewWidth
     * @param viewHeight
     * @return
     */
    protected abstract void drawShadow(@NonNull Canvas canvas, int viewWidth, int viewHeight);

    /**
     * Called by C#'s drawShadow implementation, this simplifies several C# to Java calls into one
     * @param shadowCanvas
     * @param shadowBitmap
     * @return
     */
    protected Bitmap drawShadowCore(Canvas shadowCanvas, Bitmap shadowBitmap) {
        // Reset Canvas
        shadowCanvas.setBitmap(shadowBitmap);

        // bottom layer of natural canvas.
        super.dispatchDraw(shadowCanvas);

        // Get the alpha bounds of bitmap
        Bitmap extractAlpha = shadowBitmap.extractAlpha();

        // Clear past content to draw shadow
        shadowCanvas.drawColor(Color.BLACK, PorterDuff.Mode.CLEAR);

        return extractAlpha;
    }
}
