package com.microsoft.maui;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Path;
import android.util.AttributeSet;
import android.view.ViewGroup;

import androidx.annotation.NonNull;

public abstract class PlatformContentViewGroup extends ViewGroup {

    public PlatformContentViewGroup(Context context) {
        super(context);
    }

    public PlatformContentViewGroup(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public PlatformContentViewGroup(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }

    public PlatformContentViewGroup(Context context, AttributeSet attrs, int defStyle, int defStyleRes) {
        super(context, attrs, defStyle, defStyleRes);
    }

    private boolean hasClip;

    protected void setHasClip(boolean hasClip) {
        this.hasClip = hasClip;
        postInvalidate();
    }

    @Override
    protected void dispatchDraw(Canvas canvas) {
        if (hasClip) {
            Path path = getClipPath(canvas);
            if (path != null) {
                canvas.clipPath(path);
            }
        }

        super.dispatchDraw(canvas);
    }

    protected abstract Path getClipPath(@NonNull Canvas canvas);
}
